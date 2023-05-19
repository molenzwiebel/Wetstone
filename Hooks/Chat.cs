
using System;
using System.Linq;
using System.Runtime.InteropServices;
using MonoMod.RuntimeDetour;
using ProjectM;
using ProjectM.Network;
using Unity.Entities;
using Wetstone.API;
using Wetstone.Util;

namespace Wetstone.Hooks;

public static class Chat
{
    public delegate void ChatEventHandler(VChatEvent e);

    /// <summary>
    /// Event emitted whenever a chat message is received by the server. Will
    /// not be emitted when running on the client.
    /// </summary>
    public static event ChatEventHandler? OnChatMessage;

    private static NativeDetour? Detour;

    public static unsafe void Initialize()
    {
        if (Detour != null)
            throw new Exception("Detour already initialized. You don't need to call this. The Wetstone plugin will do it for you.");

        var ty = typeof(ChatMessageSystem).GetNestedTypes().First(x => x.Name.Contains("ChatMessageJob"));
        Detour = NativeHookUtil.Detour(ty, "OriginalLambdaBody", Hook, out Original);
    }

    public static unsafe void Uninitialize()
    {
        if (Detour == null)
            throw new Exception("Detour wasn't initialized. Are you trying to unload Wetstone twice?");

        Detour.Dispose();
        Detour = null;

        OnChatMessage = null;
    }

    private static unsafe void Hook(IntPtr _this, Entity* chatMessageEntity, ChatMessageEvent* chatMessage, FromCharacter* fromCharacter)
    {
        var ev = new VChatEvent(fromCharacter->User, fromCharacter->Character, chatMessage->MessageText.ToString(), chatMessage->MessageType);

        WetstonePlugin.Logger.LogInfo($"[Chat] [{ev.Type}] {ev.User.CharacterName}: {ev.Message}");

        try
        {
            OnChatMessage?.Invoke(ev);

            if (ev.Cancelled) return;
        }
        catch (Exception ex)
        {
            WetstonePlugin.Logger.LogError("Error dispatching chat event:");
            WetstonePlugin.Logger.LogError(ex);
        }

        Original!(_this, chatMessageEntity, chatMessage, fromCharacter);
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void OriginalLambdaBody(IntPtr _this, Entity* chatMessageEntity, ChatMessageEvent* chatMessage, FromCharacter* fromCharacter);

    private static OriginalLambdaBody? Original;
}

/// <summary>
/// Represents a chat message sent by a user.
/// </summary>
public class VChatEvent
{
    /// <summary>
    /// The user entity of the user that sent the message. This contains the `User` component.
    /// </summary>
    public Entity SenderUserEntity { get; }
    /// <summary>
    /// The character entity of the user that sent the message. This contains the character
    /// instances, such as its position, health, etc.
    /// </summary>
    public Entity SenderCharacterEntity { get; }
    /// <summary>
    /// The message that was sent.
    /// </summary>
    public string Message { get; }
    /// <summary>
    /// The type of message that was sent.
    /// </summary>
    public ChatMessageType Type { get; }

    /// <summary>
    /// Whether this message was cancelled. Cancelled messages will not be
    /// forwarded to the normal VRising chat system and will not be sent to
    /// any other clients. Use the Cancel() function to set this flag. Note
    /// that cancelled events will still be forwarded to other plugins that
    /// have subscribed to this event.
    /// </summary>
    public bool Cancelled { get; private set; } = false;

    /// <summary>
    /// The user component instance of the user that sent the message.
    /// </summary>
    public User User => VWorld.Server.EntityManager.GetComponentData<User>(SenderUserEntity);

    internal VChatEvent(Entity userEntity, Entity characterEntity, string message, ChatMessageType type)
    {
        SenderUserEntity = userEntity;
        SenderCharacterEntity = characterEntity;
        Message = message;
        Type = type;
    }

    /// <summary>
    /// Cancel this message. Cancelled messages will not be forwarded to the
    /// normal VRising chat system and will not be sent to any other clients.
    /// Note that cancelled events will still be forwarded to other plugins 
    /// that have subscribed to this event.
    /// </summary>
    public void Cancel()
    {
        Cancelled = true;
    }
}