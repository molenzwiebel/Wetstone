# Wetstone - V Rising Mod API

Wetstone is a modding library for both client and server mods for V Rising. By itself, it does not do much except allow you to reload plugins you've put in the Wetstone plugins folder.

### Installation

- Install [BepInEx](https://v-rising.thunderstore.io/package/BepInEx/BepInExPack_V_Rising/).
- Extract _Wetstone.dll_ into _`(VRising folder)/BepInEx/plugins`_.
- Optional: extract any reloadable additional plugins into _`(VRising folder)/BepInEx/WetstonePlugins`_.

### Configuration

Wetstone supports the following configuration settings, available in `BepInEx/config/xyz.molenzwiebel.wetstone.cfg`.

**Client/Server Options:**
- `EnableReloading` [default `true`]: Whether the reloading feature is enabled.
- `ReloadablePluginsFolder` [default `BepInEx/WetstonePlugins`]: The path to the directory where reloadable plugins should be searched. Relative to the game directory.

**Client Options:**
- Wetstone keybinding can be configured through the in-game settings screen.

**Server Options:**
- `ReloadCommand` [default `!reload`]: Which text command (sent in chat) should be used to trigger reloading of plugins.

### Support

Join the [modding community](https://discord.gg/CWzkHvekg3), and ping `@molenzwiebel#2773`.

Post an issue on the [GitHub repository](https://github.com/molenzwiebel/Wetstone). 

### Changelog

- **1.0.0** Initial release