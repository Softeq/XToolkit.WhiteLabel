// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.WhiteLabel.Validation
{
    /// <summary>
    ///    Verify that the data a user enters in a record meets the standards you specify before the user can save the record.
    /// </summary>
    /// <typeparam name="T">Type of rule.</typeparam>
    public interface IValidationRule<in T>
    {
        /// <summary>
        ///     Gets the validation error message that will be displayed if validation fails.
        /// </summary>
        string ValidationMessage { get; }

        /// <summary>
        ///     Used to perform the required validation.
        /// </summary>
        /// <param name="value">Value to validate.</param>
        /// <returns>Returns <c>true</c> when value is valid.</returns>
        bool Check(T value);
    }
}
