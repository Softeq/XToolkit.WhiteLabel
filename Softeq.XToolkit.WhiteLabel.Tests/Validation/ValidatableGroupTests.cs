// Developed by Softeq Development Corporation
// http://www.softeq.com

using NSubstitute;
using Softeq.XToolkit.WhiteLabel.Validation;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Validation
{
    public class ValidatableGroupTests
    {
        [Fact]
        public void Validate_Empty_ReturnsTrue()
        {
            var group = new ValidatableGroup();

            var result = group.Validate();

            Assert.True(result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Validate_NotEmpty_ReturnsValue(bool validationResult)
        {
            var obj1 = CreateValidatableObject(validationResult);
            var group = new ValidatableGroup(obj1);

            var result = group.Validate();

            Assert.Equal(validationResult, result);
        }

        [Fact]
        public void Validate_NotEmpty_AllChecked()
        {
            var obj1 = CreateValidatableObject(false);
            var obj2 = CreateValidatableObject(true);
            var group = new ValidatableGroup(obj1, obj2);

            group.Validate();

            obj1.Received(1).Validate();
            obj2.Received(1).Validate();
        }

        [Fact]
        public void Validate_NotEmpty_ReturnsCorrectValue()
        {
            var obj1 = CreateValidatableObject(true);
            var obj2 = CreateValidatableObject(false);
            var group = new ValidatableGroup(obj1, obj2);

            var result = group.Validate();

            Assert.False(result);
        }

        private IValidatableObject CreateValidatableObject(bool validationResult)
        {
            var obj = Substitute.For<IValidatableObject>();
            obj.Validate().Returns(validationResult);
            return obj;
        }
    }
}
