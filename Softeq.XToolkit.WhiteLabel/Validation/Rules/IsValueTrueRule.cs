// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.WhiteLabel.Validation.Rules
{
    public class IsValueTrueRule : IValidationRule<bool>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="IsValueTrueRule"/> class.
        /// </summary>
        /// <param name="validationMessage">
        ///     The validation error message that will be displayed if validation fails.
        /// </param>
        public IsValueTrueRule(string validationMessage)
        {
            ValidationMessage = validationMessage;
        }

        /// <inheritdoc />
        public string ValidationMessage { get; }

        /// <inheritdoc />
        public bool Check(bool value)
        {
            return value;
        }
    }
}
