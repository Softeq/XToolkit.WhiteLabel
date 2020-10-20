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
        [Theory]
        [InlineData("one", "One")]
        [InlineData("tWO", "TWO")]
        [InlineData("Three", "Three")]
        [InlineData("four five", "Four five")]
        [InlineData("1", "1")]
        public void CapitalizeFirstLetter_NotEmptyString_ReturnsExpectedString(string input, string expected)
        {
            var result = input.CapitalizeFirstLetter();

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void CapitalizeFirstLetter_NullOrEmptyString_ThrowsException(string input)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                input.CapitalizeFirstLetter();
            });
        }

        [Theory]
        [InlineData("1Line\n\n2Line\r\n  \t  \n3Line", "1Line2Line3Line")]
        [InlineData("1Line\n       \n2Line\r\n     \r\n3Line", "1Line2Line3Line")]
        public void RemoveEmptyLines_StringWithEmptyLines_ReturnsStringWithoutEmptyLines(string input, string expected)
        {
            var result = input.RemoveEmptyLines();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void RemoveEmptyLines_StringtWithoutEmptyLines_ReturnsTheSameString()
        {
            const string Input = "1Line2Line3Line";

            var result = Input.RemoveEmptyLines();

            Assert.Equal(Input, result);
        }

        [Fact]
        public void RemoveEmptyLines_EmptyString_ReturnsEmptyString()
        {
            Assert.Equal(string.Empty, string.Empty.RemoveEmptyLines());
        }

        [Fact]
        public void RemoveEmptyLines_NullString_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                (null as string).RemoveEmptyLines();
            });
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void TryParseDouble_NullOrEmptyString_ParsesToNull(string input)
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
        public void TryParseDouble_DoubleStringUsCulture_ParsesCorrectly(string input, double expectedResult)
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
        public void TryParseDouble_DoubleStringRuCulture_ParsesCorrectly(string input, double expectedResult)
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
        public void TryParseDouble_EdgeDoubleString_ParsesCorrectly(double inputDouble, string culture)
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
        public void TryParseDouble_NotDoubleString_ReturnsFalse(string input)
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
        [InlineData("Links:http://link.link ; custom://softeq.com ; https://link.to.softeq.com.link", 3)]
        [InlineData("text softeq.com la-la-la softeq.com", 2)]
        [InlineData("softeq.com/test", 1)]
        [InlineData("www.softeq.com", 1)]
        public void FindLinks_TextWithLinks_ReturnsCorrectAmountOfLinks(string text, int linksCount)
        {
            var result = text.FindLinks();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(linksCount, result.Count());
        }
    }
}
