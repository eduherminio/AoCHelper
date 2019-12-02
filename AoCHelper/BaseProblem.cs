using System;
using System.IO;

namespace AoCHelper
{
    public abstract class BaseProblem : IProblem
    {
        public virtual string FilePath { get; }

        /// <summary>
        /// Providing class name is ProblemXX, it parses problem input from Inputs/XX.in
        /// </summary>
        protected BaseProblem()
        {
            string typeName = GetType().Name;
            string problemIndex = typeName.Substring(typeName.IndexOf("Problem") + "Problem".Length);

            FilePath = Path.Combine("Inputs", problemIndex + ".in");
        }

        public abstract string Solve_1();

        public abstract string Solve_2();
    }
}
