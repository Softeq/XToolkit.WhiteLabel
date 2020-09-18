// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Validation;
using Softeq.XToolkit.WhiteLabel.Validation.Rules;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Validation.Rules
{
    public class IsValueTrueRuleTests
    {
        private const string ErrorMessage = "error message";

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(ErrorMessage)]
        public void Ctor_WithValidValues_InitializesPropertiesCorrectly(string message)
        {
            var rule = new IsValueTrueRule(message);

            Assert.Equal(message, rule.ValidationMessage);
            Assert.IsAssignableFrom<IValidationRule<bool>>(rule);
        }

        [Fact]
        public void Check_ForFalseValue_ReturnsFalse()
        {
            var rule = new IsValueTrueRule(ErrorMessage);

            var isValid = rule.Check(false);

            Assert.False(isValid);
        }

        [Fact]
        public void Check_ForTrueValue_ReturnsTrue()
        {
            var rule = new IsValueTrueRule(ErrorMessage);

            var isValid = rule.Check(true);

            Assert.True(isValid);
        }
    }
}
