namespace AoCHelper
{
    public interface IProblem
    {
        string FilePath { get; }

        string Solve_1();

        string Solve_2();
    }
}
