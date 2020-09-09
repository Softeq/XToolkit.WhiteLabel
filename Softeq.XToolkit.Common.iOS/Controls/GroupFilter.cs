// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Linq;
using Foundation;
using UIKit;

namespace Softeq.XToolkit.Common.iOS.Controls
{
    public class GroupFilter : ITextFilter
    {
        private readonly ITextFilter[] _filters;

        public GroupFilter(params ITextFilter[] filters)
        {
            _filters = filters;
        }

        public bool ShouldChangeText(UIResponder responder, string? oldText, NSRange range, string replacementString)
        {
            return _filters.All(filter => filter.ShouldChangeText(responder, oldText, range, replacementString));
        }
    }
}
