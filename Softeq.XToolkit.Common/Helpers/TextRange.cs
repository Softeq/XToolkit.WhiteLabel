// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.Common.Helpers
{
    /// <summary>
    /// A class containing information about a range of text (it's start position and length)
    /// </summary>
    public class TextRange
    {
        /// <summary>
        /// Creates a text range. Parameters can not be negative
        /// </summary>
        /// <param name="position">Start position of this range</param>
        /// <param name="length">Length of this range</param>
        public TextRange(int position, int length)
        {
            if (position < 0 || length < 0)
            {
                throw new ArgumentOutOfRangeException("Position and Length can not be negative");
            }

            Position = position;
            Length = length;
        }

        /// <summary>
        /// Start position of the text range
        /// </summary>
        public int Position { get; }

        /// <summary>
        /// Length of the text range
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// Replaces the content of this text range with a new string
        /// </summary>
        /// <param name="fullString">Full string whose range will be altered</param>
        /// <param name="newString">A new string to be inserted instead of the text range content</param>
        /// <returns></returns>
        public string BuildNewString(string fullString, string newString)
        {
            if (fullString == null || newString == null)
            {
                throw new ArgumentNullException("Parameters can not be null");
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
