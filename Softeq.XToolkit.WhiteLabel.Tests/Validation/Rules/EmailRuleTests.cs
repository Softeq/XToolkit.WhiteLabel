// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.WhiteLabel.Validation;
using Softeq.XToolkit.WhiteLabel.Validation.Rules;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Validation.Rules
{
    public class EmailRuleTests
    {
        private const string ErrorMessage = "error message";
        private const string CustomEmailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(ErrorMessage)]
        public void Ctor_WithoutPattern_InitializesPropertiesCorrectly(
            string message)
        {
            var rule = new EmailRule(message);

            Assert.Equal(message, rule.ValidationMessage);
            Assert.IsAssignableFrom<IValidationRule<string>>(rule);
        }

        [Theory]
        [PairwiseData]
        public void Ctor_WithNonNullPattern_InitializesPropertiesCorrectly(
            [CombinatorialValues(null, "", ErrorMessage)] string message,
            [CombinatorialValues("", CustomEmailPattern)] string emailPattern)
        {
            var rule = new EmailRule(message, emailPattern);

            Assert.Equal(message, rule.ValidationMessage);
            Assert.IsAssignableFrom<IValidationRule<string>>(rule);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(ErrorMessage)]
        public void Ctor_WithNullPattern_ThrowsCorrectException(
            string message)
        {
            Assert.Throws<ArgumentNullException>(() => new EmailRule(message, null));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("^%")]
        [InlineData("test")]
        [InlineData("test@test")]
        [InlineData("test.com")]
        [InlineData("j.@server1.domain.com")]
        public void Check_WithDefaultEmailPattern_ForInvalidEmail_ReturnsFalse(string value)
        {
            var rule = new EmailRule(ErrorMessage);

            var isValid = rule.Check(value);

            Assert.False(isValid);
        }

        [Theory]
        [InlineData("test@test.com")]
        [InlineData("test123@test.io")]
        [InlineData("123@test.io")]
        [InlineData("test@domain.online")]
        [InlineData("test_name@test.com")]
        [InlineData("test.name@test.com")]
        [InlineData("test.name+2@test.com")]
        public void Check_WithDefaultEmailPattern_ForValidEmail_ReturnsTrue(string value)
        {
            var rule = new EmailRule(ErrorMessage);

            var isValid = rule.Check(value);

            Assert.True(isValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("test@domain.online")]
        [InlineData("123@test.b")]
        public void Check_WithCustomEmailPattern_ForInvalidEmail_ReturnsFalse(string value)
        {
            var rule = new EmailRule(ErrorMessage, CustomEmailPattern);

            var isValid = rule.Check(value);

            Assert.False(isValid);
        }

        [Theory]
        [InlineData("test@domain.223")]
        [InlineData("123@test.by")]
        public void Check_WithCustomEmailPattern_ForValidEmail_ReturnsTrue(string value)
        {
            var rule = new EmailRule(ErrorMessage, CustomEmailPattern);

            var isValid = rule.Check(value);

            Assert.True(isValid);
        }
    }
}
