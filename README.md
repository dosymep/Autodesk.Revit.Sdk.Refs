# Autodesk.Revit.SDK.Refs

Revit SDK refs assemblies.  
Only metadata from assemblies by [Refasmer](https://github.com/JetBrains/Refasmer).

## Usage

Add project configurations with revit versions in name

```xml
<PropertyGroup>
    <RevitVersion>2016</RevitVersion> <!-- set default revit version -->
    <TargetFramework>net45</TargetFramework> <!-- set default target framework -->
    <Configurations>Debug;Release;D2016;D2017;D2018;D2019;D2020;D2021;D2022;D2023;D2024</Configurations>
</PropertyGroup>
```

Add package reference

```xml
<ItemGroup>
    <!-- package reference with revit dlls -->
    <PackageReference Include="Autodesk.Revit.Sdk.Refs.$(RevitVersion)" Version="1.0.0" />

    <!-- package reference with revit constants -->
    <PackageReference Include="Autodesk.Revit.Sdk.Refs" Version="1.0.0">
        <PrivateAssets>all</PrivateAssets>
        <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
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
REVIT_<RevitVersion>  
REVIT_<RevitVersion>_OR_LESS  
REVIT_<RevitVersion>_OR_GREATER  
```

### Usage defined constants
```csharp
#if REVIT_<RevitVersion>
    // This code will be available for the specified version
#endif

#if REVIT_<RevitVersion>_OR_LESS
     // This code will be available for all versions less than specified
#endif

#if REVIT_<RevitVersion>_OR_GREATER
     // This code will be available for all versions greater than specified
#endif
```

## Known Issues

Constants `REVIT_<RevitVersion>_OR_GREATER` doesn't work in IDE.

## How to add new version Autodesk Revit

Copy libs

```csharp
string version = "2025";
string oldVersion = "2024";

string target = @"lib";
string source = @"lib\" + oldVersion;
string originals = @"C:\Program Files\Autodesk\Revit " + version;

foreach (string enumerateFile in Directory.EnumerateFiles(source))
{
    string fileName = Path.GetFileName(enumerateFile);
    string targetFile = Path.Combine(target, version, fileName);
    string originalFile = Path.Combine(originals, version, fileName);

    Directory.CreateDirectory(Path.Combine(target, version));
    File.Copy(originalFile, targetFile);
}

```

Install [Refasmer](https://github.com/JetBrains/Refasmer) and run:

```
refasmer -m -i -w AdWindows.dll
refasmer -r -i -w PackageContentsParser.dll
refasmer -r -i -w Revit.IFC.Common.dll
refasmer -r -i -w Revit.IFC.Export.dll
refasmer -r -i -w Revit.IFC.Import.dll
refasmer -r -i -w RevitAddInUtility.dll
refasmer -r -i -w RevitAPI.dll
refasmer -r -i -w RevitAPIBrowserUtils.dll
refasmer -r -i -w RevitAPIIFC.dll
refasmer -r -i -w RevitAPIMacros.dll
refasmer -r -i -w RevitAPIUI.dll
refasmer -r -i -w RevitNET.dll
refasmer -m -i -w UIFramework.dll
```