---
layout: default
title: Plugin Reloading
nav_order: 1
parent: Wetstone Features
---

Client & Server
{: .label .label-blue }

# Plugin Reloading

Wetstone offers the ability to hot-reload plugins while the game is running, both on the client and on the server. 

**Reloading is a developer feature and is DISABLED BY DEFAULT.** If you are simply playing with mods, you will likely not need the ability to reload. If you run into any strange issues and are currently using the reloading feature, please make sure that they're not caused by the reloading feature by moving them to the normal plugins folder.

**Plugins must officially support reloading** before they can be reloaded! In order to add reloading support to your plugin, check out the documentation [here](../getting-started/reloading.html).

By default, Wetstone will load plugins from the `BepInEx/WetstonePlugins` folder. This can be configured through the `ReloadablePluginsFolder` configuration option.

To reload plugins:
- On a client, use the **F6** key (reconfigurable through the in-game keybindings settings).
- On a server, send a chat message containing `!reload` as admin (reconfigurable through the `ReloadCommand` configuration option).

---

The reload feature supports the following configuration settings, available in `BepInEx/plugins/xyz.molenzwiebel.wetstone.cfg`:

**Client/Server Options:**
- `EnableReloading` [default `false`]: Whether the reloading feature is enabled.
- `ReloadablePluginsFolder` [default `BepInEx/WetstonePlugins`]: The path to the directory where reloadable plugins should be searched. Relative to the game directory.

**Client Options:**
- Wetstone keybinding can be configured through the in-game settings screen.

**Server Options:**
- `ReloadCommand` [default `!reload`]: Which text command (sent in chat) should be used to trigger reloading of plugins.

# Shortcomings

The method that Wetstone uses to reload plugins is not completely consistent with how BepInEx works, which may cause plugins to break. See [this](../getting-started/reloading.html#shortcomings) section for more information on known shortcomings.