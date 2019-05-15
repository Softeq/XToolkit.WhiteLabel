// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.Common.Models
{
    public class TextRange
    {
        public TextRange(int position, int length)
        {
            Position = position;
            Length = length;
        }

        public int Position { get; }

        public int Length { get; }

        public string BuildNewString(string oldString, string newString)
        {
            return oldString
                .Remove(Position, Length)
                .Insert(Position, newString);
        }
    }
}