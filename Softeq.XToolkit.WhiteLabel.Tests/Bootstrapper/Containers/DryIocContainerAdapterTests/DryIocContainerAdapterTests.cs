// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using NSubstitute;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Containers;
using Softeq.XToolkit.WhiteLabel.Tests.Stubs;
using Xunit;
using IDryContainer = DryIoc.IContainer;

namespace Softeq.XToolkit.WhiteLabel.Tests.Bootstrapper.Containers.DryIocContainerAdapterTests
{
    public class DryIocContainerAdapterTests
    {
        private readonly IDryContainer _container;
        private readonly DryIocContainerAdapter _dryIocContainerAdapter;

        public DryIocContainerAdapterTests()
        {
            _container = Substitute.For<IDryContainer>();
            _dryIocContainerAdapter = new DryIocContainerAdapter();
        }

        [Fact]
        public void Initialize_Null_ThrowsCorrectException()
        {
            Assert.Throws<ArgumentNullException>(() => _dryIocContainerAdapter.Initialize(null));
        }

        [Fact]
        public void Initialize_NotNull_DoesNotThrowException()
        {
            _dryIocContainerAdapter.Initialize(_container);
        }

        [Theory]
        [MemberData(
            nameof(DryIocContainerAdapterTestsDataProvider.ParamsData),
            MemberType = typeof(DryIocContainerAdapterTestsDataProvider))]
        public void Resolve_BeforeInitialization_ThrowsCorrectException(object[] parameters)
        {
            Assert.Throws<InvalidOperationException>(() => _dryIocContainerAdapter.Resolve<ViewModelStub>(parameters));
        }

        [Theory]
        [MemberData(
            nameof(DryIocContainerAdapterTestsDataProvider.ParamsData),
            MemberType = typeof(DryIocContainerAdapterTestsDataProvider))]
        public void Resolve_AfterInitialization_DoesNotThrow(object[] parameters)
        {
            _dryIocContainerAdapter.Initialize(_container);
            _dryIocContainerAdapter.Resolve<ViewModelStub>(parameters);

            //TODO: PS: Find out why the following does not work:
            //_container.Received(1).Resolve<ViewModelStub>(
            //    Arg.Is(parameters),
            //    IfUnresolved.Throw,
            //    (Type) null,
            //    (object) null);
        }
    }
}
