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
        public void GetColor_WithoutColors_WithNullName_ThrowsCorrectException()
        {
            Assert.Throws<ArgumentNullException>(()
                => AvatarPlaceholderBuilder.GetColor(null!));
        }

        [Theory]
        [MemberData(
            nameof(AvatarPlaceholderBuilderTestsDataProvider.NonNullNameData),
            MemberType = typeof(AvatarPlaceholderBuilderTestsDataProvider))]
        public void GetColor_WithoutColors_WithNonNullName_ReturnsSomeColor(string name)
        {
            var color = AvatarPlaceholderBuilder.GetColor(name);

            Assert.NotNull(color);
            Assert.NotEmpty(color);
        }

        [Theory]
        [MemberData(
            nameof(AvatarPlaceholderBuilderTestsDataProvider.ColorsData),
            MemberType = typeof(AvatarPlaceholderBuilderTestsDataProvider))]
        public void GetColor_WithAnyColors_WithNullName_ThrowsCorrectException(string[] colors)
        {
            Assert.Throws<ArgumentNullException>(()
                => AvatarPlaceholderBuilder.GetColor(null!, colors));
        }

        [Theory]
        [MemberData(
            nameof(AvatarPlaceholderBuilderTestsDataProvider.NonNullNameData),
            MemberType = typeof(AvatarPlaceholderBuilderTestsDataProvider))]
        public void GetColor_WithNullColors_WithNonNullName_ThrowsCorrectException(string name)
        {
            Assert.Throws<ArgumentNullException>(()
                => AvatarPlaceholderBuilder.GetColor(name, null!));
        }

        [Theory]
        [MemberData(
            nameof(AvatarPlaceholderBuilderTestsDataProvider.NonNullNameData),
            MemberType = typeof(AvatarPlaceholderBuilderTestsDataProvider))]
        public void GetColor_WithEmptyColors_WithNonNullName_ThrowsCorrectException(string name)
        {
            Assert.Throws<ArgumentException>(()
                => AvatarPlaceholderBuilder.GetColor(name, new string[] { }));
        }

        [Theory]
        [MemberData(
            nameof(AvatarPlaceholderBuilderTestsDataProvider.NonNullNameData),
            MemberType = typeof(AvatarPlaceholderBuilderTestsDataProvider))]
        public void GetColor_WithNonEmptyColors_WithNonNullName_ReturnsColorFromTheList(string name)
        {
            var colors = new[] { "Color1", "Color2", "Color3" };
            var color = AvatarPlaceholderBuilder.GetColor(name, colors);

            Assert.Contains(color, colors);
        }
    }
}
