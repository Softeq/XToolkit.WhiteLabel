// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.WhiteLabel.Validation;
using Softeq.XToolkit.WhiteLabel.Validation.Rules;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Validation.Rules
{
    public class StringLengthRuleTests
    {
        private const string ErrorMessage = "error message";

        [Theory]
        [InlineData(-1, 0)]
        [InlineData(0, -1)]
        [InlineData(-2, -2)]
        [InlineData(-2, 2)]
        [InlineData(2, -2)]
        public void Ctor_WithNegativeValues_ThrowsCorrectException(int min, int max)
        {
            Assert.Throws<ArgumentException>(() => new StringLengthRule(min, max, ErrorMessage));
        }

        [Theory]
        [InlineData(5, 0)]
        [InlineData(2, 1)]
        public void Ctor_WithMinGreaterThanMax_ThrowsCorrectException(int min, int max)
        {
            Assert.Throws<ArgumentException>(() => new StringLengthRule(min, max, ErrorMessage));
        }

        [Theory]
        [InlineData(0, 0, null)]
        [InlineData(0, 5, "")]
        [InlineData(1, 2, ErrorMessage)]
        public void Ctor_WithValidValues_InitializesPropertiesCorrectly(int min, int max, string message)
        {
            var rule = new StringLengthRule(min, max, message);

            Assert.Equal(message, rule.ValidationMessage);
            Assert.IsAssignableFrom<IValidationRule<string>>(rule);
        }

        [Theory]
        [InlineData(0, 0, "1")]
        [InlineData(2, 2, null)]
        [InlineData(2, 2, "1")]
        [InlineData(2, 2, "123")]
        [InlineData(3, 6, null)]
        [InlineData(3, 6, "1")]
        [InlineData(3, 6, "12")]
        [InlineData(3, 6, "1234567")]
        public void Check_ForIncorrectLengthString_ReturnsFalse(int min, int max, string value)
        {
            var rule = new StringLengthRule(min, max, ErrorMessage);

            var isValid = rule.Check(value);

            Assert.False(isValid);
        }

        [Theory]
        [InlineData(0, 0, null)]
        [InlineData(0, 0, "")]
        [InlineData(2, 2, "12")]
        [InlineData(3, 6, "123")]
        [InlineData(3, 6, "1234")]
        [InlineData(3, 6, "12345")]
        [InlineData(3, 6, "123456")]
        public void Check_ForCorrectLengthString_ReturnsTrue(int min, int max, string value)
        {
            var rule = new StringLengthRule(min, max, ErrorMessage);

            var isValid = rule.Check(value);

            Assert.True(isValid);
        }
    }
}
