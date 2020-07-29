// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.WhiteLabel.Validation.Rules
{
    public class StringLengthRule : IValidationRule<string>
    {
        private readonly int _min;
        private readonly int _max;

        public StringLengthRule(int min, int max, string validationMessage)
        {
            if (min < 0)
            {
                throw new ArgumentException("Argument can't be negative", nameof(min));
            }

            _min = min;
            _max = max;

            ValidationMessage = validationMessage;
        }

        public string ValidationMessage { get; }

        public bool Check(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            return value.Length >= _min && value.Length <= _max;
        }
    }
}
