// Developed by Softeq Development Corporation
// http://www.softeq.com

using Xunit;

namespace Softeq.XToolkit.Common.Tests.ObservableObjectTests.Utils
{
    public class FactInDebugOnlyAttribute : FactAttribute
    {
        internal const string SkipMessage = "Only running in debug configuration.";

        public FactInDebugOnlyAttribute()
        {
#if !DEBUG
            Skip = SkipMessage;
#endif
        }
    }

    public class TheoryInDebugOnlyAttribute : TheoryAttribute
    {
        public TheoryInDebugOnlyAttribute()
        {
#if !DEBUG
            Skip = FactInDebugOnlyAttribute.SkipMessage;
#endif
        }
    }
}
