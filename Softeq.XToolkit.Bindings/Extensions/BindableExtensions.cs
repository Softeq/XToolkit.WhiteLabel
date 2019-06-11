using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Common.Interfaces;

namespace Softeq.XToolkit.Bindings.Extensions
{
    public static class BindableExtensions
    {
        public static Binding Bind<T1, T2>(this IBindable obj,
            Expression<Func<T1>> sourcePropertyExpression,
            Expression<Func<T2>> targetPropertyExpression = null,
            BindingMode mode = BindingMode.OneWay,
            IConverter<T2, T1> converter = null)
        {
            var binding = obj.SetBinding(sourcePropertyExpression, targetPropertyExpression, mode)
                .SetConverter(converter);
            SetBindingTo(obj, binding);
            return binding;
        }

        public static Binding Bind<T1>(this IBindable obj, Expression<Func<T1>> sourcePropertyExpression,
            Action<T1> action)
        {
            var binding = obj.SetBinding(sourcePropertyExpression).WhenSourceChanges(action);
            SetBindingTo(obj, binding);
            return binding;
        }

        public static Binding Bind<T1>(this IBindable obj, Expression<Func<T1>> sourcePropertyExpression,
            Func<T1, Task> whenSourceChanges)
        {
            var binding = obj.SetBinding(sourcePropertyExpression).WhenSourceChanges(whenSourceChanges);
            SetBindingTo(obj, binding);
            return binding;
        }

        public static Binding Bind<T1>(this IBindable obj, Expression<Func<T1>> sourcePropertyExpression,
            Action<T1> action,
            BindingMode bindingMode)
        {
            var binding = obj.SetBinding(sourcePropertyExpression, bindingMode).WhenSourceChanges(action);
            SetBindingTo(obj, binding);
            return binding;
        }

        private static void SetBindingTo(IBindable bindable, Binding binding)
        {
            bindable.Bindings.Add(binding);
        }
    }
}
