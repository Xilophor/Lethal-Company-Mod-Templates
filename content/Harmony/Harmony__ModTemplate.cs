using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
#if (UseNetcodePatcher)
using System;
using System.Reflection;
#endif

namespace Harmomy._ModTemplate;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Harmony__ModTemplate : BaseUnityPlugin
{
    public static Plugin Instance { get; private set; } = null!;
    internal new static ManualLogSource Logger { get; private set; } = null!;
    private static Harmony harmony = null!;

    private void Awake()
    {
        Logger = base.Logger;
        Instance = this;

#if (UseNetcodePatcher)
        NetcodePatcher();
#endif
        Patch();

        Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");
    }

    private void Patch()
    {
        harmony ??= new Harmony(MyPluginInfo.PLUGIN_GUID);

        Logger.LogDebug("Patching...");

        harmony.PatchAll();

        Logger.LogDebug("Finished Patching!");
    }

    private void Unpatch()
    {
        Logger.LogDebug("Unpatching...");

        harmony.UnpatchSelf();

        Logger.LogDebug("Finished Unpatching!");
    }
#if (UseNetcodePatcher)

    private void NetcodePatcher()
    {
        var types = Assembly.GetExecutingAssembly().GetTypes();
        foreach (var type in types)
        {
            var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(RuntimeInitializeOnLoadMethodAttribute), false);
                if (attributes.Length > 0)
                {
                    method.Invoke(null, null);
                }
            }
        }
    }
#endif
}
