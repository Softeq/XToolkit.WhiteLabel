// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Windows.Input;
using NSubstitute;
using Softeq.XToolkit.Common.Commands;
using Xunit;

namespace Softeq.XToolkit.Bindings.Tests
{
    public static class AssertHelpers
    {
        public static void CommandHandler(
            Delegate handler,
            EventArgs eventArgs,
            ICommand command,
            bool canExecute,
            object commandParameter)
        {
            // basic
            Assert.NotNull(handler);
            Assert.IsAssignableFrom<EventHandler>(handler);

            // invokes handler for assert correct handler
            ((EventHandler)handler).Invoke(null, eventArgs);
            ReceivedCommandInterface(command, canExecute, commandParameter);
        }

        public static void CommandHandler<T>(
            Delegate handler,
            T eventArgs,
            ICommand<T> command,
            bool canExecute,
            T commandParameter)
        {
            // basic
            Assert.NotNull(handler);
            Assert.IsAssignableFrom<EventHandler<T>>(handler);

            // invokes handler for assert correct handler
            ((EventHandler<T>)handler).Invoke(null, eventArgs);
            ReceivedCommandInterface(command, canExecute, commandParameter);
        }

        public static void ReceivedCommandInterface(ICommand command, bool canExecute, object commandParameter)
        {
            command.Received(1).CanExecute(commandParameter);

            if (canExecute)
            {
                command.Received(1).Execute(commandParameter);
            }
            else
            {
                command.DidNotReceive().Execute(Arg.Any<object>());
            }
        }
    }
}
