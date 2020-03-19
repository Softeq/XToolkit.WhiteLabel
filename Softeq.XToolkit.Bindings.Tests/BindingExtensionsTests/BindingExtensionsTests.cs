// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Windows.Input;
using NSubstitute;
using Xunit;

namespace Softeq.XToolkit.Bindings.Tests.BindingExtensionsTests
{
    public class BindingExtensionsTests : IDisposable
    {
        private readonly ICommand _command;
        private readonly StubProducer _obj;

        public BindingExtensionsTests()
        {
            _command = Substitute.For<ICommand>();
            _obj = new StubProducer();

            BindingExtensions.Initialize(new MockBindingFactory());
        }

        public void Dispose()
        {
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SetCommand_CommandCanExecute_Executes(bool canExecute)
        {
            // arrange
            _command.CanExecute(Arg.Any<object>()).Returns(canExecute);

            // act
            _obj.SetCommand(nameof(_obj.SimpleEvent), _command);

            // assert
            _obj.RaiseSimpleEvent();
            AssertHelpers.ReceivedCommandInterface(_command, canExecute, null);
        }

        [Theory]
        [MemberData(nameof(CommandDataProvider.Data), MemberType = typeof(CommandDataProvider))]
        public void SetCommandTArgs_CommandCanExecute_Executes(bool canExecute, object commandParameter)
        {
            // arrange
            _command.CanExecute(Arg.Any<string>()).Returns(canExecute);

            // act
            _obj.SetCommand<string>(nameof(_obj.GenericStringEvent), _command);

            // assert
            _obj.RaiseGenericStringEvent((string)commandParameter);
            AssertHelpers.ReceivedCommandInterface(_command, canExecute, commandParameter);
        }
    }
}
