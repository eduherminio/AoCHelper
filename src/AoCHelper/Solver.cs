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
        public static void Solve<TProblem>()
            where TProblem : BaseProblem, new()
        {
            TProblem problem = new TProblem();

            Solve(problem, GetTable());
        }

        /// <summary>
        /// Solves the provided problems.
        /// It also prints the elapsed time in <see cref="BaseProblem.Solve_1"/> and <see cref="BaseProblem.Solve_2"/> methods.
        /// </summary>
        public static void Solve(params Type[] problems)
        {
            Solve(problems.AsEnumerable());
        }

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
                    Solve(problem, table);
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
                    Solve(problem, table);
                }
            }
        }

        public static void SolveLast()
        {
            if (Activator.CreateInstance(LoadAllProblems(Assembly.GetEntryAssembly()!).Last()) is BaseProblem problem)
            {
                Solve(problem, GetTable());
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

        private static void Solve(BaseProblem problem, Table table)
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

            RenderRow(table, problemTitle, 1, solution1, stopwatch!);

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

            RenderRow(table, problemTitle, 2, solution2, stopwatch);

            table.AddEmptyRow();
        }

        private static void RenderRow(Table table, string problemTitle, int part, string solution, Stopwatch stopwatch)
        {
            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            var color = elapsedMilliseconds switch
            {
                0 => Color.Blue,
                < 100 => Color.Lime,
                < 500 => Color.GreenYellow,
                < 1000 => Color.Yellow1,
                < 2000 => Color.OrangeRed1,
                _ => Color.Red1
            };

            var elapsedTime = elapsedMilliseconds < 60_000
                ? elapsedMilliseconds < 1_000
                    ? $"{elapsedMilliseconds} ms"
                    : $"{0.001 * elapsedMilliseconds:F} s"
                : $"{elapsedMilliseconds / 60_000} min {Math.Round(0.001 * (elapsedMilliseconds % 60_000))} s";

            table.AddRow(problemTitle, $"Part {part}", solution, $"[{color}]{elapsedTime}[/]");

            if (IsInteractiveEnvironment)
            {
                Console.Clear();
            }

            AnsiConsole.Render(table);
        }
    }
}
