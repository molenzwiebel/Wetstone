using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using Wetstone.API;

namespace Wetstone
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class WetstonePlugin : BasePlugin
    {
#nullable disable
        public static ManualLogSource Logger { get; private set; }
        internal static WetstonePlugin Instance { get; private set; }
#nullable enable

        private ConfigEntry<bool> _enableReloadCommand;
        private ConfigEntry<string> _reloadCommand;
        private ConfigEntry<string> _reloadPluginsFolder;

        public WetstonePlugin() : base()
        {
            WetstonePlugin.Logger = Log;
            Instance = this;

            _enableReloadCommand = Config.Bind("General", "EnableReloadCommand", true, "Enable the reload command.");
            _reloadCommand = Config.Bind("General", "ReloadCommand", "!reload", "The command to reload the plugin.");
            _reloadPluginsFolder = Config.Bind("General", "ReloadPluginsFolder", "BepInEx/WetstonePlugins", "The folder to (re)load plugins from.");
        }

        public override void Load()
        {
            // Hooks
            if (VWorld.IsServer)
            {
                Hooks.Chat.Initialize();
                Hooks.OnInitialize.Initialize();
            }

            if (VWorld.IsClient)
            {
                API.KeybindManager.Load();
                Hooks.Keybindings.Initialize();
            }

            Logger.LogInfo($"Wetstone v{PluginInfo.PLUGIN_VERSION} loaded.");

            // NOTE: MUST BE LAST. This initializes plugins that depend on our state being set up.
            if (VWorld.IsClient || _enableReloadCommand.Value)
            {
                Features.Reload.Initialize(_reloadCommand.Value, _reloadPluginsFolder.Value);
            }
        }

        public override bool Unload()
        {
            // Hooks
            if (VWorld.IsServer)
            {
                Hooks.Chat.Uninitialize();
                Hooks.OnInitialize.Uninitialize();
            }

            if (VWorld.IsClient)
            {
                API.KeybindManager.Save();
                Hooks.Keybindings.Uninitialize();
            }

            return true;
        }
    }
}
