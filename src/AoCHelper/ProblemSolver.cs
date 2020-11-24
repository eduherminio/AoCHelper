using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace AoCHelper
{
    public class ProblemSolver
    {
        #region Public methods

        /// <summary>
        /// Solves a problem
        /// </summary>
        /// <typeparam name="TProblem"></typeparam>
        public void Solve<TProblem>()
            where TProblem : IProblem, new()
        {
            IProblem problem = new TProblem();

            Solve(problem);
        }

        /// <summary>
        /// Solves a problem providing metrics
        /// </summary>
        /// <typeparam name="TProblem"></typeparam>
        public void SolveWithMetrics<TProblem>()
            where TProblem : IProblem, new()
        {
            IProblem problem = new TProblem();

            SolveWithMetrics(problem);
        }

        /// <summary>
        /// Solve all problems
        /// </summary>
        public void SolveAllProblems()
        {
            foreach (Type problemType in LoadAllProblems(Assembly.GetCallingAssembly()))
            {
                if (Activator.CreateInstance(problemType) is IProblem problem)
                {
                    Solve(problem);
                }
            }
        }

        /// <summary>
        /// Solves all problems providing metrics
        /// </summary>
        public void SolveAllProblemsWithMetrics()
        {
            foreach (Type problemType in LoadAllProblems(Assembly.GetCallingAssembly()))
            {
                if (Activator.CreateInstance(problemType) is IProblem problem)
                {
                    SolveWithMetrics(problem);
                }
            }
        }

        #endregion

        /// <summary>
        /// Loads problems to be solved by <see cref="SolveAllProblems"/> and <see cref="SolveAllProblemsWithMetrics"/>
        /// </summary>
        /// <returns></returns>
        internal static IEnumerable<Type> LoadAllProblems(Assembly assembly)
        {
            return assembly.GetTypes()
                .Where(type => typeof(IProblem).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);
        }

        /// <summary>
        /// Solves both parts of a problem, each one in a different line, adding an empty line in the end
        /// </summary>
        /// <param name="problem"></param>
        protected virtual void Solve(IProblem problem)
        {
            var problemIndex = problem.CalculateIndex();
            var lineStart = problemIndex != default
                ? $"Day {problemIndex}"
                : $"{problem.GetType().Name}";

            var solution1 = problem.Solve_1();
            Console.WriteLine($"{lineStart}, part 1:\t\t{solution1}");

            var solution2 = problem.Solve_2();
            Console.WriteLine($"{lineStart}, part 2:\t\t{solution2}\n");
        }

        /// <summary>
        /// Solves both parts of a problem, each one in a different line, adding an empty line in the end
        /// Prints the time consumed by each part next to the result produced by it
        /// </summary>
        /// <param name="problem"></param>
        protected virtual void SolveWithMetrics(IProblem problem)
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

        protected virtual void PrintElapsedTime(Stopwatch stopwatch, bool newLine = false)
        {
            ConsoleColor originalColor = Console.ForegroundColor;

            long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            Performance performance = EvaluatePerformance(elapsedMilliseconds);

            ChangeForegroundConsoleColor(performance);

            string elapsedTime = elapsedMilliseconds < 1000
                ? $"{elapsedMilliseconds} ms"
                : $"{0.001 * elapsedMilliseconds:F} s";

            Console.WriteLine($"\t\t\t\t{elapsedTime}");

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
        protected virtual Performance EvaluatePerformance(long elapsedMilliseconds)
        {
            if (elapsedMilliseconds == 0)
            {
                return Performance.Unknown;
            }

            return (Performance)Enum.ToObject(
                typeof(Performance),
                Clamp(value: elapsedMilliseconds / 1000, min: 0, max: _actionDictionary.Count - 1));
        }

        /// <summary>
        /// Console foreground colors for different <see cref="Performance"/>
        /// </summary>
        protected Dictionary<Performance, Action> _actionDictionary = new Dictionary<Performance, Action>()
        {
            [Performance.Good] = () => Console.ForegroundColor = ConsoleColor.DarkGreen,
            [Performance.Average] = () => Console.ForegroundColor = ConsoleColor.DarkYellow,
            [Performance.Bad] = () => Console.ForegroundColor = ConsoleColor.DarkRed,
            [Performance.Unknown] = () => Console.ForegroundColor = ConsoleColor.DarkBlue
        };

        protected void ChangeForegroundConsoleColor(Performance key)
        {
            if (_actionDictionary.TryGetValue(key, out Action action))
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
