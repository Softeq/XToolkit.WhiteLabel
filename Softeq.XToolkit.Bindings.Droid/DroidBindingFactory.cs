// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Input;
using Android.Views;
using Android.Widget;
using Softeq.XToolkit.Common.Disposables;
using Softeq.XToolkit.Common.Threading;

namespace Softeq.XToolkit.Bindings.Droid
{
    /// <summary>
    ///     The Android-specific factory to create bindings.
    /// </summary>
    public class DroidBindingFactory : BindingFactoryBase
    {
        /// <inheritdoc />
        public override Binding<TSource, TTarget> CreateBinding<TSource, TTarget>(
            object source,
            Expression<Func<TSource>> sourcePropertyExpression,
            bool? resolveTopField,
            object? target = null,
            Expression<Func<TTarget>>? targetPropertyExpression = null,
            BindingMode mode = BindingMode.Default,
            [MaybeNull] TSource fallbackValue = default!,
            [MaybeNull] TSource targetNullValue = default!)
        {
            return new DroidBinding<TSource, TTarget>(
                source,
                sourcePropertyExpression,
                resolveTopField,
                target,
                targetPropertyExpression,
                mode,
                fallbackValue,
                targetNullValue);
        }

        /// <inheritdoc />
        public override Binding<TSource, TTarget> CreateBinding<TSource, TTarget>(
            object source,
            Expression<Func<TSource>> sourcePropertyExpression,
            object? target = null,
            Expression<Func<TTarget>>? targetPropertyExpression = null,
            BindingMode mode = BindingMode.Default,
            [MaybeNull] TSource fallbackValue = default!,
            [MaybeNull] TSource targetNullValue = default!)
        {
            return new DroidBinding<TSource, TTarget>(
                source,
                sourcePropertyExpression,
                target,
                targetPropertyExpression,
                mode,
                fallbackValue,
                targetNullValue);
        }

        /// <inheritdoc />
        public override Binding<TSource, TTarget> CreateBinding<TSource, TTarget>(
            object source,
            string sourcePropertyName,
            object? target = null,
            string? targetPropertyName = null,
            BindingMode mode = BindingMode.Default,
            [MaybeNull] TSource fallbackValue = default!,
            [MaybeNull] TSource targetNullValue = default!)
        {
            return new DroidBinding<TSource, TTarget>(
                source,
                sourcePropertyName,
                target,
                targetPropertyName,
                mode,
                fallbackValue,
                targetNullValue);
        }

        /// <inheritdoc />
        public override Delegate GetCommandHandler(
            EventInfo info,
            string eventName,
            Type elementType,
            ICommand command,
            object? commandParameter = null)
        {
            if (string.IsNullOrEmpty(eventName) && elementType == typeof(CheckBox))
            {
                return new EventHandler<CompoundButton.CheckedChangeEventArgs>((s, args) =>
                {
                    if (command.CanExecute(commandParameter))
                    {
                        command.Execute(commandParameter);
                    }
                });
            }

            return base.GetCommandHandler(info, eventName, elementType, command, commandParameter);
        }

        /// <inheritdoc />
        public override Delegate GetCommandHandler<T>(
            EventInfo info,
            string eventName,
            Type elementType,
            ICommand command,
            Binding<T, T>? castedBinding)
        {
            if (string.IsNullOrEmpty(eventName) && elementType == typeof(CheckBox))
            {
                return new EventHandler<CompoundButton.CheckedChangeEventArgs>((s, args) =>
                {
                    var param = castedBinding == null ? default : castedBinding.Value;

                    if (command.CanExecute(param))
                    {
                        command.Execute(param);
                    }
                });
            }

            return base.GetCommandHandler(info, eventName, elementType, command, castedBinding);
        }

        /// <inheritdoc />
        public override string? GetDefaultEventNameForControl(Type type)
        {
            if (type == typeof(CheckBox) || typeof(CheckBox).IsAssignableFrom(type))
            {
                return "CheckedChange";
            }

            if (type == typeof(Button) || typeof(Button).IsAssignableFrom(type))
            {
                return "Click";
            }

            if (type == typeof(ImageButton) || typeof(ImageButton).IsAssignableFrom(type))
            {
                return "Click";
            }

            return null;
        }

        /// <inheritdoc />
        public override IDisposable HandleCommandCanExecute<T>(
            object element,
            ICommand command,
            Binding<T, T>? commandParameterBinding)
        {
            if (element is View view)
            {
                return HandleViewEnabled(view, command, commandParameterBinding);
            }

            return Disposable.Create(() => { });
        }

        private static IDisposable HandleViewEnabled<T>(
            View view,
            ICommand command,
            Binding<T, T>? commandParameterBinding)
        {
            var commandParameter = commandParameterBinding == null
                ? default
                : commandParameterBinding.Value;

            Execute.BeginOnUIThread(
                () => view.Enabled = command.CanExecute(commandParameter));

            // set by CanExecute
            command.CanExecuteChanged += OnCommandCanExecuteChanged;

            // set by bindable command parameter
            if (commandParameterBinding != null)
            {
                commandParameterBinding.ValueChanged += OnCommandParameterBindingValueChanged;
            }

            return Disposable.Create(() =>
            {
                if (commandParameterBinding != null)
                {
                    commandParameterBinding.ValueChanged -= OnCommandParameterBindingValueChanged;
                }

                command.CanExecuteChanged -= OnCommandCanExecuteChanged;
            });

            void OnCommandCanExecuteChanged(object? s, EventArgs args)
            {
                Execute.BeginOnUIThread(() => view.Enabled = command.CanExecute(commandParameter));
            }

            void OnCommandParameterBindingValueChanged(object? s, EventArgs args)
            {
                Execute.BeginOnUIThread(() => view.Enabled = command.CanExecute(commandParameterBinding.Value));
            }
        }
    }
}
