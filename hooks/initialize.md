---
layout: default
title: Game Initialization
nav_order: 2
parent: Wetstone Hooks
---

Client & Server
{: .label .label-blue }

# Game Initialization Hook

Wetstone offers an easy hook if you'd like to execute code after the game client has finished initializing. This can be useful because BepInEx loads your plugins very early in the startup process, when not all types and ECS components have loaded yet.

In order to run code when the game has finished initializing, simply implement the `Wetstone.API.IRunOnInitialized` interface in your main plugin:

```csharp
public class Plugin : BasePlugin, IRunOnInitialized
{
    public void OnGameInitialized()
    {
        Log.LogInfo("Game has initialized!");
    }
}
```

Wetstone will automatically detect that your mod has the interface and call the function when appropriate.

If your plugin [supports reloading](../getting-started/reloading.html), Wetstone will immediately call the `OnGameInitialized` function after a reload to ensure that the method always runs.

---

See [API/IRunOnInitialized.cs](https://github.com/molenzwiebel/Wetstone/blob/master/API/IRunOnInitialized.cs) for full documentation.
