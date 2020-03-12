// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq.Expressions;
using UIKit;

#nullable disable

namespace Softeq.XToolkit.Bindings.iOS
{
    // Partial class for Apple only.
    public class AppleBinding<TSource, TTarget> : Binding<TSource, TTarget>
    {
        public AppleBinding(object source, string sourcePropertyName, object target = null,
            string targetPropertyName = null, BindingMode mode = BindingMode.Default,
            TSource fallbackValue = default, TSource targetNullValue = default) : base(source,
            sourcePropertyName, target, targetPropertyName, mode, fallbackValue, targetNullValue)
        {
        }

        public AppleBinding(object source, Expression<Func<TSource>> sourcePropertyExpression, object target = null,
            Expression<Func<TTarget>> targetPropertyExpression = null, BindingMode mode = BindingMode.Default,
            TSource fallbackValue = default, TSource targetNullValue = default) : base(source,
            sourcePropertyExpression, target, targetPropertyExpression, mode, fallbackValue, targetNullValue)
        {
        }

        public AppleBinding(object source, Expression<Func<TSource>> sourcePropertyExpression, bool? resolveTopField,
            object target = null, Expression<Func<TTarget>> targetPropertyExpression = null,
            BindingMode mode = BindingMode.Default, TSource fallbackValue = default,
            TSource targetNullValue = default) : base(source, sourcePropertyExpression, resolveTopField,
            target, targetPropertyExpression, mode, fallbackValue, targetNullValue)
        {
        }

        public Binding<TSource, TTarget> ObserveSourceEvent()
        {
            return ObserveSourceEvent(UpdateTriggerMode.PropertyChanged);
        }

        public Binding<TSource, TTarget> ObserveSourceEvent(UpdateTriggerMode mode)
        {
            switch (mode)
            {
                case UpdateTriggerMode.LostFocus:
                    throw new ArgumentException(
                        "UpdateTriggerMode.LostFocus is only supported in Android at this time",
                        nameof(mode));

                case UpdateTriggerMode.PropertyChanged:
                    return CheckControlSource();
            }

            return this;
        }

        public Binding<TSource, TTarget> ObserveTargetEvent()
        {
            return ObserveTargetEvent(UpdateTriggerMode.PropertyChanged);
        }

        public Binding<TSource, TTarget> ObserveTargetEvent(UpdateTriggerMode mode)
        {
            switch (mode)
            {
                case UpdateTriggerMode.LostFocus:
                    throw new ArgumentException(
                        "UpdateTriggerMode.LostFocus is only supported in Android at this time",
                        nameof(mode));

                case UpdateTriggerMode.PropertyChanged:
                    return CheckControlTarget();
            }

            return this;
        }

        protected override Binding<TSource, TTarget> CheckControlSource()
        {
            if (PropertySource.Target is UITextView textBox)
            {
                var binding = ObserveSourceEvent(nameof(textBox.Changed));
                binding.SourceHandlers[nameof(textBox.Changed)].IsDefault = true;
                return binding;
            }

            if (PropertySource.Target is UITextField textField)
            {
                var binding = ObserveSourceEvent(nameof(textField.EditingChanged));
                binding.SourceHandlers[nameof(textField.EditingChanged)].IsDefault = true;
                return binding;
            }

            if (PropertySource.Target is UISwitch checkbox)
            {
                var binding = ObserveSourceEvent(nameof(checkbox.ValueChanged));
                binding.SourceHandlers[nameof(checkbox.ValueChanged)].IsDefault = true;
                return binding;
            }

            return this;
        }

        protected override Binding<TSource, TTarget> CheckControlTarget()
        {
            if (Mode != BindingMode.TwoWay)
            {
                return this;
            }

            if (PropertyTarget.Target is UITextView textBox)
            {
                var binding = ObserveTargetEvent(nameof(textBox.Changed));
                binding.TargetHandlers[nameof(textBox.Changed)].IsDefault = true;
                return binding;
            }

            if (PropertyTarget.Target is UITextField textField)
            {
                var binding = ObserveTargetEvent(nameof(textField.EditingChanged));
                binding.TargetHandlers[nameof(textField.EditingChanged)].IsDefault = true;
                return binding;
            }

            if (PropertyTarget.Target is UISwitch checkbox)
            {
                var binding = ObserveTargetEvent(nameof(checkbox.ValueChanged));
                binding.TargetHandlers[nameof(checkbox.ValueChanged)].IsDefault = true;
                return binding;
            }

            return this;
        }
    }
}
