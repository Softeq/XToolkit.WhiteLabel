// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    /// <summary>
    ///     Model for transferring information while navigating from one screento another.
    /// </summary>
    public class NavigationParameterModel
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="NavigationParameterModel"/> class.
        /// </summary>
        /// <param name="value">Parameter value.</param>
        /// <param name="propertyInfo">
        ///     Model representing information about
        ///     a property that should store <paramref name="value"/>.
        /// </param>
        public NavigationParameterModel(object? value, PropertyInfoModel propertyInfo)
        {
            Value = value;
            PropertyInfo = propertyInfo;
        }

        /// <summary>
        ///     Gets the value of the parameter.
        /// </summary>
        public object? Value { get; }

        /// <summary>
        ///     Gets the model representing information about
        ///     a property that should store <paramref name="value"/>.
        /// </summary>
        public PropertyInfoModel PropertyInfo { get; }
    }
}