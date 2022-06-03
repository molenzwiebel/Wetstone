---
layout: default
title: Keybindings
nav_order: 1
parent: Wetstone APIs
---

Client-Only
{: .label .label-green }

# Keybinding APIs

Wetstone offers an easy to use API for keybinding configurations on client mods. By simply registering a new keybinding, Wetstone will ensure that it gets persisted and that shows up in the in-client keybindings menu.

To add a new keybinding configuration, invoke the `Register` method on `Wetstone.API.KeybindManager`:

```csharp
var myKeybinding = KeybindManager.Register(new()
{
    Id = "me.mymod.mykeybinding",
    Category = "My Mod",
    Name = "My Keybinding",
    DefaultKeybinding = KeyCode.A,
});
```

The following fields are required:
- `Id`: A unique identifier for this specific keybinding. The ID will be used to identify the keybinding in storage, so it should be unique and constant across versions. You are recommended to prefix it with your plugin name/id,so that it cannot conflict with other plugins.
- `Category`: The title of the category in which this keybind will appear. Multiple keybinds with the same category will appear in the same category. It is recommended to use a single category for all your plugin's keybindings. If you use multiple, prefix them with the name of your plugin so that users know which plugin a keybinding belongs to.
- `Name`: The name of this keybind, as shown in the settings menu.
- `DefaultKeybinding`: The default primary key for this keybinding. If you want the keybinding to be disabled by default, set it to `KeyCode.None`.

The configuration above leads looks like this in the settings screen:

![](https://i.imgur.com/iB3dyKE.png)

You can use the resulting `Keybinding` instance to retrieve the currently bound keys and to check whether the key is currently down:

```csharp
Logger.LogInfo("My Keybinding is bound to " + myKeybinding.Primary);

if (UnityEngine.Input.GetKeyDown(myKeybinding.Primary) || UnityEngine.Input.GetKeyDown(myKeybinding.Secondary))
{
    // do something...
}

// or, shorter

if (myKeybinding.IsPressed)
{
    // do something...
}
```

If your plugin [supports reloading](../getting-started/reloading.html), make sure to `Unregister` your keybinding when you unload your plugin:

```csharp
public override bool Unload()
{
    KeybindManager.Unregister(myKeybinding);

    return true;
}
```

Unregistering your keys will ensure that they no longer show up in the controls settings when your plugin is not loaded. Unregistering will not remove the saved keybinds for the user.

---

For more information on the Keybinding API, see the [API/Keybindings.cs](https://github.com/molenzwiebel/Wetstone/blob/master/API/Keybindings.cs).