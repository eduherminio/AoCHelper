namespace AoCHelper
{
    public interface IProblem
    {
        /// <summary>
        /// Problem's input file path.
        /// </summary>
        string FilePath { get; }

        uint CalculateIndex();

        string Solve_1();

        string Solve_2();
    }
}
