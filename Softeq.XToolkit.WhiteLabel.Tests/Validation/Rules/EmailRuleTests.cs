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
        [InlineData("j.@server1.domain.com")]
        public void Check_Incorrect_Invalid(string value)
        {
            var isValid = _rule.Check(value);

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
        public void Check_Correct_Valid(string value)
        {
            var isValid = _rule.Check(value);

            Assert.True(isValid);
        }

        [Fact]
        public void Check_CustomEmailPattern_Valid()
        {
            var rule = new EmailRule("error message", @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

            var isValid = rule.Check("test@domain.online");

            Assert.False(isValid);
        }
    }
}
