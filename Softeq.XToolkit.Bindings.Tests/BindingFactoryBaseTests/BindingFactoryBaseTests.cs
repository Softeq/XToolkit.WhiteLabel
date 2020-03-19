// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Reflection;
using System.Windows.Input;
using NSubstitute;
using Xunit;

namespace Softeq.XToolkit.Bindings.Tests.BindingFactoryBaseTests
{
    public class BindingFactoryBaseTests : IDisposable
    {
        private readonly ICommand _command;
        private readonly MockBindingFactory _factory;

        private readonly string _eventName;
        private readonly Type _elementType;
        private readonly EventInfo _eventInfo;

        public BindingFactoryBaseTests()
        {
            _factory = new MockBindingFactory();
            _command = Substitute.For<ICommand>();

            var obj = new StubProducer();
            _eventName = nameof(obj.SimpleEvent);
            _elementType = obj.GetType();
            _eventInfo = _elementType.GetRuntimeEvent(_eventName);
        }

        public void Dispose()
        {
        }

        [Theory]
        [MemberData(nameof(BindingFactoryBaseDataProvider.Data), MemberType = typeof(BindingFactoryBaseDataProvider))]
        public void GetCommandHandler_CommandParameter_ReturnsCorrectHandler(bool canExecute, object commandParameter)
        {
            // arrange
            _command.CanExecute(Arg.Any<object>()).Returns(canExecute);

            // act
            var handler = _factory.GetCommandHandler(_eventInfo, _eventName, _elementType, _command, commandParameter);

            // assert
            Assert_CommandHandler((EventHandler)handler, EventArgs.Empty, canExecute, commandParameter);
        }

        [Theory]
        [MemberData(nameof(BindingFactoryBaseDataProvider.Data), MemberType = typeof(BindingFactoryBaseDataProvider))]
        public void GetCommandHandler_NullEventInfo_ReturnsCorrectHandler(bool canExecute, object commandParameter)
        {
            // arrange
            _command.CanExecute(Arg.Any<object>()).Returns(canExecute);

            // act
            var handler = _factory.GetCommandHandler(null, _eventName, _elementType, _command, commandParameter);

            // assert
            Assert_CommandHandler((EventHandler)handler, EventArgs.Empty, canExecute, commandParameter);
        }

        [Theory]
        [MemberData(nameof(BindingFactoryBaseDataProvider.Data), MemberType = typeof(BindingFactoryBaseDataProvider))]
        public void GetCommandHandler_NullEventName_ReturnsCorrectHandler(bool canExecute, object commandParameter)
        {
            // arrange
            _command.CanExecute(Arg.Any<object>()).Returns(canExecute);

            // act
            var handler = _factory.GetCommandHandler(_eventInfo, null, _elementType, _command, commandParameter);

            // assert
            Assert_CommandHandler((EventHandler)handler, EventArgs.Empty, canExecute, commandParameter);
        }

        [Theory]
        [MemberData(nameof(BindingFactoryBaseDataProvider.Data), MemberType = typeof(BindingFactoryBaseDataProvider))]
        public void GetCommandHandler_NullElementType_ReturnsCorrectHandler(bool canExecute, object commandParameter)
        {
            // arrange
            _command.CanExecute(Arg.Any<object>()).Returns(canExecute);

            // act
            var handler = _factory.GetCommandHandler(_eventInfo, _eventName, null, _command, commandParameter);

            // assert
            Assert_CommandHandler((EventHandler)handler, EventArgs.Empty, canExecute, commandParameter);
        }

        [Theory]
        [MemberData(nameof(BindingFactoryBaseDataProvider.Data), MemberType = typeof(BindingFactoryBaseDataProvider))]
        public void GetCommandHandler_NullCommand_ThrowsNullRefExceptionAfterInvoke(bool canExecute, object commandParameter)
        {
            // arrange
            _command.CanExecute(Arg.Any<object>()).Returns(canExecute);

            // act
            var handler = _factory.GetCommandHandler(_eventInfo, _eventName, _elementType, null, commandParameter);

            // assert
            Assert.Throws<NullReferenceException>(() =>
                Assert_CommandHandler((EventHandler) handler, EventArgs.Empty, canExecute, commandParameter));
        }


        // Helpers:

        private void Assert_CommandHandler(EventHandler handler, EventArgs eventArgs, bool canExecute, object commandParameter)
        {
            // basic asserts
            Assert.NotNull(handler);
            Assert.IsAssignableFrom<EventHandler>(handler);

            // invokes handler for assert correct handler

            handler.Invoke(this, eventArgs);

            _command.Received(1).CanExecute(Arg.Is(commandParameter));

            if (canExecute)
            {
                _command.Received(1).Execute(Arg.Is(commandParameter));
            }
            else
            {
                _command.DidNotReceive().Execute(Arg.Is(commandParameter));
            }
        }
    }
}
