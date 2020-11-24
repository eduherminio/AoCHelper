using System.IO;

namespace AoCHelper.PoC
{
    internal class Problem01 : BaseProblem
    {
        public Problem01()
        {
            if (!File.Exists(InputFilePath))
            {
                throw new SolvingException($"Path {InputFilePath} not found for {GetType().Name}");
            }
        }

        public override string Solve_1()
        {
            return "Solution 1";
        }

        public override string Solve_2()
        {
            System.Threading.Thread.Sleep(2001);
            return "Solution 2";
        }
    }
}
