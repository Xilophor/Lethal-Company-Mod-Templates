# Xilo's LC Mod Templates

Harmony & MonoMod templates to get you a quick-start with making Lethal Company mods!

## Installation

Use the following command in the commandline or terminal to install the templates:

```shell
dotnet new install Xilophor.LCModTemplates
```

The templates will also be updated on occasion. To ensure that your copy of the template is up-to-date, use the following command:

```shell
dotnet new update
```

## Usage

There are two main methods of using the templates. The first is through Visual Studio or Rider, with a relatively intuitive UX.

For a detailed overview of terminal/commandline options, use either `lchmod --help` for the Harmony template, or `lcmmod --help` for the MonoMod template, like so:

```shell
dotnet new lchmod --help
dotnet new lcmmod --help
```

## Contributing

If you've made a library or API that you've posted onto [NuGet](https://nuget.org/) and want it in this template,
either make a PR adding it, or create an Issue requesting it, with the NuGet posting link and the mod's GUID.
