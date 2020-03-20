// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Windows.Input;
using NSubstitute;
using Xunit;

namespace Softeq.XToolkit.Bindings.Tests.BindingFactoryBaseTests
{
    public class GetCommandHandlerTests : IDisposable
    {
        private readonly BindingFactoryBase _factory;
        private readonly ICommand _command;
        private readonly StubEvent _e;

        public GetCommandHandlerTests()
        {
            _factory = new MockBindingFactory();
            _command = Substitute.For<ICommand>();

            var obj = new StubProducer();
            _e = new StubEvent(obj, nameof(obj.SimpleEvent));
        }

        public void Dispose()
        {
        }

        [Theory]
        [MemberData(nameof(CommandDataProvider.Data), MemberType = typeof(CommandDataProvider))]
        public void GetCommandHandler_CommandParameter_ReturnsCorrectHandler(bool canExecute, object commandParameter)
        {
            // arrange
            _command.CanExecute(Arg.Any<object>()).Returns(canExecute);

            // act
            var handler = _factory.GetCommandHandler(_e.EventInfo, _e.EventName, _e.ElementType, _command, commandParameter);

            // assert
            AssertHelpers.CommandHandler(handler, EventArgs.Empty, _command, canExecute, commandParameter);
        }

        [Theory]
        [MemberData(nameof(CommandDataProvider.Data), MemberType = typeof(CommandDataProvider))]
        public void GetCommandHandler_NullEventInfo_ReturnsCorrectHandler(bool canExecute, object commandParameter)
        {
            // arrange
            _command.CanExecute(Arg.Any<object>()).Returns(canExecute);

            // act
            var handler = _factory.GetCommandHandler(null, _e.EventName, _e.ElementType, _command, commandParameter);

            // assert
            AssertHelpers.CommandHandler(handler, EventArgs.Empty, _command, canExecute, commandParameter);
        }

        [Theory]
        [MemberData(nameof(CommandDataProvider.Data), MemberType = typeof(CommandDataProvider))]
        public void GetCommandHandler_NullEventName_ReturnsCorrectHandler(bool canExecute, object commandParameter)
        {
            // arrange
            _command.CanExecute(Arg.Any<object>()).Returns(canExecute);

            // act
            var handler = _factory.GetCommandHandler(_e.EventInfo, null, _e.ElementType, _command, commandParameter);

            // assert
            AssertHelpers.CommandHandler(handler, EventArgs.Empty, _command, canExecute, commandParameter);
        }

        [Theory]
        [MemberData(nameof(CommandDataProvider.Data), MemberType = typeof(CommandDataProvider))]
        public void GetCommandHandler_NullElementType_ReturnsCorrectHandler(bool canExecute, object commandParameter)
        {
            // arrange
            _command.CanExecute(Arg.Any<object>()).Returns(canExecute);

            // act
            var handler = _factory.GetCommandHandler(_e.EventInfo, _e.EventName, null, _command, commandParameter);

            // assert
            AssertHelpers.CommandHandler(handler, EventArgs.Empty, _command, canExecute, commandParameter);
        }

        [Theory]
        [MemberData(nameof(CommandDataProvider.Data), MemberType = typeof(CommandDataProvider))]
        public void GetCommandHandler_NullCommand_ReturnsHandlerWithNullRefException(bool canExecute, object commandParameter)
        {
            // arrange
            _command.CanExecute(Arg.Any<object>()).Returns(canExecute);

            // act
            var handler = _factory.GetCommandHandler(_e.EventInfo, _e.EventName, _e.ElementType, null, commandParameter);

            // assert
            Assert.Throws<NullReferenceException>(() =>
                AssertHelpers.CommandHandler(handler, EventArgs.Empty, _command, canExecute, commandParameter));
        }
    }
}
