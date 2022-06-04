using System;
using System.Runtime.InteropServices;
using BepInEx.IL2CPP.Hook;
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

    private static FastNativeDetour? _serializeDetour;
    private static FastNativeDetour? _deserializeDetour;

    // Detour events.
    public static void Initialize()
    {
        unsafe
        {
            _serializeDetour = NativeHookUtil.Detour(typeof(NetworkEvents_Serialize), "SerializeEvent", SerializeHook, out SerializeOriginal);
            _deserializeDetour = NativeHookUtil.Detour(typeof(NetworkEvents_Serialize), "DeserializeEvent", DeserializeHook, out DeserializeOriginal);
        }
    }

    // Undo detours.
    public static void Uninitialize()
    {
        _serializeDetour?.Dispose();
        _deserializeDetour?.Dispose();
    }

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public unsafe delegate void SerializeEvent(IntPtr entityManager, UInt64 networkEventType, UInt64 netBufferOut, IntPtr entity);

    public static SerializeEvent? SerializeOriginal;

    public unsafe static void SerializeHook(IntPtr entityManager, UInt64 networkEventType, UInt64 netBufferOut, IntPtr entity)
    {
        var eventType = *(NetworkEventType*)&networkEventType;

        // if this is not a custom event, just call the original function
        if (eventType.EventId != SerializationHooks.WETSTONE_NETWORK_EVENT_ID)
        {
            SerializeOriginal!(entityManager, networkEventType, netBufferOut, entity);
            return;
        }

        // we need to adjust by 0x10 here because the managed proxy that Il2CppUnhollower
        // generates expects a boxed struct, whereas we have a pointer to an unboxed one.
        // subtracting 0x10 is exactly two pointers, which allows us to fake a boxed struct
        // with the same underlying data
        var netBuffer = new NetBufferOut(new IntPtr((long)(netBufferOut - 0x10)));

        // extract the custom network event
        var realEntity = *(Entity*)&entity;
        var data = (CustomNetworkEvent)VWorld.Server.EntityManager.GetComponentObject<Il2CppSystem.Object>(realEntity, CustomNetworkEvent.ComponentType);

        // write out the event ID and the data
        netBuffer.Write((uint)SerializationHooks.WETSTONE_NETWORK_EVENT_ID);
        data.Serialize(netBuffer);
    }

    // --------------------------------------------------------------------------------------

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public unsafe delegate void DeserializeEvent(IntPtr commandBuffer, IntPtr netBuffer, DeserializeNetworkEventParams eventParams);

    public static DeserializeEvent? DeserializeOriginal;

    public unsafe static void DeserializeHook(IntPtr commandBuffer, IntPtr netBuffer, DeserializeNetworkEventParams eventParams)
    {
        // this is the same adjustment that we do in the Serialize hook
        var netBufferIn = new NetBufferIn(new IntPtr((long)(netBuffer - 0x10)));

        // read event ID, and if it's not our custom event, call the original function
        var eventId = netBufferIn.ReadUInt32();
        if (eventId != SerializationHooks.WETSTONE_NETWORK_EVENT_ID)
        {
            // rewind the buffer
            netBufferIn.m_readPosition -= 32;

            DeserializeOriginal!(commandBuffer, netBuffer, eventParams);
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