// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.Common.Tests.WeakTests.Utils
{
    public interface ICallCounter
    {
        void OnActionCalled();
        void OnActionCalled<TIn>(TIn parameter);
        TOut OnFuncCalled<TOut>();
        TOut OnFuncCalled<TIn, TOut>(TIn parameter);
    }
}
