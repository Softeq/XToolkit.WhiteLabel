// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq.Expressions;
using System.Windows.Input;
using NSubstitute;

namespace Softeq.XToolkit.Bindings.Tests
{
    internal class MockBindingFactory : BindingFactoryBase
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
            return Substitute.For<Binding<TSource, TTarget>>();
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
            return Substitute.For<Binding<TSource, TTarget>>();
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
            return Substitute.For<Binding<TSource, TTarget>>();
        }

        public override string GetDefaultEventNameForControl(Type type)
        {
            return null;
        }

        public override void HandleCommandCanExecute<T>(object element, ICommand command, Binding<T, T> commandParameterBinding)
        {
        }
    }
}
