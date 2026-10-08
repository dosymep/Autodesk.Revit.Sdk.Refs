# RevitPlugin.SingleAssembly

[![JetBrains Rider](https://img.shields.io/badge/JetBrains-Rider-blue.svg)](https://www.jetbrains.com/rider)
[![License MIT](https://img.shields.io/badge/License-MIT-blue.svg)](../../LICENSE.md)
[![Revit 2016-2027](https://img.shields.io/badge/Revit-2016--2027-blue.svg)](https://www.autodesk.com/products/revit/overview)

Это пример проекта плагина для Revit, демонстрирующий использование кастомного MSBuild SDK `Autodesk.Revit.Sdk.Refs` для
сборки единого многоверсионного проекта.

## Обзор

Плагин поддерживает версии Revit от **2016 по 2027** и компилируется в сборку под каждую целевую версию, используя
директивы условной компиляции и константы версий Revit (например, от `REVIT2016` до `REVIT2027`, а также
`REVIT2025_OR_GREATER` и т.д.), которые автоматически предоставляются MSBuild SDK.

## Особенности

- Использование `<Project Sdk="Autodesk.Revit.Sdk.Refs/2.0.0">` для упрощения конфигурации многоцелевой сборки
  (multi-targeting).
- Автоматический выбор целевого фреймворка (Target Framework) на основе `RevitVersion` (`net48`, `net8.0-windows` или
  `net10.0-windows`).
- Условная компиляция кода в зависимости от версии Revit для бесшовной адаптации под изменения API.
- Использование NUKE Build для автоматизации упаковки и сборки плагина под все настроенные конфигурации.

## Структура проекта

- **`RevitPlugin.SingleAssembly/`**: Основной проект плагина.
    - `RevitCommand.cs`, `RevitCommand.Equal.cs`, `RevitCommand.Greater.cs`: Демонстрирует логику работы плагина под
      конкретные версии Revit с использованием констант условной компиляции.
    - `Views/MainWindow.xaml`: Интерфейс пользователя на WPF, отображаемый внутри Revit.
- **`build/`**: Проект автоматизации сборки на основе NUKE.

## Инструкция по сборке

Перед сборкой этого примера необходимо упаковать основной проект `Autodesk.Revit.Sdk.Refs` в локальную директорию
`artifacts`, чтобы NuGet мог выполнить восстановление зависимостей.

### 1. Упаковка основного решения (Pack)

Выполните команду из корня основного репозитория:

```bash
dotnet pack Autodesk.Revit.Sdk.Refs.slnx --output artifacts
```

Файл `nuget.config` в этой директории предварительно настроен на использование корневой папки `artifacts` в качестве
локального источника пакетов.

### 2. Сборка примера (Sample)

Вы можете собрать плагин для конкретных версий Revit с помощью стандартного .NET CLI или с помощью входящего в проект
скрипта сборки NUKE.

#### Сборка через NUKE Build (Рекомендуется)

Используйте загрузочные скрипты (`build.cmd`, `build.ps1` или `build.sh`) для запуска автоматизированной сборки через
NUKE:

```bash
# Список всех целей сборки и конфигураций
./build.cmd --help

# Запуск сборки по умолчанию
./build.cmd
```

#### Сборка через .NET CLI

Скомпилируйте проект под конкретную версию Revit и конфигурацию:

```bash
dotnet build RevitPlugin.SingleAssembly/RevitPlugin.SingleAssembly.csproj -c Debug -p:RevitVersion=2027
```
