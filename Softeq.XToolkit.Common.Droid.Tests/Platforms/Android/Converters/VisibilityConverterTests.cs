// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Views;
using Softeq.XToolkit.Common.Droid.Converters;
using Xunit;

namespace Softeq.XToolkit.Common.Droid.Tests.Converters;

public class VisibilityConverterTests
{
    [Fact]
    public void Gone_ReturnsNotNull()
    {
        Assert.NotNull(VisibilityConverter.Gone);
    }

    [Fact]
    public void Invisible_ReturnsNotNull()
    {
        Assert.NotNull(VisibilityConverter.Invisible);
    }

    [Fact]
    public void Instance_ReturnsInvisible()
    {
        Assert.Same(VisibilityConverter.Invisible, VisibilityConverter.Instance);
    }

    [Theory]
    [InlineData(true, ViewStates.Visible)]
    [InlineData(false, ViewStates.Gone)]
    public void ConvertValue_Gone_ReturnsExpectedValue(bool value, ViewStates expectedResult)
    {
        var converter = VisibilityConverter.Gone;

        var result = converter.ConvertValue(value);

        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(true, ViewStates.Visible)]
    [InlineData(false, ViewStates.Invisible)]
    public void ConvertValue_Invisible_ReturnsExpectedValue(bool value, ViewStates expectedResult)
    {
        var converter = VisibilityConverter.Invisible;

        var result = converter.ConvertValue(value);

        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(ViewStates.Visible, true)]
    [InlineData(ViewStates.Invisible, false)]
    [InlineData(ViewStates.Gone, false)]
    public void ConvertValueBack_Gone_ReturnsExpectedValue(ViewStates value, bool expectedResult)
    {
        var converter = VisibilityConverter.Gone;

        var result = converter.ConvertValueBack(value);

        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(ViewStates.Visible, true)]
    [InlineData(ViewStates.Invisible, false)]
    [InlineData(ViewStates.Gone, false)]
    public void ConvertValueBack_Invisible_ReturnsExpectedValue(ViewStates value, bool expectedResult)
    {
        var converter = VisibilityConverter.Invisible;

        var result = converter.ConvertValueBack(value);

        Assert.Equal(expectedResult, result);
    }
}
