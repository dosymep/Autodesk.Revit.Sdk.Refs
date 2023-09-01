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