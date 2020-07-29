// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Validation.Rules;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Validation.Rules
{
    public class EmailRuleTests
    {
        private readonly EmailRule _rule;

        public EmailRuleTests()
        {
            _rule = new EmailRule("error message");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("^%")]
        [InlineData("test")]
        [InlineData("test@test")]
        [InlineData("test.com")]
        public void Check_Incorrect_Invalid(string value)
        {
            var isValid = _rule.Check(value);

            Assert.False(isValid);
        }

        [Theory]
        [InlineData("test@test.com")]
        [InlineData("test123@test.io")]
        [InlineData("123@test.io")]
        public void Check_Correct_Valid(string value)
        {
            var isValid = _rule.Check(value);

            Assert.True(isValid);
        }
    }
}
