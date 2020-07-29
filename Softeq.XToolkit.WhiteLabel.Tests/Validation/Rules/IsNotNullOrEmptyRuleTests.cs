// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Validation.Rules;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Validation.Rules
{
    public class IsNotNullOrEmptyRuleTests
    {
        private readonly IsNotNullOrEmptyRule _rule;

        public IsNotNullOrEmptyRuleTests()
        {
            _rule = new IsNotNullOrEmptyRule("error message");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Check_Incorrect_Invalid(string value)
        {
            var isValid = _rule.Check(value);

            Assert.False(isValid);
        }

        [Theory]
        [InlineData("test")]
        public void Check_Correct_Valid(string value)
        {
            var isValid = _rule.Check(value);

            Assert.True(isValid);
        }
    }
}
