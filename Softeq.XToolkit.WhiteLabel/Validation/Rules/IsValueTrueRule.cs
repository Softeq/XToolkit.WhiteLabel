// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.WhiteLabel.Validation.Rules
{
    public class IsValueTrueRule : IValidationRule<bool>
    {
        public IsValueTrueRule(string validationMessage)
        {
            ValidationMessage = validationMessage;
        }

        public string ValidationMessage { get; }

        public bool Check(bool value)
        {
            return value;
        }
    }
}
