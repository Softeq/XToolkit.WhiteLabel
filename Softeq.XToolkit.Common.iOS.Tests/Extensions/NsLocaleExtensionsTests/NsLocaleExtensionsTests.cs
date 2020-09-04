// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;
using Foundation;
using Softeq.XToolkit.Common.iOS.Extensions;
using Xunit;

namespace Softeq.XToolkit.Common.iOS.Tests.Extensions.NsLocaleExtensionsTests
{
    [SuppressMessage("ReSharper", "InvokeAsExtensionMethod", Justification = "Need for tests")]
    public class NsLocaleExtensionsTests
    {
        [Fact]
        public void Is24HourFormat_Null_ThrowsArgumentNullException()
        {
            var locale = null as NSLocale;

            Assert.Throws<ArgumentNullException>(() =>
            {
                NsLocaleExtensions.Is24HourFormat(locale!);
            });
        }

        [Fact]
        public void Is24HourFormat_24HourLocaleFormat_ReturnsTrue()
        {
            var locale = NSLocale.FromLocaleIdentifier("ru_RU");

            var result = locale.Is24HourFormat();

            Assert.True(result);
        }

        [Fact]
        public void Is24HourFormat_12HourLocaleFormat_ReturnsFalse()
        {
            var locale = NSLocale.FromLocaleIdentifier("en_US");

            var result = locale.Is24HourFormat();

            Assert.False(result);
        }
    }
}
