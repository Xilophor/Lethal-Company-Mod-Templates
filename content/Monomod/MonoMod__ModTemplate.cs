using BepInEx;
using BepInEx.Logging;
using MonoMod.RuntimeDetour;
#if (UseNetcodePatcher)
using System;
using System.Reflection;
#endif

namespace MonoMod._ModTemplate;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
#if (NuGetPackages == ConfigurableCompany)
[BepInDependency(LethalConfiguration.PLUGIN_GUID, BepInDependency.DependencyFlags.HardDependency)]
#endif
#if (NuGetPackages == CSync)
[BepInDependency("io.github.CSync", BepInDependency.DependencyFlags.HardDependency)]
#endif
#if (NuGetPackages == LethalLib)
[BepInDependency("evaisa.lethallib", BepInDependency.DependencyFlags.HardDependency)]
#endif
#if (NuGetPackages == LethalNetworkAPI)
[BepInDependency("LethalNetworkAPI", BepInDependency.DependencyFlags.HardDependency)]
#endif
#if (NuGetPackages == LethalSettings)
[BepInDependency("com.willis.lc.lethalsettings", BepInDependency.DependencyFlags.HardDependency)]
#endif
#if (NuGetPackages == TerminalAPI)
[BepInDependency("atomic.terminalapi", BepInDependency.DependencyFlags.HardDependency)]
#endif
public class MonoMod__ModTemplate : BaseUnityPlugin
{
    public static MonoMod__ModTemplate Instance { get; private set; } = null!;
    internal new static ManualLogSource Logger { get; private set; } = null!;

    // If you use the method of hooking shown in the README, add to this list; otherwise ignore or remove this list.
    internal static readonly List<IDetour> Hooks { get; set; } = new List<IDetour>();

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
         *  Subscribe with 'On.Class.Method += CustomClass.CustomMethod;' for each method you're patching
         *  or add to the list for each method you're patching with:
         *
         *  Hooks.Add(
#if (PublicizeGameAssemblies)
         *      new Hook(typeof(Class).GetMethod(nameof(Class.Method)), CustomClass.CustomMethod);
#else
         *      new Hook(typeof(Class).GetMethod("Method"), CustomClass.CustomMethod);
#end
         */

        Logger.LogDebug("Finished Hooking!");
    }

    internal static void Unhook()
    {
        Logger.LogDebug("Unhooking...");

        /*
         *  Unsubscribe with 'On.Class.Method -= CustomClass.CustomMethod;' for each method you're patching
         *  or remove from the list with:
         *
         *  foreach (var detour in Hooks)
         *      detour.Undo();
         *  Hooks.Clear();
         */

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
