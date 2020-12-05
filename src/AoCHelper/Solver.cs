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
        /// <summary>
        /// Numeric format strings, see https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings
        /// </summary>
        public static string? ElapsedTimeFormatSpecifier { get; set; } = null;

        private static readonly bool IsInteractiveEnvironment = Environment.UserInteractive && !Console.IsOutputRedirected;

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
            TProblem problem = new TProblem();

            SolveProblem(problem, GetTable(), configuration);
        }

        /// <summary>
        /// Solves those problems whose <see cref="BaseProblem.CalculateIndex"/> method matches one of the provided numbers.
        /// 0 can be used for those problems whose <see cref="BaseProblem.CalculateIndex"/> returns the default value due to not being able to deduct the index.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="problemNumbers"></param>
        public static void Solve(SolverConfiguration? configuration = null, params uint[] problemNumbers) => Solve(problemNumbers.AsEnumerable(), configuration);

        /// <summary>
        /// Solves those problems whose <see cref="BaseProblem.CalculateIndex"/> method matches one of the provided numbers.
        /// 0 can be used for those problems whose <see cref="BaseProblem.CalculateIndex"/> returns the default value due to not being able to deduct the index.
        /// </summary>
        /// <param name="problemNumbers"></param>
        /// <param name="configuration"></param>
        public static void Solve(IEnumerable<uint> problemNumbers, SolverConfiguration? configuration = null)
        {
            var table = GetTable();

            foreach (Type problemType in LoadAllProblems(Assembly.GetEntryAssembly()!))
            {
                if (Activator.CreateInstance(problemType) is BaseProblem problem && problemNumbers.Contains(problem.CalculateIndex()))
                {
                    SolveProblem(problem, table, configuration);
                }
            }
        }

        /// <summary>
        /// Solves last problem.
        /// It also prints the elapsed time in <see cref="BaseProblem.Solve_1"/> and <see cref="BaseProblem.Solve_2"/> methods.
        /// </summary>
        /// <param name="configuration"></param>
        public static void SolveLast(SolverConfiguration? configuration = null)
        {
            var lastProblem = LoadAllProblems(Assembly.GetEntryAssembly()!).LastOrDefault();
            if (lastProblem is not null && Activator.CreateInstance(lastProblem) is BaseProblem problem)
            {
                SolveProblem(problem, GetTable(), configuration);
            }
        }

        /// <summary>
        /// Solves the provided problems.
        /// It also prints the elapsed time in <see cref="BaseProblem.Solve_1"/> and <see cref="BaseProblem.Solve_2"/> methods.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="problems"></param>
        public static void Solve(SolverConfiguration? configuration = null, params Type[] problems) => Solve(problems.AsEnumerable(), configuration);

        /// <summary>
        /// Solves the provided problems.
        /// It also prints the elapsed time in <see cref="BaseProblem.Solve_1"/> and <see cref="BaseProblem.Solve_2"/> methods.
        /// </summary>
        /// <param name="problems"></param>
        /// <param name="configuration"></param>
        public static void Solve(IEnumerable<Type> problems, SolverConfiguration? configuration = null)
        {
            var table = GetTable();

            foreach (Type problemType in LoadAllProblems(Assembly.GetEntryAssembly()!))
            {
                if (problems.Contains(problemType) && Activator.CreateInstance(problemType) is BaseProblem problem)
                {
                    SolveProblem(problem, table, configuration);
                }
            }
        }

        /// <summary>
        /// Solves all problems in the assembly.
        /// It also prints the elapsed time in <see cref="BaseProblem.Solve_1"/> and <see cref="BaseProblem.Solve_2"/> methods.
        /// </summary>
        /// <param name="configuration"></param>
        public static void SolveAll(SolverConfiguration? configuration = null)
        {
            var totalElapsedTime = new List<(double part1, double part2)>();
            var table = GetTable();

            foreach (Type problemType in LoadAllProblems(Assembly.GetEntryAssembly()!))
            {
                if (Activator.CreateInstance(problemType) is BaseProblem problem)
                {
                    totalElapsedTime.Add(SolveProblem(problem, table, configuration));
                }
            }

            RenderOverallResultsPanel(totalElapsedTime);
        }

        #endregion

        #region Obsolete methods

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
                .AddColumns("[bold white]Day[/]", "[bold white]Part[/]", "[bold white]Solution[/]", "[bold white]Elapsed time[/]")
                .RoundedBorder()
                .BorderColor(Color.Grey);
        }

        private static (double part1, double part2) SolveProblem(BaseProblem problem, Table table, SolverConfiguration? configuration)
        {
            configuration ??= new SolverConfiguration();
            var problemIndex = problem.CalculateIndex();
            var problemTitle = problemIndex != default
                ? $"Day {problemIndex}"
                : $"{problem.GetType().Name}";

            (string solution1, double elapsedMillisecondsPart1) = SolvePart(isPart1: true, problem);
            RenderRow(table, problemTitle, 1, solution1, elapsedMillisecondsPart1, configuration.ClearConsole);

            (string solution2, double elapsedMillisecondsPart2) = SolvePart(isPart1: false, problem);
            RenderRow(table, problemTitle, 2, solution2, elapsedMillisecondsPart2, configuration.ClearConsole);

            table.AddEmptyRow();

            return (elapsedMillisecondsPart1, elapsedMillisecondsPart2);
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

        private static void RenderRow(Table table, string problemTitle, int part, string solution, double elapsedMilliseconds, bool clearConsole)
        {
            var formattedTime = FormatTime(elapsedMilliseconds);

            table.AddRow(problemTitle, $"Part {part}", solution, formattedTime);

            if (IsInteractiveEnvironment)
            {
                if (clearConsole)
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

        private static string FormatTime(double elapsedMilliseconds, bool useColor = true)
        {
            var message = ElapsedTimeFormatSpecifier is null
                ? elapsedMilliseconds switch
                {
                    < 1 => $"{elapsedMilliseconds:F} ms",
                    < 1_000 => $"{Math.Round(elapsedMilliseconds)} ms",
                    < 60_000 => $"{0.001 * elapsedMilliseconds:F} s",
                    _ => $"{elapsedMilliseconds / 60_000} min {Math.Round(0.001 * (elapsedMilliseconds % 60_000))} s",
                }
                : elapsedMilliseconds switch
                {
                    < 1 => $"{elapsedMilliseconds.ToString(ElapsedTimeFormatSpecifier)} ms",
                    < 1_000 => $"{elapsedMilliseconds.ToString(ElapsedTimeFormatSpecifier)} ms",
                    < 60_000 => $"{(0.001 * elapsedMilliseconds).ToString(ElapsedTimeFormatSpecifier)} s",
                    _ => $"{elapsedMilliseconds / 60_000} min {(0.001 * (elapsedMilliseconds % 60_000)).ToString(ElapsedTimeFormatSpecifier)} s",
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

        private static void RenderOverallResultsPanel(List<(double part1, double part2)> totalElapsedTime)
        {
            var totalPart1 = totalElapsedTime.Select(t => t.part1).Sum();
            var totalPart2 = totalElapsedTime.Select(t => t.part2).Sum();
            var total = totalPart1 + totalPart2;

            var grid = new Grid()
                .AddColumn(new GridColumn().NoWrap().PadRight(4))
                .AddColumn()
                .AddRow()
                .AddRow($"Total ({totalElapsedTime.Count} days)", FormatTime(total, useColor: false))
                .AddRow("Total parts 1", FormatTime(totalPart1, useColor: false))
                .AddRow("Total parts 2", FormatTime(totalPart2, useColor: false))
                .AddRow()
                .AddRow("Mean  (per day)", FormatTime(total / totalElapsedTime.Count))
                .AddRow("Mean  parts 1", FormatTime(totalElapsedTime.Select(t => t.part1).Average()))
                .AddRow("Mean  parts 2", FormatTime(totalElapsedTime.Select(t => t.part2).Average()));

            AnsiConsole.Render(
                    new Panel(grid)
                        .Header("[b] Overall results [/]", Justify.Center));
        }
    }
}
