---
layout: default
title: Creating a new mod with Wetstone
nav_order: 1
parent: Getting Started
---

If you'd like to start working on a new mod that uses Wetstone, follow these steps to you get started. 

First, ensure that you've read the [BepInEx plugin development tutorial](https://docs.bepinex.dev/master/articles/dev_guide/plugin_tutorial/index.html). V Rising mods use BepInEx as a method of loading their code into the game, and Wetstone requires BepInEx itself. Also ensure that you have the [.NET SDK installed](https://docs.bepinex.dev/master/articles/dev_guide/plugin_tutorial/1_setup.html#net-sdk), and that you have [installed the BepInEx templates](https://docs.bepinex.dev/master/articles/dev_guide/plugin_tutorial/1_setup.html#installing-bepinex-plugin-templates).

First, create a new empty BepInEx IL2CPP project, by executing the following command in the directory where you'd like to start your project (replace your plugin name accordingly):

```shell
$ dotnet new bep6plugin_il2cpp -n MyNewPlugin
```

This will create a new `MyNewPlugin` folder that will contain the source code for your plugin.