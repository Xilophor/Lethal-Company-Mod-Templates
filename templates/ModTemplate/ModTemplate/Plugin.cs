using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace ModTemplate;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    public static Plugin Instance { get; private set; }
    internal static new ManualLogSource Logger { get; private set; }
    private static Harmony harmony;

    private void Awake()
    {
        Logger = base.Logger;
        Instance = this;
        harmony = new(MyPluginInfo.PLUGIN_GUID);

        Patch();

        // Plugin startup logic
        Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} is loaded!");
    }

    private void Patch()
    {
        Logger.LogDebug("Patching...");

        harmony.PatchAll();

        Logger.LogDebug("Finished Patching!");
    }
}
