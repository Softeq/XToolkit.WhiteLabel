// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq.Expressions;
using UIKit;

namespace Softeq.XToolkit.Bindings.iOS
{
    // Partial class for Apple only.
    public class AppleBinding<TSource, TTarget> : Binding<TSource, TTarget>
    {
        public AppleBinding(object source, string sourcePropertyName, object target = null,
            string targetPropertyName = null, BindingMode mode = BindingMode.Default,
            TSource fallbackValue = default(TSource), TSource targetNullValue = default(TSource)) : base(source,
            sourcePropertyName, target, targetPropertyName, mode, fallbackValue, targetNullValue)
        {
        }

        public AppleBinding(object source, Expression<Func<TSource>> sourcePropertyExpression, object target = null,
            Expression<Func<TTarget>> targetPropertyExpression = null, BindingMode mode = BindingMode.Default,
            TSource fallbackValue = default(TSource), TSource targetNullValue = default(TSource)) : base(source,
            sourcePropertyExpression, target, targetPropertyExpression, mode, fallbackValue, targetNullValue)
        {
        }

        public AppleBinding(object source, Expression<Func<TSource>> sourcePropertyExpression, bool? resolveTopField,
            object target = null, Expression<Func<TTarget>> targetPropertyExpression = null,
            BindingMode mode = BindingMode.Default, TSource fallbackValue = default(TSource),
            TSource targetNullValue = default(TSource)) : base(source, sourcePropertyExpression, resolveTopField,
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
            var textBox = PropertySource.Target as UITextView;
            if (textBox != null)
            {
                var binding = ObserveSourceEvent("Changed");
                binding.SourceHandlers["Changed"].IsDefault = true;
                return binding;
            }

            var textField = PropertySource.Target as UITextField;
            if (textField != null)
            {
                var binding = ObserveSourceEvent("EditingChanged");
                binding.SourceHandlers["EditingChanged"].IsDefault = true;
                return binding;
            }

            var checkbox = PropertySource.Target as UISwitch;
            if (checkbox != null)
            {
                var binding = ObserveSourceEvent("ValueChanged");
                binding.SourceHandlers["ValueChanged"].IsDefault = true;
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

            var textBox = PropertyTarget.Target as UITextView;
            if (textBox != null)
            {
                var binding = ObserveTargetEvent("Changed");
                binding.TargetHandlers["Changed"].IsDefault = true;
                return binding;
            }

            var textField = PropertyTarget.Target as UITextField;
            if (textField != null)
            {
                var binding = ObserveTargetEvent("EditingChanged");
                binding.TargetHandlers["EditingChanged"].IsDefault = true;
                return binding;
            }

            var checkbox = PropertyTarget.Target as UISwitch;
            if (checkbox != null)
            {
                var binding = ObserveTargetEvent("ValueChanged");
                binding.TargetHandlers["ValueChanged"].IsDefault = true;
                return binding;
            }

            return this;
        }
    }
}