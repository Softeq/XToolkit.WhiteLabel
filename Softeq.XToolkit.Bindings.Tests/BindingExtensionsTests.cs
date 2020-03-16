// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Windows.Input;
using NSubstitute;
using Xunit;

namespace Softeq.XToolkit.Bindings.Tests
{
    public class BindingExtensionsTests : IDisposable
    {
        private const string CommandParameter = "TEST_PARAMETER";

        private readonly ICommand _command;
        private readonly MockProducer _obj;

        public BindingExtensionsTests()
        {
            _command = Substitute.For<ICommand>();
            _obj = new MockProducer();

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

            _obj.RaiseSimpleEvent();

            // assert
            _command.Received(1).CanExecute(Arg.Any<object>());

            if (canExecute)
            {
                _command.Received(1).Execute(Arg.Any<object>());
            }
            else
            {
                _command.DidNotReceive().Execute(Arg.Any<object>());
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SetCommandTArgs_CommandCanExecute_Executes(bool canExecute)
        {
            // arrange
            _command.CanExecute(Arg.Any<string>()).Returns(canExecute);

            // act
            _obj.SetCommand<string>(nameof(_obj.GenericStringEvent), _command);

            _obj.RaiseGenericStringEvent(CommandParameter);

            // assert
            _command.Received(1).CanExecute(Arg.Is(CommandParameter));

            if (canExecute)
            {
                _command.Received(1).Execute(Arg.Is(CommandParameter));
            }
            else
            {
                _command.DidNotReceive().Execute(Arg.Any<object>());
            }
        }
    }
}
