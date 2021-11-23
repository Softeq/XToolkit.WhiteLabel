// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq;

namespace Softeq.XToolkit.Common.Helpers
{
    /// <summary>
    ///     Class helps to get a hash code for a number of objects combined.
    /// </summary>
    [Obsolete("Please use System.HashCode to calculate HashCode value")]
    public static class HashHelper
    {
        private const int PrimeOne = 17;
        private const int PrimeTwo = 23;

        /// <summary>
        ///     Get hashcode from objects.
        /// </summary>
        /// <returns>The hash code.</returns>
        /// <param name="arg1">Mandatory argument.</param>
        /// <param name="otherArgs">Optional arguments.</param>
        public static int GetHashCode(object? arg1, params object?[] otherArgs)
        {
            unchecked
            {
                return otherArgs
                    .Prepend(arg1)
                    .Aggregate(PrimeOne, (hash, arg) => hash * PrimeTwo + (arg?.GetHashCode() ?? 0));
            }
        }
    }
}
