// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.Common.Interfaces
{
    public interface IConverter<TOut, TIn>
    {
        TOut ConvertValue(TIn TIn, object parameter = null, string language = null);
        TIn ConvertValueBack(TOut value, object parameter = null, string language = null);
    }
}