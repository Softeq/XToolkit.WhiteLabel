﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Softeq.XToolkit.Common.Weak;

namespace Softeq.XToolkit.Common.Commands
{
    /// <summary>
    ///     A generic command whose sole purpose is to relay its functionality to other
    ///     objects by invoking delegates. The default return value for the CanExecute
    ///     method is 'true'. This class allows you to accept command parameters in the
    ///     Execute and CanExecute callback methods.
    /// </summary>
    /// <typeparam name="T">The type of the command parameter.</typeparam>
    public class RelayCommand<T> : ICommand<T>, IRaisableCanExecute
    {
        private readonly WeakFunc<T, bool>? _canExecute;
        private readonly WeakAction<T> _execute;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RelayCommand{T}"/> class.
        /// </summary>
        /// <param name="execute">
        ///     The execution logic. IMPORTANT: Note that closures are not supported at the moment
        ///     due to the use of WeakActions (see http://stackoverflow.com/questions/25730530/).
        /// </param>
        /// <param name="canExecute">
        ///     The execution status logic. IMPORTANT: Note that closures are not supported at the moment
        ///     due to the use of WeakActions (see http://stackoverflow.com/questions/25730530/).
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">If the execute argument is null.</exception>
        public RelayCommand(Action<T> execute, Func<T, bool>? canExecute = null)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            _execute = new WeakAction<T>(execute);

            if (canExecute != null)
            {
                _canExecute = new WeakFunc<T, bool>(canExecute);
            }
        }

        /// <summary>
        ///     Occurs when changes occur that affect whether the command should execute.
        /// </summary>
        public event EventHandler? CanExecuteChanged;

        /// <inheritdoc />
        /// <summary>
        ///     Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">
        ///     Data used by the command. If the command does not require data
        ///     to be passed, this object can be set to a null reference.
        /// </param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public bool CanExecute(object? parameter)
        {
            if (_execute == null)
            {
                return false;
            }

            if (!_execute.IsStatic && !_execute.IsAlive)
            {
                return false;
            }

            if (_canExecute == null)
            {
                return true;
            }

            if (!_canExecute.IsStatic && !_canExecute.IsAlive)
            {
                return false;
            }

            return parameter switch
            {
                null when typeof(T).GetTypeInfo().IsValueType => _canExecute.Execute(default!),
                null => _canExecute.Execute(default!),
                T p => _canExecute.Execute(p),
                _ => false
            };
        }

        /// <inheritdoc />
        /// <summary>
        ///     Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        ///     Data used by the command. If the command does not require data
        ///     to be passed, this object can be set to a null reference.
        /// </param>
        public virtual void Execute(object? parameter)
        {
            if (parameter == null && typeof(T).GetTypeInfo().IsValueType)
            {
                throw new ArgumentException($"Relay Command wait parameter with type: {typeof(T)}", nameof(parameter));
            }

            Execute((T) parameter!);
        }

        public void Execute(T parameter)
        {
            if (CanExecute(parameter))
            {
                _execute.Execute(parameter);
            }
        }

        public bool CanExecute(T parameter)
        {
            return CanExecute(ReferenceEquals(parameter, null) ?
                default : (object) parameter);
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
            var handler = CanExecuteChanged;
            handler?.Invoke(this, EventArgs.Empty);
        }
    }
}
