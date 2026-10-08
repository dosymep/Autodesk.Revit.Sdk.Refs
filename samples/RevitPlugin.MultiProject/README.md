# RevitPlugin.MultiProject

[![JetBrains Rider](https://img.shields.io/badge/JetBrains-Rider-blue.svg)](https://www.jetbrains.com/rider)
[![License MIT](https://img.shields.io/badge/License-MIT-blue.svg)](../../LICENSE.md)
[![Revit 2016-2027](https://img.shields.io/badge/Revit-2016--2027-blue.svg)](https://www.autodesk.com/products/revit/overview)

This is a sample multi-project Revit plugin demonstrating the usage of the `Autodesk.Revit.SDK.Refs`
MSBuild SDK to build single-codebase plugins targeting multiple Revit versions (2016-2027).

It leverages a modern project layout (`.slnx` solution) with separate project files per target Revit version (e.g.,
Revit 2024, 2025, 2026, 2026.5, 2027) while sharing all code and views from a single source location.

## Overview

This is a sample multi-project Revit plugin demonstrating how to use the `Autodesk.Revit.Sdk.Refs` custom MSBuild SDK to
build single-codebase plugins targeting multiple Revit versions.

## Features

- Uses `<Project Sdk="Autodesk.Revit.Sdk.Refs/2.0.0">` with lightweight per-version project files.
- Automatically handles correct Target Framework selection based on `RevitVersion` (`net48`, `net8.0-windows`, or
  `net10.0-windows`).
- Shares a single codebase (all `.cs` and `.xaml` files) across all target versions using a root
  `Directory.Build.props`.
- Conditionally compiles code depending on the Revit version, showing how to handle API changes seamlessly.
- Uses NUKE Build to automate packaging and compiling across all configured versions.

## How it Works

1. **Lightweight Project Files:**
   Each project file (e.g. `RevitPlugin.MultiProject.2027.csproj`) uses the custom MSBuild SDK and defines its targeted
   `RevitVersion`:
   ```xml
   <Project Sdk="Autodesk.Revit.Sdk.Refs/2.0.0">
       <PropertyGroup>
           <UseWpf>true</UseWpf>
           <Platforms>x64</Platforms>
           <OutputType>Library</OutputType>
           <RootNamespace>RevitPlugin.MultiProject</RootNamespace>
           <RevitVersion>2027</RevitVersion>
       </PropertyGroup>
   </Project>
   ```

2. **Shared Source Files:**
   `Directory.Build.props` in the root automatically adds all `.cs` and `.xaml` files from two directories up (the
   shared project folder) while excluding the specific version subfolders:
   ```xml
   <Project>
       <ItemGroup>
           <Compile Include="../../**/*.cs" Exclude="**/Revit/**"/>
           <Page Include="../../**/*.xaml">
               <SubType>Designer</SubType>
               <Generator>MSBuild:Compile</Generator>
           </Page>
           <Compile Update="../../**/*.xaml.cs">
               <DependentUpon>%(Filename)</DependentUpon>
           </Compile>
       </ItemGroup>
   </Project>
   ```

3. **Conditional Compilation Constants:**
   The `Autodesk.Revit.Sdk.Refs` SDK automatically defines version compilation constants such as `REVIT2024`,
   `REVIT2024_OR_GREATER`, etc., based on the defined `RevitVersion`.

## Project Structure

- **`RevitPlugin.MultiProject/`**: Contains the shared source code:
    - `RevitCommand.cs` - The main external command implementation.
    - `RevitCommand.Equal.cs` - Partial class demonstrating conditional compilation (`REVIT2016`, etc.) for exact Revit
      versions.
    - `RevitCommand.Greater.cs` - Partial class demonstrating conditional compilation with `_OR_GREATER` constants.
    - `Views/MainWindow.xaml` - WPF view shared across all targeted versions.
    - **`Revit/`**: Subfolders containing lightweight `.csproj` files for each specific Revit version (2024, 2025, 2026,
      2026.5, 2027).
- **`build/`**: NUKE build project (`_build.csproj`) for building all targets, cleaning directories, and automating the
  workflow.
- **`Directory.Build.props`**: Configures the automatic inclusion of shared source and XAML files in each project.

## How to Build

Before compiling this sample, you need to pack the main `Autodesk.Revit.Sdk.Refs` package to the local `artifacts`
directory so NuGet can restore it.

### 1. Pack the Main Solution

From the root of the main repository, run:

```bash
dotnet pack Autodesk.Revit.Sdk.Refs.slnx --output artifacts
```

The `nuget.config` in this directory is preconfigured to use the root `artifacts` directory as a local package source.

### 2. Build the Sample

You can build the plugin for specific Revit versions using either the standard .NET CLI or the included NUKE build
script.

#### Using NUKE Build (Recommended)

To clean, restore, and compile all versions into the `bin/` directory, simply run:

```bash
# On Windows (cmd/PowerShell)
.\build.cmd

# On macOS/Linux (sh)
./build.sh
```

This triggers the NUKE build runner, which builds all project variants and outputs them into the `bin` directory.

#### Using .NET CLI

To compile a specific Revit version target:

```bash
dotnet build RevitPlugin.MultiProject/Revit/2027/RevitPlugin.MultiProject.2027.csproj -c Debug
```
