// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq.Expressions;

namespace Softeq.XToolkit.Bindings.Tests
{
    public class MockBinding<TSource, TTarget> : Binding<TSource, TTarget>
    {
        public MockBinding(object source, string sourcePropertyName, object target = null,
            string targetPropertyName = null, BindingMode mode = BindingMode.Default,
            TSource fallbackValue = default, TSource targetNullValue = default)
            : base(source, sourcePropertyName, target, targetPropertyName, mode, fallbackValue, targetNullValue)
        {
        }

        public MockBinding(object source, Expression<Func<TSource>> sourcePropertyExpression,
            object target = null, Expression<Func<TTarget>> targetPropertyExpression = null,
            BindingMode mode = BindingMode.Default, TSource fallbackValue = default,
            TSource targetNullValue = default)
            : base(source, sourcePropertyExpression, target, targetPropertyExpression, mode, fallbackValue, targetNullValue)
        {
        }

        public MockBinding(object source, Expression<Func<TSource>> sourcePropertyExpression,
            bool? resolveTopField, object target = null, Expression<Func<TTarget>> targetPropertyExpression = null,
            BindingMode mode = BindingMode.Default, TSource fallbackValue = default, TSource targetNullValue = default)
            : base(source, sourcePropertyExpression, resolveTopField, target, targetPropertyExpression, mode, fallbackValue, targetNullValue)
        {
        }

        protected override Binding<TSource, TTarget> CheckControlSource()
        {
            return this;
        }

        protected override Binding<TSource, TTarget> CheckControlTarget()
        {
            return this;
        }
    }
}
