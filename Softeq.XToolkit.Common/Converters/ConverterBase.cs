// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.Common.Converters
{
    /// <inheritdoc cref="IConverter{TOut,TIn}"/>
    public abstract class ConverterBase<TOut, TIn> : IConverter<TOut, TIn>
    {
        /// <inheritdoc />
        public abstract TOut ConvertValue(TIn value, object? parameter = null, string? language = null);

        /// <inheritdoc />
        public virtual TIn ConvertValueBack(TOut value, object? parameter = null, string? language = null)
        {
            throw new System.NotImplementedException();
        }
    }
}
