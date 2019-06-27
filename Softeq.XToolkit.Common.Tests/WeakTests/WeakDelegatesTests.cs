using NSubstitute;
using Softeq.XToolkit.Common.Tests.Helpers;
using Softeq.XToolkit.Tests.Core.Common.Helpers;

namespace Softeq.XToolkit.Common.Tests.WeakTests
{
    public partial class WeakDelegatesTests
    {
        private readonly IMethodRunner _callCounter;

        public WeakDelegatesTests()
        {
            _callCounter = Substitute.For<IMethodRunner>();
        }

        private InternalTestClass CreateInternalClass()
        {
            return new InternalTestClass(_callCounter);
        }
    }
}
