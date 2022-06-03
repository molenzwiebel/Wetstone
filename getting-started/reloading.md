---
layout: default
title: Making your mod reloadable
nav_order: 3
parent: Getting Started
---

# Making your mod reloadable

One of Wetstone's features is the ability to reload mods without restarting the game, both on the client and on the server. For more information on this feature, see [the feature documentation for reloading](../features/reloading.html). To add reloading support to your mod, you must make a few changes:

First, add the `Wetstone.API.Reloadable` attribute to your plugin class. This indicates to Wetstone that your plugin supports reloading:

```csharp
[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("xyz.molenzwiebel.wetstone")]
[Wetstone.API.Reloadable] // <- added!
public class Plugin : BasePlugin
```

**Wetstone will refuse to load any plugins not marked as reloadable!** Reloading plugins can be quite fragile and easily leave the game in a broken state, so you must add this attribute to indicate that you know what you're doing.

Secondly, make sure to override the `Unload` function from `BasePlugin` in your plugin class. Wetstone will call this method when it reloads your plugin, and you will need to make sure that you undo any hooks and listeners in this method to ensure clean reloads. Returning `false` from `Unload` (the default, if you don't override it), will prevent Wetstone from unloading and reloading your plugin, as it will assume that something went wrong during unloading.

The following is an example on how you might want to structure your mod:

```csharp
[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("xyz.molenzwiebel.wetstone")]
[Wetstone.API.Reloadable]
public class Plugin : BasePlugin
{
    private Harmony _myHook;
    private Component _myInjectedComponent;

    public override void Load()
    {
        _myHook = Harmony.CreateAndPatchAll(typeof(MyHooks));
        _myInjectedComponent = AddComponent<MyInjectedComponent>();
    }

    public override bool Unload()
    {
        _myHook.UnpatchSelf(); // make sure to clean up!
        UnityEngine.Object.Destroy(_myInjectedComponent); // and to remove any components you added!

        return true;
    }
}
```

Finally, instead of placing your mod `.dll` in the `BepInEx/plugins` folder, place it in the `BepInEx/WetstonePlugins` folder. Plugins in this folder will be automatically loaded during startup, and reloaded when triggered (by default `F6` on client and `!reload` on server).

# Shortcomings

There are several known issues/shortcomings with the reload feature that you must consider:

- Wetstone may not (re-)run your module constructor.
- Wetstone does not respect dependency requirements. It will unconditionally load your mod, and may load it before a dependency.
- Wetstone cannot forcibly unload the old instance. You **must** clean up everything in your `Unload` method, or you may be left with multiple versions of the plugin running concurrently.
- IL2CPP has issues with re-injecting managed classes into the native domain (most commonly when you use `AddComponent<T>` in your plugin).