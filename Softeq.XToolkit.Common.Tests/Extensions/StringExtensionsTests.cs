// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Common.Extensions;
using Xunit;

#nullable disable

namespace Softeq.XToolkit.Common.Tests.Extensions
{
    public class StringExtensionsTests
    {
        [Fact]
        public void CapitalizeFirstLetter_ShouldReturnString_WhenGivenCorrectString()
        {
            Assert.Equal("One", "one".CapitalizeFirstLetter());
            Assert.Equal("TWO", "tWO".CapitalizeFirstLetter());
            Assert.Equal("Three", "Three".CapitalizeFirstLetter());
            Assert.Equal("Four five", "four five".CapitalizeFirstLetter());
            Assert.Equal("1", "1".CapitalizeFirstLetter());
        }

        [Fact]
        public void CapitalizeFirstLetter_ShouldTrowException_WhenGivenNull()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                (null as string).CapitalizeFirstLetter();
            });
        }

        [Fact]
        public void CapitalizeFirstLetter_ShouldTrowException_WhenGivenEmptyString()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                string.Empty.CapitalizeFirstLetter();
            });
        }

        [Fact]
        public void RemoveEmptyLines_ShouldReturnStringWithoutEmptyLines_WhenGivenStringWithEmptyLines()
        {
            const string expect = "1Line2Line3Line";
            const string input1 = "1Line\n\n2Line\r\n  \t  \n3Line";
            const string input2 = "1Line\n       \n2Line\r\n     \r\n3Line";

            var result1 = input1.RemoveEmptyLines();
            var result2 = input2.RemoveEmptyLines();

            Assert.Equal(expect, result1);
            Assert.Equal(expect, result2);
        }

        [Fact]
        public void RemoveEmptyLines_ShouldReturnStringWithoutEmptyLines_WhenGivenStringWithoutEmptyLines()
        {
            const string expect = "1Line2Line3Line";
            const string input = "1Line2Line3Line";

            var result = input.RemoveEmptyLines();

            Assert.Equal(expect, result);
        }

        [Fact]
        public void RemoveEmptyLines_ShouldReturnEmptyString_WhenGivenEmptyString()
        {
            Assert.Equal(string.Empty, string.Empty.RemoveEmptyLines());
        }

        [Fact]
        public void RemoveEmptyLines_ShouldThrowException_WhenGivenNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                (null as string).RemoveEmptyLines();
            });
        }

    }
}
