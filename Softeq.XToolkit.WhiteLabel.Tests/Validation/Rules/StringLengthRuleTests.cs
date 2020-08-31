// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.WhiteLabel.Validation.Rules;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Validation.Rules
{
    public class StringLengthRuleTests
    {
        private readonly StringLengthRule _rule;

        public StringLengthRuleTests()
        {
            _rule = new StringLengthRule(3, 6, "error message");
        }

        [Fact]
        public void Ctor_MinNegative_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => new StringLengthRule(-1, 10, "message"));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Check_Empty_Invalid(string value)
        {
            var isValid = _rule.Check(value);

            Assert.False(isValid);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("12")]
        [InlineData("1234567")]
        public void Check_IncorrectLength_Invalid(string value)
        {
            var isValid = _rule.Check(value);

            Assert.False(isValid);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("1234")]
        [InlineData("12345")]
        [InlineData("123456")]
        public void Check_CorrectLength_Valid(string value)
        {
            var isValid = _rule.Check(value);

            Assert.True(isValid);
        }
    }
}
