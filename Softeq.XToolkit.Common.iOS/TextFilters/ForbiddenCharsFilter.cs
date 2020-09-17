// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq;
using Foundation;
using UIKit;

namespace Softeq.XToolkit.Common.iOS.TextFilters
{
    /// <summary>
    ///     This filter will constrain edits not to make text contains forbidden symbols.
    /// </summary>
    public class ForbiddenCharsFilter : ITextFilter
    {
        private readonly char[] _forbiddenChars;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ForbiddenCharsFilter"/> class.
        /// </summary>
        /// <param name="forbiddenChars">Char array of characters not allowed in the input.</param>
        public ForbiddenCharsFilter(params char[] forbiddenChars)
        {
            _forbiddenChars = forbiddenChars ?? throw new ArgumentNullException(nameof(forbiddenChars));
        }

        /// <inheritdoc cref="ITextFilter" />
        public bool ShouldChangeText(UIResponder responder, string? oldText, NSRange range, string inputText)
        {
            var intersection = inputText.ToCharArray().Intersect(_forbiddenChars);
            return !intersection.Any();
        }
    }
}
