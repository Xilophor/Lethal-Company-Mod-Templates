using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
#if (UseNetcodePatcher)
using System.Reflection;
using UnityEngine;
#endif
#if (LobbyCompatibility)
using LobbyCompatibility.Attributes;
using LobbyCompatibility.Enums;
#endif

namespace Harmony._ModTemplate;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
#if (LobbyCompatibility)
[BepInDependency("BMX.LobbyCompatibility", BepInDependency.DependencyFlags.HardDependency)]
[LobbyCompatibility(CompatibilityLevel.{CompatibilityLevel}, VersionStrictness.{VersionStrictness})]
#endif
public class Harmony__ModTemplate : BaseUnityPlugin
{
    public static Harmony__ModTemplate Instance { get; private set; } = null!;
    internal new static ManualLogSource Logger { get; private set; } = null!;
    internal static Harmony? Harmony { get; set; }

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

    internal static void Patch()
    {
        Harmony ??= new Harmony(MyPluginInfo.PLUGIN_GUID);

        Logger.LogDebug("Patching...");

        Harmony.PatchAll();

        Logger.LogDebug("Finished patching!");
    }

    internal static void Unpatch()
    {
        Logger.LogDebug("Unpatching...");

        Harmony?.UnpatchSelf();

        Logger.LogDebug("Finished unpatching!");
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
