using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace AoCHelper
{
    public static class Solver
    {
        private static readonly bool IsInteractiveEnvironment = Environment.UserInteractive && !Console.IsOutputRedirected;
        private record ElapsedTime(double Constructor, double Part1, double Part2);

        #region Public methods

        /// <summary>
        /// Solves a problem.
        /// It also prints the elapsed time in <see cref="BaseProblem.Solve_1"/> and <see cref="BaseProblem.Solve_2"/> methods.
        /// </summary>
        /// <typeparam name="TProblem"></typeparam>
        /// <param name="configuration"></param>
        public static void Solve<TProblem>(SolverConfiguration? configuration = null)
            where TProblem : BaseProblem, new()
        {
            configuration ??= new();

            var sw = new Stopwatch();
            sw.Start();
            TProblem problem = new TProblem();
            sw.Stop();

            SolveProblem(problem, GetTable(), CalculateElapsedMilliseconds(sw), configuration);
        }

        /// <summary>
        /// Solves last problem.
        /// It also prints the elapsed time in <see cref="BaseProblem.Solve_1"/> and <see cref="BaseProblem.Solve_2"/> methods.
        /// </summary>
        /// <param name="configuration"></param>
        public static void SolveLast(SolverConfiguration? configuration = null)
        {
            configuration ??= new();

            var lastProblem = LoadAllProblems(Assembly.GetEntryAssembly()!).LastOrDefault();
            if (lastProblem is not null)
            {
                var sw = new Stopwatch();
                sw.Start();
                var potentialProblem = Activator.CreateInstance(lastProblem);
                sw.Stop();

                if (potentialProblem is BaseProblem problem)
                {
                    SolveProblem(problem, GetTable(), CalculateElapsedMilliseconds(sw), configuration);
                }
            }
        }

        /// <summary>
        /// Solves those problems whose <see cref="BaseProblem.CalculateIndex"/> method matches one of the provided numbers.
        /// 0 can be used for those problems whose <see cref="BaseProblem.CalculateIndex"/> returns the default value due to not being able to deduct the index.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="problemNumbers"></param>
        public static void Solve(SolverConfiguration? configuration = null, params uint[] problemNumbers)
            => Solve(problemNumbers.AsEnumerable(), configuration);

        /// <summary>
        /// Solves the provided problems.
        /// It also prints the elapsed time in <see cref="BaseProblem.Solve_1"/> and <see cref="BaseProblem.Solve_2"/> methods.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="problems"></param>
        public static void Solve(SolverConfiguration? configuration = null, params Type[] problems)
            => Solve(problems.AsEnumerable(), configuration);

        /// <summary>
        /// Solves those problems whose <see cref="BaseProblem.CalculateIndex"/> method matches one of the provided numbers.
        /// 0 can be used for those problems whose <see cref="BaseProblem.CalculateIndex"/> returns the default value due to not being able to deduct the index.
        /// </summary>
        /// <param name="problemNumbers"></param>
        /// <param name="configuration"></param>
        public static void Solve(IEnumerable<uint> problemNumbers, SolverConfiguration? configuration = null)
        {
            configuration ??= new();

            var totalElapsedTime = new List<ElapsedTime>();
            var table = GetTable();

            var sw = new Stopwatch();
            foreach (Type problemType in LoadAllProblems(Assembly.GetEntryAssembly()!))
            {
                sw.Restart();
                var potentialProblem = Activator.CreateInstance(problemType);
                sw.Stop();

                if (potentialProblem is BaseProblem problem && problemNumbers.Contains(problem.CalculateIndex()))
                {
                    totalElapsedTime.Add(SolveProblem(problem, table, CalculateElapsedMilliseconds(sw), configuration));
                }
            }

            RenderOverallResultsPanel(totalElapsedTime, configuration);
        }

        /// <summary>
        /// Solves the provided problems.
        /// It also prints the elapsed time in <see cref="BaseProblem.Solve_1"/> and <see cref="BaseProblem.Solve_2"/> methods.
        /// </summary>
        /// <param name="problems"></param>
        /// <param name="configuration"></param>
        public static void Solve(IEnumerable<Type> problems, SolverConfiguration? configuration = null)
        {
            configuration ??= new();

            var totalElapsedTime = new List<ElapsedTime>();
            var table = GetTable();

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
                        totalElapsedTime.Add(SolveProblem(problem, table, CalculateElapsedMilliseconds(sw), configuration));
                    }
                }
            }

            RenderOverallResultsPanel(totalElapsedTime, configuration);
        }

        /// <summary>
        /// Solves all problems in the assembly.
        /// It also prints the elapsed time in <see cref="BaseProblem.Solve_1"/> and <see cref="BaseProblem.Solve_2"/> methods.
        /// </summary>
        /// <param name="configuration"></param>
        public static void SolveAll(SolverConfiguration? configuration = null)
        {
            configuration ??= new();

            var totalElapsedTime = new List<ElapsedTime>();
            var table = GetTable();

            var sw = new Stopwatch();
            foreach (Type problemType in LoadAllProblems(Assembly.GetEntryAssembly()!))
            {
                sw.Restart();
                var potentialProblem = Activator.CreateInstance(problemType);
                sw.Stop();

                if (potentialProblem is BaseProblem problem)
                {
                    totalElapsedTime.Add(SolveProblem(problem, table, CalculateElapsedMilliseconds(sw), configuration));
                }
            }

            RenderOverallResultsPanel(totalElapsedTime, configuration);
        }

        #endregion

        #region Obsolete methods and properties

        /// <summary>
        /// This property is obsolete. Use <see cref="SolverConfiguration.ElapsedTimeFormatSpecifier"/> instead
        /// </summary>
        [Obsolete("This property is obsolete. Use SolverConfiguration.ElapsedTimeFormatSpecifier instead")]
        public static string? ElapsedTimeFormatSpecifier { get; set; }

        /// <summary>
        /// This method is obsolete. Use <see cref="Solve{TProblem}(SolverConfiguration?)"/> instead.
        /// </summary>
        /// <typeparam name="TProblem"></typeparam>
        /// <param name="clearConsole"></param>
        [Obsolete("This method is obsolete. Use Solve<TProblem>(SolverConfiguration?) instead")]
        public static void Solve<TProblem>(bool clearConsole)
             where TProblem : BaseProblem, new()
        {
            Solve<TProblem>(new SolverConfiguration { ClearConsole = clearConsole });
        }

        /// <summary>
        /// This method is obsolete. Use <see cref="SolveLast(SolverConfiguration?)"/> instead.
        /// </summary>
        /// <param name="clearConsole"></param>
        [Obsolete("This method is obsolete. Use SolveLast(SolverConfiguration?) instead")]
        public static void SolveLast(bool clearConsole) => SolveLast(new SolverConfiguration { ClearConsole = clearConsole });

        /// <summary>
        /// This method is obsolete. Use <see cref="Solve(SolverConfiguration?, Type[])"/> instead or <see cref="Solve(IEnumerable{Type}, SolverConfiguration?)"/> instead.
        /// </summary>
        /// <param name="problems"></param>
        [Obsolete("This method is obsolete. Use Solve(SolverConfiguration?, params Type[]) or Solve(IEnumerable<Type>, SolverConfiguration?) instead")]
        public static void Solve(params Type[] problems) => Solve(null, problems);

        /// <summary>
        /// This method is obsolete. Use <see cref="Solve(SolverConfiguration?, uint[])"/> or <see cref="Solve(IEnumerable{uint}, SolverConfiguration?)"/> instead.
        /// </summary>
        /// <param name="problemNumbers"></param>
        [Obsolete("This method is obsolete. Use Solve(SolverConfiguration?, params uint[]) or Solve(IEnumerable<uint>, SolverConfiguration?) instead")]
        public static void Solve(params uint[] problemNumbers) => Solve(null, problemNumbers);

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

        private static ElapsedTime SolveProblem(BaseProblem problem, Table table, double constructorElapsedTime, SolverConfiguration configuration)
        {
            var problemIndex = problem.CalculateIndex();
            var problemTitle = problemIndex != default
                ? $"Day {problemIndex}"
                : $"{problem.GetType().Name}";

            if (configuration.ShowConstructorElapsedTime)
            {
                RenderRow(table, problemTitle, $"{problem.GetType().Name}()", "-----------", constructorElapsedTime, configuration);
            }

            (string solution1, double elapsedMillisecondsPart1) = SolvePart(isPart1: true, problem);
            RenderRow(table, problemTitle, "Part 1", solution1, elapsedMillisecondsPart1, configuration);

            (string solution2, double elapsedMillisecondsPart2) = SolvePart(isPart1: false, problem);
            RenderRow(table, problemTitle, "Part 2", solution2, elapsedMillisecondsPart2, configuration);

            if (configuration.ShowTotalElapsedTimePerDay)
            {
                RenderRow(table, problemTitle, "[bold]Total[/]", "-----------", constructorElapsedTime + elapsedMillisecondsPart1 + elapsedMillisecondsPart2, configuration);
            }

            table.AddEmptyRow();

            return new ElapsedTime(constructorElapsedTime, elapsedMillisecondsPart1, elapsedMillisecondsPart2);
        }

        private static (string solution, double elapsedTime) SolvePart(bool isPart1, BaseProblem problem)
        {
            Stopwatch stopwatch = new Stopwatch();
            var solution = string.Empty;

            try
            {
                Func<string> solve = isPart1
                    ? problem.Solve_1
                    : problem.Solve_2;

                stopwatch.Start();
                solution = solve();
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

        private static void RenderRow(Table table, string problemTitle, string part, string solution, double elapsedMilliseconds, SolverConfiguration configuration)
        {
            var formattedTime = FormatTime(elapsedMilliseconds, configuration);

            table.AddRow(problemTitle, part, solution, formattedTime);

            if (IsInteractiveEnvironment)
            {
                if (configuration?.ClearConsole == true)
                {
                    Console.Clear();
                }
                else
                {
                    AnsiConsole.Console.Clear(true);
                }
            }

            AnsiConsole.Render(table);
        }

        private static string FormatTime(double elapsedMilliseconds, SolverConfiguration configuration, bool useColor = true)
        {
#pragma warning disable CS0618 // Type or member is obsolete - Needed to keep compatibility
            var customFormatSpecifier = configuration?.ElapsedTimeFormatSpecifier ?? ElapsedTimeFormatSpecifier;
#pragma warning restore CS0618 // Type or member is obsolete

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

            AnsiConsole.Render(
                    new Panel(grid)
                        .Header("[b] Overall results [/]", Justify.Center));
        }
    }
}
