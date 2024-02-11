# Lethal Company Mod Template

Thank you for using the mod template! Here are a few tips to help you on your journey:

## Versioning

BepInEx uses [semantic versioning, or semver](https://semver.org/), for the mod's version info.
//#if (!UseMinVer)
To increment it, you can either modify the version tag in the `.csproj` file directly, or use your IDE's UX to increment the version. Below is an example of modifying the `.csproj` file directly:

```xml
<!-- BepInEx Properties -->
<PropertyGroup>
    <AssemblyName>{ModGuid}</AssemblyName>
    <Product>MonoMod.ModTemplate</Product>
    <!-- Change to whatever version you're currently on. -->
    <Version>{Version}</Version>
</PropertyGroup>
```

Your IDE will have the setting in `Package` or `NuGet` under `General` or `Metadata`, respectively.
//#else
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
//#endif

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

