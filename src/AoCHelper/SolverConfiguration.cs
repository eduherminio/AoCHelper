namespace AoCHelper
{
    public class SolverConfiguration
    {
        /// <summary>
        /// Clears previous runs information from the console.
        /// True by default.
        /// </summary>
        public bool ClearConsole { get; set; }

        /// <summary>
        /// Shows a panel at the end of the run with aggregated stats of the solved problems.
        /// True by default when solving multiple problems, false otherwise.
        /// </summary>
        public bool ShowOverallResults { get; set; }

        /// <summary>
        /// Custom numeric format strings used when .
        /// See https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings
        /// </summary>
        public string? ElapsedTimeFormatSpecifier { get; set; }

        public SolverConfiguration()
        {
            ClearConsole = true;
        }
    }
}