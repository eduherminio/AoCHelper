namespace AoCHelper.PoC.Library
{
    public class BaseLibraryDay16 : BaseDay
    {
        public override ValueTask<string> Solve_1()
        {
            throw new SolvingException("Exception in Library 6 part 1");
        }

        public override ValueTask<string> Solve_2()
        {
            Thread.Sleep(123);
            return new("Solution Library 6.2");
        }
    }
}
