// Developed by Softeq Development Corporation
// http://www.softeq.com

using Xunit;

#pragma warning disable SA1649

namespace Softeq.XToolkit.Common.Tests.ObservableObjectTests.Utils
{
    public sealed class FactInDebugOnlyAttribute : FactAttribute
    {
        internal const string SkipMessage = "Only running in debug configuration.";

        public FactInDebugOnlyAttribute()
        {
#if !DEBUG
            Skip = SkipMessage;
#endif
        }
    }

    public sealed class TheoryInDebugOnlyAttribute : TheoryAttribute
    {
        public TheoryInDebugOnlyAttribute()
        {
#if !DEBUG
            Skip = FactInDebugOnlyAttribute.SkipMessage;
#endif
        }
    }
}
