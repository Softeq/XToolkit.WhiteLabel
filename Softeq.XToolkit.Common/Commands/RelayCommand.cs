// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using Softeq.XToolkit.Common.Weak;

namespace Softeq.XToolkit.Common.Commands
{
    /// <summary>
    ///     A command whose sole purpose is to relay its functionality to other
    ///     objects by invoking delegates. The default return value for the CanExecute
    ///     method is 'true'. This class does not allow you to accept command parameters in the
    ///     Execute and CanExecute callback methods.
    /// </summary>
    public class RelayCommand : ICommand, IRaisableCanExecute
    {
        private readonly WeakFunc<bool>? _canExecute;
        private readonly WeakAction _execute;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">
        ///     The execution logic. IMPORTANT: Note that closures are not supported at the moment
        ///     due to the use of WeakActions (see http://stackoverflow.com/questions/25730530/).
        /// </param>
        /// <param name="canExecute">
        ///     The execution status logic. Null-value means that execution is always allowed.
        ///     IMPORTANT: Note that closures are not supported at the moment due to the use of WeakActions
        ///     (see http://stackoverflow.com/questions/25730530/).
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     If the execute argument is null. IMPORTANT: Note that closures are not supported at the moment
        ///     due to the use of WeakActions (see http://stackoverflow.com/questions/25730530/).
        /// </exception>
        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            _execute = new WeakAction(execute);

            if (canExecute != null)
            {
                _canExecute = new WeakFunc<bool>(canExecute);
            }
        }

        /// <summary>
        ///     Occurs when changes occur that affect whether the command should execute.
        /// </summary>
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        ///     Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">This parameter will always be ignored.</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public bool CanExecute(object? parameter)
        {
            AssertParameterNotUsed(parameter);

            if (!_execute.IsAlive)
            {
                return false;
            }

            if (_canExecute == null)
            {
                return true;
            }

            if (!_canExecute.IsAlive)
            {
                return false;
            }

            return _canExecute.Execute();
        }

        /// <summary>
        ///     Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">This parameter will always be ignored.</param>
        public virtual void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                _execute.Execute();
            }
        }

        /// <summary>
        ///     Raises the <see cref="CanExecuteChanged" /> event.
        /// </summary>
        [SuppressMessage(
            "Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "The this keyword is used in the Silverlight version")]
        [SuppressMessage(
            "Microsoft.Design",
            "CA1030:UseEventsWhereAppropriate",
            Justification = "This cannot be an event")]
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        [Conditional("DEBUG")]
        private static void AssertParameterNotUsed(object? parameter)
        {
            if (parameter == null)
            {
                return;
            }

            Debug.WriteLine($"Command do not use parameter, but was provided with not-null value of type {parameter.GetType()}");
            Debug.WriteLine(Environment.StackTrace);
        }
    }
}
