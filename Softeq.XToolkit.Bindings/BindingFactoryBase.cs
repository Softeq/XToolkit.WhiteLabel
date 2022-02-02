// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Input;

namespace Softeq.XToolkit.Bindings
{
    /// <summary>
    ///     The base factory to create bindings.
    /// </summary>
    public abstract class BindingFactoryBase : IBindingFactory
    {
        /// <inheritdoc />
        public abstract Binding<TSource, TTarget> CreateBinding<TSource, TTarget>(
            object source,
            Expression<Func<TSource>> sourcePropertyExpression,
            bool? resolveTopField,
            object? target = null,
            Expression<Func<TTarget>>? targetPropertyExpression = null,
            BindingMode mode = BindingMode.Default,
            [MaybeNull] TSource fallbackValue = default!,
            [MaybeNull] TSource targetNullValue = default!);

        /// <inheritdoc />
        public abstract Binding<TSource, TTarget> CreateBinding<TSource, TTarget>(
            object source,
            Expression<Func<TSource>> sourcePropertyExpression,
            object? target = null,
            Expression<Func<TTarget>>? targetPropertyExpression = null,
            BindingMode mode = BindingMode.Default,
            [MaybeNull] TSource fallbackValue = default!,
            [MaybeNull] TSource targetNullValue = default!);

        /// <inheritdoc />
        public abstract Binding<TSource, TTarget> CreateBinding<TSource, TTarget>(
            object source,
            string sourcePropertyName,
            object? target = null,
            string? targetPropertyName = null,
            BindingMode mode = BindingMode.Default,
            [MaybeNull] TSource fallbackValue = default!,
            [MaybeNull] TSource targetNullValue = default!);

        /// <inheritdoc />
        public abstract string? GetDefaultEventNameForControl(Type type);

        /// <inheritdoc />
        public abstract void HandleCommandCanExecute<T>(
            object element,
            ICommand command,
            Binding<T, T>? commandParameterBinding);

        /// <inheritdoc />
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

        /// <inheritdoc />
        public virtual Delegate GetCommandHandler<T>(
            EventInfo info,
            string eventName,
            Type elementType,
            ICommand command,
            Binding<T, T>? castedBinding)
        {
            EventHandler handler = (_, __) =>
            {
                var param = castedBinding == null ? default : castedBinding.Value;

                if (command.CanExecute(param))
                {
                    command.Execute(param);
                }
            };
            return handler;
        }

        /// <inheritdoc />
        public virtual Delegate GetCommandHandler<TEventArgs>(
            EventInfo info,
            string eventName,
            Type elementType,
            ICommand command)
        {
            EventHandler<TEventArgs> handler = (_, args) =>
            {
                if (command.CanExecute(args))
                {
                    command.Execute(args);
                }
            };
            return handler;
        }

        /// <inheritdoc />
        public virtual Delegate GetCommandHandler<T, TEventArgs>(
            EventInfo info,
            string eventName,
            Type elementType,
            ICommand command,
            T commandParameter)
        {
            EventHandler<TEventArgs> handler = (_, __) =>
            {
                if (command.CanExecute(commandParameter))
                {
                    command.Execute(commandParameter);
                }
            };
            return handler;
        }

        /// <inheritdoc />
        public virtual Delegate GetCommandHandler<T, TEventArgs>(
            EventInfo info,
            string eventName,
            Type elementType,
            ICommand command,
            Binding<T, T>? castedBinding)
        {
            EventHandler<TEventArgs> handler = (_, __) =>
            {
                var param = castedBinding == null ? default : castedBinding.Value;

                if (command.CanExecute(param))
                {
                    command.Execute(param);
                }
            };
            return handler;
        }
    }
}
