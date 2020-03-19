// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Input;
using Softeq.XToolkit.Common.Commands;

namespace Softeq.XToolkit.Bindings
{
    public abstract class BindingFactoryBase : IBindingFactory
    {
        public abstract Binding<TSource, TTarget> CreateBinding<TSource, TTarget>(
            object source,
            Expression<Func<TSource>> sourcePropertyExpression, bool? resolveTopField,
            object? target = null,
            Expression<Func<TTarget>>? targetPropertyExpression = null,
            BindingMode mode = BindingMode.Default,
            TSource fallbackValue = default,
            TSource targetNullValue = default);

        public abstract Binding<TSource, TTarget> CreateBinding<TSource, TTarget>(
            object source,
            Expression<Func<TSource>> sourcePropertyExpression,
            object? target = null,
            Expression<Func<TTarget>>? targetPropertyExpression = null,
            BindingMode mode = BindingMode.Default,
            TSource fallbackValue = default,
            TSource targetNullValue = default);

        public abstract Binding<TSource, TTarget> CreateBinding<TSource, TTarget>(
            object source,
            string sourcePropertyName,
            object? target = null,
            string? targetPropertyName = null,
            BindingMode mode = BindingMode.Default,
            TSource fallbackValue = default,
            TSource targetNullValue = default);

        public abstract string GetDefaultEventNameForControl(Type type);

        public virtual Delegate GetCommandHandler(
            EventInfo info,
            string eventName,
            Type elementType,
            ICommand command,
            object? commandParameter = null)
        {
            EventHandler handler = (_, __) =>
            {
                if (command.CanExecute(commandParameter))
                {
                    command.Execute(commandParameter);
                }
            };
            return handler;
        }

        public virtual Delegate GetCommandHandler<T>(
            EventInfo info,
            string eventName,
            Type elementType,
            ICommand command,
            Binding<T, T> castedBinding)
        {
            EventHandler handler = (_, __) =>
            {
                object param = (castedBinding == null ? default : castedBinding.Value)!;

                if (command.CanExecute(param))
                {
                    command.Execute(param);
                }
            };
            return handler;
        }

        public virtual Delegate GetCommandHandler<T>(
            EventInfo info,
            string eventName,
            Type elementType,
            ICommand<T> command)
        {
            EventHandler<T> handler = (_, args) =>
            {
                if (command.CanExecute(args))
                {
                    command.Execute(args);
                }
            };
            return handler;
        }
    }
}
