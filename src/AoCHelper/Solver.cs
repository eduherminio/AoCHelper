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

        private static Table GetTable() => new Table()
                    .AddColumns("[bold white]Day[/]", "[bold white]Part[/]", "[bold white]Solution[/]", "[bold white]Elapsed time[/]")
                    .RoundedBorder()
                    .BorderColor(Color.Grey);

        #region Public methods

        /// <summary>
        /// Solves a problem.
        /// It also prints the elapsed time in <see cref="BaseProblem.Solve_1"/> and <see cref="BaseProblem.Solve_2"/> methods.
        /// </summary>
        /// <typeparam name="TProblem"></typeparam>
        /// <param name="clearConsole"></param>
        public static void Solve<TProblem>(bool clearConsole = true)
            where TProblem : BaseProblem, new()
        {
            TProblem problem = new TProblem();

            Solve(problem, GetTable(), clearConsole);
        }

        /// <summary>
        /// Solves those problems whose <see cref="BaseProblem.CalculateIndex"/> method matches one of the provided numbers.
        /// 0 can be used for those problems whose <see cref="BaseProblem.CalculateIndex"/> returns the default value due to not being able to deduct the index.
        /// </summary>
        /// <param name="problemNumbers"></param>
        public static void Solve(params uint[] problemNumbers) => Solve(problemNumbers.AsEnumerable());

        /// <summary>
        /// Solves those problems whose <see cref="BaseProblem.CalculateIndex"/> method matches one of the provided numbers.
        /// 0 can be used for those problems whose <see cref="BaseProblem.CalculateIndex"/> returns the default value due to not being able to deduct the index.
        /// </summary>
        /// <param name="problemNumbers"></param>
        public static void Solve(IEnumerable<uint> problemNumbers)
        {
            var table = GetTable();

            foreach (Type problemType in LoadAllProblems(Assembly.GetEntryAssembly()!))
            {
                if (Activator.CreateInstance(problemType) is BaseProblem problem && problemNumbers.Contains(problem.CalculateIndex()))
                {
                    Solve(problem, table, clearConsole: true);
                }
            }
        }

        /// <summary>
        /// Solves last problem.
        /// It also prints the elapsed time in <see cref="BaseProblem.Solve_1"/> and <see cref="BaseProblem.Solve_2"/> methods.
        /// </summary>
        /// <param name="clearConsole"></param>
        public static void SolveLast(bool clearConsole = true)
        {
            var lastProblem = LoadAllProblems(Assembly.GetEntryAssembly()!).LastOrDefault();
            if (lastProblem is not null && Activator.CreateInstance(lastProblem) is BaseProblem problem)
            {
                Solve(problem, GetTable(), clearConsole);
            }
        }

        /// <summary>
        /// Solves the provided problems.
        /// It also prints the elapsed time in <see cref="BaseProblem.Solve_1"/> and <see cref="BaseProblem.Solve_2"/> methods.
        /// </summary>
        public static void Solve(params Type[] problems) => Solve(problems.AsEnumerable());

        /// <summary>
        /// Solves the provided problems.
        /// It also prints the elapsed time in <see cref="BaseProblem.Solve_1"/> and <see cref="BaseProblem.Solve_2"/> methods.
        /// </summary>
        public static void Solve(IEnumerable<Type> problems)
        {
            var table = GetTable();

            foreach (Type problemType in LoadAllProblems(Assembly.GetEntryAssembly()!))
            {
                if (problems.Contains(problemType) && Activator.CreateInstance(problemType) is BaseProblem problem)
                {
                    Solve(problem, table, clearConsole: true);
                }
            }
        }

        /// <summary>
        /// Solves all problems in the assembly.
        /// It also prints the elapsed time in <see cref="BaseProblem.Solve_1"/> and <see cref="BaseProblem.Solve_2"/> methods.
        /// </summary>
        public static void SolveAll()
        {
            var table = GetTable();

            foreach (Type problemType in LoadAllProblems(Assembly.GetEntryAssembly()!))
            {
                if (Activator.CreateInstance(problemType) is BaseProblem problem)
                {
                    Solve(problem, table, clearConsole: true);
                }
            }
        }

        #endregion

        /// <summary>
        /// Loads all <see cref="BaseProblem"/> in the entry assembly
        /// </summary>
        /// <returns></returns>
        internal static IEnumerable<Type> LoadAllProblems(Assembly assembly)
        {
            return assembly.GetTypes()
                .Where(type => typeof(BaseProblem).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);
        }

        private static void Solve(BaseProblem problem, Table table, bool clearConsole)
        {
            var problemIndex = problem.CalculateIndex();
            var problemTitle = problemIndex != default
                ? $"Day {problemIndex}"
                : $"{problem.GetType().Name}";

            Stopwatch stopwatch = null!;

            var solution1 = string.Empty;
            try
            {
                stopwatch = Stopwatch.StartNew();
                solution1 = problem.Solve_1();
            }
            catch (NotImplementedException)
            {
                solution1 = "[[Not implemented]]";
            }
            catch (Exception e)
            {
                solution1 = e.Message + Environment.NewLine + e.StackTrace;
            }
            finally
            {
                stopwatch.Stop();
            }

            RenderRow(table, problemTitle, 1, solution1, stopwatch!, clearConsole);

            var solution2 = string.Empty;
            try
            {
                stopwatch.Reset();
                stopwatch.Restart();
                solution2 = problem.Solve_2();
            }
            catch (NotImplementedException)
            {
                solution2 = "[[Not implemented]]";
            }
            catch (Exception e)
            {
                solution2 = e.Message + Environment.NewLine + e.StackTrace;
            }
            finally
            {
                stopwatch.Stop();
            }

            RenderRow(table, problemTitle, 2, solution2, stopwatch, clearConsole);

            table.AddEmptyRow();
        }

        private static void RenderRow(Table table, string problemTitle, int part, string solution, Stopwatch stopwatch, bool clearConsole)
        {
            var elapsedMilliseconds = 1000 * stopwatch.ElapsedTicks / (double)Stopwatch.Frequency;

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

            var elapsedTime = ElapsedTimeFormatSpecifier is null
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

            table.AddRow(problemTitle, $"Part {part}", solution, $"[{color}]{elapsedTime}[/]");

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
    }
}
