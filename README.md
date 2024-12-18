# AoCHelper

[![GitHub Actions][githubactionslogo]][githubactionslink]
[![Nuget][nugetlogo]][nugetlink]

**AoCHelper** is a support library for solving [Advent of Code](https://adventofcode.com/) puzzles, available for .NET and .NET Standard 2.x.

It provides a 'framework' so that you only have to worry about solving the problems, and measures the performance of your solutions.

Problem example:

```csharp
using AoCHelper;
using System.Threading.Tasks;

namespace AdventOfCode;

public class Day_01 : BaseDay
{
    public override ValueTask<string> Solve_1() => new("Solution 1");

    public override ValueTask<string> Solve_2() => new("Solution 2");
}
```

Output example:

![aochelper](https://user-images.githubusercontent.com/11148519/142051856-16d9d5bf-885c-44cd-94ae-6f678bcbc04f.gif)

## AdventOfCode.Template

Creating your Advent of Code repository from [**AdventOfCode.Template**](https://github.com/eduherminio/AdventOfCode.Template) is the quickest way to get up and running with `AoCHelper`.

There's also [AdventOfCode.MultiYearTemplate](https://github.com/eduherminio/AdventOfCode.MultiYearTemplate) available of you want to keep all the years in the same repository, but I'd recommend to use the former one if you're participating in Advent of Code or using AoCHelper for the first time.

## Simple usage

- **Add [AoCHelper NuGet package](https://www.nuget.org/packages/AoCHelper/)** to your project.
- **Create one class per day/problem**, using one of the following approaches:
  - Name them `DayXX` or `Day_XX` and make them inherit `BaseDay`.
  - Name them `ProblemXX` or `Problem_XX`and make them inherit `BaseProblem`.
- **Put your input files under `Inputs/` directory** and follow `XX.txt` naming convention for day `XX`. Make sure to copy those files to your output folder.
- Choose your **solving strategy** in your `Main()` method, adjusting it with your custom `Action<SolverConfiguration>` if needed:
  - `Solver.SolveAll();`
  - `Solver.SolveLast();`
  - `Solver.SolveLast(opt => opt.ClearConsole = false);`
  - `Solver.Solve<Day_05>();`
  - `Solver.Solve(new List<uint>{ 5, 6 });`
  - `Solver.Solve(new List<Type> { typeof(Day_05), typeof(Day_06) });`

## Customization

A **custom `Action<SolverConfiguration>`** can be provided to any of the `Solver` methods. It has the following configurable options (`false` or `null` by default unless otherwise specified):

- **`bool ClearConsole`**: Clears previous runs information from the console. True by default.
- **`bool ShowOverallResults`**: Shows a panel at the end of the run with aggregated stats of the solved problems. True by default when solving multiple problems, false otherwise.
- **`bool ShowConstructorElapsedTime`**: Shows the time elapsed during the instantiation of a `BaseProblem`. This normally reflects the elapsed time while parsing the input data.
- **`bool ShowTotalElapsedTimePerDay`**: Shows total elapsed time per day. This includes constructor time + part 1 + part 2.
- **`string? ElapsedTimeFormatSpecifier`**: Custom numeric format strings used for elapsed milliseconds. See [Standard numeric format strings](https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings).
- **`List<Assembly>`**: Assembly/Assemblies where the problems are located. Defaults to `Assembly.GetEntryAssembly()`, which assumes the problems are in the same project where `Solver` is invoked.

## Advanced usage

You can also:

- Create your own abstract base class tha(t inherits `BaseProblem`, make all your problem classes inherit it and use this custom base class to:
  - Override `ClassPrefix` property, to be able to follow your own `$(ClassPrefix)XX` or `$(ClassPrefix)_XX` convention in each one of your problem classes.
  - Override `InputFileDirPath` to change the input files directory
  - Override `InputFileExtension` to change the input files extension.
  - Override `CalculateIndex()` to follow a different `XX` or `_XX` convention in your class names.
  - Override `InputFilePath` to follow a different naming convention in your input files. Check the [current implementation](https://github.com/eduherminio/AoCHelper/blob/master/src/AoCHelper/BaseProblem.cs) to understand how to reuse all the other properties and methods.
- _[Not recommended]_ Override `InputFilePath` in any specific problem class to point to a concrete file. This will make the values of `ClassPrefix`, `InputFileDirPath` and `InputFileExtension` and the implementation of `CalculateIndex()` irrelevant (see the [current implementation](https://github.com/eduherminio/AoCHelper/blob/master/src/AoCHelper/BaseProblem.cs)).

## Testing

- Example of simple AoC solutions testing: [SampleTests](tests/AoCHelper.Test/SampleTests.cs)
- Example of advanced AoC solutions testing by providing a custom input test filepath: [ModifyInputFilePathTests_SampleTests](tests/AoCHelper.Test/ModifyInputFilePathTests_SampleTests.cs)
- Example of advanced AoC solutions testing by providing a custom input test dir path: [ModifyInputFileDirPath_SampleTests](tests/AoCHelper.Test/ModifyInputFileDirPath_SampleTests.cs)

## Usage examples

Example projects can be found at:

- [AoC2023](https://github.com/eduherminio/AoC2023) (v3.x)
- [AoC2022](https://github.com/eduherminio/AoC2022) (v2.x)
- [AoC2021](https://github.com/eduherminio/AoC2021) (v1.x)
- [AdventOfCode.Template](https://github.com/eduherminio/AdventOfCode.Template)
- [AoCHelper.PoC](src/AoCHelper.PoC)
- [AoCHelper.Test](tests/AoCHelper.Test)
- [AoC2020](https://github.com/eduherminio/AoC2020) (v0.x)
- [All these repositories](https://github.com/eduherminio/AoCHelper/network/dependents)

Some cool repositories that add their own abstractions/customizations on top of `AocHelper`:

- [RachaelBooth/AdventOfCode2022](https://github.com/RachaelBooth/AdventOfCode2022/): `BaseSolver<U>` and `BaseSolver<U, V>` wrappers around `BaseProblem` to have `U Solve1()` and `V Solve2` methods.
- [Morphix84/AdventOfCode](https://github.com/Morphix84/AdventOfCode/): input fetching and same-repository-multi-year support.
- [codemonkey85/Advent-of-Code-2023](https://github.com/codemonkey85/Advent-of-Code-2023): problems located in a library project and `Solver` invoked from both a CLI and a Web project.

## `v1` to `v2+` migration

Methods that accept an instance of `SolverConfiguration` were deprecated in `v2` and removed in `v3`.

They have been replaced by methods that accept `Action<SolverConfiguration>`.

`v1`:

```csharp
await Solver.SolveAll(new SolverConfiguration
{
    ShowConstructorElapsedTime = true,
    ShowOverallResults = true,
    ClearConsole = false
});
```

`v2+`:

```csharp
await Solver.SolveAll(options =>
{
    options.ShowConstructorElapsedTime = true;
    options.ShowOverallResults = true;
    options.ClearConsole = false;
});
```

## `v0.x` to `v1.x` migration

`BaseProblem.Solve_1()` and `BaseProblem.Solve_2()` signature has changed: they must return `ValueTask<string>` now.

`ValueTask<T>` has constructors that accept both `T` and `Task<T>`, so:

`v0.x`:

```csharp
public class Day_01 : BaseDay
{
    public override string Solve_1() => "Solution 2";

    public override string Solve_2() => FooAsync().Result;

    private async Task<string> FooAsync()
    {
        await Task.Delay(1000);
        return "Solution 2";
    }
}
```

becomes now in `v1.x`:

```csharp
public class Day_01 : BaseDay
{
    public override ValueTask<string> Solve_1() => new("Solution 2");

    public override ValueTask<string> Solve_2() => new(FooAsync());

    private async Task<string> FooAsync()
    {
        await Task.Delay(1000);
        return "Solution 2";
    }
}
```

or in case we prefer `async`/`await` over returning the task, as recommended [here](https://github.com/davidfowl/AspNetCoreDiagnosticScenarios/blob/master/AsyncGuidance.md#prefer-asyncawait-over-directly-returning-task):

```csharp
public class Day_01 : BaseDay
{
    public override ValueTask<string> Solve_1() => new("Solution 2");

    public override async ValueTask<string> Solve_2() => new(await FooAsync());

    private async Task<string> FooAsync()
    {
        await Task.Delay(1000);
        return "Solution 2";
    }
}
```

## Tips

Your problem/day classes are instantiated only once, so parsing the input file (`InputFilePath`) in your class constructor allows you to:

- Avoid executing parsing logic twice per problem.
- Measure more accurately your part 1 and part 2 solutions performance\*.

\* Consider enabling `ShowConstructorElapsedTime` and `ShowTotalElapsedTimePerDay` in `Action<SolverConfiguration>`.

[githubactionslogo]: https://github.com/eduherminio/AoCHelper/workflows/CI/badge.svg
[githubactionslink]: https://github.com/eduherminio/AoCHelper/actions?query=workflow%3ACI
[nugetlogo]: https://img.shields.io/nuget/v/AocHelper.svg?style=flat-square&label=nuget
[nugetlink]: https://www.nuget.org/packages/AocHelper

## Other libraries templates and projects

Feel free to experiment with this project and the related templates I provide to create your own thing. There are a few examples under [Usage examples](#usage-examples) that show how to create your custom framework on top of `AoCHelper`.

If there's something in this project that just doesn't click for you, there are dozens of alternatives out there that you can explore instead. These are some well-known examples:

- [viceroypenguin/adventofcode.template](https://github.com/viceroypenguin/adventofcode.template): supports more accurate benchmarks using `BenchmarkDotNet`.
- [Zaneris/AdventOfCodeSupport](https://github.com/Zaneris/AdventOfCodeSupport): supports automatic input downloading and solution submitting.

I would also like to encourage anyone to create their own libraries or templates. Nothing like building your own!

But beware, you might spend more time working on the template than on the problems themselves (been there, done that, would do it again!).
