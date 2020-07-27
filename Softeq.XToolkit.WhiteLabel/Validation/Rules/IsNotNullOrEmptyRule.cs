// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.WhiteLabel.Validation.Rules
{
    public class IsNotNullOrEmptyRule : IValidationRule<string>
    {
        public string ValidationMessage { get; set; } = string.Empty;

        public bool Check(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }
    }
}
