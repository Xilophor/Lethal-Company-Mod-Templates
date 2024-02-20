# Lethal Company Mod Template

Thank you for using the mod template! Here are a few tips to help you on your journey:

## Versioning

BepInEx uses [semantic versioning, or semver](https://semver.org/), for the mod's version info.
<!--#if (!UseMinVer) -->
To increment it, you can either modify the version tag in the `.csproj` file directly, or use your IDE's UX to increment the version. Below is an example of modifying the `.csproj` file directly:

```xml
<!-- BepInEx Properties -->
<PropertyGroup>
    <AssemblyName>{ModGuid}</AssemblyName>
    <Product>{Product}</Product>
    <!-- Change to whatever version you're currently on. -->
    <Version>{Version}</Version>
</PropertyGroup>
```

Your IDE will have the setting in `Package` or `NuGet` under `General` or `Metadata`, respectively.
<!--#else -->
[MinVer](https://github.com/adamralph/minver?tab=readme-ov-file#usage) will automatically
version your mod based on the latest git tag, as well as the number of commits made since then.

To create a new git tag, you can either use the git cli, or a git client,
such as GitHub Desktop, GitKraken, or the one built into your IDE.
For command line use, you can simply type in the following commands in the terminal/shell:

```shell
git tag v1.2.3
git push --tags
```

This creates a new tag, `v1.2.3`, at the currently checked-out commit,
and pushes the tag to the git version-control system (vcs).
MinVer will then be able to use this when you build your project to set your mod's version.

> **Note:** You *must* have a `v` in front of the version number, otherwise MinVer will not recognize it.
>
> If you prefer not to have `v1.2.3` and instead `1.2.3`, you can remove the `<MinVerTagPrefix>v</MinVerTagPrefix>` line in your `.csproj` file.
<!--#endif -->

## Logging

A logger is provided to help with logging to the console.
You can access it by doing `Plugin.Logger` in any class outside the `Plugin` class.

***Please use*** `LogDebug()` ***whenever possible, as any other log method
will be displayed to the console and potentially cause performance issues for users.***

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
<!--#if (PublicizeGameAssemblies) -->
`[HarmonyPatch(typeof(ClassToPatch), nameof(ClassToPatch.MethodToPatch))]`
<!--#else -->
`[HarmonyPatch(typeof(ClassToPatch), "MethodToPatch")]`
<!--#endif -->
where `ClassToPatch` is the class you're patching (ie `TVScript`), and `MethodToPatch` is the method you're patching (ie `SwitchTVLocalClient`).

Then you can use
[the appropriate prefix, postfix, transpiler, or finalizer](https://harmony.pardeike.net/articles/patching.html) attribute.

_While you can use_ `return false;` _in a prefix patch,
it is **HIGHLY DISCOURAGED** as it can **AND WILL** cause compatibility issues with other mods._

For example, we want to add a patch that will debug log the current players' position.
We have the following postfix patch patching the `SwitchTVLocalClient` method
in `TVScript`:

```csharp
using HarmonyLib;

namespace Harmony._ModTemplate.Patches;

[HarmonyPatch(typeof(TVScript))]
public class ExampleTVPatch
{
<!--#if (PublicizeGameAssemblies) -->
    [HarmonyPatch(nameof(TVScript.SwitchTVLocalClient))]
<!--#else -->
    [HarmonyPatch("SwitchTVLocalClient")]
<!--#endif -->
    [HarmonyPrefix]
    private static void SwitchTvPrefix(TVScript __instance)
    {
        /*
         *  When the method is called, the TV will be turning off when we want to
         *  turn the lights on and vice-versa. At that time, the TV's tvOn field
         *  will be the opposite of what it's doing, ie it'll be on when turning off.
         *  So, we want to set the lights to what the tv's state was
         *  when this method is called.
         */
        StartOfRound.Instance.shipRoomLights.SetShipLightsBoolean(__instance.tvOn);
    }
}
```

In this case we include the type of the class we're patching in the attribute
before our `ExampleTVPatch` class,
as our class will only patch the `TVScript` class.
