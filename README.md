# AoCHelper

[![GitHub Actions][githubactionslogo]][githubactionslink]
[![Nuget][nugetlogo]][nugetlink]

**AoCHelper** is a support library for solving [Advent of Code](https://adventofcode.com/) puzzles available for .NET and .NET Standard 2.x.

## Simple usage

- **Add [AoCHelper NuGet package](https://www.nuget.org/packages/AoCHelper/)** to your project.
- **Create one class per day/problem**, using one of the following approaches:
  - Name them `DayXX` or `Day_XX` and make them inherit `BaseDay`.
  - Name them `ProblemXX` or ``Problem_XX``and make them inherit `BaseProblem`.
- **Put your input files under `Inputs/` directory** and follow `XX.txt` naming convention for day `XX`. Make sure to copy those files to your output folder.

See [Simple usage example](Simple-usage-example) or [AoCHelper.PoC](https://github.com/eduherminio/AoCHelper/tree/master/src/AoCHelper.PoC) project.

## Advanced usage

You can also:

- Create your own abstract base class that inherits `BaseProblem`, make all your problem classes inherit it and use this custom base class to:
  - Override `ClassPrefix` property, to be able to follow your own `$(ClassPrefix)XX` or `$(ClassPrefix)_XX` convention in each one of your problem classes.
  - Override `InputFileDirPath` to change the input files directory
  - Override `InputFileExtension` to change the input files extension.
  - Override `CalculateIndex()` to follow a different `XX` or `_XX` convention in your classes and input files.
- Override `InputFilePath` for each problem class, which will make the values of `ClassPrefix`, `InputFileDirPath` and `InputFileExtension` and the implementation of `CalculateIndex()` irrelevant, since it's originally calculated using them.

## Simple usage example

Project structure

```bash
- MyProject.csproj
- Program.cs
- Day_01.cs
- Inputs
  - 01.txt
```

`MyProject.csproj`

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net5.0</TargetFramework>
    <OutputType>Exe</OutputType>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="AoCHelper" Version="0.8.0" />
  </ItemGroup>

  <ItemGroup>
    <None Update="Inputs\*">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
  </ItemGroup>

</Project>
```

`Program.cs`

```csharp
using AoCHelper;

Solver.SolveAll();
```

`Day_01.cs`

```csharp
using AoCHelper;
using System.IO;

namespace MyProject
{
    public class Day_01 : BaseDay
    {
        private readonly string _input;

        public Day_01()
        {
            _input = ParseInput();
        }

        public override string Solve_1()
        {
            return _input.Length.ToString();
        }

        public override string Solve_2()
        {
            return 2 * _input.Length.ToString();
        }
    }
}
```

You can also check [AoCHelper.PoC](https://github.com/eduherminio/AoCHelper/tree/master/src/AoCHelper.PoC) project as example and/or base your  project on it (remember replacing the `AoCHelper` project reference with a NuGet one!).

[githubactionslogo]: https://github.com/eduherminio/AoCHelper/workflows/CI/badge.svg
[githubactionslink]: https://github.com/eduherminio/AoCHelper/actions?query=workflow%3ACI
[nugetlogo]: https://img.shields.io/nuget/v/AocHelper.svg?style=flat-square&label=nuget
[nugetlink]: https://www.nuget.org/packages/AocHelper
