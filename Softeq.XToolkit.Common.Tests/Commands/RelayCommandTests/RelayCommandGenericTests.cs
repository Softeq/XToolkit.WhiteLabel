// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using NSubstitute;
using Softeq.XToolkit.Common.Commands;
using Xunit;
using Command = Softeq.XToolkit.Common.Tests.Commands.RelayCommandTests.RelayCommandsFactory;

namespace Softeq.XToolkit.Common.Tests.Commands.RelayCommandTests
{
    [SuppressMessage("ReSharper", "xUnit1026", Justification = "Generic parameters used just for test case generation")]
    public class RelayCommandGenericTests
    {
        [Fact]
        public void Constructor_Default_ReturnsICommand()
        {
            var command = new RelayCommand<string>(_ => { });

            Assert.IsAssignableFrom<ICommand>(command);
        }

        [Fact]
        public void Constructor_Default_ReturnsICommandGeneric()
        {
            var command = new RelayCommand<string>(_ => { });

            Assert.IsAssignableFrom<ICommand<string>>(command);
        }

        [Fact]
        public void Constructor_ExecuteActionNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _ = new RelayCommand<string>(null!);
            });
        }

        [Fact]
        public void Constructor_AllDelegatesProvided_CreatesCorrectCommand()
        {
            _ = new RelayCommand<string>(_ => { }, _ => true);
        }

        [Fact]
        public void Constructor_CanExecuteActionIsNull_CreatesCorrectCommand()
        {
            _ = new RelayCommand<string>(_ => { });
        }

        [Theory]
        [InlineData(CommandsDataProvider.DefaultParameter)]
        [InlineData(null)]
        public void CanExecute_ForObjectCommandWithoutCanExecuteAction_ReturnsTrue(string parameter)
        {
            var command = new RelayCommand<string>(_ => { });

            var result = command.CanExecute(parameter);

            Assert.True(result);
        }

        [Theory]
        [InlineData(1234)]
        public void CanExecute_ForStructCommandWithoutCanExecuteAction_ReturnsTrue(object parameter)
        {
            var command = new RelayCommand<int>(_ => { });

            var result = command.CanExecute(parameter);

            Assert.True(result);
        }

        [Fact]
        public void CanExecute_ForStructCommandWithoutCanExecuteAction_ForNullValue_ReturnsFalse()
        {
            var command = new RelayCommand<int>(_ => { });

            var result = command.CanExecute(null);

            Assert.False(result);
        }

        [Theory]
        [MemberData(nameof(CommandsDataProvider.Data), MemberType = typeof(CommandsDataProvider))]
        public void CanExecute_WithCanExecuteAction_ReturnsCorrectValue(object parameter, bool expectedResult)
        {
            var command = new RelayCommand<string>(_ => { }, p => p == CommandsDataProvider.DefaultParameter);

            var result = command.CanExecute(parameter);

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [MemberData(nameof(CommandsDataProvider.Data), MemberType = typeof(CommandsDataProvider))]
        public void CanExecute_StaticCanExecuteAction_Executes(object parameter, bool expectedResult)
        {
            var command = new RelayCommand<string>(_ => { }, CommandsDataProvider.CanExecuteWhenNotNull);

            var result = command.CanExecute(parameter);

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(1234, true)]
        [InlineData(null, false)]
        public void CanExecute_ForStructCommandAndStaticCanExecuteAction_ReturnsTrue(object parameter, bool expectedResult)
        {
            var command = new RelayCommand<int>(_ => { }, CommandsDataProvider.CanExecuteWhenPositive);

            var result = command.CanExecute(parameter);

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(0, "test")]
        [InlineData("test", 0)]
        [InlineData(0, null)]
        public void Execute_WithUnsupportedParameterType_ThrowsException<TCommand, TParameter>(
            TCommand commandType,
            TParameter parameter)
        {
            var execute = Substitute.For<Action<TCommand>>();
            var command = Command.Create(execute);

            Assert.Throws<ArgumentException>(() => command.Execute(parameter));
        }

        [Theory]
        [InlineData("test")]
        public void CanExecute_AfterExecuteTargetGarbageCollected_ReturnsFalse<T>(T parameter)
        {
            var command = Command.WithGarbageCollectableExecuteTarget<T>(_ => { });

            GC.Collect();

            var result = command.CanExecute(parameter);

            Assert.False(result);
        }

        [Theory]
        [InlineData("test")]
        public void Execute_AfterExecuteTargetGarbageCollected_DoesNotExecute<T>(T parameter)
        {
            var execute = Substitute.For<Action<T>>();
            var command = Command.WithGarbageCollectableExecuteTarget(execute);

            GC.Collect();

            command.Execute(parameter);

            execute.DidNotReceive().Invoke(Arg.Any<T>());
        }

        [Theory]
        [InlineData("test")]
        public void CanExecute_AfterCanExecuteTargetGarbageCollected_ReturnsFalse<T>(T parameter)
        {
            var command = Command.WithGarbageCollectableCanExecuteTarget<T>(_ => { }, _ => true);

            GC.Collect();

            var result = command.CanExecute(parameter);

            Assert.False(result);
        }

        [Theory]
        [InlineData("test")]
        public void Execute_AfterCanExecuteTargetGarbageCollected_DoesNotExecute<T>(T parameter)
        {
            var execute = Substitute.For<Action<T>>();
            var command = Command.WithGarbageCollectableCanExecuteTarget(execute, _ => true);

            GC.Collect();

            command.Execute(parameter);

            execute.DidNotReceive().Invoke(Arg.Any<T>());
        }
    }
}
