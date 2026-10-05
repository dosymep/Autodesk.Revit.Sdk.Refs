# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.0.0] - 2026-10-05

### Added

- **MSBuild SDK Support**: Introduced a custom MSBuild SDK `Autodesk.Revit.Sdk.Refs` to simplify project references and
  framework resolution.
- **Support for Revit 2026.5 and Revit 2027**.
- Support for target frameworks `net8.0-windows` (for 2025-2026) and `net10.0-windows` (for 2026.5 and 2027)
  automatically selected based on the `RevitVersion` property.
- Multi-language support: added Russian documentation (`README.ru.md`).

### Changed

- **Implicit Package References**: Both `Autodesk.Revit.Sdk.Refs` and specific version packages are now implicitly
  handled when using the MSBuild SDK.
- Optimized and updated `README.md` with explicit tables for frameworks, properties, versioning policies, and a
  comprehensive migration guide from v1 to v2.

## [v2025.04.02] - 2025-04-02

### Add

- Revit 2026 support

## [v2024.04.12] - 2024-04-12

### Add

- Revit 2025 support
- Define constants `REVIT<RevitVersion>` and `REVIT<RevitVersion>_OR_GREATER`
- Revit paths properies `RevitPath` and `RevitExePath` and `RevitAddinsPath` and `RevitAddinsUserPath` and
  `RevitApplicationsPath`
- This `CHANGELOG.md` file

### Changed

- Update `README.md`
- Update `CHANGELOG.md`
- Update [SamplePlugin](sample/SamplePlugin)

## [v2023.11.09] - 2023-11-9

### Add

- Revit 2016 support
- Revit 2017 support
- Revit 2018 support
- Revit 2019 support
- Revit 2020 support
- Revit 2021 support
- Revit 2022 support
- Revit 2023 support
- Revit 2024 support
- Revit 2025 support
