// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;
using Foundation;
using Softeq.XToolkit.Common.iOS.Extensions;
using UIKit;
using Xunit;

namespace Softeq.XToolkit.Common.iOS.Tests.Extensions.NSStringExtensions
{
    [SuppressMessage("ReSharper", "InvokeAsExtensionMethod", Justification = "Need for tests")]
    public class NSStringExtensionsTests
    {
        [Fact]
        public void NewParagraphStyle_CreateNewInstance_ReturnsInstance()
        {
            var result = iOS.Extensions.NSStringExtensions.NewParagraphStyle;

            Assert.IsType<NSMutableParagraphStyle>(result);
        }

        [Fact]
        public void ToNSUrl_Null_ThrowsArgumentNullException()
        {
            var link = null as string;

            Assert.Throws<ArgumentNullException>(() =>
            {
                iOS.Extensions.NSStringExtensions.ToNSUrl(link!);
            });
        }

        [Theory]
        [InlineData("")]
        [InlineData("localhost")]
        [InlineData("example.com")]
        public void ToNSUrl_InvalidLink_ThrowsUriFormatException(string link)
        {
            Assert.Throws<UriFormatException>(() =>
            {
                link.ToNSUrl();
            });
        }

        [Theory]
        [InlineData("http://softeq.com", "http://softeq.com/")]
        [InlineData("https://softeq.com", "https://softeq.com/")]
        [InlineData("https://www.softeq.com/", "https://www.softeq.com/")]
        [InlineData("https://softeq.com//", "https://softeq.com//")]
        [InlineData("https://example.com/??a", "https://example.com/??a")]
        [InlineData("https://example.com/?arg=1", "https://example.com/?arg=1")]
        [InlineData("https://api.example.com:3000/", "https://api.example.com:3000/")]
        [InlineData("https://example.com/?arg=1&asd=&q=тест+-+test", "https://example.com/?arg=1&asd=&q=%D1%82%D0%B5%D1%81%D1%82+-+test")]
        [InlineData("https://com.dev.api.example.com/&a=1?b=2", "https://com.dev.api.example.com/&a=1?b=2")]
        [InlineData("https://com.dev.api.example.com/?a=b=", "https://com.dev.api.example.com/?a=b=")]
        [InlineData("http://localhost/../..", "http://localhost/")]
        [InlineData("http://localhost/%2E%2E/%2E%2E", "http://localhost/")]
        [InlineData("htTP://localhost/", "http://localhost/")]
        [InlineData("http://localHOST/", "http://localhost/")]
        [InlineData("https://localhost:3000", "https://localhost:3000/")]
        [InlineData("https://127.0.0.1", "https://127.0.0.1/")]
        [InlineData("https://127.0.0.1:8080", "https://127.0.0.1:8080/")]
        [InlineData("https://user:password@www.example.com:80/Home/Index.htm?q1=v1&q2=v2#FragmentName", "https://user:password@www.example.com:80/Home/Index.htm?q1=v1&q2=v2#FragmentName")]
        [InlineData("ftp://example.com", "ftp://example.com/")]
        [InlineData("mailto://example.com", "mailto://example.com")]
        [InlineData("mailto:test@example.com", "mailto:test@example.com")]
        [InlineData("test://localhost/", "test://localhost/")]
        [InlineData("localhost:3000", "localhost:3000")]
        [InlineData(@"\\some\directory\name\", "file://some/directory/name/")]
        public void ToNSUrl_ValidLink_ReturnsExpectedNSUrl(string link, string expectedLink)
        {
            var result = link.ToNSUrl();

            Assert.IsType<NSUrl>(result);
            Assert.Equal(expectedLink, result.AbsoluteString);
        }

        [Fact]
        public void BuildAttributedString_Null_ThrowsArgumentNullException()
        {
            var input = null as string;

            Assert.Throws<ArgumentNullException>(() =>
            {
                iOS.Extensions.NSStringExtensions.BuildAttributedString(input!);
            });
        }

        [Theory]
        [InlineData("")]
        [InlineData("test string")]
        public void BuildAttributedString_NotNull_ReturnsAttributedString(string input)
        {
            var result = input.BuildAttributedString();

            Assert.IsType<NSMutableAttributedString>(result);
            Assert.Equal(input, result.Value);
        }

        [Fact]
        public void BuildAttributedStringFromHtml_Null_ThrowsArgumentNullException()
        {
            var input = null as string;

            Assert.Throws<ArgumentNullException>(() =>
            {
                iOS.Extensions.NSStringExtensions.BuildAttributedStringFromHtml(input!);
            });
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("test string", "test string")]
        [InlineData("<p>test string</p>", "test string\n")]
        [InlineData("<b>test string</b>", "test string")]
        [InlineData("<i>test string</i>", "test string")]
        [InlineData("<p><i>test</i> - <b>string</b></p>", "test - string\n")]
        [InlineData("<html><body>test string</body></html>", "test string")]
        [InlineData("<html><body>test string", "test string")]
        public void BuildAttributedStringFromHtml_NotNull_ReturnsAttributedString(string html, string expectedResult)
        {
            var result = html.BuildAttributedStringFromHtml();

            Assert.IsType<NSMutableAttributedString>(result);
            Assert.Equal(expectedResult, result.Value);
        }

        [Theory]
        [InlineData("", NSStringEncoding.Unicode, "")]
        [InlineData("test строка \u2022", NSStringEncoding.UTF8, "test строка •")]
        [InlineData("test строка 👍", NSStringEncoding.UTF8, "test строка 👍")]
        [InlineData("“test” string", NSStringEncoding.UTF8, "“test” string")]
        [InlineData("<b>test</b> \u203C string", NSStringEncoding.UTF8, "test ‼ string")]
        public void BuildAttributedStringFromHtml_SpecificEncoding_ReturnsAttributedString(
            string html,
            NSStringEncoding encoding,
            string expectedResult)
        {
            var result = html.BuildAttributedStringFromHtml(encoding);

            Assert.IsType<NSMutableAttributedString>(result);
            Assert.Equal(expectedResult, result.Value);
        }

        [Fact]
        public void DetectLinks_NullAttributedString_ThrowsNullReferenceException()
        {
            var obj = null as NSMutableAttributedString;

            Assert.Throws<NullReferenceException>(() =>
            {
                iOS.Extensions.NSStringExtensions.DetectLinks(
                    obj!,
                    UIColor.Red,
                    NSUnderlineStyle.Single,
                    false,
                    out _);
            });
        }

        [Fact]
        public void DetectLinks_NullColor_ExecutesWithoutExceptions()
        {
            var obj = "test string".BuildAttributedString();
            var color = null as UIColor;

            var modifiedObj = obj.DetectLinks(color!, NSUnderlineStyle.Single, false, out _);

            Assert.IsType<NSMutableAttributedString>(modifiedObj);
        }

        [Theory]
        [InlineData("link: https://softeq.com", 1)]
        [InlineData("link: https://softeq.com, google: google.com", 1)]
        [InlineData("test string: https://www.softeq.com/featured_projects#mobile, example: http://example.com/test/page", 2)]
        public void DetectLinks_TextWithLinks_ExecutesWithoutExceptions(string text, int linkCount)
        {
            var obj = text.BuildAttributedString();

            var modifiedObj = obj.DetectLinks(
                UIColor.Red,
                NSUnderlineStyle.Single,
                true,
                out var tags);

            Assert.IsType<NSMutableAttributedString>(modifiedObj);
            Assert.Equal(linkCount, tags.Length);
        }
    }
}
