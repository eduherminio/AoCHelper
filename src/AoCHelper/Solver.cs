using Spectre.Console;
using System.Diagnostics;
using System.Reflection;

namespace AoCHelper
{
    public static class Solver
    {
        private static readonly bool IsInteractiveEnvironment = Environment.UserInteractive && !Console.IsOutputRedirected;

        private record ElapsedTime(double Constructor, double Part1, double Part2);

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
                    var lastProblem = LoadAllProblems(Assembly.GetEntryAssembly()!).LastOrDefault();
                    if (lastProblem is not null)
                    {
                        var sw = new Stopwatch();
                        sw.Start();
                        var potentialProblem = Activator.CreateInstance(lastProblem);
                        sw.Stop();

                        if (potentialProblem is BaseProblem problem)
                        {
                            await SolveProblem(problem, table, CalculateElapsedMilliseconds(sw), configuration);
                            ctx.Refresh();
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
                    TProblem problem = new();
                    sw.Stop();

                    await SolveProblem(problem, table, CalculateElapsedMilliseconds(sw), configuration);
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
                    foreach (Type problemType in LoadAllProblems(Assembly.GetEntryAssembly()!))
                    {
                        if (problems.Contains(problemType))
                        {
                            sw.Restart();
                            var potentialProblem = Activator.CreateInstance(problemType);
                            sw.Stop();

                            if (potentialProblem is BaseProblem problem)
                            {
                                totalElapsedTime.Add(await SolveProblem(problem, table, CalculateElapsedMilliseconds(sw), configuration));
                                ctx.Refresh();
                            }
                        }
                    }
                });

            RenderOverallResultsPanel(totalElapsedTime, configuration);
        }

        /// <summary>
        /// Solves those problems whose <see cref="BaseProblem.CalculateIndex"/> method matches one of the provided numbers.
        /// 0 can be used for those problems whose <see cref="BaseProblem.CalculateIndex"/> returns the default value due to not being able to deduct the index.
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
                    foreach (Type problemType in LoadAllProblems(Assembly.GetEntryAssembly()!))
                    {
                        sw.Restart();
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
                    foreach (Type problemType in LoadAllProblems(Assembly.GetEntryAssembly()!))
                    {
                        sw.Restart();
                        var potentialProblem = Activator.CreateInstance(problemType);
                        sw.Stop();

                        if (potentialProblem is BaseProblem problem)
                        {
                            totalElapsedTime.Add(await SolveProblem(problem, table, CalculateElapsedMilliseconds(sw), configuration));
                            ctx.Refresh();
                        }
                    }
                });

            RenderOverallResultsPanel(totalElapsedTime, configuration);
        }

        #region Obsolete

        /// <summary>
        /// Use <see cref="SolveLast(Action{SolverConfiguration}?)"/> instead
        /// <example>
        /// <code>
        /// await Solver.SolveLast(opt => opt.ShowConstructorElapsedTime = true);
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="configuration"></param>
        [Obsolete("Use Action<SolverConfiguration>? overload instead")]
        public static async Task SolveLast(SolverConfiguration? configuration)
        {
            configuration ??= new();
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
                    var lastProblem = LoadAllProblems(Assembly.GetEntryAssembly()!).LastOrDefault();
                    if (lastProblem is not null)
                    {
                        var sw = new Stopwatch();
                        sw.Start();
                        var potentialProblem = Activator.CreateInstance(lastProblem);
                        sw.Stop();

                        if (potentialProblem is BaseProblem problem)
                        {
                            await SolveProblem(problem, table, CalculateElapsedMilliseconds(sw), configuration);
                            ctx.Refresh();
                        }
                    }
                });
        }

        /// <summary>
        /// Use <see cref="Solve{TProblem}(Action{SolverConfiguration}?)"/> instead
        /// <example>
        /// <code>
        /// await Solver.Solve{Day01}(opt => opt.ShowConstructorElapsedTime = true);
        /// </code>
        /// </example>
        /// </summary>
        /// <typeparam name="TProblem"></typeparam>
        /// <param name="configuration"></param>
        [Obsolete("Use Action<SolverConfiguration>? overload instead")]
        public static async Task Solve<TProblem>(SolverConfiguration? configuration)
            where TProblem : BaseProblem, new()
        {
            configuration ??= new();
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
                    TProblem problem = new();
                    sw.Stop();

                    await SolveProblem(problem, table, CalculateElapsedMilliseconds(sw), configuration);
                    ctx.Refresh();
                });
        }

        /// <summary>
        /// Use <see cref="Solve(Action{SolverConfiguration}?, uint[])"/> instead
        /// <example>
        /// <code>
        /// await Solver.Solve(opt => opt.ShowConstructorElapsedTime = true, 1, 2);
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="problemNumbers"></param>
        [Obsolete("Use Action<SolverConfiguration>? overload instead")]
        public static async Task Solve(SolverConfiguration? configuration, params uint[] problemNumbers)
            => await Solve(problemNumbers.AsEnumerable(), configuration);

        /// <summary>
        /// Use <see cref="Solve(Action{SolverConfiguration}?, Type[])"/> instead
        /// <example>
        /// <code>
        /// await Solver.Solve(opt => opt.ShowConstructorElapsedTime = true, typeof(Problem66));
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="problems"></param>
        [Obsolete("Use Action<SolverConfiguration>? overload instead")]
        public static async Task Solve(SolverConfiguration? configuration, params Type[] problems)
            => await Solve(problems.AsEnumerable(), configuration);

        /// <summary>
        /// Use <see cref="Solve(IEnumerable{uint}, Action{SolverConfiguration}?)"/> instead
        /// <example>
        /// <code>
        /// await Solver.Solve(new uint[] { 1 }, opt => opt.ShowConstructorElapsedTime = true);
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="problemNumbers"></param>
        /// <param name="configuration"></param>
        [Obsolete("Use Action<SolverConfiguration>? overload instead")]
        public static async Task Solve(IEnumerable<uint> problemNumbers, SolverConfiguration? configuration)
        {
            configuration ??= new();
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
                    foreach (Type problemType in LoadAllProblems(Assembly.GetEntryAssembly()!))
                    {
                        sw.Restart();
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
        /// Use <see cref="Solve(IEnumerable{Type}, Action{SolverConfiguration}?)"/> instead
        /// <example>
        /// <code>
        /// await Solver.Solve(new [] { typeof(Day10) }, opt => opt.ShowConstructorElapsedTime = true);
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="problems"></param>
        /// <param name="configuration"></param>
        [Obsolete("Use Action<SolverConfiguration>? overload instead")]
        public static async Task Solve(IEnumerable<Type> problems, SolverConfiguration? configuration)
        {
            configuration ??= new();
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
                    foreach (Type problemType in LoadAllProblems(Assembly.GetEntryAssembly()!))
                    {
                        if (problems.Contains(problemType))
                        {
                            sw.Restart();
                            var potentialProblem = Activator.CreateInstance(problemType);
                            sw.Stop();

                            if (potentialProblem is BaseProblem problem)
                            {
                                totalElapsedTime.Add(await SolveProblem(problem, table, CalculateElapsedMilliseconds(sw), configuration));
                                ctx.Refresh();
                            }
                        }
                    }
                });

            RenderOverallResultsPanel(totalElapsedTime, configuration);
        }

        /// <summary>
        /// Use <see cref="SolveAll(Action{SolverConfiguration}?)"/> instead
        /// <example>
        /// <code>
        /// await Solver.SolveAll(options =>
        /// {
        ///     options.ShowConstructorElapsedTime = true;
        ///     options.ShowOverallResults = true;
        ///     options.ClearConsole = false;
        /// });
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="configuration"></param>
        [Obsolete("Use Action<SolverConfiguration>? overload instead")]
        public static async Task SolveAll(SolverConfiguration? configuration)
        {
            configuration ??= new();
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
                    foreach (Type problemType in LoadAllProblems(Assembly.GetEntryAssembly()!))
                    {
                        sw.Restart();
                        var potentialProblem = Activator.CreateInstance(problemType);
                        sw.Stop();

                        if (potentialProblem is BaseProblem problem)
                        {
                            totalElapsedTime.Add(await SolveProblem(problem, table, CalculateElapsedMilliseconds(sw), configuration));
                            ctx.Refresh();
                        }
                    }
                });

            RenderOverallResultsPanel(totalElapsedTime, configuration);
        }

        #endregion

        /// <summary>
        /// Loads all <see cref="BaseProblem"/> in the given assembly
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        internal static IEnumerable<Type> LoadAllProblems(Assembly assembly)
        {
            return assembly.GetTypes()
                .Where(type => typeof(BaseProblem).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);
        }

        private static SolverConfiguration PopulateConfiguration(Action<SolverConfiguration>? options)
        {
            var configuration = new SolverConfiguration();
            options?.Invoke(configuration);

            return configuration;
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

            table.AddRow(problemTitle, part, solution, formattedTime);
        }

        private static void RenderOverallResultsPanel(List<ElapsedTime> totalElapsedTime, SolverConfiguration configuration)
        {
            if (configuration?.ShowOverallResults != true || totalElapsedTime.Count <= 1)
            {
                return;
            }

            var totalConstructors = totalElapsedTime.Select(t => t.Constructor).Sum();
            var totalPart1 = totalElapsedTime.Select(t => t.Part1).Sum();
            var totalPart2 = totalElapsedTime.Select(t => t.Part2).Sum();
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
                grid.AddRow("Mean constructors", FormatTime(totalElapsedTime.Select(t => t.Constructor).Average(), configuration));
            }

            grid
                .AddRow("Mean parts 1", FormatTime(totalElapsedTime.Select(t => t.Part1).Average(), configuration))
                .AddRow("Mean parts 2", FormatTime(totalElapsedTime.Select(t => t.Part2).Average(), configuration));

            AnsiConsole.Write(
                    new Panel(grid)
                        .Header("[b] Overall results [/]", Justify.Center));
        }
    }
}
