using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.IL2CPP;
using Mono.Cecil;
using UnityEngine;
using Wetstone.API;
using Wetstone.Hooks;

namespace Wetstone.Features;

internal static class Reload
{
#nullable disable
    private static string _reloadCommandChat;
    private static string _reloadCommandRcon;
    private static string _reloadPluginsFolder;
    private static ReloadBehaviour _clientBehavior;
    private static Keybinding _clientReloadKeybinding;
#nullable enable

    private static readonly List<BasePlugin> LoadedPlugins = new();

    internal static void Initialize(string reloadCommandChat,string reloadCommandRcon, string reloadPluginsFolder)
    {
        _reloadCommandChat = reloadCommandChat;
        _reloadCommandRcon = reloadCommandRcon;
        _reloadPluginsFolder = reloadPluginsFolder;

        // note: no need to remove this on unload, since we'll unload the hook itself anyway
        Hooks.Chat.OnChatMessage += HandleReloadChatCommand;
        Hooks.Rcon.OnRconMessage += HandleReloadRconCommand;

        if (VWorld.IsClient)
        {
            _clientReloadKeybinding = KeybindManager.Register(new()
            {
                Id = "xyz.molenzwiebel.wetstone.reload",
                Category = "Wetstone",
                Name = "Reload Plugins",
                DefaultKeybinding = KeyCode.F6,
            });
            _clientBehavior = WetstonePlugin.Instance.AddComponent<ReloadBehaviour>();
        }

        LoadPlugins();
    }

    internal static void Uninitialize()
    {
        Hooks.Chat.OnChatMessage -= HandleReloadChatCommand;
        Hooks.Rcon.OnRconMessage -= HandleReloadRconCommand;

        if (_clientBehavior != null)
        {
            UnityEngine.Object.Destroy(_clientBehavior);
        }
    }

    private static void HandleReloadChatCommand(VChatEvent ev)
    {
        if (ev.Message != _reloadCommandChat) return;
        if (!ev.User.IsAdmin) return; // ignore non-admin reload attempts

        ev.Cancel();

        UnloadPlugins();
        var loaded = LoadPlugins();

        if (loaded.Count > 0)
        {
            ev.User.SendSystemMessage($"Reloaded {string.Join(", ", loaded)}. See console for details.");
        }
        else
        {
            ev.User.SendSystemMessage($"Did not reload any plugins because no reloadable plugins were found. Check the console for more details.");
        }
    }
    private static void HandleReloadRconCommand(VRconEvent ev)
    {
        WetstonePlugin.Logger.LogInfo($"HandleReloadRconCommand: \"{ev.Message}\"");

        if (ev.Message != _reloadCommandRcon) return;

        UnloadPlugins();
        var loaded = LoadPlugins();

        if (loaded.Count > 0)
        {
            WetstonePlugin.Logger.LogInfo($"Reloaded {string.Join(", ", loaded)}. See console for details.");
        }
        else
        {
            WetstonePlugin.Logger.LogInfo($"Did not reload any plugins because no reloadable plugins were found. Check the console for more details.");
        }
    }

    private static void UnloadPlugins()
    {
        for (int i = LoadedPlugins.Count - 1; i >= 0; i--)
        {
            var plugin = LoadedPlugins[i];

            if (!plugin.Unload())
            {
                WetstonePlugin.Logger.LogWarning($"Plugin {plugin.GetType().FullName} does not support unloading, skipping...");
            }
            else
            {
                LoadedPlugins.RemoveAt(i);
            }
        }
    }

    private static List<string> LoadPlugins()
    {
        if (!Directory.Exists(_reloadPluginsFolder)) return new();

        return Directory.GetFiles(_reloadPluginsFolder, "*.dll", SearchOption.AllDirectories).SelectMany(LoadPlugin).ToList();
    }

    private static List<string> LoadPlugin(string path)
    {
        var defaultResolver = new DefaultAssemblyResolver();
        defaultResolver.AddSearchDirectory(_reloadPluginsFolder);
        defaultResolver.AddSearchDirectory(Paths.ManagedPath);
        defaultResolver.AddSearchDirectory(Paths.BepInExAssemblyDirectory);

        using var dll = AssemblyDefinition.ReadAssembly(path, new() { AssemblyResolver = defaultResolver });
        dll.Name.Name = $"{dll.Name.Name}-{DateTime.Now.Ticks}";

        using var ms = new MemoryStream();
        dll.Write(ms);

        var loaded = new List<string>();

        var assembly = Assembly.Load(ms.ToArray());
        foreach (var pluginType in assembly.GetTypes().Where(x => typeof(BasePlugin).IsAssignableFrom(x)))
        {
            // skip plugins not marked as reloadable
            if (!pluginType.GetCustomAttributes<ReloadableAttribute>().Any())
            {
                WetstonePlugin.Logger.LogWarning($"Plugin {pluginType.FullName} is not marked as reloadable, skipping...");
                continue;
            }

            // skip plugins already loaded
            if (LoadedPlugins.Any(x => x.GetType() == pluginType)) continue;

            try
            {
                // we skip chainloader here and don't check dependencies. Fast n dirty.
                var plugin = (BasePlugin)Activator.CreateInstance(pluginType);
                var metadata = MetadataHelper.GetMetadata(plugin);
                LoadedPlugins.Add(plugin);
                plugin.Load();
                loaded.Add(metadata.Name);

                // ensure initialize hook runs even if we reload far after initialization is already done
                if (Hooks.OnInitialize.HasInitialized && plugin is IRunOnInitialized runOnInitialized)
                {
                    runOnInitialized.OnGameInitialized();
                }

                WetstonePlugin.Logger.LogInfo($"Loaded plugin {pluginType.FullName}");
            }
            catch (Exception ex)
            {
                WetstonePlugin.Logger.LogError($"Plugin {pluginType.FullName} threw an exception during initialization:");
                WetstonePlugin.Logger.LogError(ex);
            }
        }

        return loaded;
    }

    private class ReloadBehaviour : UnityEngine.MonoBehaviour
    {
        private void Update()
        {
            if (_clientReloadKeybinding.IsPressed)
            {
                WetstonePlugin.Logger.LogInfo("Reloading client plugins...");

                UnloadPlugins();
                LoadPlugins();
            }
        }
    }
}