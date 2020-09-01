// Developed by Softeq Development Corporation
// http://www.softeq.com

using NSubstitute;
using Softeq.XToolkit.WhiteLabel.Validation;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Validation
{
    public class ValidatableObjectTests
    {
        [Fact]
        public void Ctor_Default_CreatesCorrectly()
        {
            var obj = Create<string>();

            Assert.NotNull(obj);
        }

        [Fact]
        public void IsValid_Default_ReturnsTrue()
        {
            var obj = Create<string>();

            Assert.True(obj.IsValid);
        }

        [Fact]
        public void Errors_Default_ReturnsEmptyCollection()
        {
            var obj = Create<string>();

            Assert.Empty(obj.Errors);
        }

        [Fact]
        public void FirstError_Default_ReturnsEmptyString()
        {
            var obj = Create<string>();

            Assert.Empty(obj.FirstError);
        }

        [Fact]
        public void AddRule_WithCorrectRule_ExecutesCorrectly()
        {
            var rule = Substitute.For<IValidationRule<string>>();
            var obj = Create<string>();

            obj.AddRule(rule);
        }

        [Fact]
        public void Errors_AddNew_ChangeIsValidFalse()
        {
            var obj = Create<string>();

            obj.Errors.Add("Test error");

            Assert.False(obj.IsValid);
        }

        [Fact]
        public void Errors_Clear_ResetsIsValidTrue()
        {
            var obj = CreateWithError<string>();

            obj.Errors.Clear();

            Assert.True(obj.IsValid);
        }

        [Fact]
        public void FirstError_AddNewErrorToNotEmptyErrors_ReturnsFirst()
        {
            var firstErrorMessage = "New error 2";
            var obj = Create<string>();
            obj.Errors.Add(firstErrorMessage);

            obj.Errors.Add("New error 2");

            Assert.Equal(firstErrorMessage, obj.FirstError);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void Value_WithErrorAndCleanOnChange_ChangesIsValid(bool cleanErrors, bool expectedIsValid)
        {
            var obj = CreateWithError<string>();
            obj.CleanErrorsOnChange = cleanErrors;

            obj.Value = "test value";

            Assert.Equal(expectedIsValid, obj.IsValid);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void Validate_WithValidationRule_ChangesIsValid(bool checkResult, bool expectedIsValid)
        {
            var obj = CreateWithValidationRule<string>(checkResult);

            var result = obj.Validate();

            Assert.Equal(expectedIsValid, result);
            Assert.Equal(expectedIsValid, obj.IsValid);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void Validate_WithValidationRule_ChangesFirstError(bool checkResult, bool isFirstErrorEmpty)
        {
            var obj = CreateWithValidationRule<string>(checkResult);

            obj.Validate();

            Assert.Equal(isFirstErrorEmpty, string.IsNullOrEmpty(obj.FirstError));
        }

        private ValidatableObject<T> Create<T>()
        {
            return new ValidatableObject<T>();
        }

        private ValidatableObject<T> CreateWithError<T>()
        {
            var obj = Create<T>();
            obj.Errors.Add("Test error 1");
            return obj;
        }

        private ValidatableObject<T> CreateWithValidationRule<T>(bool checkResult)
        {
            var rule = Substitute.For<IValidationRule<T>>();
            rule.Check(Arg.Any<T>()).Returns(checkResult);
            rule.ValidationMessage.Returns("Test error");

            var obj = Create<T>();
            obj.AddRule(rule);
            return obj;
        }
    }
}
