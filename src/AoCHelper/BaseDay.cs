﻿namespace AoCHelper
{
    /// <summary>
    /// <see cref="BaseProblem  "/> with custom <see cref="BaseProblem.ClassPrefix"/> ("Day") and <see cref="BaseProblem.InputFileExtension"/> (".txt")
    /// </summary>
    public abstract class BaseDay : BaseProblem
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override string ClassPrefix { get; } = "Day";
    }
}
