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
        private ConfigEntry<string> _reloadCommandChat;
        private ConfigEntry<string> _reloadCommandRcon;
        private ConfigEntry<string> _reloadPluginsFolder;

        public WetstonePlugin() : base()
        {
            WetstonePlugin.Logger = Log;
            Instance = this;

            _enableReloadCommand = Config.Bind("General", "EnableReloading", true, "Whether to enable the reloading feature (both client and server).");
            _reloadCommandChat = Config.Bind("General", "ReloadCommandChat", "!plugins.reload *", "Server chat command to reload plugins. User must be an admin.");
            _reloadCommandRcon = Config.Bind("General", "ReloadCommandRcon", "plugins.reload *", "Server rcon command to reload plugins. The user must be authorized for the server to execute the commands.");
            _reloadPluginsFolder = Config.Bind("General", "ReloadablePluginsFolder", "BepInEx/WetstonePlugins", "The folder to (re)load plugins from, relative to the game directory.");
        }

        public override void Load()
        {
            // Hooks
            if (VWorld.IsServer)
            {
                Hooks.Rcon.Initialize();
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
                Features.Reload.Initialize(_reloadCommandChat.Value, _reloadCommandRcon.Value, _reloadPluginsFolder.Value);
            }
        }

        public override bool Unload()
        {
            // Hooks
            if (VWorld.IsServer)
            {
                Hooks.Rcon.Uninitialize();
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
