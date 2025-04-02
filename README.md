# Autodesk.Revit.SDK.Refs

[![JetBrains Rider](https://img.shields.io/badge/JetBrains-Rider-blue.svg)](https://www.jetbrains.com/rider)
[![License MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE.md)
[![Revit 2016-2026](https://img.shields.io/badge/Revit-2016--2026-blue.svg)](https://www.autodesk.com/products/revit/overview)

Revit SDK refs assemblies.  
Only metadata from assemblies by [Refasmer](https://github.com/JetBrains/Refasmer).

## Usage

Add project configurations with revit versions in name

```xml
<PropertyGroup>
    <RevitVersion Condition="'$(RevitVersion)' == ''">2016</RevitVersion> <!-- set default revit version -->
    <TargetFramework Condition="'$(TargetFramework)' == ''">net45</TargetFramework> <!-- set default target framework -->
    <Configurations>Debug;Release;D2016;D2017;D2018;D2019;D2020;D2021;D2022;D2023;D2024;D2025;D2026</Configurations>
</PropertyGroup>
```

Add package reference

```xml
<ItemGroup>
    <!-- package reference with revit dlls -->
    <PackageReference Include="Autodesk.Revit.Sdk.Refs" Version="*" />
    <PackageReference Include="Autodesk.Revit.Sdk.Refs.$(RevitVersion)" Version="*" />
</ItemGroup>
```

### Build Revit Project

See sample project in this [folder](sample/SamplePlugin).  
You should compile Debug or Release configuration.  
Other configurations needs to help write code with constants.

#### dotnet cli

```
dotnet build -c <Configuration> -p:RevitVersion=<RevitVersion>
```

#### nuke build

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
#if REVIT<RevitVersion>
    // This code will be available for the specified version
#endif

#if REVIT<RevitVersion>_OR_GREATER
     // This code will be available for all versions greater than specified
#endif
```

## How to add new version Autodesk Revit

Copy libs

```csharp

string version = "2026";
string oldVersion = "2025";

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

version = "2026"
old_version = "2025"

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
refasmer -m -i -w AdWindows.dll
refasmer -r -i -w PackageContentsParser.dll
refasmer -r -i -w RevitAddInUtility.dll
refasmer -r -i -w RevitAPI.dll
refasmer -r -i -w RevitAPIBrowserUtils.dll
refasmer -r -i -w RevitAPIIFC.dll
refasmer -r -i -w RevitAPIMacros.dll
refasmer -r -i -w RevitAPIUI.dll
refasmer -r -i -w RevitAPIUIMacros.dll
refasmer -r -i -w RevitNET.dll
refasmer -m -i -w UIFramework.dll
```