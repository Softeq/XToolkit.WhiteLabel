// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Foundation;
using Softeq.XToolkit.Common.iOS.Extensions;
using Softeq.XToolkit.Common.iOS.Tests.TextFilters.GroupFilterTests;
using UIKit;
using Xunit;

namespace Softeq.XToolkit.Common.iOS.Tests.Extensions.UITextViewExtensionsTests;

[SuppressMessage("ReSharper", "InvokeAsExtensionMethod", Justification = "Need for tests")]
public class UITextViewExtensionsTests
{
    [Fact]
    public void SetFilter_UITextFieldIsNull_ThrowsNullReferenceException()
    {
        var textField = (UITextField) null!;

        Assert.Throws<NullReferenceException>(() =>
        {
            UITextViewExtensions.SetFilter(textField, new MockTextFilter(true));
        });
    }

    [Fact]
    public async Task SetFilter_UITextFieldWithNullFilter_ThrowsArgumentNullException()
    {
        var result = Helpers.RunOnUIThreadAsync(() =>
        {
            var textField = new UITextField();

            textField.SetFilter(null!);

            return false;
        });

        await Assert.ThrowsAsync<ArgumentNullException>(() => result);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task SetFilter_UITextFieldWithFilter_ReturnsExpected(bool expectedResult)
    {
        var result = await Helpers.RunOnUIThreadAsync(() =>
        {
            var textField = new UITextField();
            var filter = new MockTextFilter(expectedResult);

            textField.SetFilter(filter);

            return textField.ShouldChangeCharacters(textField, new NSRange(1, 0), "new");
        });

        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void SetFilter_UITextViewIsNull_ThrowsNullReferenceException()
    {
        var textView = (UITextView) null!;

        Assert.Throws<NullReferenceException>(() =>
        {
            UITextViewExtensions.SetFilter(textView, new MockTextFilter(true));
        });
    }

    [Fact]
    public async Task SetFilter_UITextViewWithNullFilter_ThrowsArgumentNullException()
    {
        var result = Helpers.RunOnUIThreadAsync(() =>
        {
            var textField = new UITextField();

            textField.SetFilter(null!);

            return false;
        });

        await Assert.ThrowsAsync<ArgumentNullException>(() => result);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task SetFilter_UITextViewWithFilter_ReturnsExpected(bool expectedResult)
    {
        var result = await Helpers.RunOnUIThreadAsync(() =>
        {
            var textView = new UITextView();
            var filter = new MockTextFilter(expectedResult);

            textView.SetFilter(filter);

            return textView.ShouldChangeText!(textView, new NSRange(1, 0), "new");
        });

        Assert.Equal(expectedResult, result);
    }
}
