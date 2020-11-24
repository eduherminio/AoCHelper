namespace AoCHelper
{
    /// <summary>
    /// <see cref="BaseProblem  "/> with custom <see cref="BaseProblem.ClassPrefix"/> ("Day") and <see cref="BaseProblem.FileExtension"/> (".txt")
    /// </summary>
    public abstract class BaseDay : BaseProblem
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override string ClassPrefix { get; } = "Day";

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override string FileExtension { get; } = ".txt";
    }
}
