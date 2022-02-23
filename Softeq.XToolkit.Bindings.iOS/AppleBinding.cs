// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using UIKit;

namespace Softeq.XToolkit.Bindings.iOS
{
    /// <inheritdoc />
    public class AppleBinding<TSource, TTarget> : Binding<TSource, TTarget>
    {
        /// <inheritdoc cref="Binding{TSource,TTarget}"/>
        public AppleBinding(
            object source,
            string sourcePropertyName,
            object? target = null,
            string? targetPropertyName = null,
            BindingMode mode = BindingMode.Default,
            [MaybeNull] TSource fallbackValue = default!,
            [MaybeNull] TSource targetNullValue = default!)
            : base(
                source,
                sourcePropertyName,
                target,
                targetPropertyName,
                mode,
                fallbackValue,
                targetNullValue)
        {
        }

        /// <inheritdoc cref="Binding{TSource,TTarget}"/>
        public AppleBinding(
            object source,
            Expression<Func<TSource>> sourcePropertyExpression,
            object? target = null,
            Expression<Func<TTarget>>? targetPropertyExpression = null,
            BindingMode mode = BindingMode.Default,
            [MaybeNull] TSource fallbackValue = default!,
            [MaybeNull] TSource targetNullValue = default!)
            : base(
                source,
                sourcePropertyExpression,
                target,
                targetPropertyExpression,
                mode,
                fallbackValue,
                targetNullValue)
        {
        }

        /// <inheritdoc cref="Binding{TSource,TTarget}"/>
        public AppleBinding(
            object source,
            Expression<Func<TSource>> sourcePropertyExpression,
            bool? resolveTopField,
            object? target = null,
            Expression<Func<TTarget>>? targetPropertyExpression = null,
            BindingMode mode = BindingMode.Default,
            [MaybeNull] TSource fallbackValue = default!,
            [MaybeNull] TSource targetNullValue = default!)
            : base(
                source,
                sourcePropertyExpression,
                resolveTopField,
                target,
                targetPropertyExpression,
                mode,
                fallbackValue,
                targetNullValue)
        {
        }

        /// <summary>
        ///     Define when the binding should be evaluated when the bound source object is a control.
        ///
        ///     Because Xamarin controls are not DependencyObjects,
        ///     the bound property will not automatically update the binding attached to it.
        ///
        ///     Instead, use this method to define which of the control's events should be observed.
        /// </summary>
        /// <param name="mode">
        ///     Defines the binding's update mode.
        ///
        ///     Use <see cref="UpdateTriggerMode.PropertyChanged" /> to update the binding when the source control's property changes.
        ///
        ///     The PropertyChanged mode should only be used with the following items:
        ///     <para>- <see cref="T:UIKit.UITextView"/> control and its <c>Text</c> property (<c>TextChanged</c> event).</para>
        ///     <para>- <see cref="T:UIKit.UITextField"/> control and its <c>Text</c> property (<c>EditingChanged</c> event).</para>
        ///     <para>- <see cref="T:UIKit.UISwitch"/> control and its <c>On</c> property (<c>ValueChanged</c> event).</para>
        /// </param>
        /// <returns>The Binding instance.</returns>
        /// <exception cref="T:System.InvalidOperationException">
        ///     When this method is called on a OneTime binding. Such bindings cannot be updated.
        ///     This exception can also be thrown when the source object is null or has already been
        ///     garbage collected before this method is called.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///     When <paramref name="mode"/> is <see cref="UpdateTriggerMode.LostFocus" />,
        ///     because it only supported in Android at this time.
        /// </exception>
        public Binding<TSource, TTarget> ObserveSourceEvent(UpdateTriggerMode mode = UpdateTriggerMode.PropertyChanged)
        {
            return mode switch
            {
                UpdateTriggerMode.LostFocus => throw new ArgumentException(
                    "UpdateTriggerMode.LostFocus is only supported in Android at this time", nameof(mode)),
                UpdateTriggerMode.PropertyChanged => CheckControlSource(),
                _ => this
            };
        }

        /// <summary>
        ///     Define when the binding should be evaluated when the bound target object is a control.
        ///
        ///     Because Xamarin controls are not DependencyObjects,
        ///     the bound property will not automatically update the binding attached to it.
        ///
        ///     Instead, use this method to define which of the control's events should be observed.
        /// </summary>
        /// <param name="mode">
        ///     Defines the binding's update mode.
        ///
        ///     Use <see cref="UpdateTriggerMode.PropertyChanged" /> to update the binding when the target control's property changes.
        ///
        ///     The PropertyChanged mode should only be used with the following items:
        ///     <para>- <see cref="T:UIKit.UITextView"/> control and its <c>Text</c> property (<c>TextChanged</c> event).</para>
        ///     <para>- <see cref="T:UIKit.UITextField"/> control and its <c>Text</c> property (<c>EditingChanged</c> event).</para>
        ///     <para>- <see cref="T:UIKit.UISwitch"/> control and its <c>On</c> property (<c>ValueChanged</c> event).</para>
        /// </param>
        /// <returns>The Binding instance.</returns>
        /// <exception cref="T:System.InvalidOperationException">
        ///     When this method is called on a OneTime or a OneWay binding.
        ///     This exception can also be thrown when the source object is null or
        ///     has already been garbage collected before this method is called.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///     When <paramref name="mode"/> is <see cref="UpdateTriggerMode.LostFocus" />,
        ///     because it only supported in Android at this time.
        /// </exception>
        public Binding<TSource, TTarget> ObserveTargetEvent(UpdateTriggerMode mode)
        {
            return mode switch
            {
                UpdateTriggerMode.LostFocus => throw new ArgumentException(
                    "UpdateTriggerMode.LostFocus is only supported in Android at this time", nameof(mode)),
                UpdateTriggerMode.PropertyChanged => CheckControlTarget(),
                _ => this
            };
        }

        /// <inheritdoc />
        [SuppressMessage("ReSharper", "RedundantAssignment", Justification = "Reviewed.")]
        [SuppressMessage("ReSharper", "EntityNameCapturedOnly.Local", Justification = "Reviewed.")]
        protected override Binding<TSource, TTarget> CheckControlSource()
        {
            switch (_propertySource!.Target)
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

        /// <inheritdoc />
        [SuppressMessage("ReSharper", "RedundantAssignment", Justification = "Reviewed.")]
        [SuppressMessage("ReSharper", "EntityNameCapturedOnly.Local", Justification = "Reviewed.")]
        protected override Binding<TSource, TTarget> CheckControlTarget()
        {
            if (Mode != BindingMode.TwoWay)
            {
                return this;
            }

            switch (_propertyTarget!.Target)
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
