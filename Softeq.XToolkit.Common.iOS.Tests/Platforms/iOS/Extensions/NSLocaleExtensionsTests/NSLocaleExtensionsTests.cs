// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;
using Foundation;
using Softeq.XToolkit.Common.iOS.Extensions;
using Xunit;

namespace Softeq.XToolkit.Common.iOS.Tests.Extensions.NSLocaleExtensionsTests;

[SuppressMessage("ReSharper", "InvokeAsExtensionMethod", Justification = "Need for tests")]
public class NSLocaleExtensionsTests
{
    [Fact]
    public void Is24HourFormat_Null_ThrowsArgumentNullException()
    {
        var locale = null as NSLocale;

        Assert.Throws<ArgumentNullException>(() =>
        {
            NSLocaleExtensions.Is24HourFormat(locale!);
        });
    }

    [Theory]
    [InlineData("en_US", false)]
    [InlineData("en_CA", false)]
    [InlineData("fil_PH", false)]
    [InlineData("ru_RU", true)]
    [InlineData("en_BY", true)]
    [InlineData("de_DE", true)]
    [InlineData("it_IT", true)]
    public void Is24HourFormat_24HourLocaleFormat_ReturnsTrue(string localeId, bool expectedResult)
    {
        var locale = NSLocale.FromLocaleIdentifier(localeId);

        var result = locale.Is24HourFormat();

        Assert.Equal(expectedResult, result);
    }
}
