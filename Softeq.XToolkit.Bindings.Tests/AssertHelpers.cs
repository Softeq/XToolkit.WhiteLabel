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
            EventHandler handler, EventArgs eventArgs,
            ICommand command, bool canExecute, object commandParameter)
        {
            // basic
            Assert.NotNull(handler);
            Assert.IsAssignableFrom<EventHandler>(handler);

            // invokes handler for assert correct handler
            handler.Invoke(null, eventArgs);
            ReceivedCommandInterface(command, canExecute, commandParameter);
        }

        public static void CommandHandler<T>(
            EventHandler<T> handler, T eventArgs,
            ICommand<T> command, bool canExecute, T commandParameter)
        {
            // basic
            Assert.NotNull(handler);
            Assert.IsAssignableFrom<EventHandler<T>>(handler);

            // invokes handler for assert correct handler
            handler.Invoke(null, eventArgs);
            ReceivedCommandInterface(command, canExecute, commandParameter);
        }

        public static void ReceivedCommandInterface(ICommand command, bool canExecute, object commandParameter)
        {
            command.Received(1).CanExecute(Arg.Is(commandParameter));

            if (canExecute)
            {
                command.Received(1).Execute(Arg.Is(commandParameter));
            }
            else
            {
                command.DidNotReceive().Execute(Arg.Is(commandParameter));
            }
        }

        // YP: Generic method for support NSubstitute to explicit resolving method signatures.
        public static void ReceivedCommandInterface<T>(ICommand<T> command, bool canExecute, T commandParameter)
        {
            command.Received(1).CanExecute(Arg.Is(commandParameter));

            if (canExecute)
            {
                command.Received(1).Execute(Arg.Is(commandParameter));
            }
            else
            {
                command.DidNotReceive().Execute(Arg.Is(commandParameter));
            }
        }
    }
}
