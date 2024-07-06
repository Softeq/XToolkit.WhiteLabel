// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Windows.Input;
using Softeq.XToolkit.Common.Disposables;
using UIKit;

namespace Softeq.XToolkit.Bindings.iOS
{
    /// <summary>
    ///     The iOS-specific factory to create bindings.
    /// </summary>
    public class AppleBindingFactory : BindingFactoryBase
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
            return new AppleBinding<TSource, TTarget>(
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
            return new AppleBinding<TSource, TTarget>(
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
            return new AppleBinding<TSource, TTarget>(
                source,
                sourcePropertyName,
                target,
                targetPropertyName,
                mode,
                fallbackValue,
                targetNullValue);
        }

        /// <inheritdoc />
        public override string? GetDefaultEventNameForControl(Type type)
        {
            if (type == typeof(UIButton) || typeof(UIButton).IsAssignableFrom(type))
            {
                return "TouchUpInside";
            }

            if (type == typeof(UIBarButtonItem) || typeof(UIBarButtonItem).IsAssignableFrom(type))
            {
                return "Clicked";
            }

            if (type == typeof(UISwitch) || typeof(UISwitch).IsAssignableFrom(type))
            {
                return "ValueChanged";
            }

            return null;
        }

        /// <inheritdoc />
        public override IDisposable HandleCommandCanExecute<T>(
            object element,
            ICommand command,
            Binding<T, T>? commandParameterBinding)
        {
            if (element is UIControl control)
            {
                return HandleControlEnabled(control, command, commandParameterBinding);
            }

            return Disposable.Create(() => { });
        }

        private static IDisposable HandleControlEnabled<T>(
            UIControl control,
            ICommand command,
            Binding<T, T>? commandParameterBinding)
        {
            var commandParameter = commandParameterBinding == null
                ? default
                : commandParameterBinding.Value;

            control.BeginInvokeOnMainThread(
                () => control.Enabled = command.CanExecute(commandParameter));

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
                control.BeginInvokeOnMainThread(() => control.Enabled = command.CanExecute(commandParameter));
            }

            void OnCommandParameterBindingValueChanged(object? s, EventArgs args)
            {
                control.BeginInvokeOnMainThread(() => control.Enabled = command.CanExecute(commandParameterBinding.Value));
            }
        }
    }
}
