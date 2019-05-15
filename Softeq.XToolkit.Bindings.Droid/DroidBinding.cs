// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq.Expressions;
using Android.Text;
using Android.Views;
using Android.Widget;

namespace Softeq.XToolkit.Bindings.Droid
{
    public class DroidBinding<TSource, TTarget> : Binding<TSource, TTarget>
    {
        public DroidBinding(object source, string sourcePropertyName, object target = null,
            string targetPropertyName = null, BindingMode mode = BindingMode.Default,
            TSource fallbackValue = default(TSource), TSource targetNullValue = default(TSource)) : base(source,
            sourcePropertyName, target, targetPropertyName, mode, fallbackValue, targetNullValue)
        {
        }

        public DroidBinding(object source, Expression<Func<TSource>> sourcePropertyExpression, object target = null,
            Expression<Func<TTarget>> targetPropertyExpression = null, BindingMode mode = BindingMode.Default,
            TSource fallbackValue = default(TSource), TSource targetNullValue = default(TSource)) : base(source,
            sourcePropertyExpression, target, targetPropertyExpression, mode, fallbackValue, targetNullValue)
        {
        }

        public DroidBinding(object source, Expression<Func<TSource>> sourcePropertyExpression, bool? resolveTopField,
            object target = null, Expression<Func<TTarget>> targetPropertyExpression = null,
            BindingMode mode = BindingMode.Default, TSource fallbackValue = default(TSource),
            TSource targetNullValue = default(TSource)) : base(source, sourcePropertyExpression, resolveTopField,
            target, targetPropertyExpression, mode, fallbackValue, targetNullValue)
        {
        }

        /// <summary>
        ///     Define that the binding should be evaluated when the bound control's source property changes.
        ///     Because Xamarin controls are not DependencyObjects, the
        ///     bound property will not automatically update the binding attached to it. Instead,
        ///     use this method to specify that the binding must be updated when the property changes.
        /// </summary>
        /// <remarks>
        ///     This method should only be used with the following items:
        ///     <para>- an EditText control and its Text property (TextChanged event).</para>
        ///     <para>- a CompoundButton control and its Checked property (CheckedChange event).</para>
        /// </remarks>
        /// <returns>The Binding instance.</returns>
        /// <exception cref="InvalidOperationException">
        ///     When this method is called
        ///     on a OneTime binding. Such bindings cannot be updated. This exception can
        ///     also be thrown when the source object is null or has already been
        ///     garbage collected before this method is called.
        /// </exception>
        public Binding<TSource, TTarget> ObserveSourceEvent()
        {
            return ObserveSourceEvent(UpdateTriggerMode.PropertyChanged);
        }

        /// <summary>
        ///     Define when the binding should be evaluated when the bound source object
        ///     is a control. Because Xamarin controls are not DependencyObjects, the
        ///     bound property will not automatically update the binding attached to it. Instead,
        ///     use this method to define which of the control's events should be observed.
        /// </summary>
        /// <param name="mode">
        ///     Defines the binding's update mode. Use
        ///     <see cref="UpdateTriggerMode.LostFocus" /> to update the binding when
        ///     the source control loses the focus. You can also use
        ///     <see cref="UpdateTriggerMode.PropertyChanged" /> to update the binding
        ///     when the source control's property changes.
        ///     The PropertyChanged mode should only be used with the following items:
        ///     <para>- an EditText control and its Text property (TextChanged event).</para>
        ///     <para>- a CompoundButton control and its Checked property (CheckedChange event).</para>
        /// </param>
        /// <returns>The Binding instance.</returns>
        /// <exception cref="InvalidOperationException">
        ///     When this method is called
        ///     on a OneTime binding. Such bindings cannot be updated. This exception can
        ///     also be thrown when the source object is null or has already been
        ///     garbage collected before this method is called.
        /// </exception>
        public Binding<TSource, TTarget> ObserveSourceEvent(UpdateTriggerMode mode)
        {
            switch (mode)
            {
                case UpdateTriggerMode.LostFocus:
                    return ObserveSourceEvent<View.FocusChangeEventArgs>("FocusChange");

                case UpdateTriggerMode.PropertyChanged:
                    return CheckControlSource();
            }

            return this;
        }

        /// <summary>
        ///     Define that the binding should be evaluated when the bound control's target property changes.
        ///     Because Xamarin controls are not DependencyObjects, the
        ///     bound property will not automatically update the binding attached to it. Instead,
        ///     use this method to specify that the binding must be updated when the property changes.
        /// </summary>
        /// <remarks>
        ///     This method should only be used with the following items:
        ///     <para>- an EditText control and its Text property (TextChanged event).</para>
        ///     <para>- a CompoundButton control and its Checked property (CheckedChange event).</para>
        /// </remarks>
        /// <returns>The Binding instance.</returns>
        /// <exception cref="InvalidOperationException">
        ///     When this method is called
        ///     on a OneTime or a OneWay binding. This exception can
        ///     also be thrown when the target object is null or has already been
        ///     garbage collected before this method is called.
        /// </exception>
        public Binding<TSource, TTarget> ObserveTargetEvent()
        {
            return ObserveTargetEvent(UpdateTriggerMode.PropertyChanged);
        }

        /// <summary>
        ///     Define when the binding should be evaluated when the bound target object
        ///     is a control. Because Xamarin controls are not DependencyObjects, the
        ///     bound property will not automatically update the binding attached to it. Instead,
        ///     use this method to define which of the control's events should be observed.
        /// </summary>
        /// <param name="mode">
        ///     Defines the binding's update mode. Use
        ///     <see cref="UpdateTriggerMode.LostFocus" /> to update the binding when
        ///     the source control loses the focus. You can also use
        ///     <see cref="UpdateTriggerMode.PropertyChanged" /> to update the binding
        ///     when the source control's property changes.
        ///     The PropertyChanged mode should only be used with the following items:
        ///     <para>- an EditText control and its Text property (TextChanged event).</para>
        ///     <para>- a CompoundButton control and its Checked property (CheckedChange event).</para>
        /// </param>
        /// <returns>The Binding instance.</returns>
        /// <exception cref="InvalidOperationException">
        ///     When this method is called
        ///     on a OneTime or a OneWay binding. This exception can
        ///     also be thrown when the source object is null or has already been
        ///     garbage collected before this method is called.
        /// </exception>
        public Binding<TSource, TTarget> ObserveTargetEvent(UpdateTriggerMode mode)
        {
            switch (mode)
            {
                case UpdateTriggerMode.LostFocus:
                    return ObserveTargetEvent<View.FocusChangeEventArgs>("FocusChange");

                case UpdateTriggerMode.PropertyChanged:
                    return CheckControlTarget();
            }

            return this;
        }

        protected override Binding<TSource, TTarget> CheckControlSource()
        {
            var textBox = PropertySource.Target as EditText;
            if (textBox != null)
            {
                var binding = ObserveSourceEvent<TextChangedEventArgs>("TextChanged");
                binding.SourceHandlers["TextChanged"].IsDefault = true;
                return binding;
            }

            var checkbox = PropertySource.Target as CompoundButton;
            if (checkbox != null)
            {
                var binding = ObserveSourceEvent<CompoundButton.CheckedChangeEventArgs>("CheckedChange");
                binding.SourceHandlers["CheckedChange"].IsDefault = true;
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

            var textBox = PropertyTarget.Target as EditText;
            if (textBox != null)
            {
                var binding = ObserveTargetEvent<TextChangedEventArgs>("TextChanged");
                binding.TargetHandlers["TextChanged"].IsDefault = true;
                return binding;
            }

            var checkbox = PropertyTarget.Target as CompoundButton;
            if (checkbox != null)
            {
                var binding = ObserveTargetEvent<CompoundButton.CheckedChangeEventArgs>("CheckedChange");
                binding.TargetHandlers["CheckedChange"].IsDefault = true;
                return binding;
            }

            return this;
        }
    }
}