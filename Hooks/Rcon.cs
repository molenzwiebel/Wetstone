using System;
using System.Text;
using HarmonyLib;
using RCONServerLib;
using UnhollowerBaseLib;
using Wetstone.API;

namespace Wetstone.Hooks;

public static class Rcon
{
#nullable disable 
    private static Harmony _harmony;
#nullable enable

    public delegate void RconEventHandler(VRconEvent e);

    /// <summary>
    /// Event emitted whenever a rcon message is received by the server. Will
    /// not be emitted when running on the client.
    /// </summary>
    public static event RconEventHandler? OnRconMessage;

    public static void Initialize()
    {
        if (!VWorld.IsServer) return;

        _harmony = Harmony.CreateAndPatchAll(typeof(Rcon));
    }

    public static void Uninitialize()
    {
        if (!VWorld.IsServer) return;

        _harmony.UnpatchSelf();
    }

    [HarmonyPatch(typeof(RemoteConTcpClient), "ParsePacket")]
    [HarmonyPrefix]
    public static void ParsePacket(ref Il2CppStructArray<byte> rawPacket)
    {
        if ((int)rawPacket[8] == (int)RemoteConPacket.PacketType.ExecCommand) // if authorization was successful
        {
            var command = Encoding.ASCII.GetString(rawPacket, 12, rawPacket.Count - 12)/* remove '\0'  from end of string */.TrimEnd('\0');

            var ev = new VRconEvent(command, RemoteConPacket.PacketType.ExecCommand);

            try
            {
                OnRconMessage?.Invoke(ev);
            }
            catch (Exception ex)
            {
                WetstonePlugin.Logger.LogError("Error dispatching rcon event:");
                WetstonePlugin.Logger.LogError(ex);
            }
        }
    }
}

/// <summary>
/// Represents a rcon message sent by a user.
/// </summary>
public class VRconEvent
{
    /// <summary>
    /// The message that was sent.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// The type of message that was sent.
    /// </summary>
    public RemoteConPacket.PacketType Type { get; }

    internal VRconEvent(string message, RemoteConPacket.PacketType type)
    {
        Message = message;
        Type = type;
    }
}