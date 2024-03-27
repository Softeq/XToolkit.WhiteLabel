// Developed by Softeq Development Corporation
// http://www.softeq.com

using Foundation;
using Softeq.XToolkit.Common.iOS.TextFilters;
using UIKit;

namespace Softeq.XToolkit.Common.iOS.Tests.TextFilters.GroupFilterTests;

internal class MockTextFilter : ITextFilter
{
    private readonly bool _result;

    public MockTextFilter(bool result)
    {
        _result = result;
    }

    public bool ShouldChangeText(UIResponder responder, string? oldText, NSRange range, string replacementString)
    {
        return _result;
    }
}
