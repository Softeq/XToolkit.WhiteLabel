// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Content;
using Softeq.XToolkit.Common.Droid.Extensions;
using Xunit;

namespace Softeq.XToolkit.Common.Droid.Tests.Extensions.ContextExtensionsTests;

public class ContextExtensionsTests
{
    private readonly Context _context = MainActivity.Current;

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(10)]
    public void PxToDp_DpToPx_Equivalence(double pixels)
    {
        var resultDp = _context.PxToDp(pixels);
        var resultPx = _context.DpToPx(resultDp);

        Assert.Equal(pixels, resultPx);
    }

    [Fact]
    public void GetStatusBarHeight_Default_PositiveValue()
    {
        var result = _context.GetStatusBarHeight();

        Assert.True(result > 0);
    }
}
