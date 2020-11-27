using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace AoCHelper
{
    public static class Solver
    {
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

            SolveWithMetrics(problem);
        }

        /// <summary>
        /// Solves all problems in the assembly
        /// Prints the time consumed by each part next to the result produced by it
        /// </summary>
        public static void SolveAll()
        {
            foreach (Type problemType in LoadAllProblems(Assembly.GetEntryAssembly()!))
            {
                if (Activator.CreateInstance(problemType) is BaseProblem problem)
                {
                    SolveWithMetrics(problem);
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
            foreach (Type problemType in LoadAllProblems(Assembly.GetEntryAssembly()!))
            {
                if (problems.Contains(problemType) && Activator.CreateInstance(problemType) is BaseProblem problem)
                {
                    SolveWithMetrics(problem);
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

        private static void SolveWithMetrics(BaseProblem problem)
        {
            var problemIndex = problem.CalculateIndex();
            var lineStart = problemIndex != default
                ? $"Day {problemIndex}"
                : $"{problem.GetType().Name}";

            var stopwatch = Stopwatch.StartNew();

            var solution1 = problem.Solve_1();

            stopwatch.Stop();

            Console.Write($"{lineStart}, part 1:\t\t{solution1}");
            PrintElapsedTime(stopwatch);
            stopwatch.Reset();
            stopwatch.Restart();

            var solution2 = problem.Solve_2();

            stopwatch.Stop();
            Console.Write($"{lineStart}, part 2:\t\t{solution2}");
            PrintElapsedTime(stopwatch, newLine: true);
        }

        private static void PrintElapsedTime(Stopwatch stopwatch, bool newLine = false)
        {
            ConsoleColor originalColor = Console.ForegroundColor;

            long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            Performance performance = EvaluatePerformance(elapsedMilliseconds);

            ChangeForegroundConsoleColor(performance);

            string elapsedTime = elapsedMilliseconds < 1000
                ? $"{elapsedMilliseconds} ms"
                : $"{0.001 * elapsedMilliseconds:F} s";

            Console.WriteLine($"\t\t\t\t\t{elapsedTime}");

            if (newLine)
            {
                Console.WriteLine();
            }

            Console.ForegroundColor = originalColor;
        }

        /// <summary>
        /// Provides a <see cref="Performance"/> value according to the number of milliseconds spent
        /// </summary>
        /// <param name="elapsedMilliseconds"></param>
        /// <returns></returns>
        private static Performance EvaluatePerformance(long elapsedMilliseconds)
        {
            if (elapsedMilliseconds == 0)
            {
                return Performance.Unknown;
            }

            return (Performance)Enum.ToObject(
                typeof(Performance),
                Clamp(value: elapsedMilliseconds / 1000, min: 0, max: ActionDictionary.Count - 1));
        }

        /// <summary>
        /// Console foreground colors for different <see cref="Performance"/>
        /// </summary>
        private static readonly IReadOnlyDictionary<Performance, Action> ActionDictionary = new Dictionary<Performance, Action>()
        {
            [Performance.Good] = () => Console.ForegroundColor = ConsoleColor.DarkGreen,
            [Performance.Average] = () => Console.ForegroundColor = ConsoleColor.DarkYellow,
            [Performance.Bad] = () => Console.ForegroundColor = ConsoleColor.DarkRed,
            [Performance.Unknown] = () => Console.ForegroundColor = ConsoleColor.DarkBlue
        };

        private static void ChangeForegroundConsoleColor(Performance key)
        {
            if (ActionDictionary.TryGetValue(key, out Action? action))
            {
                action.Invoke();
            }
        }

        private static long Clamp(long value, long min, long max)
        {
            return (value.CompareTo(min) <= 0)
                ? min
                : (value.CompareTo(max) >= 0)
                    ? max
                    : value;
        }
    }
}
