# Lethal Company Mod Template

Thank you for using the mod template! Here are a few tips to help you on your journey:

## Versioning

BepInEx uses [semantic versioning, or semvar](https://semver.org/), for the mod's version info. To increment it, you can either modify the version tag in the `.csproj` file directly, or use your IDE's UX to increment the version. Below is an example of modifying the `.csproj` file directly:

```xml
<PropertyGroup>
    <TargetFramework>netstandard2.1</TargetFramework>
    <AssemblyName>xilophor.ExampleMod</AssemblyName>
    <Product>ExampleMod</Product>
    <Description>An example mod.</Description>
    <!-- Change 1.2.3 to whatever version you're currently on. -->
    <Version>1.2.3</Version>
    <!---->
    <AllowUnsafeBlocks>true</AllowUnsafeBlocks>
    <Nullable>enable</Nullable>
    <LangVersion>latest</LangVersion>
</PropertyGroup>
```

## Logging

A logger is provided to help with logging to the console. You can access it by doing `Plugin.Logger` in any class outside the `Plugin` class.

***Please use `LogDebug()` whenever possible, as any other log method will be displayed to the console and potentially cause performance issues for users.***
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

