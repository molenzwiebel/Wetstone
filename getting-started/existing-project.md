---
layout: default
title: Adding Wetstone to an existing project
nav_order: 2
parent: Getting Started
---

# Adding Wetstone to an existing project

If you already have an existing mod project for V Rising, adding Wetstone is a breeze.

First, add Wetstone as a NuGet dependency:

```shell
$ dotnet add package Wetstone
```

Secondly, add a `BepInDependency` for Wetstone to your plugin class:

```
[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("xyz.molenzwiebel.wetstone")] // <- added!
public class Plugin : BasePlugin
```

Done! You are now able to use Wetstone [APIs](../apis/) and [hooks](../hooks/).

# Making The Mod Reloadable

Wetstone has built-in support for automatically reloading plugins. In order for a plugin to be reloadable, you must explicitly add support to your mod. To do so, follow the instructions [here](./reloading.html).