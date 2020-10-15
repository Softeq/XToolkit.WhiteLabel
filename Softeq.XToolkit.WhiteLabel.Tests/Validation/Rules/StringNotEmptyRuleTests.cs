// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Validation;
using Softeq.XToolkit.WhiteLabel.Validation.Rules;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Validation.Rules
{
    public class StringNotEmptyRuleTests
    {
        private const string ErrorMessage = "error message";

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(ErrorMessage)]
        public void Ctor_InitializesPropertiesCorrectly(
            string message)
        {
            var rule = new StringNotEmptyRule(message);

            Assert.Equal(message, rule.ValidationMessage);
            Assert.IsAssignableFrom<IValidationRule<string>>(rule);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Check_ForEmptyOrWhiteSpaceString_ReturnsFalse(string value)
        {
            var rule = new StringNotEmptyRule(ErrorMessage);

            var isValid = rule.Check(value);

            Assert.False(isValid);
        }

        [Theory]
        [InlineData("  . ")]
        [InlineData("a")]
        [InlineData("test")]
        public void Check_ForNonEmptyString_ReturnsTrue(string value)
        {
            var rule = new StringNotEmptyRule(ErrorMessage);

            var isValid = rule.Check(value);

            Assert.True(isValid);
        }
    }
}
