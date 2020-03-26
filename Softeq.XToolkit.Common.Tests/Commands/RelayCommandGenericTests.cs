// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using Softeq.XToolkit.Common.Commands;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Commands
{
    public class RelayCommandGenericTests
    {
        [Fact]
        public void Constructor_Default_ReturnsICommand()
        {
            var command = new RelayCommand<string>(_ => {});

            Assert.IsAssignableFrom<ICommand>(command);
        }

        [Fact]
        public void Constructor_Default_ReturnsICommandGeneric()
        {
            var command = new RelayCommand<string>(_ => {});

            Assert.IsAssignableFrom<ICommand<string>>(command);
        }

        [Fact]
        public void Constructor_ExecuteActionNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var _ = new RelayCommand<string>(null);
            });
        }

        [Fact]
        [SuppressMessage("ReSharper", "RedundantArgumentDefaultValue")]
        public void Constructor_CanExecuteActionNull_CreatesCorrectCommand()
        {
            var _ = new RelayCommand<string>(__ => {}, null);
        }

        [Theory]
        [InlineData(CommandsDataProvider.DefaultParameter)]
        [InlineData(null)]
        public void CanExecute_ForObjectCommandWithoutCanExecuteAction_ReturnsTrue(string parameter)
        {
            var command = new RelayCommand<string>(_ => {});

            var result = command.CanExecute(parameter);

            Assert.True(result);
        }

        [Theory]
        [InlineData(1234)]
        [InlineData(null)]
        public void CanExecute_ForStructCommandWithoutCanExecuteAction_ReturnsTrue(object parameter)
        {
            var command = new RelayCommand<int>(_ => {});

            var result = command.CanExecute(parameter);

            Assert.True(result);
        }

        [Theory]
        [MemberData(nameof(CommandsDataProvider.Data), MemberType = typeof(CommandsDataProvider))]
        public void CanExecute_WithCanExecuteAction_ReturnsCorrectValue(object parameter, bool expectedResult)
        {
            var command = new RelayCommand<string>(_ => {}, p => p == CommandsDataProvider.DefaultParameter);

            var result = command.CanExecute(parameter);

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [MemberData(nameof(CommandsDataProvider.Data), MemberType = typeof(CommandsDataProvider))]
        public void CanExecute_StaticCanExecuteAction_Executes(object parameter, bool expectedResult)
        {
            var command = new RelayCommand<string>(_ => {}, CommandsDataProvider.CanExecuteWhenNotNull);

            var result = command.CanExecute(parameter);

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(1234, true)]
        [InlineData(null, false)]
        public void CanExecute_ForStructCommandAndStaticCanExecuteAction_ReturnsTrue(object parameter, bool expectedResult)
        {
            var command = new RelayCommand<int>(_ => {}, CommandsDataProvider.CanExecuteWhenPositive);

            var result = command.CanExecute(parameter);

            Assert.Equal(expectedResult, result);
        }
    }
}
