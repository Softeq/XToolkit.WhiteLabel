// Developed by Softeq Development Corporation
// http://www.softeq.com

using Xunit;

namespace Softeq.XToolkit.Common.Tests.ObservableObjectTests.Utils
{
    public class FactInDebugOnlyAttribute : FactAttribute
    {
        public FactInDebugOnlyAttribute()
        {
#if !DEBUG
            Skip = "Only running in debug build configuration.";
#endif
        }
    }
}
