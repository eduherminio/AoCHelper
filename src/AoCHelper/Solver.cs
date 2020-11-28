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
        private static Table GetTable() => new Table()
            .AddColumns("[bold white]Day[/]", "[bold white]Part[/]", "[bold white]Solution[/]", "[bold white]Elapsed time[/]")
            .RoundedBorder()
            .BorderColor(Color.Grey);

        #region Public methods

        /// <summary>
        /// Solves both parts of a problem.
        /// Prints the time consumed by each part next to the result produced by it
        /// </summary>
        /// <typeparam name="TProblem"></typeparam>
        public static void Solve<TProblem>()
            where TProblem : BaseProblem, new()
        {
            TProblem problem = new TProblem();

            Solve(problem, GetTable());
        }

        /// <summary>
        /// Solves all problems in the assembly
        /// Prints the time consumed by each part next to the result produced by it
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

        /// <summary>
        /// Solves the provided problems.
        /// Prints the time consumed by each part next to the result produced by it
        /// </summary>
        public static void Solve(params Type[] problems)
        {
            Solve(problems.AsEnumerable());
        }

        /// <summary>
        /// Solves the provided problems.
        /// Prints the time consumed by each part next to the result produced by it
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

            var stopwatch = Stopwatch.StartNew();
            var solution1 = problem.Solve_1();
            stopwatch.Stop();

            RenderRow(table, problemTitle, 1, solution1, stopwatch);

            stopwatch.Reset();
            stopwatch.Restart();
            var solution2 = problem.Solve_2();
            stopwatch.Stop();

            RenderRow(table, problemTitle, 2, solution2, stopwatch);

            table.AddEmptyRow();
        }

        private static void RenderRow(Table table, string problemTitle, int part, string solution, Stopwatch stopwatch)
        {
            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            var color = elapsedMilliseconds switch
            {
                0 => Color.Blue,
                <= 100 => Color.Lime,
                <= 500 => Color.GreenYellow,
                <= 1000 => Color.Yellow1,
                <= 2000 => Color.OrangeRed1,
                _ => Color.Red1
            };

            var elapsedTime = elapsedMilliseconds < 1000
                ? $"{elapsedMilliseconds} ms"
                : $"{0.001 * elapsedMilliseconds:F} s";

            table.AddRow(problemTitle, $"Part {part}", solution, $"[{color}]{elapsedTime}[/]");

            if (Environment.UserInteractive)
            {
                Console.Clear();
            }

            AnsiConsole.Render(table);
        }
    }
}
