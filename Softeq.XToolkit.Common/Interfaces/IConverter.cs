// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.Common.Interfaces
{
    /// <summary>
    ///     Represents methods for converting one type to another and back
    /// </summary>
    /// <typeparam name="TOut">Initial type</typeparam>
    /// <typeparam name="TIn">Result type</typeparam>
    public interface IConverter<TOut, TIn>
    {
        TOut ConvertValue(TIn TIn, object parameter = null, string language = null);
        TIn ConvertValueBack(TOut value, object parameter = null, string language = null);
    }
}
