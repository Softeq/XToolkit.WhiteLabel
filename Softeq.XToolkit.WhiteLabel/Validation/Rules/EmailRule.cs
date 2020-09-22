// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Text.RegularExpressions;

namespace Softeq.XToolkit.WhiteLabel.Validation.Rules
{
    public class EmailRule : IValidationRule<string>
    {
        private const string DefaultEmailPattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

        private readonly Regex _regex;

        /// <summary>
        ///     Initializes a new instance of the <see cref="EmailRule"/> class.
        /// </summary>
        /// <param name="validationMessage">
        ///     The validation error message that will be displayed if validation fails.
        /// </param>
        /// <param name="emailPattern">
        ///     Custom regular expression for checking the input data.
        ///     If not set, the default regular expression is used.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="emailPattern"/> parameter cannot be <see langword="null"/>.
        /// </exception>
        public EmailRule(string validationMessage, string emailPattern = DefaultEmailPattern)
        {
            if (emailPattern == null)
            {
                throw new ArgumentNullException(nameof(emailPattern));
            }

            ValidationMessage = validationMessage;

            _regex = new Regex(emailPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
        }

        /// <inheritdoc />
        public string ValidationMessage { get; }

        /// <inheritdoc />
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
