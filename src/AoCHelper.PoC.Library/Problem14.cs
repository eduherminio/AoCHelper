namespace AoCHelper.PoC.Library
{
    internal class Problem14 : BaseProblem
    {
        protected override string InputFileExtension => ".in";

        public Problem14()
        {
            if (!File.Exists(InputFilePath))
            {
                throw new SolvingException($"Path {InputFilePath} not found for {GetType().Name}");
            }
        }

        public override ValueTask<string> Solve_1()
        {
            Thread.Sleep(1500);
            return new("Solution Library 4.1");
        }

        public override ValueTask<string> Solve_2()
        {
            Thread.Sleep(2000);
            return new("Solution Library 4.2");
        }
    }
}
