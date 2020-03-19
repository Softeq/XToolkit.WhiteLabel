// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Input;
using Softeq.XToolkit.Common.Commands;

namespace Softeq.XToolkit.Bindings
{
    public interface IBindingFactory
    {
        Binding<TSource, TTarget> CreateBinding<TSource, TTarget>(
            object source,
            Expression<Func<TSource>> sourcePropertyExpression,
            bool? resolveTopField,
            object? target = null,
            Expression<Func<TTarget>>? targetPropertyExpression = null,
            BindingMode mode = BindingMode.Default,
            TSource fallbackValue = default,
            TSource targetNullValue = default);

        Binding<TSource, TTarget> CreateBinding<TSource, TTarget>(
            object source,
            Expression<Func<TSource>> sourcePropertyExpression,
            object? target = null,
            Expression<Func<TTarget>>? targetPropertyExpression = null,
            BindingMode mode = BindingMode.Default,
            TSource fallbackValue = default,
            TSource targetNullValue = default);

        Binding<TSource, TTarget> CreateBinding<TSource, TTarget>(
            object source,
            string sourcePropertyName,
            object? target = null,
            string? targetPropertyName = null,
            BindingMode mode = BindingMode.Default,
            TSource fallbackValue = default,
            TSource targetNullValue = default);

        Delegate GetCommandHandler(
            EventInfo info,
            string eventName,
            Type elementType,
            ICommand command,
            object? commandParameter = null);

        Delegate GetCommandHandler<T>(
            EventInfo info,
            string eventName,
            Type elementType,
            ICommand command,
            Binding<T, T> castedBinding);

        Delegate GetCommandHandler<T>(
            EventInfo info,
            string eventName,
            Type elementType,
            ICommand<T> command);

        string GetDefaultEventNameForControl(Type type);
    }
}
