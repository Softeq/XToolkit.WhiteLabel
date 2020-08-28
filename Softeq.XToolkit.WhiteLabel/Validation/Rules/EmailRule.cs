// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Text.RegularExpressions;

namespace Softeq.XToolkit.WhiteLabel.Validation.Rules
{
    public class EmailRule : IValidationRule<string>
    {
        private const string DefaultEmailPattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

        private readonly Regex _regex;

        public EmailRule(string validationMessage, string emailPattern = DefaultEmailPattern)
        {
            ValidationMessage = validationMessage;

            _regex = new Regex(emailPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
        }

        public string ValidationMessage { get; }

        public bool Check(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            var match = _regex.Match(value);

            return match.Success;
        }
    }
}
