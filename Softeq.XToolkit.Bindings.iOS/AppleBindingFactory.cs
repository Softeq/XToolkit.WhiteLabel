// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq.Expressions;
using System.Windows.Input;
using UIKit;

#nullable disable

namespace Softeq.XToolkit.Bindings.iOS
{
    public class AppleBindingFactory : BindingFactoryBase
    {
        /// <inheritdoc />
        public override Binding<TSource, TTarget> CreateBinding<TSource, TTarget>(
            object source,
            Expression<Func<TSource>> sourcePropertyExpression,
            bool? resolveTopField,
            object target = null,
            Expression<Func<TTarget>> targetPropertyExpression = null,
            BindingMode mode = BindingMode.Default,
            TSource fallbackValue = default,
            TSource targetNullValue = default)
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
            object target = null,
            Expression<Func<TTarget>> targetPropertyExpression = null,
            BindingMode mode = BindingMode.Default,
            TSource fallbackValue = default,
            TSource targetNullValue = default)
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
            object target = null,
            string targetPropertyName = null,
            BindingMode mode = BindingMode.Default,
            TSource fallbackValue = default,
            TSource targetNullValue = default)
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

        public override string GetDefaultEventNameForControl(Type type)
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

        public override void HandleCommandCanExecute<T>(
            object element,
            ICommand command,
            Binding<T, T> commandParameterBinding)
        {
            if (element is UIControl control)
            {
                HandleControlEnabled(control, command, commandParameterBinding);
            }
        }

        private static void HandleControlEnabled<T>(
            UIControl control,
            ICommand command,
            Binding<T, T> commandParameterBinding)
        {
            var commandParameter = commandParameterBinding == null
                ? default
                : commandParameterBinding.Value;

            control.BeginInvokeOnMainThread(
                () => control.Enabled = command.CanExecute(commandParameter));

            // set by CanExecute
            command.CanExecuteChanged += (s, args) =>
            {
                control.BeginInvokeOnMainThread(
                    () => control.Enabled = command.CanExecute(commandParameter));
            };

            // set by bindable command parameter
            if (commandParameterBinding != null)
            {
                commandParameterBinding.ValueChanged += (s, args) =>
                {
                    control.BeginInvokeOnMainThread(
                        () => control.Enabled = command.CanExecute(commandParameterBinding.Value));
                };
            }
        }
    }
}
