# Autodesk.Revit.SDK.Refs

[![JetBrains Rider](https://img.shields.io/badge/JetBrains-Rider-blue.svg)](https://www.jetbrains.com/rider)
[![License MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE.md)
[![Revit 2016-2027](https://img.shields.io/badge/Revit-2016--2027-blue.svg)](https://www.autodesk.com/products/revit/overview)

Revit SDK refs assemblies.  
Only metadata from assemblies by [Refasmer](https://github.com/JetBrains/Refasmer).

## Usage

You can use the custom **MSBuild SDK** `Autodesk.Revit.Sdk.Refs`. Just reference it in your project's `<Project>` tag.

### Versioning Policy

We strictly follow SemVer:

- **Minor version** increases (e.g. `2.1.0`) when new Revit reference libraries/versions are added.
- **Patch version** increases (e.g. `2.0.1`) when minor fixes or internal SDK improvements are released.
- **Major version** increases (e.g. `3.0.0`) only when there are breaking changes in the MSBuild SDK's public contract,
  which might require manual migration.

```xml

<Project Sdk="Autodesk.Revit.Sdk.Refs/2.0.0">

    <PropertyGroup>
        <UseWpf>true</UseWpf>
        <Platforms>x64</Platforms>
        <OutputType>Library</OutputType>

        <!-- Optional: set to true to print build info during compilations -->
        <ShowBuildInfo>true</ShowBuildInfo>

        <!-- Define configurations you want to use -->
        <Configurations>Debug;Release;D2024;D2025;D2026;В2026.5;D2027</Configurations>
    </PropertyGroup>

</Project>
```

The MSBuild SDK automatically handles:

- **Target Framework resolution** based on `RevitVersion` (e.g. `net48` for versions prior to 2025, `net8.0-windows` for 2025-2026, and `net10.0-windows` for 2027).
- **Implicit references** to `Autodesk.Revit.Sdk.Refs` and version-specific `Autodesk.Revit.Sdk.Refs.$(RevitVersion)` reference assemblies packages.
- **Revit compilation constants** auto-definition (e.g. `REVIT2024`, `REVIT2024_OR_GREATER`).

### Frameworks & Revit Versions

| Revit Version | Target Framework  |
|---------------|-------------------|
| 2016 - 2024   | `net48`           |
| 2025          | `net8.0-windows`  |
| 2026          | `net8.0-windows`  |
| 2026.5        | `net10.0-windows` |
| 2027          | `net10.0-windows` |

### Automatically Defined Revit Properties

| Property Name           | Value / Path                                                                                    | Description                                                      |
|-------------------------|-------------------------------------------------------------------------------------------------|------------------------------------------------------------------|
| `RevitPath`             | `$(ProgramFiles)\Autodesk\Revit $(RevitVersion)` *(or `...Revit MEP $(RevitVersion)` for 2016)* | Path to the Revit installation directory.                        |
| `RevitExePath`          | `$(RevitPath)\Revit.exe`                                                                        | Path to the `Revit.exe` executable.                              |
| `RevitAddinsPath`       | `$(ProgramData)\Autodesk\Revit\Addins\$(RevitVersion)`                                          | Path to the machine-wide (all users) Revit Addins directory.     |
| `RevitAddinsUserPath`   | `$(AppData)\Autodesk\Revit\Addins\$(RevitVersion)`                                              | Path to the user-specific (current user) Revit Addins directory. |
| `RevitApplicationsPath` | `$(ProgramData)\Autodesk\ApplicationPlugins`                                                    | Path to Autodesk Application Plugins directory.                  |

### Optional SDK Properties

| Property Name            | Default Value | Description                                                                                                                                                                                                                   |
|--------------------------|---------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `ShowBuildInfo`          | `false`       | Set to `true` to print diagnostic build details during compilation.                                                                                                                                                           |
| `IncludeEmbeddedLibrary` | `true`        | When `true`, the SDK implicitly references the `Autodesk.Revit.Sdk.Refs.$(RevitVersion)` package containing reference assemblies. Set to `false` if you want to prevent automatic referencing and provide libraries manually. |

## Migration from v1 to v2

In version 1.x, you had to manually specify target frameworks, default properties, and reference two separate NuGet
packages in every project:

```xml
<!-- v1.x Style (Old) -->
<Project Sdk="Microsoft.NET.Sdk">
    <PropertyGroup>
        <RevitVersion Condition="'$(RevitVersion)' == ''">2016</RevitVersion>
        <TargetFramework Condition="'$(TargetFramework)' == ''">net48</TargetFramework>
    </PropertyGroup>
    <ItemGroup>
        <PackageReference Include="Autodesk.Revit.Sdk.Refs" Version="1.*"/>
        <PackageReference Include="Autodesk.Revit.Sdk.Refs.$(RevitVersion)" Version="1.*"/>
    </ItemGroup>
</Project>
```

In version 2.x, the repository introduces a custom **MSBuild SDK** that simplifies this process significantly.

### Migration Steps:

1. **Update `<Project>` Tag:** Replace your standard `<Project Sdk="Microsoft.NET.Sdk">` (or similar) with the MSBuild
   SDK reference:
   ```xml
   <Project Sdk="Autodesk.Revit.Sdk.Refs/2.0.0">
       <!-- some properties -->
   </Project>
   ```
2. **Remove Manual Package References:** Remove the `PackageReference` tags for `Autodesk.Revit.Sdk.Refs` and
   `Autodesk.Revit.Sdk.Refs.$(RevitVersion)` from your `.csproj` files. They are now implicitly referenced by the
   MSBuild SDK.
3. **Remove Redundant Properties:** You can remove manual definitions of `TargetFramework`, `RevitPath`, `RevitExePath`,
   etc., unless you need to override them. The SDK handles target framework resolution (`net48`, `net8.0-windows`,
   `net10.0-windows`) and path definitions automatically based on the `RevitVersion`.

#### 2. Build Revit Project

Now you can compile your project for your desired configuration/Revit version:

##### dotnet cli

```bash
dotnet build <ProjectName>.csproj -c <Configuration> -p:RevitVersion=<RevitVersion>
```

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