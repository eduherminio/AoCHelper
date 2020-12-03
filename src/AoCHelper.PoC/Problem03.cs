using System.IO;

namespace AoCHelper.PoC
{
    internal class Problem03 : BaseProblem
    {
        public Problem03()
        {
            if (!File.Exists(InputFilePath))
            {
                throw new SolvingException($"Path {InputFilePath} not found for {GetType().Name}");
            }
        }

        public override string Solve_1()
        {
            System.Threading.Thread.Sleep(101);
            return "Solution 1";
        }

        public override string Solve_2()
        {
            System.Threading.Thread.Sleep(501);
            return "Solution 2";
        }
    }
}
