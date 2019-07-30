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

        private InternalTestClass CreateInternalWeakDelegateProvider()
        {
            return new InternalTestClass(_callCounter);
        }

        private InternalGenericWeakDelegateProvider CreateInternalGenericWeakDelegateProvider()
        {
            return new InternalGenericWeakDelegateProvider(_callCounter);
        }

        private InternalStaticTestClass CreateInternalStaticWeakDelegateProvider()
        {
            InternalStaticTestClass.CallCounter = _callCounter;
            return new InternalStaticTestClass();
        }

        private PublicTestClass CreatePublicWeakDelegateProvider()
        {
            return new PublicTestClass(_callCounter);
        }

        private PublicGenericWeakDelegateProvider CreatePublicGenericWeakDelegateProvider()
        {
            return new PublicGenericWeakDelegateProvider(_callCounter);
        }

        private PublicStaticTestClass CreatePublicStaticWeakDelegateProvider()
        {
            PublicStaticTestClass.CallCounter = _callCounter;
            return new PublicStaticTestClass();
        }

        private NestedTestClass CreateNestedWeakDelegateProvider()
        {
            return new NestedTestClass(_callCounter);
        }

        private NestedGenericWeakDelegateProvider CreateNestedGenericWeakDelegateProvider()
        {
            return new NestedGenericWeakDelegateProvider(_callCounter);
        }
    }
}
