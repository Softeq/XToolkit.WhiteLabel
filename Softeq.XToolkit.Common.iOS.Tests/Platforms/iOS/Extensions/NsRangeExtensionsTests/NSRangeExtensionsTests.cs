// Developed by Softeq Development Corporation
// http://www.softeq.com

using Foundation;
using Softeq.XToolkit.Common.Helpers;
using Softeq.XToolkit.Common.iOS.Extensions;
using Xunit;

namespace Softeq.XToolkit.Common.iOS.Tests.Extensions.NsRangeExtensionsTests;

public class NSRangeExtensionsTests
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 10)]
    [InlineData(5, 10)]
    [InlineData(9, 10)]
    [InlineData(15, 10)]
    public void ToTextRange_ForNsRange_Converts(int position, int length)
    {
        var nsRange = new NSRange(position, length);

        var textRange = nsRange.ToTextRange();

        Assert.NotNull(textRange);
        Assert.IsType<TextRange>(textRange);
        Assert.Equal(position, textRange.Position);
        Assert.Equal(length, textRange.Length);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 10)]
    [InlineData(5, 10)]
    [InlineData(9, 10)]
    [InlineData(15, 10)]
    public void ToNsRange_ForTextRange_Converts(int position, int length)
    {
        var textRange = new TextRange(position, length);

        var nsRange = textRange.ToNSRange();

        Assert.IsType<NSRange>(nsRange);
        Assert.Equal(position, nsRange.Location);
        Assert.Equal(length, nsRange.Length);
    }
}
