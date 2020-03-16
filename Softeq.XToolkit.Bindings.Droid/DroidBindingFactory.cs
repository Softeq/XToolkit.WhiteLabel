// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Input;
using Android.Widget;

namespace Softeq.XToolkit.Bindings.Droid
{
    public class DroidBindingFactory : BindingFactoryBase
    {
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

        public override Binding<TSource, TTarget> CreateBinding<TSource, TTarget>(
            object source,
            Expression<Func<TSource>> sourcePropertyExpression,
            object target = null,
            Expression<Func<TTarget>> targetPropertyExpression = null,
            BindingMode mode = BindingMode.Default,
            TSource fallbackValue = default,
            TSource targetNullValue = default)
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

        public override Binding<TSource, TTarget> CreateBinding<TSource, TTarget>(
            object source,
            string sourcePropertyName,
            object target = null,
            string targetPropertyName = null,
            BindingMode mode = BindingMode.Default,
            TSource fallbackValue = default,
            TSource targetNullValue = default)
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

        public override Delegate GetCommandHandler(
            EventInfo info,
            string eventName,
            Type elementType,
            ICommand command,
            object commandParameter = null)
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

        public override Delegate GetCommandHandler<T>(
            EventInfo info,
            string eventName,
            Type elementType,
            ICommand command,
            Binding<T, T> castedBinding)
        {
            if (string.IsNullOrEmpty(eventName) && elementType == typeof(CheckBox))
            {
                return new EventHandler<CompoundButton.CheckedChangeEventArgs>((s, args) =>
                {
                    object param = castedBinding == null ? default : castedBinding.Value;

                    if (command.CanExecute(param))
                    {
                        command.Execute(param);
                    }
                });
            }

            return base.GetCommandHandler(info, eventName, elementType, command, castedBinding);
        }

        public override string GetDefaultEventNameForControl(Type type)
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
    }
}
