# RevitPlugin.Library

[![JetBrains Rider](https://img.shields.io/badge/JetBrains-Rider-blue.svg)](https://www.jetbrains.com/rider)
[![License MIT](https://img.shields.io/badge/License-MIT-blue.svg)](../../LICENSE.md)
[![Revit 2016-2027](https://img.shields.io/badge/Revit-2016--2027-blue.svg)](https://www.autodesk.com/products/revit/overview)

This is a sample Revit plugin library project demonstrating how to use the `Autodesk.Revit.Sdk.Refs` custom MSBuild SDK
to build a multi-version class library.

## Overview

The library targets Revit versions from **2016 to 2027** and compiles into multiple version-specific class library
assemblies, using preprocessor directives and Revit conditional constants (such as `REVIT2024_OR_GREATER` and standard
Revit version constants) automatically provided by the MSBuild SDK.

Unlike the `RevitPlugin.SingleAssembly` sample which builds an application command/app, this sample demonstrates a
utility extension library with useful extensions for `Parameter`, `Element`, and `ElementId` objects. It is designed to
be referenced or shared across Revit plugin projects.

## Features

- Uses `<Project Sdk="Autodesk.Revit.Sdk.Refs/2.0.0">` to simplify multi-targeting configuration.
- Automatically handles correct Target Framework selection based on `RevitVersion` (`net48`, `net8.0-windows`, or
  `net10.0-windows`).
- Implements extension methods utilizing modern C# features (such as experimental C# Roles/Extensions concept if
  supported or traditional extension patterns) and Revit API.
- Conditionally compiles code depending on the Revit version (e.g., handling the transition from 32-bit `IntegerValue`
  to 64-bit `Value` for `ElementId` in Revit 2024 using `REVIT2024_OR_GREATER`).
- Uses NUKE Build to automate compiling across all configured versions.

## Project Structure

- **`RevitPlugin.Library/`**: The core utility extension library project.
    - `ElementExtensions.cs`: Provides utility parameter-retrieval extensions for `Element`.
    - `ElementIdExtensions.cs`: Handles 32-bit vs 64-bit `ElementId` differences across Revit versions.
    - `ParamExtension.cs`: Provides type-safe value retrieval and removal extensions for Revit `Parameter`.
- **`build/`**: The NUKE compilation build project.

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

You can build the library for specific Revit versions using either the standard .NET CLI or the included NUKE build
script.

#### Using NUKE Build (Recommended)

Use the bootstrapper scripts (`build.cmd`, `build.ps1`, or `build.sh`) to run the automated NUKE build:

```bash
# List all targets and configurations
./build.cmd --help

# Run the default build target
./build.cmd
```

#### Using .NET CLI

Compile the project for a specific Revit version and configuration:

```bash
dotnet build RevitPlugin.Library/RevitPlugin.Library.csproj -c Debug -p:RevitVersion=2027
```
