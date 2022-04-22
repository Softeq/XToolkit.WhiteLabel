// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Android.Text;
using Android.Views;
using Android.Widget;

namespace Softeq.XToolkit.Bindings.Droid
{
    /// <inheritdoc />
    public class DroidBinding<TSource, TTarget> : Binding<TSource, TTarget>
    {
        /// <inheritdoc cref="Binding{TSource,TTarget}"/>
        public DroidBinding(
            object source,
            string sourcePropertyName,
            object? target = null,
            string? targetPropertyName = null,
            BindingMode mode = BindingMode.Default,
            [MaybeNull] TSource fallbackValue = default!,
            [MaybeNull] TSource targetNullValue = default!)
            : base(source, sourcePropertyName, target, targetPropertyName, mode, fallbackValue, targetNullValue)
        {
        }

        /// <inheritdoc cref="Binding{TSource,TTarget}"/>
        public DroidBinding(
            object source,
            Expression<Func<TSource>> sourcePropertyExpression,
            object? target = null,
            Expression<Func<TTarget>>? targetPropertyExpression = null,
            BindingMode mode = BindingMode.Default,
            [MaybeNull] TSource fallbackValue = default!,
            [MaybeNull] TSource targetNullValue = default!)
            : base(source, sourcePropertyExpression, target, targetPropertyExpression, mode, fallbackValue, targetNullValue)
        {
        }

        /// <inheritdoc cref="Binding{TSource,TTarget}"/>
        public DroidBinding(
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
        ///     Use <see cref="UpdateTriggerMode.LostFocus" /> to update the binding when the source control loses the focus.
        ///     You can also use <see cref="UpdateTriggerMode.PropertyChanged" /> to update the binding
        ///     when the source control's property changes.
        ///
        ///     The PropertyChanged mode should only be used with the following items:
        ///     <para>- <see cref="T:Android.Widget.EditText"/> control and its <c>Text</c> property (<c>TextChanged</c> event).</para>
        ///     <para>- <see cref="T:Android.Widget.CompoundButton"/> control and its <c>Checked</c> property (<c>CheckedChange</c> event).</para>
        /// </param>
        /// <returns>The Binding instance.</returns>
        /// <exception cref="T:System.InvalidOperationException">
        ///     When this method is called on a OneTime binding. Such bindings cannot be updated.
        ///     This exception can also be thrown when the source object is null or has already been
        ///     garbage collected before this method is called.
        /// </exception>
        public Binding<TSource, TTarget> ObserveSourceEvent(UpdateTriggerMode mode = UpdateTriggerMode.PropertyChanged)
        {
            return mode switch
            {
                UpdateTriggerMode.LostFocus => ObserveSourceEvent<View.FocusChangeEventArgs>("FocusChange"),
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
        ///     Use <see cref="UpdateTriggerMode.LostFocus" /> to update the binding when the target control loses the focus.
        ///     You can also use <see cref="UpdateTriggerMode.PropertyChanged" /> to update the binding
        ///     when the target control's property changes.
        ///
        ///     The PropertyChanged mode should only be used with the following items:
        ///     <para>- <see cref="T:Android.Widget.EditText"/> control and its <c>Text</c> property (<c>TextChanged</c> event).</para>
        ///     <para>- <see cref="T:Android.Widget.CompoundButton"/> control and its <c>Checked</c> property (<c>CheckedChange</c> event).</para>
        /// </param>
        /// <returns>The Binding instance.</returns>
        /// <exception cref="T:System.InvalidOperationException">
        ///     When this method is called on a OneTime or a OneWay binding.
        ///     This exception can also be thrown when the source object is null or
        ///     has already been garbage collected before this method is called.
        /// </exception>
        public Binding<TSource, TTarget> ObserveTargetEvent(UpdateTriggerMode mode = UpdateTriggerMode.PropertyChanged)
        {
            return mode switch
            {
                UpdateTriggerMode.LostFocus => ObserveTargetEvent<View.FocusChangeEventArgs>("FocusChange"),
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
                case EditText textBox:
                {
                    var binding = ObserveSourceEvent<TextChangedEventArgs>(nameof(textBox.TextChanged));
                    binding.SourceHandlers[nameof(textBox.TextChanged)].IsDefault = true;
                    return binding;
                }

                case CompoundButton checkbox:
                {
                    var binding = ObserveSourceEvent<CompoundButton.CheckedChangeEventArgs>(nameof(checkbox.CheckedChange));
                    binding.SourceHandlers[nameof(checkbox.CheckedChange)].IsDefault = true;
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
                case EditText textBox:
                {
                    var binding = ObserveTargetEvent<TextChangedEventArgs>(nameof(textBox.TextChanged));
                    binding.TargetHandlers[nameof(textBox.TextChanged)].IsDefault = true;
                    return binding;
                }

                case CompoundButton checkbox:
                {
                    var binding = ObserveTargetEvent<CompoundButton.CheckedChangeEventArgs>(nameof(checkbox.CheckedChange));
                    binding.TargetHandlers[nameof(checkbox.CheckedChange)].IsDefault = true;
                    return binding;
                }

                default:
                    return this;
            }
        }
    }
}
