---
layout: default
title: Creating a new mod with Wetstone
nav_order: 1
parent: Getting Started
---

# Creating a new mod with Wetstone

If you'd like to start working on a new mod that uses Wetstone, follow these steps to you get started. 

First, ensure that you've read the [BepInEx plugin development tutorial](https://docs.bepinex.dev/master/articles/dev_guide/plugin_tutorial/index.html). V Rising mods use BepInEx as a method of loading their code into the game, and Wetstone requires BepInEx itself. Also ensure that you have the [.NET SDK installed](https://docs.bepinex.dev/master/articles/dev_guide/plugin_tutorial/1_setup.html#net-sdk), and that you have [installed the BepInEx templates](https://docs.bepinex.dev/master/articles/dev_guide/plugin_tutorial/1_setup.html#installing-bepinex-plugin-templates).

# Creating the project

Lets start by creating a new empty BepInEx IL2CPP project, by executing the following command in the directory where you'd like to start your project (replace your plugin name accordingly):

```shell
$ dotnet new bep6plugin_il2cpp -n MyNewPlugin
```

This will create a new `MyNewPlugin` folder that will contain the source code for your plugin. Go into this folder.

# Adding game dependencies

Before we continue, **make sure that you've installed BepInEx on your local V Rising install** (either client or server, depending on whether you're looking to release a client or server mod) and **have launched the game at least once**. You need to do this to ensure that BepInEx is able to generate the necessary files for mod development.

Now that we have an empty project, we need to add the game files. Open `MyNewPlugin.csproj` in a code editor of choice, and add the following section (making sure to update the path to V Rising with your own):

```xml
<PropertyGroup>
    <UnhollowedDllPath>C:\Path\To\Your\VRising\BepInEx\unhollowed</UnhollowedDllPath>
</PropertyGroup>
```

Now, add the following section at the bottom. This will add some common dependencies from V Rising and Unity so that they're available in your project.

```xml
<ItemGroup>
    <Reference Include="IL2Cppmscorlib">
        <HintPath>$(UnhollowedDllPath)\Il2Cppmscorlib.dll</HintPath>
    </Reference>

    <Reference Include="Il2CppSystem">
        <HintPath>$(UnhollowedDllPath)\Il2CppSystem.dll</HintPath>
    </Reference>
    
    <Reference Include="ProjectM.Shared">
        <HintPath>$(UnhollowedDllPath)\ProjectM.Shared.dll</HintPath>
    </Reference>

    <Reference Include="ProjectM">
        <HintPath>$(UnhollowedDllPath)\ProjectM.dll</HintPath>
    </Reference>

    <Reference Include="Unity.Collections">
        <HintPath>$(UnhollowedDllPath)\Unity.Collections.dll</HintPath>
    </Reference>

    <Reference Include="Unity.Entities">
        <HintPath>$(UnhollowedDllPath)\Unity.Entities.dll</HintPath>
    </Reference>

    <Reference Include="Unity.Entities.Hybrid">
        <HintPath>$(UnhollowedDllPath)\Unity.Entities.Hybrid.dll</HintPath>
    </Reference>

    <Reference Include="Unity.Jobs">
        <HintPath>$(UnhollowedDllPath)\Unity.Jobs.dll</HintPath>
    </Reference>

    <Reference Include="UnityEngine">
        <HintPath>$(UnhollowedDllPath)\UnityEngine.dll</HintPath>
    </Reference>

    <Reference Include="UnityEngine.CoreModule">
        <HintPath>$(UnhollowedDllPath)\UnityEngine.CoreModule.dll</HintPath>
    </Reference>

    <Reference Include="Stunlock.Core">
        <HintPath>$(UnhollowedDllPath)\Stunlock.Core.dll</HintPath>
    </Reference>

    <Reference Include="UnityEngine.InputLegacyModule">
        <HintPath>$(UnhollowedDllPath)\UnityEngine.InputLegacyModule.dll</HintPath>
    </Reference>
</ItemGroup>
```

Note that these are not all the available dependencies. If you need to access some class, function, type etc. defined outside of these, simply add a new `Reference` element. Usually your editor will tell you exactly in which assembly it is located, at which point you can add it:

```xml
<Reference Include="NameOfModule">
    <HintPath>$(UnhollowedDllPath)\NameOfModule.dll</HintPath>
</Reference>
```

# Adding Wetstone

Now that we've added all of the game dependencies, we can add Wetstone itself as a dependency. Simply make sure you're inside the project folder, then run

```shell
$ dotnet add package Wetstone
```

This will install the latest version of Wetstone as a dependency.

Next, we need to tell BepInEx that we depend on Wetstone. This will ensure that BepInEx loads Wetstone before it loads our plugin. Open up `Plugin.cs` and find the `BepInPlugin` attribute. We'll append a new `BepInDependency` for Wetstone:

```csharp
namespace MyNewPlugin
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency("xyz.molenzwiebel.wetstone")] // <- added!
    public class Plugin : BasePlugin
```

That's it! You can now use Wetstone APIs and hooks! Check out the rest of the documentation on this site for more information.

# Building and Running

In order to check that everything works, lets build the project and load it into the game.

First, run the following command to build:

```shell
$ dotnet build
```

This will create a built version of your mod in `bin/Debug/netstandard2.1/MyNewPlugin.dll`. To install the mod, simply copy this `.dll` into your `(game folder)/BepInEx/plugins` directory.

You'll also need to install Wetstone itself, as your plugin depends on it. Download the [latest release](https://v-rising.thunderstore.io/package/molenzwiebel/Wetstone/) and place it in the `plugins` directory, next to your plugin. Finally, launch the game (or the server).

If everything worked correctly, your BepInEx logs (found in `GameFolder/BepInEx/LogOutput.log`) should look something like this:

```
[Info   :   BepInEx] Loading [Wetstone 1.0.0]
...
[Info   :  Wetstone] Wetstone v1.0.0 loaded.
...
[Info   :   BepInEx] Loading [MyNewPlugin 1.0.0]
[Info   :MyNewPlugin] Plugin MyNewPlugin is loaded!
```

Success! Both Wetstone and your plugin are loaded, and BepInEx has made sure that Wetstone has loaded first.

# Making The Mod Reloadable

By default, mods are not reloadable while the game is running. If you want to add reloading support to your mod, follow the instructions [here](./reloading.html).