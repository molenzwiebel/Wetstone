---
layout: default
title: Developing Wetstone
nav_order: 5
---

# Developing Wetstone

If you want to contribute to Wetstone, follow these steps to get started.

## Plugin Setup

1. Clone the repository and setup dependencies
```shell
$ git clone https://github.com/molenzwiebel/Wetstone
$ cd Wetstone
$ dotnet restore
```

2. Adjust the `UnhollowedDllPath` in `Wetstone.csproj` to point to your installation of VRising.
    - Make sure you have installed [BepInEx](https://v-rising.thunderstore.io/package/BepInEx/BepInExPack_V_Rising/) in your game.
    - Run the game at least once to make BepInEx generate unhollowed assemblies.
    - You should now have a directory in `{GameDir}/BepInEx/unhollowed`. Use the full path to this folder as `UnhollowedDllPath`.

3. Build the plugin
```shell
$ dotnet build
```

## Documentation Setup

This Wetstone documentation lives on the `github-pages` branch as a Jekyll project. In order to develop it, simply clone the repository and switch branches:

```shell
$ git clone https://github.com/molenzwiebel/Wetstone
$ cd Wetstone
$ git checkout github-pages
```

If you'd like to preview your changes locally, you can use the following Docker command:

```shell
$ docker run --rm --volume="$(PWD):/srv/jekyll:Z" -p 4000:4000 -it jekyll/jekyll:4.2.2 jekyll serve --force_polling
```

Open [http://localhost:4000/Wetstone](http://localhost:4000/Wetstone) in your browser after it finishes building.