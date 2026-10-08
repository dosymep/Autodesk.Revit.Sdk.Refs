# RevitPlugin.SingleAssembly

[![JetBrains Rider](https://img.shields.io/badge/JetBrains-Rider-blue.svg)](https://www.jetbrains.com/rider)
[![License MIT](https://img.shields.io/badge/License-MIT-blue.svg)](../../LICENSE.md)
[![Revit 2016-2027](https://img.shields.io/badge/Revit-2016--2027-blue.svg)](https://www.autodesk.com/products/revit/overview)

This is a sample Revit plugin project demonstrating how to use the `Autodesk.Revit.Sdk.Refs` custom MSBuild SDK to build
a single multi-version assembly.

## Overview

The plugin targets Revit versions from **2016 to 2027** and compiles into a single assembly using preprocessor
directives and Revit conditional constants (e.g., `REVIT2016` to `REVIT2027`, and `REVIT2025_OR_GREATER` etc.)
automatically provided by the MSBuild SDK.

## Features

- Uses `<Project Sdk="Autodesk.Revit.Sdk.Refs/2.0.0">` to simplify multi-targeting configuration.
- Automatically handles correct Target Framework selection based on `RevitVersion` (`net48`, `net8.0-windows`, or
  `net10.0-windows`).
- Conditionally compiles code depending on the Revit version, showing how to handle API changes seamlessly.
- Uses NUKE Build to automate packaging and compiling across all configured versions.

## Project Structure

- **`RevitPlugin.SingleAssembly/`**: The core plugin project.
    - `RevitCommand.cs`, `RevitCommand.Equal.cs`, `RevitCommand.Greater.cs`: Demonstrates version-specific logic using
      defined compilation constants.
    - `Views/MainWindow.xaml`: WPF user interface used inside Revit.
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

You can build the plugin for specific Revit versions using either the standard .NET CLI or the included NUKE build
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
dotnet build RevitPlugin.SingleAssembly/RevitPlugin.SingleAssembly.csproj -c Debug -p:RevitVersion=2027
```
