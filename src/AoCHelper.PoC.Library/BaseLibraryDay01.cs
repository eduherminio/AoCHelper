namespace AoCHelper.PoC.Library
{
    public class BaseLibraryDay01 : BaseDay
    {
        public BaseLibraryDay01()
        {
            if (!File.Exists(InputFilePath))
            {
                throw new SolvingException($"Path {InputFilePath} not found for {GetType().Name}");
            }
        }

        public override ValueTask<string> Solve_1()
        {
            return new("Solution Library 1.1");
        }

        public override ValueTask<string> Solve_2()
        {
            return new("Solution Library 1.2");
        }
    }
}
