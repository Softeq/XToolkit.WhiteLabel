// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.Common.Helpers.Hashing
{
    /// <summary>
    /// Common interface for Hash function implementations
    /// </summary>
    public interface IHashCalculator
    {
        /// <summary>
        ///     Adds value to use in hash code calculations.
        /// </summary>
        /// <returns>IHashCalculator reference to use in chained calls</returns>
        /// <typeparam name="T">Generic type of the value</typeparam>
        /// <param name="value">Value to use in hash code calculations</param>
        IHashCalculator Using<T>(T value);

        /// <summary>
        ///     Calculates hash code using specified objects.
        /// </summary>
        /// <returns>The hash code.</returns>
        int Calculate();
    }
}
