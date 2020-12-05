namespace AoCHelper
{
    public class SolverConfiguration
    {
        /// <summary>
        /// Clears previous runs information from the console.
        /// True by default.
        /// </summary>
        public bool ClearConsole { get; set; }

        public SolverConfiguration()
        {
            ClearConsole = true;
        }
    }
}