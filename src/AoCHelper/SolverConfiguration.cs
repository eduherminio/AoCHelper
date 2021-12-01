using Spectre.Console;

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
        /// Shows the time elapsed during the instantiation of a <see cref="BaseProblem"/>.
        /// This normally reflects the elapsed time while parsing the input data.
        /// </summary>
        public bool ShowConstructorElapsedTime { get; set; }

        /// <summary>
        /// Shows total elapsed time per day. This includes constructor time + part 1 + part 2
        /// </summary>
        public bool ShowTotalElapsedTimePerDay { get; set; }

        /// <summary>
        /// Custom numeric format strings used for elapsed millisecods.
        /// See https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings
        /// </summary>
        public string? ElapsedTimeFormatSpecifier { get; set; }

        /// <summary>
        /// Represents vertical overflow.
        /// <see href="https://spectreconsole.net/live/live-display"/>
        /// </summary>
        internal VerticalOverflow VerticalOverflow { get; set; }

        /// <summary>
        /// Represents vertical overflow cropping.
        /// <see href="https://spectreconsole.net/live/live-display"/>
        /// </summary>
        internal VerticalOverflowCropping VerticalOverflowCropping { get; set; }

        public SolverConfiguration()
        {
            ClearConsole = true;
            ShowOverallResults = true;
            ShowConstructorElapsedTime = false;
            ShowTotalElapsedTimePerDay = false;

            VerticalOverflow = VerticalOverflow.Ellipsis;
            VerticalOverflowCropping = VerticalOverflowCropping.Top;
        }
    }
}