// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.WhiteLabel.Helpers;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Helpers.AvatarPlaceholderBuilderTests
{
    public partial class AvatarPlaceholderBuilderTests
    {
        [Fact]
        public void GetAbbreviationAndColor_WithoutColors_WithNullName_ThrowsCorrectException()
        {
            Assert.Throws<ArgumentNullException>(()
                => AvatarPlaceholderBuilder.GetAbbreviationAndColor(null!));
        }

        [Theory]
        [MemberData(
            nameof(AvatarPlaceholderBuilderTestsDataProvider.NoLettersNameData),
            MemberType = typeof(AvatarPlaceholderBuilderTestsDataProvider))]
        public void GetAbbreviationAndColor_WithoutColors_WithNoLettersName_ReturnsEmptyAbbreviationAndSomeColor(
            string name)
        {
            var res = AvatarPlaceholderBuilder.GetAbbreviationAndColor(name);

            Assert.NotNull(res.Text);
            Assert.Empty(res.Text);

            Assert.NotNull(res.Color);
            Assert.NotEmpty(res.Color);
        }

        [Theory]
        [MemberData(
            nameof(AvatarPlaceholderBuilderTestsDataProvider.LettersNameData),
            MemberType = typeof(AvatarPlaceholderBuilderTestsDataProvider))]
        public void GetAbbreviationAndColor_WithoutColors_WithLettersName_ReturnsValidAbbreviationAndSomeColor(
            string name, string expectedAbbreviation)
        {
            var res = AvatarPlaceholderBuilder.GetAbbreviationAndColor(name);

            Assert.Equal(expectedAbbreviation, res.Text);

            Assert.NotNull(res.Color);
            Assert.NotEmpty(res.Color);
        }

        [Theory]
        [MemberData(
            nameof(AvatarPlaceholderBuilderTestsDataProvider.ColorsData),
            MemberType = typeof(AvatarPlaceholderBuilderTestsDataProvider))]
        public void GetAbbreviationAndColor_WithAnyColors_WithNullName_ThrowsCorrectException(
           string[] colors)
        {
            Assert.Throws<ArgumentNullException>(()
                => AvatarPlaceholderBuilder.GetAbbreviationAndColor(null!, colors));
        }

        [Theory]
        [MemberData(
            nameof(AvatarPlaceholderBuilderTestsDataProvider.NonNullNameData),
            MemberType = typeof(AvatarPlaceholderBuilderTestsDataProvider))]
        public void GetAbbreviationAndColor_WithNullColors_WithNonNullName_ThrowsCorrectException(
           string name)
        {
            Assert.Throws<ArgumentNullException>(()
                => AvatarPlaceholderBuilder.GetAbbreviationAndColor(name, null!));
        }

        [Theory]
        [MemberData(
            nameof(AvatarPlaceholderBuilderTestsDataProvider.NonNullNameData),
            MemberType = typeof(AvatarPlaceholderBuilderTestsDataProvider))]
        public void GetAbbreviationAndColor_WithEmptyColors_WithNonNullName_ThrowsCorrectException(
           string name)
        {
            Assert.Throws<ArgumentException>(()
                => AvatarPlaceholderBuilder.GetAbbreviationAndColor(name, new string[] { }));
        }

        [Theory]
        [MemberData(
            nameof(AvatarPlaceholderBuilderTestsDataProvider.NoLettersNameData),
            MemberType = typeof(AvatarPlaceholderBuilderTestsDataProvider))]
        public void GetAbbreviationAndColor_WithNonEmptyColors_WithNoLettersName_ReturnsEmptyAbbreviationAndColorFromTheList(
            string name)
        {
            var colors = new[] { "Color1", "Color2", "Color3" };

            var res = AvatarPlaceholderBuilder.GetAbbreviationAndColor(name, colors);

            Assert.NotNull(res.Text);
            Assert.Empty(res.Text);

            Assert.Contains(res.Color, colors);
        }

        [Theory]
        [MemberData(
            nameof(AvatarPlaceholderBuilderTestsDataProvider.LettersNameData),
            MemberType = typeof(AvatarPlaceholderBuilderTestsDataProvider))]
        public void GetAbbreviationAndColor_WithNonEmptyColors_WithLettersName_ReturnsValidAbbreviationAndColorFromTheList(
           string name, string expectedAbbreviation)
        {
            var colors = new[] { "Color1", "Color2", "Color3" };

            var res = AvatarPlaceholderBuilder.GetAbbreviationAndColor(name, colors);

            Assert.Equal(expectedAbbreviation, res.Text);
            Assert.Contains(res.Color, colors);
        }
    }
}
