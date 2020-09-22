// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.WhiteLabel.Validation.Rules
{
    public class StringLengthRule : IValidationRule<string>
    {
        private readonly int _min;
        private readonly int _max;

        /// <summary>
        ///     Initializes a new instance of the <see cref="StringLengthRule"/> class.
        /// </summary>
        /// <param name="min">Minimum string length (inclusive).</param>
        /// <param name="max">Maximum string length (inclusive).</param>
        /// <param name="validationMessage">
        ///     The validation error message that will be displayed if validation fails.
        /// </param>
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="min"/> and <paramref name="max"/> parameters cannot be negative.
        ///     - and -
        ///     <paramref name="max"/> nust be greater or equal than <paramref name="min"/>.
        /// </exception>
        public StringLengthRule(int min, int max, string validationMessage)
        {
            if (min < 0)
            {
                throw new ArgumentException("Argument can't be negative", nameof(min));
            }

            if (max < 0)
            {
                throw new ArgumentException("Argument can't be negative", nameof(max));
            }

            if (max < min)
            {
                throw new ArgumentException("Max length must be greater or equal than Min length");
            }

            _min = min;
            _max = max;

            ValidationMessage = validationMessage;
        }

        /// <inheritdoc />
        public string ValidationMessage { get; }

        /// <inheritdoc />
        public bool Check(string value)
        {
            var length = value?.Length ?? 0;

            return length >= _min && length <= _max;
        }
    }
}
