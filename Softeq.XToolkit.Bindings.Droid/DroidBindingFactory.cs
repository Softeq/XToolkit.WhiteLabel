// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Input;
using Android.Widget;
using Softeq.XToolkit.Common.Command;

namespace Softeq.XToolkit.Bindings.Droid
{
    public class DroidBindingFactory : IBindingFactory
    {
        public Binding<TSource, TTarget> CreateBinding<TSource, TTarget>(
            object source,
            Expression<Func<TSource>> sourcePropertyExpression,
            bool? resolveTopField,
            object target = null,
            Expression<Func<TTarget>> targetPropertyExpression = null,
            BindingMode mode = BindingMode.Default,
            TSource fallbackValue = default(TSource),
            TSource targetNullValue = default(TSource))
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

        public Binding<TSource, TTarget> CreateBinding<TSource, TTarget>(
            object source,
            Expression<Func<TSource>> sourcePropertyExpression,
            object target = null,
            Expression<Func<TTarget>> targetPropertyExpression = null,
            BindingMode mode = BindingMode.Default,
            TSource fallbackValue = default(TSource),
            TSource targetNullValue = default(TSource))
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

        public Binding<TSource, TTarget> CreateBinding<TSource, TTarget>(
            object source,
            string sourcePropertyName,
            object target = null,
            string targetPropertyName = null,
            BindingMode mode = BindingMode.Default,
            TSource fallbackValue = default(TSource),
            TSource targetNullValue = default(TSource))
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

        public Delegate GetCommandHandler(EventInfo info, string eventName, Type elementType, ICommand command)
        {
            Delegate result;

            if (string.IsNullOrEmpty(eventName)
                && elementType == typeof(CheckBox))
            {
                EventHandler<CompoundButton.CheckedChangeEventArgs> handler = (s, args) =>
                {
                    if (command.CanExecute(null))
                    {
                        command.Execute(null);
                    }
                };

                result = handler;
            }
            else
            {
                EventHandler handler = (s, args) =>
                {
                    if (command.CanExecute(null))
                    {
                        command.Execute(null);
                    }
                };

                result = handler;
            }

            return result;
        }

        public Delegate GetCommandHandler<T>(EventInfo info, string eventName, Type elementType,
            ICommand command,
            Binding<T, T> castedBinding)
        {
            Delegate result;

            if (string.IsNullOrEmpty(eventName)
                && elementType == typeof(CheckBox))
            {
                EventHandler<CompoundButton.CheckedChangeEventArgs> handler = (s, args) =>
                {
                    var param = castedBinding == null ? default(T) : castedBinding.Value;
                    command.Execute(param);
                };

                result = handler;
            }
            else
            {
                EventHandler handler = (s, args) =>
                {
                    var param = castedBinding == null ? default(T) : castedBinding.Value;
                    command.Execute(param);
                };

                result = handler;
            }

            return result;
        }

        public Delegate GetCommandHandler(EventInfo info, string eventName, Type elementType,
            ICommand command,
            object commandParameter)
        {
            Delegate result;

            if (string.IsNullOrEmpty(eventName)
                && elementType == typeof(CheckBox))
            {
                EventHandler<CompoundButton.CheckedChangeEventArgs> handler =
                    (s, args) => command.Execute(commandParameter);
                result = handler;
            }
            else
            {
                EventHandler handler = (s, args) => command.Execute(commandParameter);
                result = handler;
            }

            return result;
        }

        public Delegate GetCommandHandlerWithArgs<T>(
            EventInfo e,
            string eventName,
            Type t,
            ICommand<T> command)
        {
            EventHandler<T> handler = (s, args) => command.Execute(args);
            return handler;
        }

        public string GetDefaultEventNameForControl(Type type)
        {
            string eventName = null;

            if (type == typeof(CheckBox)
                || typeof(CheckBox).IsAssignableFrom(type))
            {
                eventName = "CheckedChange";
            }
            else if (type == typeof(Button)
                     || typeof(Button).IsAssignableFrom(type))
            {
                eventName = "Click";
            }
            else if (type == typeof(ImageButton)
                     || typeof(ImageButton).IsAssignableFrom(type))
            {
                eventName = "Click";
            }

            return eventName;
        }
    }
}