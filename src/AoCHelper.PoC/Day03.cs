using System.IO;

namespace AoCHelper.PoC
{
    public class Day03 : BaseDay
    {
        public Day03()
        {
            if (!File.Exists(InputFilePath))
            {
                throw new SolvingException($"Path {InputFilePath} not found for {GetType().Name}");
            }
        }

        public override string Solve_1()
        {
            System.Threading.Thread.Sleep(33);
            return "Solution 1";
        }

        public override string Solve_2()
        {
            System.Threading.Thread.Sleep(66);
            return "Solution 2";
        }
    }
}
