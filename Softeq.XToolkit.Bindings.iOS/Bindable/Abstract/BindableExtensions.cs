// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Softeq.XToolkit.Bindings.iOS.Gestures;
using Softeq.XToolkit.Common.Command;
using Softeq.XToolkit.Common.Interfaces;
using UIKit;

namespace Softeq.XToolkit.Bindings.iOS.Bindable.Abstract
{
    public static class BindableExtensions
    {
        public static Binding Bind<T1, T2>(this IBindable obj, Expression<Func<T1>> sourcePropertyExpression,
            Expression<Func<T2>> targetPropertyExpression, IConverter<T2, T1> converter)
        {
            var binding = Bind(obj, sourcePropertyExpression, targetPropertyExpression, BindingMode.OneWay, converter);
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

        public static Binding Bind(this GestureRecognizerBehavior recognizer, Action command)
        {
            recognizer.Command = new RelayCommand(command);
            return new GestureRecognizerBinding(recognizer);
        }

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

        public static void DelayBind(this IBindable bindable, Action action)
        {
            foreach (var binding in bindable.Bindings)
            {
                binding.Detach();
            }

            bindable.Bindings.Clear();
            bindable.Activator = action;

            if (bindable.BindingContext != null)
            {
                action.Invoke();
            }
        }

        public static void SetDataContext(this UIView view, object context)
        {
            var bindable = (IBindable) view;
            bindable.BindingContext = context;

            if (bindable.Activator != null)
            {
                DelayBind(bindable, bindable.Activator);
            }
        }

        private static void SetBindingTo(IBindable bindable, Binding binding)
        {
            bindable.Bindings.Add(binding);
        }
    }
}
