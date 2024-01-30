# Lethal Company Mod Template

Thank you for using the mod template! Here are a few tips to help you on your journey:

## Versioning

BepInEx uses [semantic versioning, or semver](https://semver.org/), for the mod's version info. To increment it, you can either modify the version tag in the `.csproj` file directly, or use your IDE's UX to increment the version. Below is an example of modifying the `.csproj` file directly:

```xml
<PropertyGroup>
    <TargetFramework>netstandard2.1</TargetFramework>
    <AssemblyName>xilophor.ExampleMod</AssemblyName>
    <Product>ExampleMod</Product>
    <Description>An example mod.</Description>
    <!-- Change to whatever version you're currently on. -->
    <Version>1.2.3</Version>
    <!---->
    <AllowUnsafeBlocks>true</AllowUnsafeBlocks>
    <Nullable>enable</Nullable>
    <LangVersion>latest</LangVersion>
</PropertyGroup>
```

Your IDE will have the setting in `Package` or `NuGet` under `General` or `Metadata`, respectively.

## Logging

A logger is provided to help with logging to the console. You can access it by doing `Plugin.Logger` in any class outside the `Plugin` class.

***Please use*** `LogDebug()` ***whenever possible, as any other log method will be displayed to the console and potentially cause performance issues for users.***

If you chose to do so, make sure you change the following line in the `BepInEx.cfg` file to see the Debug messages:

```toml
[Logging.Console]

# ... #

## Which log levels to show in the console output.
# Setting type: LogLevel
# Default value: Fatal, Error, Warning, Message, Info
# Acceptable values: None, Fatal, Error, Warning, Message, Info, Debug, All
# Multiple values can be set at the same time by separating them with , (e.g. Debug, Warning)
LogLevels = All
```

## Harmony

This template uses harmony. For more specifics on how to use it, look at
[the HarmonyX GitHub wiki](https://github.com/BepInEx/HarmonyX/wiki) and
[the Harmony docs](https://harmony.pardeike.net/).

To make a new harmony patch, just use `[HarmonyPatch]` before any class you make that has a patch in it.

Then in that class, you can use
//#if (PublicizeGameAssemblies)
`[HarmonyPatch(typeof(ClassToPatch), nameof(ClassToPatch.MethodToPatch))]`
//#else
`[HarmonyPatch(typeof(ClassToPatch), "MethodToPatch")]`
//#endif
where `ClassToPatch` is the class you're patching (ie `PlayerControllerB`), and `MethodToPatch` is the method you're patching (ie `Update`).

Then you can use
[the appropriate prefix, postfix, transpiler, or finalizer](https://harmony.pardeike.net/articles/patching.html) attribute.

_While you can use_ `return false;` _in a prefix patch,
it is **HIGHLY DISCOURAGED** as it can **AND WILL** cause compatibility issues with other mods._

For example, we want to add a patch that will debug log the current players' position.
We have the following postfix patch patching the `Update` method
in `PlayerControllerB`:

```csharp
using HarmonyLib;

namespace CustomMod.Patches;

[HarmonyPatch(typeof(PlayerControllerB))]
internal class PlayerControllerBPatches
{
//#if (PublicizeGameAssemblies)
    [HarmonyPatch(nameof(PlayerControllerB.Update))]
//#else
    [HarmonyPatch("MethodToPatch")]
//#endif
    [HarmonyPostfix]
    private void UpdatePostfix(PlayerControllerB __instance)
    {
        // Check to make sure the player is the locally controlled player.
        if (__instance == GameNetworkManager.Instance.localPlayerController)
            // Log the controlled player's position
            Logger.LogDebug(__instance.transform.position);
    }
}
```

In this case we include the type of the class we're patching in the attribute
before our `PlayerControllerBPatches` class,
as our class will only patch the `PlayerControllerB` class.
