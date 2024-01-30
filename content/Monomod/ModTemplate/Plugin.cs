using BepInEx;
using BepInEx.Logging;

namespace ModTemplate;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    public static Plugin Instance { get; private set; }
    internal static new ManualLogSource Logger { get; private set; }

    // If you use the method of hooking shown in the README, add to this list; otherwise ignore or remove this list.
    internal static readonly List<IDetour> Hooks { get; } = new List<IDetour>();

    private void Awake()
    {
        Logger = base.Logger;
        Instance = this;

        Hook();

        Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");
    }

    private void Hook()
    {
        Logger.LogDebug("Hooking...");

        /*
         *  Subscribe with 'On.Class.Method += CustomClass.CustomMethod;' for each method you're patching
         *  or add to the list for each method you're patching with:
         *
         *  Hooks.Add(
         *      new Hook(typeof(Class).GetMethod(Class.Method), CustomClass.CustomMethod);
         */

        Logger.LogDebug("Finished Hooking!");
    }

    private void Unhook()
    {
        Logger.LogDebug("Unhooking...");

        /*
         *  Unsubscribe with 'On.Class.Method -= CustomClass.CustomMethod;' for each method you're patching
         *  or remove from the list with:
         *
         *  foreach (var detour in Hooks)
         *      detour.Undo();
         */

        Logger.LogDebug("Finished Unhooking!");
    }
}
