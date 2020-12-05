using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;

namespace AoCHelper.Test
{
    public class SolverTest
    {
        private abstract class ProblemFixture : BaseProblem
        {
            public override string Solve_1() => Solve();

            public override string Solve_2() => Solve();

            private string Solve()
            {
                if (!File.Exists(InputFilePath))
                {
                    throw new FileNotFoundException(InputFilePath);
                }

                return string.Empty;
            }
        }

        private class Problem66 : ProblemFixture { }

        private class IllCreatedCustomProblem : ProblemFixture { }

        [Fact]
        public void Solve()
        {
            Solver.Solve<Problem66>();
            Solver.Solve<Problem66>(new SolverConfiguration());
        }

        [Fact]
        public void SolveIntParams()
        {
            Solver.Solve(null, 1, 2);
            Solver.Solve(new SolverConfiguration(), 1, 2);
        }

        [Fact]
        public void SolveIntEnumerable()
        {
            Solver.Solve(new List<uint> { 1, 2 });
            Solver.Solve(new List<uint> { 1, 2 }, new SolverConfiguration());
        }

        [Fact]
        public void SolveTypeParams()
        {
            SolverConfiguration? nullConfig = null;
            Solver.Solve(nullConfig, typeof(Problem66));
            Solver.Solve(new SolverConfiguration(), typeof(Problem66));
        }

        [Fact]
        public void SolveTypeEnumerable()
        {
            Solver.Solve(new List<Type> { typeof(Problem66) });
            Solver.Solve(new List<Type> { typeof(Problem66) }, new SolverConfiguration());
        }

        /// <summary>
        /// AoCHelper isn't actually solving anything, since Assembly.GetEntryAssembly() returns xunit assembly.
        /// </summary>
        [Fact]
        public void SolveLast()
        {
            Solver.SolveLast();
            Solver.SolveLast(new SolverConfiguration());
        }

        [Fact]
        public void ShouldNotThrowExceptionIfCantSolve()
        {
            Solver.Solve<IllCreatedCustomProblem>();
        }

        [Fact]
        public void LoadAllProblems()
        {
            Assert.Equal(
                Assembly.GetExecutingAssembly()!.GetTypes().Count(type => typeof(BaseProblem).IsAssignableFrom(type) && !type.IsAbstract),
                Solver.LoadAllProblems(Assembly.GetExecutingAssembly()).Count());
        }

        #region Obsolete methods
#pragma warning disable CS0618 // Tests should include assertions

        [Fact]
        public void ObsoleteElapsedTimeFormatSpecifier()
        {
            Solver.ElapsedTimeFormatSpecifier = "F3";
            Solver.SolveLast(false);
        }

        [Fact]
        public void ObsoleteSolve()
        {
            Solver.Solve<Problem66>(true);
        }

        [Fact]
        public void ObsoleteSolveLast()
        {
            Solver.SolveLast(true);
        }

        [Fact]
        public void ObsoleteSolveIntEnumerable()
        {
            Solver.Solve(new uint[] { 1, 2 });
        }

        [Fact]
        public void ObsoleteSolveIntParams()
        {
            Solver.Solve(1, 2);
        }

        [Fact]
        public void ObsoleteSolveTypeParams()
        {
            Solver.Solve(typeof(Problem66));
        }

#pragma warning restore CS0618
        #endregion
    }
}
