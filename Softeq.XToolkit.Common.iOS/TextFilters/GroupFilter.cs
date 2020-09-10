// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Linq;
using Foundation;
using UIKit;

namespace Softeq.XToolkit.Common.iOS.TextFilters
{
    /// <summary>
    ///     This filter combines multiple filters.
    ///     Returns <see langword="true" /> only if all filters return <see langword="true" />.
    /// </summary>
    public class GroupFilter : ITextFilter
    {
        private readonly ITextFilter[] _filters;

        public GroupFilter(params ITextFilter[] filters)
        {
            _filters = filters;
        }

        /// <inheritdoc cref="ITextFilter" />
        public bool ShouldChangeText(UIResponder responder, string? oldText, NSRange range, string replacementString)
        {
            return _filters.All(filter => filter.ShouldChangeText(responder, oldText, range, replacementString));
        }
    }
}
