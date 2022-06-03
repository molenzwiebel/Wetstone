---
layout: default
title: Chat
nav_order: 1
parent: Wetstone Hooks
---

Server-Only
{: .label .label-purple }

# Chat Hook

Wetstone offers an easy to use API for getting invoked when the server receives a chat message from the client.

To start listening to chat messages, simply add an event listener to `Wetstone.Hooks.Chat.OnChatMessage`:

```csharp
Wetstone.Hooks.Chat.OnChatMessage += (ev) =>
{
    Log.LogInfo($"Received an {ev.Type} chat message from {ev.User.CharacterName}: {ev.Message}");
};
```

Whenever a chat message is received, all plugins that have subscribed to chat messages will receive a `VChatEvent` event. Chat messages will be forwarded to plugins **first**, and only after to the game, unless the event has been cancelled. `VChatEvent` contains the following fields/properties:

- `SenderUserEntity`: The user entity that sent the message. This user entity contains components such as `Chat`.
- `SenderCharacterEntity`: The character entity that sent the message. This is the controlled character and contains components such as `Health` and `Movement`.
- `User`: Utility computed getter that retrieves the `User` component instance for the user that sent this message.
- `Type`: The `ChatMessageType` indicating the type of message sent.
- `Message`: The actual message sent.
- `IsCancelled`: Whether this event was "cancelled" by some plugin. A cancelled chat message will not be forwarded to the real V Rising server and will not be seen by players other than the sender. Cancelled chat events will still invoke chat listeners on other plugins.
- `Cancel()`: Cancels the chat message, preventing it from being forwarded to the V Rising server. Other plugins may still see the message.

Due to the way the cancellation system works, it is recommended that you check whether the event is cancelled before you process the message, and discard it if you do. Only process cancelled events if you have a good reason to do so.

It is good behavior to remove the `OnChatMessage` in `Unload` listener if your plugin [supports reloading](../getting-started/reloading.html).

---

Here is an example of a simple plugin that processes chat commands and supports reloading:

```csharp
[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("xyz.molenzwiebel.wetstone")]
[Wetstone.API.Reloadable]
public class Plugin : BasePlugin
{
    public override void Load()
    {
        if (!VWorld.IsServer) return;

        Wetstone.Hooks.Chat.OnChatMessage += HandleChatMessage;
    }

    public override bool Unload()
    {
        if (!VWorld.IsServer) return true;

        // ensure we clean up our handler when we unload
        Wetstone.Hooks.Chat.OnChatMessage -= HandleChatMessage;

        return true;
    }

    private static void HandleChatMessage(VChatEvent ev)
    {
        if (!ev.Message.StartsWith("!")) return;
        if (ev.Cancelled) return; // ignore messages already processed by some other plugin

        ev.Cancel(); // prevent the command from showing up, since we're handling it

        if (ev.Message.StartsWith("!hp"))
        {
            var hp = float.Parse(ev.Message.Substring(4));

            ev.SenderCharacterEntity.WithComponentData((ref Health health) =>
            {
                health.Value = hp;
            });

            ev.User.SendSystemMessage("Set current HP to <color=#ffff00ff>" + hp + "</color>.");
            return;
        }

        ev.User.SendSystemMessage("<color=#ff0000ff>Unknown command!</color>");
    }
}
```

This snippet additionally uses the [VWorld](../apis/ecs-extensions.md) and [VExtensions](../apis/ecs-extensions.md) APIs for cleaner code.

---

For more information on the Chat hook, see [Hooks/Chat.cs](https://github.com/molenzwiebel/Wetstone/blob/master/Hooks/Chat.cs).