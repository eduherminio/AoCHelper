# AoCHelper

[![GitHub Actions][githubactionslogo]][githubactionslink]
[![Nuget][nugetlogo]][nugetlink]

**AoCHelper** is a support library for solving [Advent of Code](https://adventofcode.com/) puzzles, available for .NET and .NET Standard 2.x.

It provides a 'framework' so that you only have to worry about solving the problems, and measures the performance of your solutions.

Problem example: 
```csharp
using AoCHelper;
using System.IO;

namespace AdventOfCode
{
    public class Day_01 : BaseDay
    {
        public override string Solve_1() => $"Solution 1";

        public override string Solve_2() => $"Solution 2";
    }
}
```

Output example:

![image](https://user-images.githubusercontent.com/11148519/100517610-0987a880-318c-11eb-897d-6278a440fd44.png)

## :new: AdventOfCode.Template

Creating your Advent of Code repository from [AdventOfCode.Template](https://github.com/eduherminio/AdventOfCode.Template) is the quickest way to get up and running with `AoCHelper`.

## Simple usage

- **Add [AoCHelper NuGet package](https://www.nuget.org/packages/AoCHelper/)** to your project.
- **Create one class per day/problem**, using one of the following approaches:
  - Name them `DayXX` or `Day_XX` and make them inherit `BaseDay`.
  - Name them `ProblemXX` or `Problem_XX`and make them inherit `BaseProblem`.
- **Put your input files under `Inputs/` directory** and follow `XX.txt` naming convention for day `XX`. Make sure to copy those files to your output folder.

## Advanced usage

You can also:

- Create your own abstract base class that inherits `BaseProblem`, make all your problem classes inherit it and use this custom base class to:
  - Override `ClassPrefix` property, to be able to follow your own `$(ClassPrefix)XX` or `$(ClassPrefix)_XX` convention in each one of your problem classes.
  - Override `InputFileDirPath` to change the input files directory
  - Override `InputFileExtension` to change the input files extension.
  - Override `CalculateIndex()` to follow a different `XX` or `_XX` convention in your classes and input files.
- Override `InputFilePath` for each problem class, which will make the values of `ClassPrefix`, `InputFileDirPath` and `InputFileExtension` and the implementation of `CalculateIndex()` irrelevant, since it's originally calculated using them.

## Usage examples

Example projects can be found at:

- [AdventOfCode.Template](https://github.com/eduherminio/AdventOfCode.Template)
- [AoCHelper.PoC](https://github.com/eduherminio/AoCHelper/tree/master/src/AoCHelper.PoC)

[githubactionslogo]: https://github.com/eduherminio/AoCHelper/workflows/CI/badge.svg
[githubactionslink]: https://github.com/eduherminio/AoCHelper/actions?query=workflow%3ACI
[nugetlogo]: https://img.shields.io/nuget/v/AocHelper.svg?style=flat-square&label=nuget
[nugetlink]: https://www.nuget.org/packages/AocHelper

## Tips

Your problem classes are only instantiated once, so parsing the input file (`InputFilePath`) in your class constructor allows you to:

- Avoid executing parsing logic twice per problem.
- Measure more accurately your part 1 and part 2 solutions performance.
