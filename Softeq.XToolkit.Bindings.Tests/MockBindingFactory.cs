// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq.Expressions;

namespace Softeq.XToolkit.Bindings.Tests
{
    public class MockBindingFactory : BindingFactoryBase
    {
        public override Binding<TSource, TTarget> CreateBinding<TSource, TTarget>(
            object source, Expression<Func<TSource>> sourcePropertyExpression, bool? resolveTopField,
            object target = null, Expression<Func<TTarget>> targetPropertyExpression = null,
            BindingMode mode = BindingMode.Default,
            TSource fallbackValue = default, TSource targetNullValue = default)
        {
            return new MockBinding<TSource, TTarget>(
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
            object source, Expression<Func<TSource>> sourcePropertyExpression, object target = null,
            Expression<Func<TTarget>> targetPropertyExpression = null,
            BindingMode mode = BindingMode.Default, TSource fallbackValue = default,
            TSource targetNullValue = default)
        {
            return new MockBinding<TSource, TTarget>(
                source,
                sourcePropertyExpression,
                target,
                targetPropertyExpression,
                mode,
                fallbackValue,
                targetNullValue);
        }

        public override Binding<TSource, TTarget> CreateBinding<TSource, TTarget>(
            object source, string sourcePropertyName, object target = null,
            string targetPropertyName = null, BindingMode mode = BindingMode.Default,
            TSource fallbackValue = default,
            TSource targetNullValue = default)
        {
            return new MockBinding<TSource, TTarget>(
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
            return null;
        }
    }
}
