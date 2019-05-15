// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Input;
using Softeq.XToolkit.Common.Command;

namespace Softeq.XToolkit.Bindings
{
    public interface IBindingFactory
    {
        Binding<TSource, TTarget> CreateBinding<TSource, TTarget>(
            object source,
            Expression<Func<TSource>> sourcePropertyExpression,
            bool? resolveTopField,
            object target = null,
            Expression<Func<TTarget>> targetPropertyExpression = null,
            BindingMode mode = BindingMode.Default,
            TSource fallbackValue = default(TSource),
            TSource targetNullValue = default(TSource));

        Binding<TSource, TTarget> CreateBinding<TSource, TTarget>(
            object source,
            Expression<Func<TSource>> sourcePropertyExpression,
            object target = null,
            Expression<Func<TTarget>> targetPropertyExpression = null,
            BindingMode mode = BindingMode.Default,
            TSource fallbackValue = default(TSource),
            TSource targetNullValue = default(TSource));

        Binding<TSource, TTarget> CreateBinding<TSource, TTarget>(
            object source,
            string sourcePropertyName,
            object target = null,
            string targetPropertyName = null,
            BindingMode mode = BindingMode.Default,
            TSource fallbackValue = default(TSource),
            TSource targetNullValue = default(TSource));

        Delegate GetCommandHandler(EventInfo info,
            string eventName,
            Type elementType,
            ICommand command);

        Delegate GetCommandHandler<T>(
            EventInfo info,
            string eventName,
            Type elementType,
            ICommand command,
            Binding<T, T> castedBinding);

        Delegate GetCommandHandler(
            EventInfo info,
            string eventName,
            Type elementType,
            ICommand command,
            object commandParameter);

        Delegate GetCommandHandlerWithArgs<T>(
            EventInfo e,
            string eventName,
            Type t,
            ICommand<T> command);

        string GetDefaultEventNameForControl(Type type);
    }
}