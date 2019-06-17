// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Common.Interfaces;

namespace Softeq.XToolkit.Bindings.Extensions
{
    public static class BindableExtensions
    {
        public static Binding Bind<T1, T2>(this IBindableOwner obj,
            Expression<Func<T1>> sourcePropertyExpression,
            Expression<Func<T2>> targetPropertyExpression,
            BindingMode mode = BindingMode.Default)
        {
            var binding = obj.SetBinding(sourcePropertyExpression, targetPropertyExpression, mode);
            SetBindingTo(obj, binding);
            return binding;
        }

        public static Binding Bind<T1, T2>(this IBindableOwner obj,
            Expression<Func<T1>> sourcePropertyExpression,
            Expression<Func<T2>> targetPropertyExpression,
            IConverter<T2, T1> converter)
        {
            var binding = obj.SetBinding(sourcePropertyExpression, targetPropertyExpression)
                .SetConverter(converter);
            SetBindingTo(obj, binding);
            return binding;
        }

        public static Binding Bind<T1, T2>(this IBindableOwner obj,
            Expression<Func<T1>> sourcePropertyExpression,
            Expression<Func<T2>> targetPropertyExpression,
            BindingMode mode,
            IConverter<T2, T1> converter)
        {
            var binding = obj.SetBinding(sourcePropertyExpression, targetPropertyExpression, mode)
                .SetConverter(converter);
            SetBindingTo(obj, binding);
            return binding;
        }

        public static Binding Bind<T1>(this IBindableOwner obj, Expression<Func<T1>> sourcePropertyExpression,
            Action<T1> action)
        {
            var binding = obj.SetBinding(sourcePropertyExpression).WhenSourceChanges(action);
            SetBindingTo(obj, binding);
            return binding;
        }

        public static Binding Bind<T1>(this IBindableOwner obj, Expression<Func<T1>> sourcePropertyExpression,
            Action<T1> action,
            BindingMode bindingMode)
        {
            var binding = obj.SetBinding(sourcePropertyExpression, bindingMode).WhenSourceChanges(action);
            SetBindingTo(obj, binding);
            return binding;
        }

        private static void SetBindingTo(IBindableOwner bindableOwner, Binding binding)
        {
            bindableOwner.Bindings.Add(binding);
        }

        public static void SetDataContext(this IBindable self, object dataContext)
        {
            self.DataContext = dataContext;

            self.Bindings.DetachAllAndClear();

            self.SetBindings();
        }
    }
}
