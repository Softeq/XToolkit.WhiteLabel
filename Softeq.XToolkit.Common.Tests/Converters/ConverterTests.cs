// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Converters
{
    public class ConverterTests
    {
        [Fact]
        public void ConvertValue()
        {
            var converter = ConverterHelper.CreateTwoWayConverter();

            var result = converter.ConvertValue(true);

            Assert.Equal("True", result);
        }

        [Fact]
        public void ConvertValueBack()
        {
            var converter = ConverterHelper.CreateTwoWayConverter();

            var result = converter.ConvertValueBack("true");

            Assert.True(result);
        }

        [Fact]
        public void ConvertValueBack_ThrowsNotImplementedException()
        {
            var converter = ConverterHelper.CreateOneWayConverter();

            Assert.Throws<NotImplementedException>(() => converter.ConvertValueBack("true"));
        }
    }
}
