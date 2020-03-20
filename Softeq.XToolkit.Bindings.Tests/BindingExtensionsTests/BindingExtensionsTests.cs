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
        public void SetCommand_SimpleSetCommandWithCanExecute_Executes(bool canExecute)
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
        public void SetCommand_GenericWithCommandParameter_Executes(bool canExecute, string commandParameter)
        {
            // arrange
            _command.CanExecute(Arg.Any<string>()).Returns(canExecute);

            // act
            _obj.SetCommand<string>(nameof(_obj.SimpleEvent), _command, commandParameter);

            // assert
            _obj.RaiseSimpleEvent();
            AssertHelpers.ReceivedCommandInterface(_command, canExecute, commandParameter);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SetCommand_GenericWithNullBinding_Executes(bool canExecute)
        {
            // arrange
            _command.CanExecute(Arg.Any<string>()).Returns(canExecute);

            // act
            _obj.SetCommand<string>(nameof(_obj.SimpleEvent), _command, null as Binding);

            // assert
            _obj.RaiseSimpleEvent();
            AssertHelpers.ReceivedCommandInterface(_command, canExecute, null);
        }

        [Theory]
        [MemberData(nameof(CommandDataProvider.Data), MemberType = typeof(CommandDataProvider))]
        public void SetCommand_GenericWithEventArgs_Executes(bool canExecute, object commandParameter)
        {
            // arrange
            _command.CanExecute(Arg.Any<string>()).Returns(canExecute);

            // act
            _obj.SetCommand<string>(nameof(_obj.GenericStringEvent), _command);

            // assert
            _obj.RaiseGenericStringEvent((string)commandParameter);
            AssertHelpers.ReceivedCommandInterface(_command, canExecute, commandParameter);
        }

        [Theory]
        [MemberData(nameof(CommandDataProvider.Data), MemberType = typeof(CommandDataProvider))]
        public void SetCommand_GenericWithEventArgsAndCommandParameter_Executes(bool canExecute, object commandParameter)
        {
            // arrange
            _command.CanExecute(Arg.Any<string>()).Returns(canExecute);

            // act
            _obj.SetCommand<string, string>(nameof(_obj.GenericStringEvent), _command, (string)commandParameter);

            // assert
            _obj.RaiseGenericStringEvent("event args will be ignored");
            AssertHelpers.ReceivedCommandInterface(_command, canExecute, commandParameter);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SetCommand_GenericWithEventArgsAndNullBinding_Executes(bool canExecute)
        {
            // arrange
            _command.CanExecute(Arg.Any<string>()).Returns(canExecute);

            // act
            _obj.SetCommand<string, string>(nameof(_obj.GenericStringEvent), _command, null as Binding);

            // assert
            _obj.RaiseGenericStringEvent("event args will be ignored");
            AssertHelpers.ReceivedCommandInterface(_command, canExecute, null);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SetCommandWithDisposing_SimpleCommand_SetsCommandCorrect(bool canExecute)
        {
            // arrange
            _command.CanExecute(Arg.Any<string>()).Returns(canExecute);

            // act
            _obj.SetCommandWithDisposing(nameof(_obj.SimpleEvent), _command);

            // assert
            _obj.RaiseSimpleEvent();
            AssertHelpers.ReceivedCommandInterface(_command, canExecute, null);
        }

        [Fact]
        public void SetCommandWithDisposing_SimpleCommand_ReturnsDisposable()
        {
            // act
            var result = _obj.SetCommandWithDisposing(nameof(_obj.SimpleEvent), _command);

            // assert
            Assert.IsAssignableFrom<IDisposable>(result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SetCommandWithDisposing_SimpleCommand_ReturnsCorrectDisposable(bool canExecute)
        {
            // arrange
            _command.CanExecute(Arg.Any<string>()).Returns(canExecute);

            // act
            var result = _obj.SetCommandWithDisposing(nameof(_obj.SimpleEvent), _command);

            // assert
            result.Dispose();

            _obj.RaiseSimpleEvent();

            _command.DidNotReceive().CanExecute(Arg.Any<object>());
            _command.DidNotReceive().CanExecute(Arg.Any<object>());
        }
    }
}
