// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Foundation;
using Softeq.XToolkit.Common.iOS.TextFilters;
using UIKit;
using Xunit;

namespace Softeq.XToolkit.Common.iOS.Tests.TextFilters.LengthFilterTests;

public class LengthFilterTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1000)]
    public void Ctor_Positive_ReturnsITextFilter(int maxLength)
    {
        var obj = new LengthFilter(maxLength);

        Assert.IsAssignableFrom<ITextFilter>(obj);
    }

    [Fact]
    public void Ctor_Negative_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new LengthFilter(-1));
    }

    [Fact]
    public void IsCharacterOverwritingEnabled_Default_ReturnsFalse()
    {
        var obj = new LengthFilter(0);

        var result = obj.IsCharacterOverwritingEnabled;

        Assert.False(result);
    }

    [Theory]
    [InlineData("old", 3, "new", 6, true)]
    [InlineData("old", 3, "new", 3, false)]
    [InlineData("old", 1, "new", 2, false)]
    public async Task ShouldChangeText_UITextField_ReturnsExpected(
        string oldText,
        int insertPosition,
        string newText,
        int maxLength,
        bool expectedResult)
    {
        var result = await Helpers.RunOnUIThreadAsync(() =>
        {
            var textField = new UITextField();
            var range = new NSRange(insertPosition, 0);
            var filter = new LengthFilter(maxLength);

            return filter.ShouldChangeText(textField, oldText, range, newText);
        });

        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData("old", 0, "new", 3, "new")]
    [InlineData("old-extra-text", 4, "new", 7, "old-new")]
    public async Task ShouldChangeText_UITextFieldWithCharacterOverwritingEnabled_ReturnsExpected(
        string oldText,
        int insertPosition,
        string newText,
        int maxLength,
        string expectedResult)
    {
        var result = await Helpers.RunOnUIThreadAsync(() =>
        {
            var textField = new UITextField { Text = oldText };
            var range = new NSRange(insertPosition, 0);
            var filter = new LengthFilter(maxLength);

            filter.IsCharacterOverwritingEnabled = true;
            filter.ShouldChangeText(textField, oldText, range, newText);

            return textField.Text;
        });

        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void ShouldChangeText_NullResponderWithCharacterOverwritingEnabled_ReturnsExpected()
    {
        var range = new NSRange(2, 0);
        var filter = new LengthFilter(0);

        filter.IsCharacterOverwritingEnabled = true;
        var result = filter.ShouldChangeText(null!, "old", range, "new");

        Assert.False(result);
    }

    [Fact]
    public async Task ShouldChangeText_InvalidRange_ThrowsArgumentOutOfRangeException()
    {
        var result = Helpers.RunOnUIThreadAsync(() =>
        {
            var textField = new UITextField();
            var filter = new LengthFilter(0);

            var range = new NSRange(0, 1000);
            return filter.ShouldChangeText(textField, "old", range, "new");
        });

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => result);
    }
}
