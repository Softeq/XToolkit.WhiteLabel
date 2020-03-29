// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq.Expressions;
using UIKit;

#nullable disable

namespace Softeq.XToolkit.Bindings.iOS
{
    /// <inheritdoc />
    public class AppleBinding<TSource, TTarget> : Binding<TSource, TTarget>
    {
        /// <inheritdoc cref="Binding{TSource,TTarget}"/>
        public AppleBinding(object source, string sourcePropertyName, object target = null,
            string targetPropertyName = null, BindingMode mode = BindingMode.Default,
            TSource fallbackValue = default, TSource targetNullValue = default) : base(source,
            sourcePropertyName, target, targetPropertyName, mode, fallbackValue, targetNullValue)
        {
        }

        /// <inheritdoc cref="Binding{TSource,TTarget}"/>
        public AppleBinding(object source, Expression<Func<TSource>> sourcePropertyExpression, object target = null,
            Expression<Func<TTarget>> targetPropertyExpression = null, BindingMode mode = BindingMode.Default,
            TSource fallbackValue = default, TSource targetNullValue = default) : base(source,
            sourcePropertyExpression, target, targetPropertyExpression, mode, fallbackValue, targetNullValue)
        {
        }

        /// <inheritdoc cref="Binding{TSource,TTarget}"/>
        public AppleBinding(object source, Expression<Func<TSource>> sourcePropertyExpression, bool? resolveTopField,
            object target = null, Expression<Func<TTarget>> targetPropertyExpression = null,
            BindingMode mode = BindingMode.Default, TSource fallbackValue = default,
            TSource targetNullValue = default) : base(source, sourcePropertyExpression, resolveTopField,
            target, targetPropertyExpression, mode, fallbackValue, targetNullValue)
        {
        }

        public Binding<TSource, TTarget> ObserveSourceEvent(UpdateTriggerMode mode = UpdateTriggerMode.PropertyChanged)
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
            // ReSharper disable LocalNameCapturedOnly
            // ReSharper disable RedundantAssignment
            switch (PropertySource.Target)
            {
                case UITextView textBox:
                {
                    var binding = ObserveSourceEvent(nameof(textBox.Changed));
                    binding.SourceHandlers[nameof(textBox.Changed)].IsDefault = true;
                    return binding;
                }
                case UITextField textField:
                {
                    var binding = ObserveSourceEvent(nameof(textField.EditingChanged));
                    binding.SourceHandlers[nameof(textField.EditingChanged)].IsDefault = true;
                    return binding;
                }
                case UISwitch checkbox:
                {
                    var binding = ObserveSourceEvent(nameof(checkbox.ValueChanged));
                    binding.SourceHandlers[nameof(checkbox.ValueChanged)].IsDefault = true;
                    return binding;
                }
                default:
                    return this;
            }
        }

        protected override Binding<TSource, TTarget> CheckControlTarget()
        {
            if (Mode != BindingMode.TwoWay)
            {
                return this;
            }

            // ReSharper disable LocalNameCapturedOnly
            // ReSharper disable RedundantAssignment
            switch (PropertyTarget.Target)
            {
                case UITextView textBox:
                {
                    var binding = ObserveTargetEvent(nameof(textBox.Changed));
                    binding.TargetHandlers[nameof(textBox.Changed)].IsDefault = true;
                    return binding;
                }
                case UITextField textField:
                {
                    var binding = ObserveTargetEvent(nameof(textField.EditingChanged));
                    binding.TargetHandlers[nameof(textField.EditingChanged)].IsDefault = true;
                    return binding;
                }
                case UISwitch checkbox:
                {
                    var binding = ObserveTargetEvent(nameof(checkbox.ValueChanged));
                    binding.TargetHandlers[nameof(checkbox.ValueChanged)].IsDefault = true;
                    return binding;
                }
                default:
                    return this;
            }
        }
    }
}
