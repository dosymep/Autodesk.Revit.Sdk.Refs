# RevitPlugin.MultiProject

[![JetBrains Rider](https://img.shields.io/badge/JetBrains-Rider-blue.svg)](https://www.jetbrains.com/rider)
[![License MIT](https://img.shields.io/badge/License-MIT-blue.svg)](../../LICENSE.md)
[![Revit 2016-2027](https://img.shields.io/badge/Revit-2016--2027-blue.svg)](https://www.autodesk.com/products/revit/overview)

Это пример многопроектного плагина для Revit, демонстрирующий использование MSBuild
SDK `Autodesk.Revit.SDK.Refs` для сборки плагинов из единой кодовой базы под несколько версий Revit
(2016-2027).

В примере используется современная структура проектов (решение `.slnx`), где под каждую целевую версию Revit (например,
Revit 2024, 2025, 2026, 2026.5, 2027) выделен свой легковесный файл проекта, в то время как весь код и представления
(views) разделяются из единого общего источника.

## Обзор

Это пример многопроектного плагина для Revit, демонстрирующий использование кастомного MSBuild SDK
`Autodesk.Revit.Sdk.Refs` для сборки плагинов из единой кодовой базы под несколько версий Revit.

## Особенности

- Использование `<Project Sdk="Autodesk.Revit.Sdk.Refs/2.0.0">` с легковесными файлами проектов для каждой версии.
- Автоматический выбор целевого фреймворка (Target Framework) на основе `RevitVersion` (`net48`, `net8.0-windows` или
  `net10.0-windows`).
- Разделение единой кодовой базы (все `.cs` и `.xaml` файлы) между всеми целевыми версиями с помощью корневого файла
  `Directory.Build.props`.
- Условная компиляция кода в зависимости от версии Revit для бесшовной адаптации под изменения API.
- Использование NUKE Build для автоматизации упаковки и сборки под все настроенные конфигурации.

## Как это работает

1. **Легковесные файлы проектов:**
   Каждый файл проекта (например, `RevitPlugin.MultiProject.2027.csproj`) использует кастомный MSBuild SDK и определяет
   целевую `RevitVersion`:
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

2. **Общий исходный код:**
   Файл `Directory.Build.props` в корневом каталоге автоматически добавляет все `.cs` и `.xaml` файлы на два уровня выше
   (общая папка проекта), исключая вложенные папки конкретных версий (`Revit`):
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

3. **Константы условной компиляции:**
   SDK `Autodesk.Revit.Sdk.Refs` автоматически определяет константы компиляции, такие как `REVIT2024`,
   `REVIT2024_OR_GREATER` и т.д., на основе указанного свойства `RevitVersion`.

## Структура проекта

- **`RevitPlugin.MultiProject/`**: Содержит общий исходный код:
    - `RevitCommand.cs` - Главная реализация внешней команды плагина.
    - `RevitCommand.Equal.cs` - Частичный класс, демонстрирующий условную компиляцию (`REVIT2016` и др.) под конкретные
      версии Revit.
    - `RevitCommand.Greater.cs` - Частичный класс, демонстрирующий условную компиляцию с константами `_OR_GREATER`.
    - `Views/MainWindow.xaml` - WPF-представление, разделяемое всеми целевыми версиями плагина.
    - **`Revit/`**: Подпапки, содержащие легковесные файлы `.csproj` под каждую конкретную версию Revit (2024, 2025,
      2026, 2026.5, 2027).
- **`build/`**: Проект сборки NUKE (`_build.csproj`) для очистки директорий, восстановления пакетов и компиляции всех
  версий плагина.
- **`Directory.Build.props`**: Конфигурирует автоматическое включение общих файлов исходного кода и XAML-представлений в
  каждый проект версии.

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

#### Сборка через NUKE Build (Рекомендуется)

Используйте загрузочные скрипты (`build.cmd`, `build.ps1` или `build.sh`) для запуска автоматизированной сборки через
NUKE:

```bash
# Список всех целей сборки и конфигураций
./build.cmd --help

# Запуск сборки по умолчанию
./build.cmd
```

Это запустит сборку NUKE, которая соберет все варианты плагина и сохранит их в общую папку `bin`.

#### Сборка через .NET CLI

Для компиляции конкретной версии плагина (например, Revit 2027):

```bash
dotnet build RevitPlugin.MultiProject/Revit/2027/RevitPlugin.MultiProject.2027.csproj -c Debug
```
