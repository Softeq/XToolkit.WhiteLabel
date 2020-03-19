// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;
using NSubstitute;
using Softeq.XToolkit.Common.Commands;
using Xunit;

namespace Softeq.XToolkit.Bindings.Tests.BindingFactoryBaseTests
{
    [SuppressMessage("ReSharper", "RedundantTypeArgumentsOfMethod")]
    public class GetCommandHandlerGenericTests: IDisposable
    {
        private readonly BindingFactoryBase _factory;
        private readonly ICommand<string> _command;
        private readonly StubEvent _e;

        public GetCommandHandlerGenericTests()
        {
            _factory = new MockBindingFactory();
            _command = Substitute.For<ICommand<string>>();

            var obj = new StubProducer();
            _e = new StubEvent(obj, nameof(obj.GenericStringEvent));
        }

        public void Dispose()
        {
        }

        [Theory]
        [MemberData(nameof(CommandDataProvider.Data), MemberType = typeof(CommandDataProvider))]
        public void GetCommandHandlerGeneric_EventArgs_ReturnsCorrectHandler(bool canExecute, string eventArgs)
        {
            // arrange
            _command.CanExecute(Arg.Any<string>()).Returns(canExecute);

            // act
            var handler = _factory.GetCommandHandler<string>(_e.EventInfo, _e.EventName, _e.ElementType, _command);

            // assert
            AssertHelpers.CommandHandler((EventHandler<string>) handler, eventArgs, _command, canExecute, eventArgs);
        }

        [Theory]
        [MemberData(nameof(CommandDataProvider.Data), MemberType = typeof(CommandDataProvider))]
        public void GetCommandHandlerGeneric_NullEventInfo_ReturnsCorrectHandler(bool canExecute, string eventArgs)
        {
            // arrange
            _command.CanExecute(Arg.Any<string>()).Returns(canExecute);

            // act
            var handler = _factory.GetCommandHandler<string>(null, _e.EventName, _e.ElementType, _command);

            // assert
            AssertHelpers.CommandHandler((EventHandler<string>) handler, eventArgs, _command, canExecute, eventArgs);
        }

        [Theory]
        [MemberData(nameof(CommandDataProvider.Data), MemberType = typeof(CommandDataProvider))]
        public void GetCommandHandlerGeneric_NullEventName_ReturnsCorrectHandler(bool canExecute, string eventArgs)
        {
            // arrange
            _command.CanExecute(Arg.Any<string>()).Returns(canExecute);

            // act
            var handler = _factory.GetCommandHandler<string>(_e.EventInfo, null, _e.ElementType, _command);

            // assert
            AssertHelpers.CommandHandler((EventHandler<string>) handler, eventArgs, _command, canExecute, eventArgs);
        }

        [Theory]
        [MemberData(nameof(CommandDataProvider.Data), MemberType = typeof(CommandDataProvider))]
        public void GetCommandHandlerGeneric_NullElementType_ReturnsCorrectHandler(bool canExecute, string eventArgs)
        {
            // arrange
            _command.CanExecute(Arg.Any<string>()).Returns(canExecute);

            // act
            var handler = _factory.GetCommandHandler<string>(_e.EventInfo, _e.EventName, null, _command);

            // assert
            AssertHelpers.CommandHandler((EventHandler<string>) handler, eventArgs, _command, canExecute, eventArgs);
        }

        [Theory]
        [MemberData(nameof(CommandDataProvider.Data), MemberType = typeof(CommandDataProvider))]
        public void GetCommandHandlerGeneric_NullCommand_ReturnsHandlerWithNullRefException(bool canExecute, string eventArgs)
        {
            // arrange
            _command.CanExecute(Arg.Any<string>()).Returns(canExecute);

            // act
            var handler = _factory.GetCommandHandler<string>(_e.EventInfo, _e.EventName, _e.ElementType, null);

            // assert
            Assert.Throws<NullReferenceException>(() =>
                AssertHelpers.CommandHandler((EventHandler<string>) handler, eventArgs, _command, canExecute, eventArgs));
        }
    }
}
