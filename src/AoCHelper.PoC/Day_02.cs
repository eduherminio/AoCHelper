using System.IO;

namespace AoCHelper.PoC
{
    public class Day_02 : BaseDay
    {
        public Day_02()
        {
            if (!File.Exists(InputFilePath))
            {
                throw new SolvingException($"Path {InputFilePath} not found for {GetType().Name}");
            }
        }

        public override string Solve_1()
        {
            System.Threading.Thread.Sleep(250);
            return "Solution 1";
        }

        public override string Solve_2()
        {
            System.Threading.Thread.Sleep(750);
            return "Solution 2";
        }
    }
}
