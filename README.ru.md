# Autodesk.Revit.SDK.Refs

[![JetBrains Rider](https://img.shields.io/badge/JetBrains-Rider-blue.svg)](https://www.jetbrains.com/rider)
[![License MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE.md)
[![Revit 2016-2027](https://img.shields.io/badge/Revit-2016--2027-blue.svg)](https://www.autodesk.com/products/revit/overview)

Сборки ссылок (reference assemblies) для Revit SDK.  
Содержат только метаданные из оригинальных сборок, сгенерированные с
помощью [Refasmer](https://github.com/JetBrains/Refasmer).

## Использование

Вы можете использовать кастомный **MSBuild SDK** `Autodesk.Revit.Sdk.Refs`. Просто укажите его в теге `<Project>` вашего
проекта.

### Политика версионирования

Мы строго следуем SemVer (семантическому версионированию):

- **Минорная версия** увеличивается (например, `2.1.0`) при добавлении библиотек/поддержки новых версий Revit.
- **Патч-версия** увеличивается (например, `2.0.1`) при исправлении мелких ошибок или улучшении логики SDK.
- **Мажорная версия** увеличивается (например, `3.0.0`) только при появлении ломающих изменений в публичном контракте
  MSBuild SDK, которые могут потребовать ручной миграции.

```xml

<Project Sdk="Autodesk.Revit.Sdk.Refs/2.0.0">

    <PropertyGroup>
        <UseWpf>true</UseWpf>
        <Platforms>x64</Platforms>
        <OutputType>Library</OutputType>

        <!-- Опционально: установите true для вывода информации о сборке во время компиляции -->
        <ShowBuildInfo>true</ShowBuildInfo>

        <!-- Определите конфигурации, которые вы хотите использовать -->
        <Configurations>Debug;Release;D2024;D2025;D2026;В2026.5;D2027</Configurations>
    </PropertyGroup>

</Project>
```

MSBuild SDK автоматически берет на себя:

- Выбор правильной целевой платформы (Target Framework) на основе свойства `RevitVersion` (например, `net48` для версий
  до 2025, `net8.0-windows` для 2025-2026 и `net10.0-windows` для 2027).

### Фреймворки и версии Revit

| Версия Revit | Целевой фреймворк (Target Framework) |
|--------------|--------------------------------------|
| 2016 - 2024  | `net48`                              |
| 2025         | `net8.0-windows`                     |
| 2026         | `net8.0-windows`                     |
| 2026.5       | `net10.0-windows`                    |
| 2027         | `net10.0-windows`                    |

### Автоматически определяемые свойства Revit

| Имя свойства            | Значение / Путь                                                                                  | Описание                                                                  |
|-------------------------|--------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------|
| `RevitPath`             | `$(ProgramFiles)\Autodesk\Revit $(RevitVersion)` *(или `...Revit MEP $(RevitVersion)` для 2016)* | Путь к директории установки Revit.                                        |
| `RevitExePath`          | `$(RevitPath)\Revit.exe`                                                                         | Путь к исполняемому файлу `Revit.exe`.                                    |
| `RevitAddinsPath`       | `$(ProgramData)\Autodesk\Revit\Addins\$(RevitVersion)`                                           | Путь к общей (для всех пользователей) директории плагинов (Addins) Revit. |
| `RevitAddinsUserPath`   | `$(AppData)\Autodesk\Revit\Addins\$(RevitVersion)`                                               | Путь к пользовательской директории плагинов (Addins) Revit.               |
| `RevitApplicationsPath` | `$(ProgramData)\Autodesk\ApplicationPlugins`                                                     | Путь к директории Autodesk Application Plugins.                           |

- Неявное подключение пакетов `Autodesk.Revit.Sdk.Refs` и соответствующей версии пакета сборок
  `Autodesk.Revit.Sdk.Refs.$(RevitVersion)`.
- Автоматическая генерация констант компиляции для версий Revit.

## Миграция с v1 на v2

В версии 1.x вам приходилось вручную указывать целевые фреймворки, дефолтные свойства и подключать два отдельных
NuGet-пакета в каждом проекте:

```xml
<!-- Стиль v1.x (Устаревший) -->
<Project Sdk="Microsoft.NET.Sdk">
    <PropertyGroup>
        <RevitVersion Condition="'$(RevitVersion)' == ''">2016</RevitVersion>
        <TargetFramework Condition="'$(TargetFramework)' == ''">net48</TargetFramework>
    </PropertyGroup>
    <ItemGroup>
        <PackageReference Include="Autodesk.Revit.Sdk.Refs" Version="1.*"/>
        <PackageReference Include="Autodesk.Revit.Sdk.Refs.$(RevitVersion)" Version="1.*"/>
    </ItemGroup>
</Project>
```

В версии 2.x проект предлагает кастомный **MSBuild SDK**, который значительно упрощает этот процесс.

### Шаги по миграции:

1. **Обновите тег `<Project>`:** Замените ваш стандартный `<Project Sdk="Microsoft.NET.Sdk">` (или аналогичный) на
   ссылку на MSBuild SDK:
   ```xml
   <Project Sdk="Autodesk.Revit.Sdk.Refs/2.0.0">
       <!-- some properties -->
   </Project>
   ```
2. **Удалите вручную добавленные ссылки на пакеты:** Удалите теги `PackageReference` для пакетов
   `Autodesk.Revit.Sdk.Refs` и `Autodesk.Revit.Sdk.Refs.$(RevitVersion)` из ваших `.csproj` файлов. Теперь они
   подключаются неявно с помощью MSBuild SDK.
3. **Удалите избыточные свойства:** Вы можете удалить ручные определения `TargetFramework`, `RevitPath`, `RevitExePath`
   и т.д., если вам не требуется их переопределять. SDK автоматически определяет целевой фреймворк (`net48`,
   `net8.0-windows`, `net10.0-windows`) и пути на основе переданного `RevitVersion`.

### Как собрать и запустить пример (Sample)

Пример проекта в папке `sample/SamplePlugin` завязан на локально собранные NuGet-пакеты. Чтобы скомпилировать и
запустить пример, вам сначала нужно упаковать (pack) SDK и пакеты версий в директорию `artifacts`, чтобы NuGet мог
восстановить их локально.

#### 1. Упаковка решения (Pack)

Запустите команду `dotnet pack` для решения, указав путь вывода в папку `artifacts`:

```bash
dotnet pack Autodesk.Revit.Sdk.Refs.slnx --output artifacts
```

Это создаст файлы `.nupkg` (например, `Autodesk.Revit.Sdk.Refs.2.0.0.nupkg` и все версии пакетов ссылок) в папке
`artifacts/`. Файл `nuget.config` примера настроен на чтение пакетов из этой директории как локального источника.

#### 2. Сборка проекта Revit (Sample)

Теперь вы можете перейти в папку примера и скомпилировать проект под нужную конфигурацию/версию Revit:

##### Использование dotnet cli

```bash
dotnet build sample/SamplePlugin/SamplePlugin/SamplePlugin.csproj -c <Configuration> -p:RevitVersion=<RevitVersion>
```

##### Использование nuke build

```csharp
DotNetBuild(s => s
    .DisableNoRestore()
    .SetProjectFile(<ProjectName>)
    .SetConfiguration(<Configuration>)
    .SetProperty("RevitVersion", (int) <RevitVersion>));
```

## Определенные константы

Эти константы объявляются для всех поддерживаемых версий Revit.

```
REVIT<RevitVersion>
REVIT<RevitVersion>_OR_GREATER
```

### Пример использования констант

```csharp
#if REVIT2024
    // Этот код будет активен только для Revit 2024
#endif

#if REVIT2025_OR_GREATER
     // Этот код будет активен для Revit 2025 и новее
#endif
```

## Как добавить новую версию Autodesk Revit

Копирование библиотек

```csharp

string version = "2027";
string oldVersion = "2026";

string target = "lib";
string source = Path.Combine(target, oldVersion);
string originals = @"C:\Program Files\Autodesk\Revit " + version;

foreach (string enumerateFile in Directory.EnumerateFiles(source))
{
    string fileName = Path.GetFileName(enumerateFile);
    string targetFile = Path.Combine(target, version, fileName);
    string originalFile = Path.Combine(originals, fileName);

    if (File.Exists(originalFile))
    {
        Console.WriteLine($"Copying: {originalFile} -> {targetFile}");

        Directory.CreateDirectory(Path.Combine(target, version));
        File.Copy(originalFile, targetFile);
    }
    else
    {
        Console.WriteLine($"Original file not found: {originalFile}");
    }
}

```

```python
import os
import shutil

version = "2027"
old_version = "2026"

target = "lib"
source = os.path.join(target, old_version)
originals = os.path.join(r"C:\Program Files\Autodesk", f"Revit {version}")

for enumerate_file in os.listdir(source):
    file_name = enumerate_file
    target_file = os.path.join(target, version, file_name)
    original_file = os.path.join(originals, file_name)

    if os.path.exists(original_file):
        print(f"Copying: {original_file} -> {target_file}")
        os.makedirs(os.path.join(target, version), exist_ok=True)
        shutil.copy(original_file, target_file)
    else:
        print(f"Original file not found: {original_file}")
```

Установите [Refasmer](https://github.com/JetBrains/Refasmer) и запустите в папке refs:

```
refasmer -m -i -w --omit-non-api-members=true AdWindows.dll
refasmer -r -i -w --omit-non-api-members=true PackageContentsParser.dll
refasmer -r -i -w --omit-non-api-members=true RevitAddInUtility.dll
refasmer -r -i -w --omit-non-api-members=true RevitAPI.dll
refasmer -r -i -w --omit-non-api-members=true RevitAPIBrowserUtils.dll
refasmer -r -i -w --omit-non-api-members=true RevitAPIIFC.dll
refasmer -r -i -w --omit-non-api-members=true RevitAPIMacros.dll
refasmer -r -i -w --omit-non-api-members=true RevitAPIUI.dll
refasmer -r -i -w --omit-non-api-members=true RevitAPIUIMacros.dll
refasmer -r -i -w --omit-non-api-members=true RevitNET.dll
refasmer -m -i -w --omit-non-api-members=true UIFramework.dll
```
