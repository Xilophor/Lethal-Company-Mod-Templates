using BepInEx;
using BepInEx.Logging;
using MonoMod._ModTemplate.Patches;
using HarmonyLib;
#if (UseNetcodePatcher || MMHOOKLocation == "")
using System.Reflection;
#endif
#if (MMHOOKLocation == "")
using System.Collections.Generic;
using MonoMod.RuntimeDetour;
#endif
#if (UseNetcodePatcher)
using UnityEngine;
#endif

namespace MonoMod._ModTemplate;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class MonoMod__ModTemplate : BaseUnityPlugin
{
    public static MonoMod__ModTemplate Instance { get; private set; } = null!;
    internal new static ManualLogSource Logger { get; private set; } = null!;
#if (MMHOOKLocation == "")
    internal static List<IDetour> Hooks { get; set; } = new List<IDetour>();
#endif

    private void Awake()
    {
        Logger = base.Logger;
        Instance = this;

#if (UseNetcodePatcher)
        NetcodePatcher();
#endif
        Hook();

        Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");
    }

    internal static void Hook()
    {
        Logger.LogDebug("Hooking...");

        /*
#if (MMHOOKLocation != "")
         *  Subscribe with 'On.Class.Method += CustomClass.CustomMethod;' for each method you're patching.
#else
         *  Add to the Hooks list for each method you're patching with:
         *
#if (PublicizeGameAssemblies)
         *  Hooks.Add(new Hook(
         *      typeof(Class).GetMethod(nameof(Class.Method), AccessTools.allDeclared),
         *      CustomClass.CustomMethod));
#else
         *  Hooks.Add(new Hook(
         *      typeof(Class).GetMethod("Method", AccessTools.allDeclared),
         *      CustomClass.CustomMethod));
#endif
#endif
         */

#if (MMHOOKLocation != "")
        On.TVScript.SwitchTVLocalClient += ExampleTVPatch.SwitchTVPatch;
#else
        Hooks.Add(new Hook(
#if (PublicizeGameAssemblies)
                typeof(TVScript).GetMethod(nameof(TVScript.SwitchTVLocalClient), AccessTools.allDeclared),
#else
                typeof(TVScript).GetMethod("SwitchTVLocalClient", AccessTools.allDeclared),
#endif
                ExampleTVPatch.SwitchTVPatch));
#endif

        Logger.LogDebug("Finished Hooking!");
    }

    internal static void Unhook()
    {
        Logger.LogDebug("Unhooking...");

#if (MMHOOKLocation != "")
        /*
         *  Unsubscribe with 'On.Class.Method -= CustomClass.CustomMethod;' for each method you're patching.
         */

        On.TVScript.SwitchTVLocalClient -= ExampleTVPatch.SwitchTVPatch;
#else
        foreach (var detour in Hooks)
            detour.Undo();
        Hooks.Clear();
#endif

        Logger.LogDebug("Finished Unhooking!");
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
