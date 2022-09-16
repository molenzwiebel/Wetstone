---
layout: default
title: Game Frame
nav_order: 3
parent: Wetstone Hooks
---

Client & Server
{: .label .label-blue }

# Game Frame Hook

Wetstone offers an easy hook if you'd like to execute code on every rendering frame, without injecting your own MonoBehaviour in the game. Using Wetstone's hooks will additionally allow you to [reload your plugins](../getting-started/reloading.html) without running into issues.

In order to subscribe to frame events, simply add a handler to the `Wetstone.Hooks.GameFrame.OnUpdate` or `Wetstone.Hooks.GameFrame.OnLateUpdate` events:

```csharp
Wetstone.Hooks.GameFrame.OnUpdate += () =>
{
    Log.LogInfo("This prints every tick!");
};
```

Note that, if your plugin [supports reloading](../getting-started/reloading.html), you will also need to remove these listeners when your plugin unloads:

```csharp
public class Plugin : BasePlugin
{
    public override void Load()
    {
        Wetstone.Hooks.GameFrame.OnUpdate += HandleUpdate;
    }

    public override bool Unload()
    {
        Wetstone.Hooks.GameFrame.OnUpdate -= HandleUpdate;
        return true;
    }

    private void HandleUpdate()
    {
        Log.LogInfo("This prints every tick!");
    }
}
```

---

For more information on the Game Frame hook, see [Hooks/GameFrame.cs](https://github.com/molenzwiebel/Wetstone/blob/master/Hooks/GameFrame.cs).