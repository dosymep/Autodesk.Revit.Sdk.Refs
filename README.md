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
        <Configurations>Debug;Release;D2024;D2025;D2026;D2026.5;D2027</Configurations>
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

### How to Build & Run the Sample

The sample project under `samples/SamplePlugin` relies on the locally built NuGet packages. To compile and run the
sample, you first need to pack the SDK and the version packages into the `artifacts` directory so that NuGet can restore
them locally.

#### 1. Pack the Solution

Run `dotnet pack` on the solution, specifying the output path as `artifacts`:

```bash
dotnet pack Autodesk.Revit.Sdk.Refs.slnx --output artifacts
```

This will generate the `.nupkg` files (e.g. `Autodesk.Revit.Sdk.Refs.2.0.0.nupkg` and all versioned reference packages)
in the `artifacts/` folder. The sample's `nuget.config` is configured to read packages from this directory as a local
source.

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

## How to add new version Autodesk Revit

Copy libs

```csharp

string version = "2027";
string oldVersion = "2026";

string target = "lib";
string source = Path.Combine(target, oldVersion);
string originals = @"C:\Program Files\Autodesk\Revit " + version;

foreach (string enumerateFile in Directory.EnumerateFiles(source))
{
    string fileName = Path.GetFileName(enumerateFile);
    string targetFile = Path.Combine(target, version, fileName);
    string originalFile = Path.Combine(originals, fileName);

    if (File.Exists(originalFile))
    {
        Console.WriteLine($"Copying: {originalFile} -> {targetFile}");

        Directory.CreateDirectory(Path.Combine(target, version));
        File.Copy(originalFile, targetFile);
    }
    else
    {
        Console.WriteLine($"Original file not found: {originalFile}");
    }
}

```

```python
import os
import shutil

version = "2027"
old_version = "2026"

target = "lib"
source = os.path.join(target, old_version)
originals = os.path.join(r"C:\Program Files\Autodesk", f"Revit {version}")

for enumerate_file in os.listdir(source):
    file_name = enumerate_file
    target_file = os.path.join(target, version, file_name)
    original_file = os.path.join(originals, file_name)

    if os.path.exists(original_file):
        print(f"Copying: {original_file} -> {target_file}")
        os.makedirs(os.path.join(target, version), exist_ok=True)
        shutil.copy(original_file, target_file)
    else:
        print(f"Original file not found: {original_file}")
```

Install [Refasmer](https://github.com/JetBrains/Refasmer) and run on refs folder:

```
refasmer -m -i -w --omit-non-api-members=true AdWindows.dll
refasmer -r -i -w --omit-non-api-members=true PackageContentsParser.dll
refasmer -r -i -w --omit-non-api-members=true RevitAddInUtility.dll
refasmer -r -i -w --omit-non-api-members=true RevitAPI.dll
refasmer -r -i -w --omit-non-api-members=true RevitAPIBrowserUtils.dll
refasmer -r -i -w --omit-non-api-members=true RevitAPIIFC.dll
refasmer -r -i -w --omit-non-api-members=true RevitAPIMacros.dll
refasmer -r -i -w --omit-non-api-members=true RevitAPIUI.dll
refasmer -r -i -w --omit-non-api-members=true RevitAPIUIMacros.dll
refasmer -r -i -w --omit-non-api-members=true RevitNET.dll
refasmer -m -i -w --omit-non-api-members=true UIFramework.dll
```