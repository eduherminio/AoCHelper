using System.Diagnostics;
using System.Reflection;
using Spectre.Console;
using System.Linq;

namespace AoCHelper;

public static class Solver
{
    private static readonly bool IsInteractiveEnvironment = Environment.UserInteractive && !Console.IsOutputRedirected;

    private sealed record ElapsedTime(double Constructor, double Part1, double Part2);

    /// <summary>
    /// Solves last problem.
    /// It also prints the elapsed time in <see cref="BaseProblem.Solve_1"/> and <see cref="BaseProblem.Solve_2"/> methods.
    /// </summary>
    /// <param name="options"></param>
    public static async Task SolveLast(Action<SolverConfiguration>? options = null)
    {
        var configuration = PopulateConfiguration(options);

        if (IsInteractiveEnvironment && configuration.ClearConsole)
        {
            AnsiConsole.Clear();
        }

        var table = GetTable();

        await AnsiConsole.Live(table)
            .AutoClear(false)
            .Overflow(configuration.VerticalOverflow)
            .Cropping(configuration.VerticalOverflowCropping)
            .StartAsync(async ctx =>
            {
                var lastProblem = LoadAllProblems(configuration.ProblemAssemblies).LastOrDefault();
                if (lastProblem is not null)
                {
                    var sw = new Stopwatch();
                    sw.Start();
                    var potentialProblem = InstantiateProblem(lastProblem);
                    sw.Stop();

                    if (potentialProblem is BaseProblem problem)
                    {
                        await SolveProblem(problem, table, CalculateElapsedMilliseconds(sw), configuration);
                        ctx.Refresh();
                    }
                    else
                    {
                        RenderEmptyProblem(lastProblem, potentialProblem as string, table, CalculateElapsedMilliseconds(sw), configuration);
                    }
                }
            });
    }

    /// <summary>
    /// Solves a problem.
    /// It also prints the elapsed time in <see cref="BaseProblem.Solve_1"/> and <see cref="BaseProblem.Solve_2"/> methods.
    /// </summary>
    /// <typeparam name="TProblem"></typeparam>
    /// <param name="options"></param>
    public static async Task Solve<TProblem>(Action<SolverConfiguration>? options = null)
        where TProblem : BaseProblem, new()
    {
        var configuration = PopulateConfiguration(options);

        if (IsInteractiveEnvironment && configuration.ClearConsole)
        {
            AnsiConsole.Clear();
        }

        var table = GetTable();

        await AnsiConsole.Live(table)
            .AutoClear(false)
            .Overflow(configuration.VerticalOverflow)
            .Cropping(configuration.VerticalOverflowCropping)
            .StartAsync(async ctx =>
            {
                var sw = new Stopwatch();
                sw.Start();
                try
                {
                    TProblem problem = new();
                    sw.Stop();

                    await SolveProblem(problem, table, CalculateElapsedMilliseconds(sw), configuration);
                }
                catch (Exception e)
                {
                    RenderEmptyProblem(typeof(TProblem), e.Message + Environment.NewLine + e.StackTrace, table, CalculateElapsedMilliseconds(sw), configuration);
                }
                ctx.Refresh();
            });
    }

    /// <summary>
    /// Solves those problems whose <see cref="BaseProblem.CalculateIndex"/> method matches one of the provided numbers.
    /// 0 can be used for those problems whose <see cref="BaseProblem.CalculateIndex"/> returns the default value due to not being able to deduct the index.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="problemNumbers"></param>
    public static async Task Solve(Action<SolverConfiguration>? options = null, params uint[] problemNumbers)
        => await Solve(problemNumbers.AsEnumerable(), options);

    /// <summary>
    /// Solves the provided problems.
    /// It also prints the elapsed time in <see cref="BaseProblem.Solve_1"/> and <see cref="BaseProblem.Solve_2"/> methods.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="problems"></param>
    public static async Task Solve(Action<SolverConfiguration>? options = null, params Type[] problems)
        => await Solve(problems.AsEnumerable(), options);

    /// <summary>
    /// Solves the provided problems.
    /// It also prints the elapsed time in <see cref="BaseProblem.Solve_1"/> and <see cref="BaseProblem.Solve_2"/> methods.
    /// </summary>
    /// <param name="problems"></param>
    /// <param name="options"></param>
    public static async Task Solve(IEnumerable<Type> problems, Action<SolverConfiguration>? options = null)
    {
        var configuration = PopulateConfiguration(options);

        if (IsInteractiveEnvironment && configuration.ClearConsole)
        {
            AnsiConsole.Clear();
        }

        var totalElapsedTime = new List<ElapsedTime>();
        var table = GetTable();

        await AnsiConsole.Live(table)
            .AutoClear(false)
            .Overflow(configuration.VerticalOverflow)
            .Cropping(configuration.VerticalOverflowCropping)
            .StartAsync(async ctx =>
            {
                var sw = new Stopwatch();
                foreach (var problemType in LoadAllProblems(configuration.ProblemAssemblies).Where(problemType => problems.Contains(problemType)))
                {
                    sw.Restart();
                    var potentialProblem = InstantiateProblem(problemType);
                    sw.Stop();
                    if (potentialProblem is BaseProblem problem)
                    {
                        totalElapsedTime.Add(await SolveProblem(problem, table, CalculateElapsedMilliseconds(sw), configuration));
                        ctx.Refresh();
                    }
                    else
                    {
                        totalElapsedTime.Add(RenderEmptyProblem(problemType, potentialProblem as string, table, CalculateElapsedMilliseconds(sw), configuration));
                    }
                }
            });

        RenderOverallResultsPanel(totalElapsedTime, configuration);
    }

    /// <summary>
    /// Solves those problems whose <see cref="BaseProblem.CalculateIndex"/> method matches one of the provided numbers.
    /// 0 can be used for those problems whose <see cref="BaseProblem.CalculateIndex"/> returns the default value due to not being able to deduct the index.
    /// This method might not work correctly if any of the problems in the assembly throws an exception in its constructor.
    /// </summary>
    /// <param name="problemNumbers"></param>
    /// <param name="options"></param>
    public static async Task Solve(IEnumerable<uint> problemNumbers, Action<SolverConfiguration>? options = null)
    {
        var configuration = PopulateConfiguration(options);

        if (IsInteractiveEnvironment && configuration.ClearConsole)
        {
            AnsiConsole.Clear();
        }

        var totalElapsedTime = new List<ElapsedTime>();
        var table = GetTable();

        await AnsiConsole.Live(table)
            .AutoClear(false)
            .Overflow(configuration.VerticalOverflow)
            .Cropping(configuration.VerticalOverflowCropping)
            .StartAsync(async ctx =>
            {
                var sw = new Stopwatch();
                foreach (Type problemType in LoadAllProblems(configuration.ProblemAssemblies))
                {
                    sw.Restart();
                    // Since we're trying to instantiate them all, we don't want to show unrelated errors or render unrelated problem rows
                    // However, without the index, calculated once the constructor success, we can't separate those unrelated errors from
                    // our desired problems' ones. So there's a limitation when using this method if other constructors are failing
                    var potentialProblem = Activator.CreateInstance(problemType);
                    sw.Stop();

                    if (potentialProblem is BaseProblem problem && problemNumbers.Contains(problem.CalculateIndex()))
                    {
                        totalElapsedTime.Add(await SolveProblem(problem, table, CalculateElapsedMilliseconds(sw), configuration));
                        ctx.Refresh();
                    }
                }
            });

        RenderOverallResultsPanel(totalElapsedTime, configuration);
    }

    /// <summary>
    /// Solves all problems in the assembly.
    /// It also prints the elapsed time in <see cref="BaseProblem.Solve_1"/> and <see cref="BaseProblem.Solve_2"/> methods.
    /// </summary>
    /// <param name="options"></param>
    public static async Task SolveAll(Action<SolverConfiguration>? options = null)
    {
        var configuration = PopulateConfiguration(options);

        if (IsInteractiveEnvironment && configuration.ClearConsole)
        {
            AnsiConsole.Clear();
        }

        var totalElapsedTime = new List<ElapsedTime>();
        var table = GetTable();

        await AnsiConsole.Live(table)
            .AutoClear(false)
            .Overflow(configuration.VerticalOverflow)
            .Cropping(configuration.VerticalOverflowCropping)
            .StartAsync(async ctx =>
            {
                var sw = new Stopwatch();
                foreach (Type problemType in LoadAllProblems(configuration.ProblemAssemblies))
                {
                    sw.Restart();
                    var potentialProblem = InstantiateProblem(problemType);
                    sw.Stop();

                    if (potentialProblem is BaseProblem problem)
                    {
                        totalElapsedTime.Add(await SolveProblem(problem, table, CalculateElapsedMilliseconds(sw), configuration));
                        ctx.Refresh();
                    }
                    else
                    {
                        totalElapsedTime.Add(RenderEmptyProblem(problemType, potentialProblem as string, table, CalculateElapsedMilliseconds(sw), configuration));
                    }
                }
            });

        RenderOverallResultsPanel(totalElapsedTime, configuration);
    }

    /// <summary>
    /// Loads all <see cref="BaseProblem"/> in the given assemblies
    /// </summary>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    internal static IEnumerable<Type> LoadAllProblems(List<Assembly> assemblies)
    {
        return assemblies.SelectMany(a => a.GetTypes())
            .Where(type => typeof(BaseProblem).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
            .OrderBy(t => t.FullName);
    }

    private static SolverConfiguration PopulateConfiguration(Action<SolverConfiguration>? options)
    {
        var configuration = new SolverConfiguration();
        options?.Invoke(configuration);

        return configuration;
    }

    private static object? InstantiateProblem(Type problemType)
    {
        try
        {
            return Activator.CreateInstance(problemType);
        }
        catch (Exception e)
        {
            return e.InnerException?.Message + Environment.NewLine + e.InnerException?.StackTrace;
        }
    }

    private static async Task<ElapsedTime> SolveProblem(BaseProblem problem, Table table, double constructorElapsedTime, SolverConfiguration configuration)
    {
        var problemIndex = problem.CalculateIndex();
        var problemTitle = problemIndex != default
            ? $"Day {problemIndex}"
            : $"{problem.GetType().Name}";

        if (configuration.ShowConstructorElapsedTime)
        {
            RenderRow(table, problemTitle, $"{problem.GetType().Name}()", "-----------", constructorElapsedTime, configuration);
        }

        (string solution1, double elapsedMillisecondsPart1) = await SolvePart(isPart1: true, problem);
        RenderRow(table, problemTitle, "Part 1", solution1, elapsedMillisecondsPart1, configuration);

        (string solution2, double elapsedMillisecondsPart2) = await SolvePart(isPart1: false, problem);
        RenderRow(table, problemTitle, "Part 2", solution2, elapsedMillisecondsPart2, configuration);

        if (configuration.ShowTotalElapsedTimePerDay)
        {
            RenderRow(table, problemTitle, "[bold]Total[/]", "-----------", constructorElapsedTime + elapsedMillisecondsPart1 + elapsedMillisecondsPart2, configuration);
        }

        table.AddEmptyRow();

        return new ElapsedTime(constructorElapsedTime, elapsedMillisecondsPart1, elapsedMillisecondsPart2);
    }

    private static ElapsedTime RenderEmptyProblem(Type problemType, string? exceptionString, Table table, double constructorElapsedTime, SolverConfiguration configuration)
    {
        var problemTitle = problemType.Name;

        RenderRow(table, problemTitle, $"{problemTitle}()", exceptionString ?? "Unhandled exception during constructor", constructorElapsedTime, configuration);

        const double elapsedMillisecondsPart1 = 0;
        const double elapsedMillisecondsPart2 = 0;
        RenderRow(table, problemTitle, "Part 1", "-----------", elapsedMillisecondsPart1, configuration);
        RenderRow(table, problemTitle, "Part 2", "-----------", elapsedMillisecondsPart2, configuration);

        if (configuration.ShowTotalElapsedTimePerDay)
        {
            RenderRow(table, problemTitle, "[bold]Total[/]", "-----------", constructorElapsedTime + elapsedMillisecondsPart1 + elapsedMillisecondsPart2, configuration);
        }

        table.AddEmptyRow();

        return new ElapsedTime(constructorElapsedTime, elapsedMillisecondsPart1, elapsedMillisecondsPart2);
    }

    private static async Task<(string solution, double elapsedTime)> SolvePart(bool isPart1, BaseProblem problem)
    {
        Stopwatch stopwatch = new();
        var solution = string.Empty;

        try
        {
            Func<ValueTask<string>> solve = isPart1
                ? problem.Solve_1
                : problem.Solve_2;

            stopwatch.Start();
            solution = await solve();
        }
        catch (NotImplementedException)
        {
            solution = "[[Not implemented]]";
        }
        catch (Exception e)
        {
            solution = e.Message + Environment.NewLine + e.StackTrace;
        }
        finally
        {
            stopwatch.Stop();
        }

        var elapsedMilliseconds = CalculateElapsedMilliseconds(stopwatch);

        return (solution, elapsedMilliseconds);
    }

    /// <summary>
    /// http://geekswithblogs.net/BlackRabbitCoder/archive/2012/01/12/c.net-little-pitfalls-stopwatch-ticks-are-not-timespan-ticks.aspx
    /// </summary>
    /// <param name="stopwatch"></param>
    /// <returns></returns>
    private static double CalculateElapsedMilliseconds(Stopwatch stopwatch)
    {
        return 1000 * stopwatch.ElapsedTicks / (double)Stopwatch.Frequency;
    }

    private static string FormatTime(double elapsedMilliseconds, SolverConfiguration configuration, bool useColor = true)
    {
        var customFormatSpecifier = configuration?.ElapsedTimeFormatSpecifier;

        var message = customFormatSpecifier is null
            ? elapsedMilliseconds switch
            {
                < 1 => $"{elapsedMilliseconds:F} ms",
                < 1_000 => $"{Math.Round(elapsedMilliseconds)} ms",
                < 60_000 => $"{0.001 * elapsedMilliseconds:F} s",
                _ => $"{Math.Floor(elapsedMilliseconds / 60_000)} min {Math.Round(0.001 * (elapsedMilliseconds % 60_000))} s",
            }
            : elapsedMilliseconds switch
            {
                < 1 => $"{elapsedMilliseconds.ToString(customFormatSpecifier)} ms",
                < 1_000 => $"{elapsedMilliseconds.ToString(customFormatSpecifier)} ms",
                < 60_000 => $"{(0.001 * elapsedMilliseconds).ToString(customFormatSpecifier)} s",
                _ => $"{elapsedMilliseconds / 60_000} min {(0.001 * (elapsedMilliseconds % 60_000)).ToString(customFormatSpecifier)} s",
            };

        if (useColor)
        {
            var color = elapsedMilliseconds switch
            {
                < 1 => Color.Blue,
                < 10 => Color.Green1,
                < 100 => Color.Lime,
                < 500 => Color.GreenYellow,
                < 1_000 => Color.Yellow1,
                < 10_000 => Color.OrangeRed1,
                _ => Color.Red1
            };

            return $"[{color}]{message}[/]";
        }
        else
        {
            return message;
        }
    }

    private static Table GetTable()
    {
        return new Table()
            .AddColumns(
                "[bold]Day[/]",
                "[bold]Part[/]",
                "[bold]Solution[/]",
                "[bold]Elapsed time[/]")
            .RoundedBorder()
            .BorderColor(Color.Grey);
    }

    private static void RenderRow(Table table, string problemTitle, string part, string solution, double elapsedMilliseconds, SolverConfiguration configuration)
    {
        var formattedTime = FormatTime(elapsedMilliseconds, configuration);

        table.AddRow(problemTitle, part, solution.EscapeMarkup(), formattedTime);
    }

    private static void RenderOverallResultsPanel(List<ElapsedTime> totalElapsedTime, SolverConfiguration configuration)
    {
        if (configuration?.ShowOverallResults != true || totalElapsedTime.Count <= 1)
        {
            return;
        }

        var totalConstructors = totalElapsedTime.Sum(t => t.Constructor);
        var totalPart1 = totalElapsedTime.Sum(t => t.Part1);
        var totalPart2 = totalElapsedTime.Sum(t => t.Part2);
        var total = totalPart1 + totalPart2 + (configuration.ShowConstructorElapsedTime ? totalConstructors : 0);

        var grid = new Grid()
            .AddColumn(new GridColumn().NoWrap().PadRight(4))
            .AddColumn()
            .AddRow()
            .AddRow($"[bold]Total ({totalElapsedTime.Count} days[/])", FormatTime(total, configuration, useColor: false));

        if (configuration.ShowConstructorElapsedTime)
        {
            grid.AddRow("Total constructors", FormatTime(totalConstructors, configuration, useColor: false));
        }

        grid
            .AddRow("Total parts 1", FormatTime(totalPart1, configuration, useColor: false))
            .AddRow("Total parts 2", FormatTime(totalPart2, configuration, useColor: false))
            .AddRow()
            .AddRow("[bold]Mean (per day)[/]", FormatTime(total / totalElapsedTime.Count, configuration));

        if (configuration.ShowConstructorElapsedTime)
        {
            grid.AddRow("Mean constructors", FormatTime(totalElapsedTime.Average(t => t.Constructor), configuration));
        }

        grid
            .AddRow("Mean parts 1", FormatTime(totalElapsedTime.Average(t => t.Part1), configuration))
            .AddRow("Mean parts 2", FormatTime(totalElapsedTime.Average(t => t.Part2), configuration));

        AnsiConsole.Write(
                new Panel(grid)
                    .Header("[b] Overall results [/]", Justify.Center));
    }
}
