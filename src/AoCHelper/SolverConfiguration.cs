﻿namespace AoCHelper
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
        /// Shows the time elapsed during the instantiation of a <see cref="BaseProblem"/>.
        /// This normally reflects the elapsed time while parsing the input data.
        /// </summary>
        public bool ShowConstructorElapsedTime { get; set; }

        /// <summary>
        /// Shows total elapsed time per day. This includes constructor time + part 1 + part 2
        /// </summary>
        public bool ShowTotalElapsedTimePerDay { get; set; }

        /// <summary>
        /// Custom numeric format strings used when .
        /// See https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings
        /// </summary>
        public string? ElapsedTimeFormatSpecifier { get; set; }

        public SolverConfiguration()
        {
            ClearConsole = true;
            ShowOverallResults = true;
            ShowConstructorElapsedTime = false;
            ShowTotalElapsedTimePerDay = false;
        }
    }
}