// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Common.iOS.Extensions;
using UIKit;
using Xunit;

namespace Softeq.XToolkit.Common.iOS.Tests.Extensions.UIColorExtensionsTests;

public class UIColorExtensionsTests
{
    [Theory]
    [InlineData("#333333", "UIColor [A=255, R=51, G=51, B=51]")]
    [InlineData("0000FF", "UIColor [A=255, R=0, G=0, B=255]")]
    [InlineData("00FF00", "UIColor [A=255, R=0, G=255, B=0]")]
    [InlineData("F00", "UIColor [A=255, R=255, G=0, B=0]")]
    [InlineData("#008080", "UIColor [A=255, R=0, G=128, B=128]")]
    [InlineData("FA8072", "UIColor [A=255, R=250, G=128, B=114]")]
    [InlineData("350027", "UIColor [A=255, R=53, G=0, B=39]")]
    public void UIColorFromHex_CorrectValueWithoutAlpha_ReturnsValidColor(string hexColor, string expected)
    {
        var result = hexColor.UIColorFromHex();

        Assert.Equal(expected, result.ToString());
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 1)]
    [InlineData(0, 0)]
    [InlineData(-2, 0)]
    [InlineData(0.5, 0.5)]
    [InlineData(0.33, 0.33)]
    public void UIColorFromHex_CorrectValueWithAlpha_ReturnsValidColor(float alpha, float expectedAlpha)
    {
        var result = "#FFF".UIColorFromHex(alpha);

        Assert.Equal(expectedAlpha, result.CGColor.Alpha);
    }

    [Theory]
    [InlineData("")]
    [InlineData("#00")]
    [InlineData("1122")]
    [InlineData("#11223344")]
    [InlineData("11223344")]
    public void UIColorFromHex_IncorrectValue_ThrowsArgumentOutOfRangeException(string hexColor)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            hexColor.UIColorFromHex();
        });
    }

    [Theory]
    [InlineData(255, 255, 255, "#FFFFFF")]
    [InlineData(0, 0, 0, "#000000")]
    [InlineData(6, 27, 250, "#061BFA")]
    [InlineData(0, 52, -1, "#0034FF")]
    public void ToHex_UIColor_ReturnsHexColor(int red, int green, int blue, string expectedHex)
    {
        var color = UIColor.FromRGB(red, green, blue);

        var result = color.ToHex();

        Assert.Equal(expectedHex, result);
    }
}
