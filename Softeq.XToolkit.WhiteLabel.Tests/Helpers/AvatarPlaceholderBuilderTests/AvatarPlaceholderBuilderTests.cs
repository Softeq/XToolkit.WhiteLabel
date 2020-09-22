// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.WhiteLabel.Helpers;
using Softeq.XToolkit.WhiteLabel.Tests.TestUtils;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Helpers.AvatarPlaceholderBuilderTests
{
    public class AvatarPlaceholderBuilderTests
    {
        #region GetAbbreviation

        [Fact]
        public void GetAbbreviation_WithNullName_ThrowsCorrectException()
        {
            Assert.Throws<ArgumentNullException>(()
                => AvatarPlaceholderBuilder.GetAbbreviation(null));
        }

        [Theory]
        [MemberData(
            nameof(AvatarPlaceholderBuilderDataProvider.EmptyOrWhiteSpaceNameData),
            MemberType = typeof(AvatarPlaceholderBuilderDataProvider))]
        public void GetAbbreviation_WithEmptyOrWhiteSpaceName_ReturnsEmptyAbbreviation(string name)
        {
            var abbr = AvatarPlaceholderBuilder.GetAbbreviation(name);

            Assert.NotNull(abbr);
            Assert.Empty(abbr);
        }

        [Theory]
        [MemberData(
            nameof(AvatarPlaceholderBuilderDataProvider.NonEmptyNameData),
            MemberType = typeof(AvatarPlaceholderBuilderDataProvider))]
        public void GetAbbreviation_WithNonEmptyName_ReturnsValidAbbreviation(string name)
        {
            var abbr = AvatarPlaceholderBuilder.GetAbbreviation(name);

            Assert.NotNull(abbr);
            Assert.NotEmpty(abbr);
            Assert.True(abbr.Length <= 3);

            foreach (var c in abbr)
            {
                Assert.True(char.IsLetterOrDigit(c));
                Assert.True(char.IsUpper(c));
                Assert.Contains(c, name, new IgnoreCaseCharComparer());
            }
        }

        #endregion

        #region GetColor

        [Fact]
        public void GetColor_WithoutColors_WithNullName_ThrowsCorrectException()
        {
            Assert.Throws<ArgumentNullException>(()
                => AvatarPlaceholderBuilder.GetColor(null));
        }

        [Theory]
        [MemberData(
            nameof(AvatarPlaceholderBuilderDataProvider.NonNullNameData),
            MemberType = typeof(AvatarPlaceholderBuilderDataProvider))]
        public void GetColor_WithoutColors_WithNonNullName_ReturnsSomeColor(string name)
        {
            var color = AvatarPlaceholderBuilder.GetColor(name);

            Assert.NotNull(color);
            Assert.NotEmpty(color);
        }

        [Theory]
        [MemberData(
            nameof(AvatarPlaceholderBuilderDataProvider.ColorsData),
            MemberType = typeof(AvatarPlaceholderBuilderDataProvider))]
        public void GetColor_WithAnyColors_WithNullName_ThrowsCorrectException(string[] colors)
        {
            Assert.Throws<ArgumentNullException>(()
                => AvatarPlaceholderBuilder.GetColor(null, colors));
        }

        [Theory]
        [MemberData(
            nameof(AvatarPlaceholderBuilderDataProvider.NonNullNameData),
            MemberType = typeof(AvatarPlaceholderBuilderDataProvider))]
        public void GetColor_WithNullColors_WithNonNullName_ThrowsCorrectException(string name)
        {
            Assert.Throws<ArgumentNullException>(()
                => AvatarPlaceholderBuilder.GetColor(name, null));
        }

        [Theory]
        [MemberData(
            nameof(AvatarPlaceholderBuilderDataProvider.NonNullNameData),
            MemberType = typeof(AvatarPlaceholderBuilderDataProvider))]
        public void GetColor_WithEmptyColors_WithNonNullName_ThrowsCorrectException(string name)
        {
            Assert.Throws<ArgumentException>(()
                => AvatarPlaceholderBuilder.GetColor(name, new string[] { }));
        }

        [Theory]
        [MemberData(
            nameof(AvatarPlaceholderBuilderDataProvider.NonNullNameData),
            MemberType = typeof(AvatarPlaceholderBuilderDataProvider))]
        public void GetColor_WithNonEmptyColors_WithNonNullName_ReturnsColorFromTheList(string name)
        {
            var colors = new string[] { "Color1", "Color2", "Color3" };
            var color = AvatarPlaceholderBuilder.GetColor(name, colors);

            Assert.Contains(color, colors);
        }

        #endregion

        #region GetAbbreviationAndColor

        [Fact]
        public void GetAbbreviationAndColor_WithoutColors_WithNullName_ThrowsCorrectException()
        {
            Assert.Throws<ArgumentNullException>(()
                => AvatarPlaceholderBuilder.GetAbbreviationAndColor(null));
        }

        [Theory]
        [MemberData(
            nameof(AvatarPlaceholderBuilderDataProvider.EmptyOrWhiteSpaceNameData),
            MemberType = typeof(AvatarPlaceholderBuilderDataProvider))]
        public void GetAbbreviationAndColor_WithoutColors_WithEmptyOrWhiteSpaceName_ReturnsEmptyAbbreviationAndSomeColor(
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
            nameof(AvatarPlaceholderBuilderDataProvider.NonEmptyNameData),
            MemberType = typeof(AvatarPlaceholderBuilderDataProvider))]
        public void GetAbbreviationAndColor_WithoutColors_WithNonEmptyName_ReturnsValidAbbreviationAndSomeColor(
            string name)
        {
            var res = AvatarPlaceholderBuilder.GetAbbreviationAndColor(name);

            Assert.NotNull(res.Text);
            Assert.NotEmpty(res.Text);
            Assert.True(res.Text.Length <= 3);

            Assert.NotNull(res.Color);
            Assert.NotEmpty(res.Color);

            foreach (var c in res.Text)
            {
                Assert.True(char.IsLetterOrDigit(c));
                Assert.True(char.IsUpper(c));
                Assert.Contains(c, name, new IgnoreCaseCharComparer());
            }
        }

        [Theory]
        [MemberData(
            nameof(AvatarPlaceholderBuilderDataProvider.ColorsData),
            MemberType = typeof(AvatarPlaceholderBuilderDataProvider))]
        public void GetAbbreviationAndColor_WithAnyColors_WithNullName_ThrowsCorrectException(
           string[] colors)
        {
            Assert.Throws<ArgumentNullException>(()
                => AvatarPlaceholderBuilder.GetAbbreviationAndColor(null, colors));
        }

        [Theory]
        [MemberData(
            nameof(AvatarPlaceholderBuilderDataProvider.NonNullNameData),
            MemberType = typeof(AvatarPlaceholderBuilderDataProvider))]
        public void GetAbbreviationAndColor_WithNullColors_WithNonNullName_ThrowsCorrectException(
           string name)
        {
            Assert.Throws<ArgumentNullException>(()
                => AvatarPlaceholderBuilder.GetAbbreviationAndColor(name, null));
        }

        [Theory]
        [MemberData(
            nameof(AvatarPlaceholderBuilderDataProvider.NonNullNameData),
            MemberType = typeof(AvatarPlaceholderBuilderDataProvider))]
        public void GetAbbreviationAndColor_WithEmptyColors_WithNonNullName_ThrowsCorrectException(
           string name)
        {
            Assert.Throws<ArgumentException>(()
                => AvatarPlaceholderBuilder.GetAbbreviationAndColor(name, new string[] { }));
        }

        [Theory]
        [MemberData(
            nameof(AvatarPlaceholderBuilderDataProvider.EmptyOrWhiteSpaceNameData),
            MemberType = typeof(AvatarPlaceholderBuilderDataProvider))]
        public void GetAbbreviationAndColor_WithNonEmptyColors_WithEmptyOrWhiteSpaceName_ReturnsEmptyAbbreviationAndColorFromTheList(
            string name)
        {
            var colors = new string[] { "Color1", "Color2", "Color3" };

            var res = AvatarPlaceholderBuilder.GetAbbreviationAndColor(name, colors);

            Assert.NotNull(res.Text);
            Assert.Empty(res.Text);

            Assert.Contains(res.Color, colors);
        }

        [Theory]
        [MemberData(
            nameof(AvatarPlaceholderBuilderDataProvider.NonEmptyNameData),
            MemberType = typeof(AvatarPlaceholderBuilderDataProvider))]
        public void GetAbbreviationAndColor_WithNonEmptyColors_WithNonEmptyName_ReturnsValidAbbreviationAndColorFromTheList(
           string name)
        {
            var colors = new string[] { "Color1", "Color2", "Color3" };

            var res = AvatarPlaceholderBuilder.GetAbbreviationAndColor(name, colors);

            Assert.NotNull(res.Text);
            Assert.NotEmpty(res.Text);
            Assert.True(res.Text.Length <= 3);

            foreach (var c in res.Text)
            {
                Assert.True(char.IsLetterOrDigit(c));
                Assert.True(char.IsUpper(c));
                Assert.Contains(c, name, new IgnoreCaseCharComparer());
            }

            Assert.Contains(res.Color, colors);
        }

        #endregion
    }
}
