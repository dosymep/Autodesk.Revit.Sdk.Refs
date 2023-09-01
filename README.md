# Autodesk.Revit.SDK.Refs

Revit SDK refs assemblies.  
Only metadata from assemblies by [Refasmer](https://github.com/JetBrains/Refasmer).

## Usage

Add project configurations with revit versions in name

```xml
<PropertyGroup>
    <Configurations>D2016;D2017;D2018;D2019;D2020;D2021;D2022;D2023;D2024</Configurations>
    <Configurations>$(Configurations);R2016;R2017;R2018;R2019;R2020;R2021;R2022;R2023;R2024</Configurations>
</PropertyGroup>
```

Add package reference

```xml
<PackageReference Include="Autodesk.Revit.Sdk.Refs" Version="1.0.0">
    <PrivateAssets>all</PrivateAssets>
    <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
</PackageReference>
```

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