// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Text.RegularExpressions;

namespace Softeq.XToolkit.WhiteLabel.Validation.Rules
{
    public class EmailRule : IValidationRule<string>
    {
        public string ValidationMessage { get; set; } = string.Empty;

        public bool Check(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            var match = regex.Match(value);

            return match.Success;
        }
    }
}
