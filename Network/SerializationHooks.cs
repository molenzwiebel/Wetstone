using System;
using System.Runtime.InteropServices;
using BepInEx.Unity.IL2CPP.Hook;
using MonoMod.RuntimeDetour;
using ProjectM.Network;
using Stunlock.Network;
using Unity.Collections;
using Unity.Entities;
using Wetstone.API;
using Wetstone.Util;
using static NetworkEvents_Serialize;

namespace Wetstone.Network;

/// Contains the serialization hooks for custom packets.
internal static class SerializationHooks
{
    // chosen by fair dice roll
    internal const int WETSTONE_NETWORK_EVENT_ID = 0x000FD00D;

    private static INativeDetour? _serializeDetour;
    private static INativeDetour? _deserializeDetour;

    // Detour events.
    public static void Initialize()
    {
        unsafe
        {
            _serializeDetour = NativeHookUtil.IDetour(typeof(NetworkEvents_Serialize), "SerializeEvent", SerializeHook, out SerializeOriginal);
            _deserializeDetour = NativeHookUtil.IDetour(typeof(NetworkEvents_Serialize), "DeserializeEvent", DeserializeHook, out DeserializeOriginal);
        }
    }

    // Undo detours.
    public static void Uninitialize()
    {
        _serializeDetour?.Dispose();
        _deserializeDetour?.Dispose();
    }


    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public unsafe delegate void SerializeEvent(IntPtr entityManager, IntPtr networkEventType, IntPtr netBufferOut, IntPtr entity);

    public static SerializeEvent? SerializeOriginal;

    public unsafe static void SerializeHook(IntPtr entityManager, IntPtr networkEventType, IntPtr netBuffer, IntPtr entity)
    {
        var eventType = *(NetworkEventType*)&networkEventType;

        // if this is not a custom event, just call the original function
        if (eventType.EventId != SerializationHooks.WETSTONE_NETWORK_EVENT_ID)
        {
            SerializeOriginal!(entityManager, networkEventType, netBuffer, entity);
            return;
        }

        ref NetBufferOut netBufferOut = ref *(NetBufferOut*)netBuffer;

        // extract the custom network event
        var realEntity = *(Entity*)&entity;
        var data = (CustomNetworkEvent)VWorld.Server.EntityManager.GetComponentObject<Il2CppSystem.Object>(realEntity, CustomNetworkEvent.ComponentType);

        // write out the event ID and the data
        netBufferOut.Write((uint)SerializationHooks.WETSTONE_NETWORK_EVENT_ID);
        data.Serialize(ref netBufferOut);
    }

    // --------------------------------------------------------------------------------------

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public unsafe delegate void DeserializeEvent(IntPtr entityManager, IntPtr commandBuffer, IntPtr netBuffer, DeserializeNetworkEventParams eventParams);

    public static DeserializeEvent? DeserializeOriginal;

    public unsafe static void DeserializeHook(IntPtr entityManager, IntPtr commandBuffer, IntPtr netBuffer, DeserializeNetworkEventParams eventParams)
    {
        ref NetBufferIn netBufferIn = ref *(NetBufferIn*)netBuffer;

        var eventId = netBufferIn.ReadUInt32();
        if (eventId != SerializationHooks.WETSTONE_NETWORK_EVENT_ID)
        {
            // rewind the buffer
            netBufferIn.m_readPosition -= 32;

            DeserializeOriginal!(entityManager, commandBuffer, netBuffer, eventParams);
            return;
        }

        var typeName = netBufferIn.ReadString(Allocator.Temp);
        if (MessageRegistry._eventHandlers.ContainsKey(typeName))
        {
            var handler = MessageRegistry._eventHandlers[typeName];
            var isFromServer = eventParams.FromCharacter.User == Entity.Null;

            try
            {
                if (isFromServer)
                    handler.OnReceiveFromServer(netBufferIn);
                else
                    handler.OnReceiveFromClient(eventParams.FromCharacter, netBufferIn);
            }
            catch (Exception ex)
            {
                WetstonePlugin.Logger.LogError($"Error handling incoming network event {typeName}:");
                WetstonePlugin.Logger.LogError(ex);
            }
        }
    }
}