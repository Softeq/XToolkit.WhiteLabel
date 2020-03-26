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
        private const string DefaultParameter = "CanExecute";

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

        [Fact]
        public void CanExecute_WithoutCanExecuteAction_ReturnsTrue()
        {
            var command = new RelayCommand<string>(_ => {});

            var result = command.CanExecute("Hello");

            Assert.True(result);
        }

        [Theory]
        [InlineData(DefaultParameter, true)]
        [InlineData(123, false)]
        [InlineData(null, false)]
        public void CanExecute_WithCanExecuteAction_ReturnsCorrectValue(object parameter, bool expectedResult)
        {
            var command = new RelayCommand<string>(_ => {}, p => p == DefaultParameter);

            var result = command.CanExecute(parameter);

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(DefaultParameter, true)]
        [InlineData(123, false)]
        [InlineData(null, false)]
        public void Execute_StaticCanExecuteAction_Executes(object canExecuteParameter, bool expectedResult)
        {
            var command = new RelayCommand<string>(_ => { }, CanExecuteWhenNotNull);

            var result = command.CanExecute(canExecuteParameter);

            Assert.Equal(expectedResult, result);
        }

        private static bool CanExecuteWhenNotNull(object parameter)
        {
            return parameter != null;
        }
    }
}
