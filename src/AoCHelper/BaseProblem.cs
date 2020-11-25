using System.IO;

namespace AoCHelper
{
    public abstract class BaseProblem
    {
        protected virtual string ClassPrefix { get; } = "Problem";

        /// <summary>
        /// Expected input file dir path
        /// </summary>
        protected virtual string InputFileDirPath { get; } = "Inputs";

        /// <summary>
        /// Expected input file extension
        /// </summary>
        protected virtual string InputFileExtension { get; } = ".in";

        /// <summary>
        /// Problem's index.
        /// Two digit number, (expect a leading '0' when appropriated).
        /// </summary>
        /// <summary>
        /// Extracts problem's index from the class name.
        /// Supported formats: <see cref="ClassPrefix"/>{Index}, <see cref="ClassPrefix"/>_{Index}.
        /// In case of unsupported class name format, <see cref="InputFilePath"/> needs to be overriden to point to the right input file.
        /// </summary>
        /// <returns>Problem's index or uint.MaxValue if unsupported class name.</returns>
        public virtual uint CalculateIndex()
        {
            var typeName = GetType().Name;

            return uint.TryParse(typeName.Substring(typeName.IndexOf(ClassPrefix) + ClassPrefix.Length).TrimStart('_'), out var index)
                ? index
                : default;
        }

        /// <summary>
        /// Expected input file path.
        /// By default, <see cref="InputFileDirPath"/>/<see cref="CalculateIndex"/>.<see cref="InputFileExtension"/>.
        /// Overriding it makes <see cref="InputFileDirPath"/> and <see cref="InputFileExtension"/> irrelevant
        /// </summary>
        public virtual string InputFilePath
        {
            get
            {
                var index = CalculateIndex();
                var leading0 = index < 10 ? "0" : "";

                return Path.Combine(InputFileDirPath, $"{leading0}{index}.{InputFileExtension.TrimStart('.')}");
            }
        }

        public abstract string Solve_1();

        public abstract string Solve_2();
    }
}
