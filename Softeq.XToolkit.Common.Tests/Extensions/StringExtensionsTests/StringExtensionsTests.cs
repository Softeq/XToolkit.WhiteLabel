// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Globalization;
using System.Linq;
using Softeq.XToolkit.Common.Extensions;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Extensions.StringExtensionsTests
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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void TryParseDouble_NullOrEmptyText_ParsesToNull(string input)
        {
            double? result;

            var didParse = input.TryParseDouble(out result);

            Assert.True(didParse);
            Assert.Null(result);
        }

        [Theory]
        [InlineData("0", 0)]
        [InlineData("1", 1)]
        [InlineData("-1", -1)]
        [InlineData("0.1", 0.1)]
        [InlineData("-0.1", -0.1)]
        [InlineData("0.0000000123", 0.0000000123)]
        [InlineData("1234.56", 1234.56)]
        [InlineData("1,234.56", 1234.56)]
        [InlineData("-1,234.56", -1234.56)]
        public void TryParseDouble_DoubleTextUsCulture_ParsesCorrectly(string input, double expectedResult)
        {
            double? result;
            CultureInfo.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");

            var didParse = input.TryParseDouble(out result);

            Assert.True(didParse);
            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("0", 0)]
        [InlineData("1", 1)]
        [InlineData("-1", -1)]
        [InlineData("0,1", 0.1)]
        [InlineData("-0,1", -0.1)]
        [InlineData("0,0000000123", 0.0000000123)]
        [InlineData("1234,56", 1234.56)]
        [InlineData("1 234,56", 1234.56)]
        [InlineData("-1 234,56", -1234.56)]
        public void TryParseDouble_DoubleTextRuCulture_ParsesCorrectly(string input, double expectedResult)
        {
            double? result;
            CultureInfo.CurrentCulture = CultureInfo.CreateSpecificCulture("ru-RU");

            var didParse = input.TryParseDouble(out result);

            Assert.True(didParse);
            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(double.MinValue, "en-US")]
        [InlineData(double.MinValue, "ru-RU")]
        [InlineData(double.MaxValue, "en-US")]
        [InlineData(double.MaxValue, "ru-RU")]
        public void TryParseDouble_EdgeDoubleText_ParsesCorrectly(double inputDouble, string culture)
        {
            double? result;
            CultureInfo.CurrentCulture = CultureInfo.CreateSpecificCulture(culture);
            var input = inputDouble.ToString(CultureInfo.CurrentCulture);

            var didParse = input.TryParseDouble(out result);

            Assert.True(didParse);
            Assert.NotNull(result);
            Assert.Equal(inputDouble, result);
        }

        [Theory]
        [InlineData("0 0 0")]
        [InlineData("abc")]
        [InlineData("d1")]
        [InlineData("0.1f")]
        [InlineData("- 1.0")]
        public void TryParseDouble_NotDoubleText_ReturnsFalse(string input)
        {
            double? result;
            CultureInfo.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");

            var didParse = input.TryParseDouble(out result);

            Assert.False(didParse);
            Assert.Null(result);
        }

        [Theory]
        [InlineData("John Doe", "JD")]
        [InlineData("   John      Doe   ", "JD")]
        [InlineData("john lowercase doe", "JD")]
        [InlineData("John Sam Doe", "JD")]
        [InlineData("John S Doe", "JD")]
        [InlineData("John S. Doe", "JD")]
        [InlineData("Matt S. Doe, Jr.", "MD")]
        [InlineData("Matt S Doe Sr.", "MD")]
        [InlineData("John Doe III", "JD")]
        [InlineData("John Doe, III", "JD")]
        [InlineData("John X", "JX")]
        [InlineData("X Doe", "XD")]
        [InlineData("X A Doe", "XD")]
        [InlineData("John", "J")]
        [InlineData("John Sr.", "J")]
        [InlineData("John O'Doe", "JO")]
        [InlineData("John McDoe", "JM")]
        [InlineData("John \"Nick\" Doe", "JD")]
        [InlineData("1John 2Doe", "JD")]
        [InlineData("I4U doe589I", "ID")]
        [InlineData("존 도우", "존도")]
        [InlineData("abracadabra", "A")]
        [InlineData("#abracadabra", "A")]
        [InlineData("a c d e f g", "AG")]
        [InlineData("John not short name doe", "JD")]
        [InlineData("John \"not short name\" Doe", "JD")]
        public void GetInitialsForName_ForNameString_ReturnsCorrectString(string name, string expected)
        {
            var result = name.GetInitialsForName();

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("         ")]
        [InlineData("1 2 3 4 5")]
        public void GetInitialsForName_NoLettersString_ReturnsEmptyString(string name)
        {
            var expected = string.Empty;

            var result = name.GetInitialsForName();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void FindLinks_NullText_ThrowsArgumentNullException()
        {
            string text = null;

            var result = text.FindLinks();


            Assert.NotNull(result);
            Assert.Throws<ArgumentNullException>(() => result.ToList());
        }

        [Theory]
        [InlineData("")]
        [InlineData("         ")]
        [InlineData("Text without links 12345")]
        public void FindLinks_NoLinkText_ReturnsEmptyEnumerable(string text)
        {
            var result = text.FindLinks();

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Theory]
        [InlineData("Text with http://link.link", 1)]
        [InlineData("https://google.com", 1)]
        [InlineData("Links:http://link.link ; custom://somelink.ru ; https://link.to.link.from.link", 3)]
        public void FindLinks_TextWithLinks_ReturnsCorrectAmountOfLinks(string text, int linksCount)
        {
            var result = text.FindLinks();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(linksCount, result.Count());
        }
    }
}
