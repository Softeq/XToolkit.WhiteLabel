// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.Common.Helpers
{
    /// <summary>
    ///     A class containing information about a range of text (it's start position and length).
    /// </summary>
    public class TextRange
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TextRange"/> class. Parameters can not be negative.
        /// </summary>
        /// <param name="position">Start position of this range.</param>
        /// <param name="length">Length of this range.</param>
        public TextRange(int position, int length)
        {
            if (position < 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(position)} can not be negative");
            }

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(length)} can not be negative");
            }

            Position = position;
            Length = length;
        }

        /// <summary>
        ///     Gets start position of the text range.
        /// </summary>
        public int Position { get; }

        /// <summary>
        ///     Gets length of the text range.
        /// </summary>
        public int Length { get; }

        /// <summary>
        ///     Replaces the content of this text range with a new string.
        /// </summary>
        /// <param name="fullString">Full string whose range will be altered.</param>
        /// <param name="newString">A new string to be inserted instead of the text range content.</param>
        /// <returns>Result string.</returns>
        public string BuildNewString(string fullString, string newString)
        {
            if (fullString == null)
            {
                throw new ArgumentNullException($"{nameof(fullString)} can not be null");
            }

            if (newString == null)
            {
                throw new ArgumentNullException($"{nameof(newString)} can not be null");
            }

            return fullString
                .Remove(Position, Length)
                .Insert(Position, newString);
        }

        public override string ToString()
        {
            return $"[{Position} {Length}]";
        }
    }
}
