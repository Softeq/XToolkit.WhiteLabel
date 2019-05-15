// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Input;
using Softeq.XToolkit.Common.Command;
using UIKit;

namespace Softeq.XToolkit.Bindings.iOS
{
    public class AppleBindingFactory : IBindingFactory
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

        public Binding<TSource, TTarget> CreateBinding<TSource, TTarget>(
            object source,
            Expression<Func<TSource>> sourcePropertyExpression,
            object target = null,
            Expression<Func<TTarget>> targetPropertyExpression = null,
            BindingMode mode = BindingMode.Default,
            TSource fallbackValue = default(TSource),
            TSource targetNullValue = default(TSource))
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

        public Binding<TSource, TTarget> CreateBinding<TSource, TTarget>(
            object source,
            string sourcePropertyName,
            object target = null,
            string targetPropertyName = null,
            BindingMode mode = BindingMode.Default,
            TSource fallbackValue = default(TSource),
            TSource targetNullValue = default(TSource))
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

        public Delegate GetCommandHandler(EventInfo info, string eventName, Type elementType, ICommand command)
        {
            // At the moment, all supported controls with default events
            // in iOS are using EventHandler, and not EventHandler<...>.
            EventHandler handler = (s, args) =>
            {
                if (command.CanExecute(null))
                {
                    command.Execute(null);
                }
            };
            return handler;
        }

        public Delegate GetCommandHandler<T>(EventInfo info, string eventName, Type elementType,
            ICommand command,
            Binding<T, T> castedBinding)
        {
            // At the moment, all supported controls with default events
            // in iOS are using EventHandler, and not EventHandler<...>.
            EventHandler handler = (s, args) =>
            {
                var param = castedBinding == null ? default(T) : castedBinding.Value;
                command.Execute(param);
            };
            return handler;
        }

        public Delegate GetCommandHandler(EventInfo info, string eventName, Type elementType,
            ICommand command,
            object commandParameter)
        {
            // At the moment, all supported controls with default events
            // in iOS are using EventHandler, and not EventHandler<...>.
            EventHandler handler = (s, args) => command.Execute(commandParameter);
            return handler;
        }

        public Delegate GetCommandHandlerWithArgs<T>(EventInfo e, string eventName, Type t, ICommand<T> command)
        {
            EventHandler<T> handler = (s, args) => command.Execute(args);
            return handler;
        }

        public string GetDefaultEventNameForControl(Type type)
        {
            string eventName = null;

            if (type == typeof(UIButton)
                || typeof(UIButton).IsAssignableFrom(type))
            {
                eventName = "TouchUpInside";
            }
            else if (type == typeof(UIBarButtonItem)
                     || typeof(UIBarButtonItem).IsAssignableFrom(type))
            {
                eventName = "Clicked";
            }
            else if (type == typeof(UISwitch)
                     || typeof(UISwitch).IsAssignableFrom(type))
            {
                eventName = "ValueChanged";
            }

            return eventName;
        }
    }
}