namespace AoCHelper
{
    public interface IProblem
    {
        string FilePath { get; }

        void Solve_1();

        void Solve_2();
    }
}
