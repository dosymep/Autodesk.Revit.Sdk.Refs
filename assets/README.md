# Autodesk.Revit.SDK.Refs

[![JetBrains Rider](https://img.shields.io/badge/JetBrains-Rider-blue.svg)](https://www.jetbrains.com/rider)
[![License MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://en.wikipedia.org/wiki/MIT_License)
[![Revit 2016-2027](https://img.shields.io/badge/Revit-2016--2027-blue.svg)](https://www.autodesk.com/products/revit/overview)

Revit SDK refs assemblies.  
Only metadata from assemblies by [Refasmer](https://github.com/JetBrains/Refasmer).

## Usage

You can use the custom **MSBuild SDK** `Autodesk.Revit.Sdk.Refs`. Just reference it in your project's `<Project>` tag:

```xml
<Project Sdk="Autodesk.Revit.Sdk.Refs/2.0.0">

    <PropertyGroup>
        <UseWpf>true</UseWpf>
        <Platforms>x64</Platforms>
        <OutputType>Library</OutputType>
        <RootNamespace>SamplePlugin</RootNamespace>

        <!-- Optional: set to true to print build info during compilations -->
        <ShowBuildInfo>true</ShowBuildInfo>
        
        <!-- Define configurations you want to use -->
        <Configurations>Debug;Release;D2016;D2017;D2018;D2019;D2020;D2021;D2022;D2023;D2024;D2025;D2026;D2027</Configurations>
    </PropertyGroup>

</Project>
```

The MSBuild SDK automatically handles:
- Correct target framework selection based on `RevitVersion` (e.g. `net48` for versions prior to 2025, `net8.0-windows` for 2025-2026, and `net10.0-windows` for 2027).
- Implicit references to `Autodesk.Revit.Sdk.Refs` and the specific versioned package `Autodesk.Revit.Sdk.Refs.$(RevitVersion)`.
- Setting properties like `RevitPath`, `RevitExePath`, `RevitAddinsPath`, `RevitAddinsUserPath`, and `RevitApplicationsPath`.
- Generating Revit version compilation constants automatically.

##### nuke build

```csharp
DotNetBuild(s => s
    .DisableNoRestore()
    .SetProjectFile(<ProjectName>)
    .SetConfiguration(<Configuration>)
    .SetProperty("RevitVersion", (int) <RevitVersion>));
```

## Defined constants

This constants defined to all supports revit version.

```
REVIT<RevitVersion>
REVIT<RevitVersion>_OR_GREATER
```

### Usage defined constants

```csharp
#if REVIT2024
    // This code will be available for Revit 2024
#endif

#if REVIT2025_OR_GREATER
     // This code will be available for Revit 2025 and newer
#endif
```