# RevitPlugin.CentralPackageManagement

[![JetBrains Rider](https://img.shields.io/badge/JetBrains-Rider-blue.svg)](https://www.jetbrains.com/rider)
[![License MIT](https://img.shields.io/badge/License-MIT-blue.svg)](../../LICENSE.md)
[![Revit 2016-2027](https://img.shields.io/badge/Revit-2016--2027-blue.svg)](https://www.autodesk.com/products/revit/overview)

Это пример проекта плагина для Revit, демонстрирующий использование кастомного MSBuild SDK `Autodesk.Revit.Sdk.Refs` совместно с механизмом **NuGet Central Package Management (CPM)**.

## Обзор

Централизованное управление пакетами (CPM) позволяет хранить и обновлять версии NuGet-пакетов в едином файле (`Directory.Packages.props`) в корне решения или репозитория, избавляя от необходимости прописывать номера версий в каждом файле проекта (`.csproj`) по отдельности.

Когда свойство `ManagePackageVersionsCentrally` установлено в `true`, кастомный MSBuild SDK `Autodesk.Revit.Sdk.Refs` автоматически берет на себя:
- Добавление нужного элемента `<PackageVersion Include="Autodesk.Revit.Sdk.Refs.$(RevitVersion)" Version="..." />` и соответствующего `<PackageReference Include="Autodesk.Revit.Sdk.Refs.$(RevitVersion)" />`.
- Устранение необходимости вручную декларировать версии пакетов Revit SDK refs в файле `Directory.Packages.props`.
- Бесшовную компиляцию в единую многоверсионную сборку под версии Revit от **2016 по 2027** с помощью директив условной компиляции и констант версий.

## Особенности

- Использование `<Project Sdk="Autodesk.Revit.Sdk.Refs/2.0.0">` для упрощения конфигурации многоцелевой сборки (multi-targeting).
- Полная поддержка **Central Package Management (CPM)** через `Directory.Packages.props`.
- Автоматический выбор целевого фреймворка (Target Framework) на основе `RevitVersion` (`net48`, `net8.0-windows` или `net10.0-windows`).
- Условная компиляция кода в зависимости от версии Revit для адаптации под изменения API.
- Использование NUKE Build для автоматизации сборки и упаковки плагина под все настроенные конфигурации.

## Структура проекта

- **`RevitPlugin.CentralPackageManagement/`**: Основной проект плагина.
    - `RevitCommand.cs`, `RevitCommand.Equal.cs`, `RevitCommand.Greater.cs`: Демонстрирует логику работы плагина под конкретные версии Revit с использованием констант условной компиляции.
    - `Views/MainWindow.xaml`: Интерфейс пользователя на WPF, отображаемый внутри Revit.
- **`Directory.Packages.props`**: Определение версий пакетов на уровне всего решения.
- **`build/`**: Проект автоматизации сборки на основе NUKE.

## Инструкция по сборке

Перед сборкой этого примера необходимо упаковать основной проект `Autodesk.Revit.Sdk.Refs` в локальную директорию `artifacts`, чтобы NuGet мог выполнить восстановление зависимостей.

### 1. Упаковка основного решения (Pack)

Выполните команду из корня основного репозитория:

```bash
dotnet pack Autodesk.Revit.Sdk.Refs.slnx --output artifacts
```

Файл `nuget.config` в этой директории предварительно настроен на использование корневой папки `artifacts` в качестве локального источника пакетов.

### 2. Сборка примера (Sample)

Вы можете собрать плагин для конкретных версий Revit с помощью стандартного .NET CLI или с помощью входящего в проект скрипта сборки NUKE.

#### Сборка через NUKE Build (Рекомендуется)

Используйте загрузочные скрипты (`build.cmd`, `build.ps1` или `build.sh`) для запуска автоматизированной сборки через NUKE:

```bash
# Список всех целей сборки и конфигураций
./build.cmd --help

# Запуск сборки по умолчанию
./build.cmd
```

#### Сборка через .NET CLI

Скомпилируйте проект под конкретную версию Revit и конфигурацию:

```bash
dotnet build RevitPlugin.CentralPackageManagement/RevitPlugin.CentralPackageManagement.csproj -c Debug -p:RevitVersion=2027
```
