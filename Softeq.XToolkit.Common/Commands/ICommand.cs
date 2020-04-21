// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Windows.Input;

namespace Softeq.XToolkit.Common.Commands
{
    /// <summary>
    ///     A generic interface representing a more specific version of <see cref="T:System.Windows.Input.ICommand"/>.
    /// </summary>
    /// <typeparam name="T">The type used as argument for the interface methods.</typeparam>
    public interface ICommand<in T> : ICommand
    {
        /// <summary>
        ///     Provides a strongly-typed variant of <see cref="ICommand.CanExecute(object)"/>.
        /// </summary>
        /// <param name="parameter">The input parameter.</param>
        /// <returns>Whether or not the current command can be executed.</returns>
        /// <remarks>Use this overload to avoid boxing, if <typeparamref name="T"/> is a value type.</remarks>
        bool CanExecute(T parameter);

        /// <summary>
        ///     Provides a strongly-typed variant of <see cref="ICommand.Execute(object)"/>.
        /// </summary>
        /// <param name="parameter">The input parameter.</param>
        /// <remarks>Use this overload to avoid boxing, if <typeparamref name="T"/> is a value type.</remarks>
        void Execute(T parameter);
    }

    public interface IRaisableCanExecute
    {
        /// <summary>
        ///     Raises the CanExecuteChanged event.
        /// </summary>
        void RaiseCanExecuteChanged();
    }
}
