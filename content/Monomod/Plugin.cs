using BepInEx;
using BepInEx.Logging;
using MonoMod.RuntimeDetour;
#if (UseNetcodePatcher)
using System;
using System.Reflection;
#endif

namespace ModTemplate;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    public static Plugin Instance { get; private set; } = null!;
    internal static new ManualLogSource Logger { get; private set; } = null!;

    // If you use the method of hooking shown in the README, add to this list; otherwise ignore or remove this list.
    internal static readonly List<IDetour> Hooks { get; } = new List<IDetour>();

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

    private void Hook()
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

    private void Unhook()
    {
        Logger.LogDebug("Unhooking...");

        /*
         *  Unsubscribe with 'On.Class.Method -= CustomClass.CustomMethod;' for each method you're patching
         *  or remove from the list with:
         *
         *  foreach (var detour in Hooks)
         *      detour.Undo();
         *
         * Hooks.Clear();
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
