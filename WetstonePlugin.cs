using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using Wetstone.API;

namespace Wetstone
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
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

            _enableReloadCommand = Config.Bind("General", "EnableReloading", true, "Whether to enable the reloading feature (both client and server).");
            _reloadCommand = Config.Bind("General", "ReloadCommand", "!reload", "Server text command to reload plugins. User must be an admin.");
            _reloadPluginsFolder = Config.Bind("General", "ReloadablePluginsFolder", "BepInEx/WetstonePlugins", "The folder to (re)load plugins from, relative to the game directory.");
        }

        public override void Load()
        {
            // Hooks
            if (VWorld.IsServer)
            {
                Hooks.Chat.Initialize();
            }

            if (VWorld.IsClient)
            {
                API.KeybindManager.Load();
                Hooks.Keybindings.Initialize();
            }

            Hooks.OnInitialize.Initialize();
            Hooks.GameFrame.Initialize();
            Network.SerializationHooks.Initialize();

            Logger.LogInfo($"Wetstone v{MyPluginInfo.PLUGIN_VERSION} loaded.");

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
            }

            if (VWorld.IsClient)
            {
                API.KeybindManager.Save();
                Hooks.Keybindings.Uninitialize();
            }

            Hooks.OnInitialize.Uninitialize();
            Hooks.GameFrame.Uninitialize();
            Network.SerializationHooks.Uninitialize();

            return true;
        }
    }
}
