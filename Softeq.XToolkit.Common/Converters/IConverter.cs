// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.Common.Converters
{
    /// <summary>
    ///     Represents methods for converting one type to another and back.
    /// </summary>
    /// <typeparam name="TOut">The type of the target property.</typeparam>
    /// <typeparam name="TIn">The type of the source property.</typeparam>
    public interface IConverter<TOut, TIn>
    {
        /// <summary>
        ///     Modifies the source value before passing it to the target object.
        /// </summary>
        /// <param name="value">The source value being passed to the target.</param>
        /// <param name="parameter">The parameter to be used in the converter logic.</param>
        /// <param name="language">The language to be used in the converter.</param>
        /// <returns>A converted value. If the method returns <c>null</c>, the valid <c>null</c> value is used.</returns>
        TOut ConvertValue(TIn value, object? parameter = null, string? language = null);

        /// <summary>
        ///     Modifies the target value before passing it to the source object.
        /// </summary>
        /// <param name="value">The target value being passed to the source.</param>
        /// <param name="parameter">The parameter to be used in the converter logic.</param>
        /// <param name="language">The language to be used in the converter.</param>
        /// <returns>A converted value. If the method returns <c>null</c>, the valid <c>null</c> value is used.</returns>
        TIn ConvertValueBack(TOut value, object? parameter = null, string? language = null);
    }
}
