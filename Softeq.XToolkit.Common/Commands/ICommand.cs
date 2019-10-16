// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Windows.Input;

namespace Softeq.XToolkit.Common.Commands
{
    /// <summary>
    ///     Defines a command.
    /// </summary>
    public interface ICommand<in T> : ICommand
    {
        bool CanExecute(T parameter);

        void Execute(T parameter);
    }
}
