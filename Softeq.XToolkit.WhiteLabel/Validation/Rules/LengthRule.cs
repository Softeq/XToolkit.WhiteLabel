// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.WhiteLabel.Validation.Rules
{
    public class LengthRule : IValidationRule<string>
    {
        private readonly int _min;
        private readonly int _max;

        public LengthRule(int min, int max = int.MaxValue)
        {
            if (min < 0)
            {
                throw new ArgumentException("Argument can't be negative", nameof(min));
            }

            _min = min;
            _max = max;
        }

        public string ValidationMessage { get; set; } = string.Empty;

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
