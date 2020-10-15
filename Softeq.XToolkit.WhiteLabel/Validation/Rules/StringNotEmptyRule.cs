// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.WhiteLabel.Validation.Rules
{
    public class StringNotEmptyRule : IValidationRule<string>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="StringNotEmptyRule"/> class.
        /// </summary>
        /// <param name="validationMessage">
        ///     The validation error message that will be displayed if validation fails.
        /// </param>
        public StringNotEmptyRule(string validationMessage)
        {
            ValidationMessage = validationMessage;
        }

        /// <inheritdoc />
        public string ValidationMessage { get; }

        /// <inheritdoc />
        public bool Check(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }
    }
}
