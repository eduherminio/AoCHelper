namespace AoCHelper.PoC
{
    internal class Problem04 : BaseProblem
    {
        protected override string InputFileExtension => ".in";

        public Problem04()
        {
            if (!File.Exists(InputFilePath))
            {
                throw new SolvingException($"Path {InputFilePath} not found for {GetType().Name}");
            }
        }

        public override ValueTask<string> Solve_1()
        {
            Thread.Sleep(1500);
            return new("Solution 4.1");
        }

        public override ValueTask<string> Solve_2()
        {
            Thread.Sleep(2000);
            return new("Solution 4.2");
        }
    }
}
