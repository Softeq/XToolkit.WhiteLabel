// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Common.Command;
using Xunit;

namespace Softeq.XToolkit.Tests.Core.Common
{
    public class CommandTests
    {
        public CommandTests()
        {
            bool CanExecuteCommandFunc()
            {
                return _canExecuteCommand;
            }

            _command = new RelayCommand(TestAction, CanExecuteCommandFunc);
            _command.CanExecuteChanged += CommandOnCanExecuteChanged;

            _genericCommand = new RelayCommand<string>(TestAction, CanExecuteGeneric);
            _genericCommand.CanExecuteChanged += CommandOnCanExecuteChanged;
        }

        private readonly RelayCommand _command;
        private readonly RelayCommand<string> _genericCommand;

        private bool _canExecuteCommand;
        private int _callTestMethodCount;
        private const string PatameterValue = "test";

        private void CommandOnCanExecuteChanged(object sender, EventArgs eventArgs)
        {
            _callTestMethodCount += 3;
        }

        private void TestAction()
        {
            _callTestMethodCount++;
        }

        private void TestAction(string parameter)
        {
            Assert.Equal(parameter, PatameterValue);
            _callTestMethodCount += 5;
        }

        public bool CanExecuteGeneric(string parameter)
        {
            return _canExecuteCommand && parameter == PatameterValue;
        }

        /// <see cref="RelayCommand" />
        [Fact]
        public void RelayCommandTest()
        {
            _command.Execute(this);
            _genericCommand.Execute(PatameterValue);

            _canExecuteCommand = true;

            _command.RaiseCanExecuteChanged();
            _command.Execute(this);

            _genericCommand.RaiseCanExecuteChanged();
            _genericCommand.Execute(PatameterValue);

            Assert.Equal(_callTestMethodCount, 12);
        }
    }
}