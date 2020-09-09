// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Linq;
using Foundation;
using UIKit;

namespace Softeq.XToolkit.Common.iOS.Controls
{
    public class ForbiddenCharactersFilter : ITextFilter
    {
        private readonly char[] _forbiddenCharacters;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ForbiddenCharactersFilter"/> class.
        /// </summary>
        /// <param name="forbiddenCharacters">Char array of characters not allowed in the input.</param>
        public ForbiddenCharactersFilter(params char[] forbiddenCharacters)
        {
            _forbiddenCharacters = forbiddenCharacters;
        }

        public bool ShouldChangeText(UIResponder responder, string? oldText, NSRange range, string inputText)
        {
            var intersection = inputText.ToCharArray().Intersect(_forbiddenCharacters);
            return !intersection.Any();
        }
    }
}
