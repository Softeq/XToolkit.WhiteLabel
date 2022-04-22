// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Converters;
using Softeq.XToolkit.Common.Weak;

namespace Softeq.XToolkit.Bindings
{
    /// <summary>
    ///     Creates a binding between two properties.
    ///
    ///     If the source implements <see cref="T:System.ComponentModel.INotifyPropertyChanged"/>,
    ///     the source property raises the PropertyChanged event and the <see cref="BindingMode"/> is OneWay or TwoWay,
    ///     the target property will be synchronized with the source property.
    ///
    ///     If the target implements <see cref="T:System.ComponentModel.INotifyPropertyChanged"/>,
    ///     the target property raises the PropertyChanged event and the <see cref="BindingMode"/> is TwoWay,
    ///     the source property will also be synchronized with the target property.
    /// </summary>
    /// <typeparam name="TSource">The type of the source property that is being data-bound.</typeparam>
    /// <typeparam name="TTarget">
    ///     The type of the target property that is being data-bound. If the source type
    ///     is not the same as the target type, an automatic conversion will be attempted. However only
    ///     simple types can be converted. For more complex conversions, use the <see cref="ConvertSourceToTarget" />
    ///     and <see cref="ConvertTargetToSource" /> methods to define custom converters.
    /// </typeparam>
#pragma warning disable SA1649
    public abstract class Binding<TSource, TTarget> : Binding
#pragma warning restore SA1649
    {
        private readonly WeakConverter _converter = new WeakConverter();
        private readonly List<IWeakEventListener> _listeners = new List<IWeakEventListener>();

        private readonly string? _sourcePropertyName;
        private readonly string? _targetPropertyName;

        private readonly Expression<Func<TSource>>? _sourcePropertyExpression;
        private readonly Expression<Func<TTarget>>? _targetPropertyExpression;

        private readonly Func<TSource>? _sourcePropertyFunc;

        /// <summary>
        ///     The source property handlers.
        /// </summary>
        public readonly Dictionary<string, DelegateInfo> SourceHandlers = new Dictionary<string, DelegateInfo>();

        /// <summary>
        ///     The target property handlers.
        /// </summary>
        public readonly Dictionary<string, DelegateInfo> TargetHandlers = new Dictionary<string, DelegateInfo>();

        private bool _isFallbackValueActive;
        private bool _resolveTopField;

        private bool _settingSourceToTarget;
        private bool _settingTargetToSource;

        private PropertyInfo? _sourceProperty;
        private PropertyInfo? _targetProperty;

        private WeakAction? _onSourceUpdate;
        private WeakAction<TSource>? _onSourceUpdateWithParameter;
        private WeakFunc<TSource, Task>? _onSourceUpdateFunctionWithParameter;

        private IConverter<TTarget, TSource>? _valueConverter;

        /// <summary>
        ///     A weak reference to the source property.
        /// </summary>
        protected WeakReference? _propertySource;

        /// <summary>
        ///     A weak reference to the target property.
        /// </summary>
        protected WeakReference? _propertyTarget;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Binding{TSource, TTarget}"/> class for
        ///     which the source and target properties are located in different objects.
        /// </summary>
        /// <param name="source">
        ///     The source of the binding. If this object implements INotifyPropertyChanged and the
        ///     BindingMode is OneWay or TwoWay, the target will be notified of changes to the target property.
        /// </param>
        /// <param name="sourcePropertyName">The name of the source property for the binding.</param>
        /// <param name="target">
        ///     The target of the binding. If this object implements INotifyPropertyChanged and the
        ///     BindingMode is TwoWay, the source will be notified of changes to the source property.
        /// </param>
        /// <param name="targetPropertyName">The name of the target property for the binding.</param>
        /// <param name="mode">
        ///     The mode of the binding. OneTime means that the target property will be set once (when the binding is
        ///     created) but that subsequent changes will be ignored. OneWay means that the target property will be set,
        ///     and if the PropertyChanged event is raised by the source, the target property will be updated.
        ///     TwoWay means that the source property will also be updated if the target raises the PropertyChanged event.
        ///     Default means OneWay if only the source implements INPC,
        ///     and TwoWay if both the source and the target implement INPC.
        /// </param>
        /// <param name="fallbackValue">
        ///     The value to use when the binding is unable to return a value. This can happen if one of the
        ///     items on the Path (except the source property itself) is null, or if the Converter throws an exception.
        /// </param>
        /// <param name="targetNullValue">
        ///     The target value to use when the binding is unable to return a value. This can happen if one of the
        ///     items on the Path (except the source property itself) is null, or if the Converter throws an exception.
        /// </param>
        protected Binding(
            object source,
            string sourcePropertyName,
            object? target = null,
            string? targetPropertyName = null,
            BindingMode mode = BindingMode.Default,
            [MaybeNull] TSource fallbackValue = default!,
            [MaybeNull] TSource targetNullValue = default!)
        {
            Mode = mode;
            FallbackValue = fallbackValue;
            TargetNullValue = targetNullValue;

            TopSource = new WeakReference(source);
            _propertySource = new WeakReference(source);
            _sourcePropertyName = sourcePropertyName;

            if (target == null)
            {
                TopTarget = TopSource;
                _propertyTarget = _propertySource;
            }
            else
            {
                TopTarget = new WeakReference(target);
                _propertyTarget = new WeakReference(target);
            }

            _targetPropertyName = targetPropertyName;
            Attach();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Binding{TSource, TTarget}"/> class for
        ///     which the source and target properties are located in different objects.
        /// </summary>
        /// <param name="source">
        ///     The source of the binding. If this object implements <see cref="T:System.ComponentModel.INotifyPropertyChanged"/> and the
        ///     <see cref="BindingMode"/> is OneWay or TwoWay, the target will be notified of changes to the target property.
        /// </param>
        /// <param name="sourcePropertyExpression">
        ///     An expression pointing to the source property. It can be
        ///     a simple expression "() => [source].MyProperty" or a composed expression "() =>
        ///     [source].SomeObject.SomeOtherObject.SomeProperty".
        /// </param>
        /// <param name="target">
        ///     The target of the binding. If this object implements INotifyPropertyChanged and the
        ///     BindingMode is TwoWay, the source will be notified of changes to the source property.
        /// </param>
        /// <param name="targetPropertyExpression">
        ///     An expression pointing to the target property. It can be
        ///     a simple expression "() => [target].MyProperty" or a composed expression "() =>
        ///     [target].SomeObject.SomeOtherObject.SomeProperty".
        /// </param>
        /// <param name="mode">
        ///     The mode of the binding.
        ///
        ///     OneTime means that the target property will be set once (when the binding is created) but that subsequent changes
        ///     will be ignored. OneWay means that the target property will be set, and if the PropertyChanged event is raised
        ///     by the source, the target property will be updated.
        ///
        ///     TwoWay means that the source property will also be updated if the target raises the PropertyChanged event.
        ///     Default means OneWay if only the source implements <see cref="T:System.ComponentModel.INotifyPropertyChanged"/>,
        ///     and TwoWay if both the source and the target implement <see cref="T:System.ComponentModel.INotifyPropertyChanged"/>.
        /// </param>
        /// <param name="fallbackValue">
        ///     The value to use when the binding is unable to return a value. This can happen if one of the
        ///     items on the Path (except the source property itself) is null, or if the Converter throws an exception.
        /// </param>
        /// <param name="targetNullValue">The value to use when the binding is unable to return a value.</param>
        protected Binding(
            object source,
            Expression<Func<TSource>> sourcePropertyExpression,
            object? target = null,
            Expression<Func<TTarget>>? targetPropertyExpression = null,
            BindingMode mode = BindingMode.Default,
            [MaybeNull] TSource fallbackValue = default!,
            [MaybeNull] TSource targetNullValue = default!)
            : this(
                source,
                sourcePropertyExpression,
                null,
                target,
                targetPropertyExpression,
                mode,
                fallbackValue,
                targetNullValue)
        {
        }

        /// <inheritdoc cref="Binding{TSource,TTarget}"/>
        protected Binding(
            object source,
            Expression<Func<TSource>> sourcePropertyExpression,
            bool? resolveTopField,
            object? target = null,
            Expression<Func<TTarget>>? targetPropertyExpression = null,
            BindingMode mode = BindingMode.Default,
            [MaybeNull] TSource fallbackValue = default!,
            [MaybeNull] TSource targetNullValue = default!)
        {
            Mode = mode;
            FallbackValue = fallbackValue;
            TargetNullValue = targetNullValue;

            TopSource = new WeakReference(source);
            _sourcePropertyExpression = sourcePropertyExpression;
            _sourcePropertyFunc = _sourcePropertyExpression.Compile();
            _sourcePropertyName = GetPropertyName(sourcePropertyExpression);

            TopTarget = target == null ? TopSource : new WeakReference(target);
            _targetPropertyExpression = targetPropertyExpression;
            _targetPropertyName = GetPropertyName(targetPropertyExpression);

            Attach(
                TopSource.Target,
                TopTarget.Target,
                mode,
                resolveTopField ?? target == null && targetPropertyExpression != null);
        }

        /// <summary>
        ///     Occurs when the value of the data-bound property changes.
        /// </summary>
        public override event EventHandler? ValueChanged;

        /// <summary>
        ///     Gets the value to use when the binding is unable to return a value. This can happen if one of the
        ///     items on the Path (except the source property itself) is null, or if the Converter throws an exception.
        /// </summary>
        public TSource FallbackValue { get; }

        /// <summary>
        ///     Gets of sets the value used when the source property is null (or equals to default(TSource)).
        /// </summary>
        public TSource TargetNullValue { get; }

        /// <summary>
        ///     Gets the current value of the binding.
        /// </summary>
        [MaybeNull]
        public TTarget Value
        {
            get
            {
                if (!(_propertySource is { IsAlive: true }))
                {
                    return default;
                }

                var type = _propertySource.Target.GetType();
                var property = type.GetRuntimeProperty(_sourcePropertyName);
                return (TTarget) property.GetValue(_propertySource.Target, null);
            }
        }

        /// <summary>
        ///     Defines a custom conversion method for a binding.
        ///
        ///     To be used when the binding's source property is of a different type than the binding's
        ///     target property, and the conversion cannot be done automatically (simple values).
        /// </summary>
        /// <param name="convert">
        ///     A func that will be called with the source property's value, and will return the target property's value.
        ///     IMPORTANT: Note that closures are not supported at the moment
        ///     due to the use of WeakActions (see http://stackoverflow.com/questions/25730530/).
        /// </param>
        /// <returns>The Binding instance.</returns>
        public Binding<TSource, TTarget> ConvertSourceToTarget(Func<TSource, TTarget> convert)
        {
            _converter.SetConvert(convert);
            ForceUpdateValueFromSourceToTarget();
            return this;
        }

        /// <summary>
        ///     Defines a custom conversion method for a two-way binding.
        ///
        ///     To be used when the binding's target property is of a different type than the binding's
        ///     source property, and the conversion cannot be done automatically (simple values).
        /// </summary>
        /// <param name="convertBack">
        ///     A func that will be called with the source property's value, and will return the target property's value.
        ///     IMPORTANT: Note that closures are not supported at the moment
        ///     due to the use of WeakActions (see http://stackoverflow.com/questions/25730530/).
        /// </param>
        /// <returns>The Binding instance.</returns>
        /// <remarks>This method is inactive on OneTime or OneWay bindings.</remarks>
        public Binding<TSource, TTarget> ConvertTargetToSource(Func<TTarget, TSource> convertBack)
        {
            _converter.SetConvertBack(convertBack);
            return this;
        }

        /// <summary>
        ///     Defines a custom conversion methods for a binding.
        ///
        ///     To be used when the binding's source/target property is of a different type than the binding's
        ///     target/source property, and the conversion cannot be done automatically (simple values).
        /// </summary>
        /// <param name="converter">
        ///     The <see cref="IConverter{TTarget, TSource}"/> instance of the converter, where:
        ///     <para>
        ///     - <see cref="IConverter{TTarget, TSource}.ConvertValue"/> will have behavior like <see cref="ConvertSourceToTarget"/> method.
        ///     </para>
        ///     <para>
        ///     - <see cref="IConverter{TTarget, TSource}.ConvertValueBack"/> will have behavior like <see cref="ConvertTargetToSource"/> method.
        ///     </para>
        /// </param>
        /// <returns>The Binding instance.</returns>
        public Binding SetConverter(IConverter<TTarget, TSource>? converter)
        {
            if (converter == null)
            {
                return this;
            }

            _valueConverter = converter;

            ConvertSourceToTarget(x => _valueConverter.ConvertValue(x));
            ConvertTargetToSource(x => _valueConverter.ConvertValueBack(x));

            return this;
        }

        /// <summary>
        ///     Instructs the <see cref="Binding"/> instance to stop listening to value changes and to remove all listeners.
        /// </summary>
        public override void Detach()
        {
            foreach (var listener in _listeners)
            {
                PropertyChangedEventManager.RemoveListener(listener);
            }

            _listeners.Clear();

            DetachAllSourceHandlers();
            DetachAllTargetHandlers();
        }

        /// <summary>
        ///     Forces the Binding's value to be reevaluated. The target value will be set to the source value.
        /// </summary>
        public override void ForceUpdateValueFromSourceToTarget()
        {
            if (_onSourceUpdate == null
                && _onSourceUpdateWithParameter == null
                && (_propertySource == null
                    || !_propertySource.IsAlive
                    || _propertySource.Target == null))
            {
                return;
            }

            if (_targetProperty != null)
            {
                try
                {
                    var value = GetSourceValue();
                    var targetValue = _targetProperty.GetValue(_propertyTarget!.Target);

                    if (!Equals(value, targetValue))
                    {
                        _settingSourceToTarget = true;
                        SetTargetValue(value!);
                        _settingSourceToTarget = false;
                    }
                }
                catch
                {
                    if (!Equals(FallbackValue, default(TSource)))
                    {
                        _settingSourceToTarget = true;
                        _targetProperty.SetValue(_propertyTarget!.Target, FallbackValue, null);
                        _settingSourceToTarget = false;
                    }
                }
            }

            if (_onSourceUpdate is { IsAlive: true })
            {
                _onSourceUpdate.Execute();
            }

            if (_onSourceUpdateWithParameter is { IsAlive: true })
            {
                _onSourceUpdateWithParameter.Execute(_sourcePropertyFunc!.Invoke());
            }

            RaiseValueChanged();
        }

        /// <summary>
        ///     Forces the Binding's value to be reevaluated. The source value will be set to the target value.
        /// </summary>
        public override void ForceUpdateValueFromTargetToSource()
        {
            if (_propertyTarget == null
                || !_propertyTarget.IsAlive
                || _propertyTarget.Target == null
                || _propertySource == null
                || !_propertySource.IsAlive
                || _propertySource.Target == null)
            {
                return;
            }

            if (_targetProperty != null)
            {
                var value = GetTargetValue();
                var sourceValue = _sourceProperty!.GetValue(_propertySource.Target);

                if (!Equals(value, sourceValue))
                {
                    _settingTargetToSource = true;
                    SetSourceValue(value!);
                    _settingTargetToSource = false;
                }
            }

            RaiseValueChanged();
        }

        /// <summary>
        ///     Define when the binding should be evaluated when the bound source object is a control.
        ///
        ///     Because Xamarin controls are not DependencyObjects,
        ///     the bound property will not automatically update the binding attached to it.
        ///
        ///     Instead, use this method to define which of the control's events should be observed.
        /// </summary>
        /// <param name="eventName">
        ///     The name of the event that should be observed to update the binding's value.
        /// </param>
        /// <returns>The Binding instance.</returns>
        /// <exception cref="T:System.InvalidOperationException">
        ///     When this method is called on a OneTime binding. Such bindings cannot be updated.
        ///     This exception can also be thrown when the source object is null or has already been
        ///     garbage collected before this method is called.
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///     When the eventName parameter is null or is an empty string.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///     When the requested event does not exist on the source control.
        /// </exception>
        public Binding<TSource, TTarget> ObserveSourceEvent(string eventName)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                return this;
            }

            if (Mode == BindingMode.OneTime)
            {
                throw new InvalidOperationException("This method cannot be used with OneTime bindings");
            }

            if (_onSourceUpdate == null
                && _onSourceUpdateWithParameter == null
                && (_propertySource == null
                    || !_propertySource.IsAlive
                    || _propertySource.Target == null))
            {
                throw new InvalidOperationException("Source is not ready");
            }

            if (string.IsNullOrEmpty(eventName))
            {
                throw new ArgumentNullException(nameof(eventName));
            }

            var type = _propertySource!.Target.GetType();
            var @event = type.GetRuntimeEvent(eventName);
            if (@event == null)
            {
                throw new ArgumentException($"Event not found: {eventName}", nameof(eventName));
            }

            EventHandler handler = HandleSourceEvent;

            var defaultHandlerInfo = SourceHandlers.Values.FirstOrDefault(x => x.IsDefault);
            if (defaultHandlerInfo != null)
            {
                DetachAllSourceHandlers();
            }

            var info = new DelegateInfo
            {
                Delegate = handler
            };

            if (SourceHandlers.ContainsKey(eventName))
            {
                SourceHandlers[eventName] = info;
            }
            else
            {
                SourceHandlers.Add(eventName, info);
            }

            @event.AddEventHandler(_propertySource.Target, handler);

            return this;
        }

        /// <summary>
        ///     Define when the binding should be evaluated when the bound source object is a control.
        ///
        ///     Because Xamarin controls are not DependencyObjects,
        ///     the bound property will not automatically update the binding attached to it.
        ///
        ///     Instead, use this method to define which of the control's events should be observed.
        /// </summary>
        /// <remarks>
        ///     Use this method when the event requires a specific EventArgs type instead of the standard EventHandler.
        /// </remarks>
        /// <typeparam name="TEventArgs">The type of the EventArgs used by this control's event.</typeparam>
        /// <param name="eventName">The name of the event that should be observed to update the binding's value.</param>
        /// <returns>The Binding instance.</returns>
        /// <exception cref="T:System.InvalidOperationException">
        ///     When this method is called on a OneTime binding. Such bindings cannot be updated. This exception can
        ///     also be thrown when the source object is null or has already been garbage collected before this method is called.
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///     When the eventName parameter is null or is an empty string.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///     When the requested event does not exist on the source control.
        /// </exception>
        public Binding<TSource, TTarget> ObserveSourceEvent<TEventArgs>(string eventName)
            where TEventArgs : EventArgs
        {
            if (string.IsNullOrEmpty(eventName)
                || SourceHandlers.ContainsKey(eventName))
            {
                return this;
            }

            if (Mode == BindingMode.OneTime)
            {
                throw new InvalidOperationException("This method cannot be used with OneTime bindings");
            }

            if (_onSourceUpdate == null
                && _onSourceUpdateWithParameter == null
                && (_propertySource == null
                    || !_propertySource.IsAlive
                    || _propertySource.Target == null))
            {
                throw new InvalidOperationException("Source is not ready");
            }

            if (string.IsNullOrEmpty(eventName))
            {
                throw new ArgumentNullException(nameof(eventName));
            }

            var type = _propertySource!.Target.GetType();
            var @event = type.GetRuntimeEvent(eventName);
            if (@event == null)
            {
                throw new ArgumentException($"Event not found: {eventName}", nameof(eventName));
            }

            EventHandler<TEventArgs> handler = HandleSourceEvent;

            var defaultHandlerInfo = SourceHandlers.Values.FirstOrDefault(x => x.IsDefault);
            if (defaultHandlerInfo != null)
            {
                DetachAllSourceHandlers();
            }

            var info = new DelegateInfo
            {
                Delegate = handler
            };

            if (SourceHandlers.ContainsKey(eventName))
            {
                SourceHandlers[eventName] = info;
            }
            else
            {
                SourceHandlers.Add(eventName, info);
            }

            @event.AddEventHandler(_propertySource.Target, handler);

            return this;
        }

        /// <summary>
        ///     Define when the binding should be evaluated when the bound target object is a control.
        ///
        ///     Because Xamarin controls are not DependencyObjects,
        ///     the bound property will not automatically update the binding attached to it.
        ///
        ///     Instead, use this method to define which of the control's events should be observed.
        /// </summary>
        /// <param name="eventName">The name of the event that should be observed to update the binding's value.</param>
        /// <returns>The Binding instance.</returns>
        /// <exception cref="T:System.InvalidOperationException">
        ///     When this method is called on a OneTime or a OneWay binding. This exception can
        ///     also be thrown when the source object is null or has already been garbage collected before this method is called.
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///     When the eventName parameter is null or is an empty string.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///     When the requested event does not exist on the target control.
        /// </exception>
        public Binding<TSource, TTarget> ObserveTargetEvent(string eventName)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                return this;
            }

            if (Mode == BindingMode.OneTime
                || Mode == BindingMode.OneWay)
            {
                throw new InvalidOperationException("This method cannot be used with OneTime or OneWay bindings");
            }

            if (_onSourceUpdate != null)
            {
                throw new InvalidOperationException("Cannot use SetTargetEvent with onSourceUpdate");
            }

            if (_onSourceUpdateWithParameter != null)
            {
                throw new InvalidOperationException("Cannot use SetTargetEvent with onSourceUpdate");
            }

            if (_propertyTarget == null
                || !_propertyTarget.IsAlive
                || _propertyTarget.Target == null)
            {
                throw new InvalidOperationException("Target is not ready");
            }

            if (string.IsNullOrEmpty(eventName))
            {
                throw new ArgumentNullException(nameof(eventName));
            }

            var type = _propertyTarget.Target.GetType();

            var @event = type.GetRuntimeEvent(eventName);
            if (@event == null)
            {
                throw new ArgumentException($"Event not found: {eventName}", nameof(eventName));
            }

            EventHandler handler = HandleTargetEvent;

            var defaultHandlerInfo = TargetHandlers.Values.FirstOrDefault(x => x.IsDefault);
            if (defaultHandlerInfo != null)
            {
                DetachAllTargetHandlers();
            }

            var info = new DelegateInfo
            {
                Delegate = handler
            };

            if (TargetHandlers.ContainsKey(eventName))
            {
                TargetHandlers[eventName] = info;
            }
            else
            {
                TargetHandlers.Add(eventName, info);
            }

            @event.AddEventHandler(_propertyTarget.Target, handler);

            return this;
        }

        /// <summary>
        ///     Define when the binding should be evaluated when the bound target object is a control.
        ///
        ///     Because Xamarin controls are not DependencyObjects,
        ///     the bound property will not automatically update the binding attached to it.
        ///
        ///     Instead, use this method to define which of the control's events should be observed.
        /// </summary>
        /// <remarks>
        ///     Use this method when the event requires a specific EventArgs type instead of the standard EventHandler.
        /// </remarks>
        /// <typeparam name="TEventArgs">The type of the EventArgs used by this control's event.</typeparam>
        /// <param name="eventName">The name of the event that should be observed to update the binding's value.</param>
        /// <returns>The Binding instance.</returns>
        /// <exception cref="T:System.InvalidOperationException">
        ///     When this method is called on a OneTime or OneWay binding. This exception can
        ///     also be thrown when the target object is null or has already been garbage collected before this method is called.
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///     When the eventName parameter is null or is an empty string.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///     When the requested event does not exist on the target control.
        /// </exception>
        public Binding<TSource, TTarget> ObserveTargetEvent<TEventArgs>(string eventName)
            where TEventArgs : EventArgs
        {
            if (string.IsNullOrEmpty(eventName))
            {
                return this;
            }

            if (Mode == BindingMode.OneTime
                || Mode == BindingMode.OneWay)
            {
                throw new InvalidOperationException("This method cannot be used with OneTime or OneWay bindings");
            }

            if (_onSourceUpdate != null)
            {
                throw new InvalidOperationException("Cannot use SetTargetEvent with onSourceUpdate");
            }

            if (_onSourceUpdateWithParameter != null)
            {
                throw new InvalidOperationException("Cannot use SetTargetEvent with onSourceUpdate");
            }

            if (_propertyTarget == null
                || !_propertyTarget.IsAlive
                || _propertyTarget.Target == null)
            {
                throw new InvalidOperationException("Target is not ready");
            }

            if (string.IsNullOrEmpty(eventName))
            {
                throw new ArgumentNullException(nameof(eventName));
            }

            var type = _propertyTarget.Target.GetType();

            var @event = type.GetRuntimeEvent(eventName);
            if (@event == null)
            {
                throw new ArgumentException($"Event not found: {eventName}", nameof(eventName));
            }

            EventHandler<TEventArgs> handler = HandleTargetEvent;

            var defaultHandlerInfo = TargetHandlers.Values.FirstOrDefault(x => x.IsDefault);
            if (defaultHandlerInfo != null)
            {
                DetachAllTargetHandlers();
            }

            var info = new DelegateInfo
            {
                Delegate = handler
            };

            if (TargetHandlers.ContainsKey(eventName))
            {
                TargetHandlers[eventName] = info;
            }
            else
            {
                TargetHandlers.Add(eventName, info);
            }

            @event.AddEventHandler(_propertyTarget.Target, handler);

            return this;
        }

        /// <summary>
        ///     Defines an action that will be executed every time that the binding value changes.
        /// </summary>
        /// <param name="callback">
        ///     The action that will be executed when the binding changes.
        ///     IMPORTANT: Note that closures are not supported at the moment
        ///     due to the use of WeakActions (see http://stackoverflow.com/questions/25730530/).
        /// </param>
        /// <returns>The Binding instance.</returns>
        /// <exception cref="T:System.InvalidOperationException">
        ///     When the method is called on a binding that already has a target property set.
        /// </exception>
        public Binding<TSource, TTarget> WhenSourceChanges(Action callback)
        {
            if (_targetPropertyExpression != null)
            {
                throw new InvalidOperationException(
                    "You cannot set both the targetPropertyExpression and call WhenSourceChanges");
            }

            _onSourceUpdate = new WeakAction(callback);

            if (_onSourceUpdate.IsAlive)
            {
                _onSourceUpdate.Execute();
            }

            return this;
        }

        /// <summary>
        ///     Defines an generic action that will be executed every time that the binding value changes.
        /// </summary>
        /// <param name="callback">
        ///     The generic action that will be executed when the binding changes.
        ///     IMPORTANT: Note that closures are not supported at the moment
        ///     due to the use of WeakActions (see http://stackoverflow.com/questions/25730530/).
        /// </param>
        /// <returns>The Binding instance.</returns>
        /// <exception cref="InvalidOperationException">
        ///     When the method is called on a binding that already has a target property set.
        /// </exception>
        public Binding<TSource, TTarget> WhenSourceChanges(Action<TSource> callback)
        {
            if (_targetPropertyExpression != null)
            {
                throw new InvalidOperationException(
                    "You cannot set both the targetPropertyExpression and call WhenSourceChanges");
            }

            _onSourceUpdateWithParameter = new WeakAction<TSource>(callback);

            if (_onSourceUpdateWithParameter.IsAlive)
            {
                _onSourceUpdateWithParameter.Execute(_sourcePropertyFunc!.Invoke());
            }

            return this;
        }

        /// <summary>
        ///     Defines an generic function that will be executed every time that the binding value changes.
        /// </summary>
        /// <param name="callback">
        ///     The function that will be executed when the binding changes.
        ///     IMPORTANT: Note that closures are not supported at the moment
        ///     due to the use of WeakActions (see http://stackoverflow.com/questions/25730530/).
        /// </param>
        /// <returns>The Binding instance.</returns>
        /// <exception cref="InvalidOperationException">
        ///     When the method is called on a binding that already has a target property set.
        /// </exception>
        public Binding<TSource, TTarget> WhenSourceChanges(Func<TSource, Task> callback)
        {
            _onSourceUpdateFunctionWithParameter = new WeakFunc<TSource, Task>(callback);

            return WhenSourceChanges(source =>
            {
                _onSourceUpdateFunctionWithParameter.Execute(_sourcePropertyFunc!.Invoke());
            });
        }

        /// <summary>
        ///     Defines the behavior to update the binding when the source control's property changes.
        /// </summary>
        /// <returns>The Binding instance.</returns>
        protected abstract Binding<TSource, TTarget> CheckControlSource();

        /// <summary>
        ///     Defines the behavior to update the binding when the source control's property changes.
        /// </summary>
        /// <returns>The Binding instance.</returns>
        protected abstract Binding<TSource, TTarget> CheckControlTarget();

        private void Attach(object? source, object? target, BindingMode mode)
        {
            Attach(source, target, mode, _resolveTopField);
        }

        private void Attach(object? source, object? target, BindingMode mode, bool resolveTopField)
        {
            _resolveTopField = resolveTopField;

            var sourceChain = GetPropertyChain(
                source,
                null,
                _sourcePropertyExpression!.Body as MemberExpression,
                _sourcePropertyName!,
                resolveTopField);

            var lastSourceInChain = sourceChain.Last();
            sourceChain.Remove(lastSourceInChain);

            _propertySource = new WeakReference(lastSourceInChain.Instance);

            if (mode != BindingMode.OneTime)
            {
                foreach (var instance in sourceChain)
                {
                    if (instance.Instance is INotifyPropertyChanged inpc)
                    {
                        var listener = new ObjectSwappedEventListener(this, inpc);
                        _listeners.Add(listener);
                        PropertyChangedEventManager.AddListener(inpc, listener, instance.Name);
                    }
                }
            }

            if (target != null
                && _targetPropertyExpression != null
                && _targetPropertyName != null)
            {
                var targetChain = GetPropertyChain(
                    target,
                    null,
                    _targetPropertyExpression.Body as MemberExpression,
                    _targetPropertyName,
                    resolveTopField);

                var lastTargetInChain = targetChain.Last();
                targetChain.Remove(lastTargetInChain);

                _propertyTarget = new WeakReference(lastTargetInChain.Instance);

                if (mode != BindingMode.OneTime)
                {
                    foreach (var instance in targetChain)
                    {
                        if (instance.Instance is INotifyPropertyChanged inpc)
                        {
                            var listener = new ObjectSwappedEventListener(this, inpc);
                            _listeners.Add(listener);
                            PropertyChangedEventManager.AddListener(inpc, listener, instance.Name);
                        }
                    }
                }
            }

            _isFallbackValueActive = false;

            if (sourceChain.Any(r => r.Instance == null))
            {
                _isFallbackValueActive = true;
            }
            else
            {
                if (lastSourceInChain.Instance == null)
                {
                    _isFallbackValueActive = true;
                }
            }

            Attach();
        }

        private void Attach()
        {
            if (_propertyTarget != null
                && _propertyTarget.IsAlive
                && _propertyTarget.Target != null
                && !string.IsNullOrEmpty(_targetPropertyName))
            {
                var targetType = _propertyTarget.Target.GetType();

                _targetProperty = targetType.GetRuntimeProperty(_targetPropertyName);
                if (_targetProperty == null)
                {
                    throw new InvalidOperationException($"Property not found: {_targetPropertyName}");
                }
            }

            if (_propertySource == null
                || !_propertySource.IsAlive
                || _propertySource.Target == null)
            {
                SetSpecialValues();
                return;
            }

            var sourceType = _propertySource.Target.GetType();

            _sourceProperty = sourceType.GetRuntimeProperty(_sourcePropertyName);

            if (_sourceProperty == null)
            {
                throw new InvalidOperationException($"Property not found: {_sourcePropertyName}");
            }

            // OneTime binding
            if (CanBeConverted(_sourceProperty, _targetProperty))
            {
                var value = GetSourceValue();

                if (_targetProperty != null
                    && _propertyTarget != null
                    && _propertyTarget.IsAlive
                    && _propertyTarget.Target != null)
                {
                    _settingSourceToTarget = true;
                    SetTargetValue(value!);
                    _settingSourceToTarget = false;
                }

                if (_onSourceUpdate != null
                    && _onSourceUpdate.IsAlive)
                {
                    _onSourceUpdate.Execute();
                }

                if (_onSourceUpdateWithParameter != null
                    && _onSourceUpdateWithParameter.IsAlive)
                {
                    _onSourceUpdateWithParameter.Execute(_sourcePropertyFunc!.Invoke());
                }
            }

            if (Mode == BindingMode.OneTime)
            {
                return;
            }

            // Check OneWay binding
            if (_propertySource.Target is INotifyPropertyChanged inpc)
            {
                var listener = new PropertyChangedEventListener(this, inpc, true);
                _listeners.Add(listener);
                PropertyChangedEventManager.AddListener(inpc, listener, _sourcePropertyName);
            }
            else
            {
                CheckControlSource();
            }

            if (Mode == BindingMode.OneWay
                || Mode == BindingMode.Default)
            {
                return;
            }

            // Check TwoWay binding
            if (_onSourceUpdate == null
                && _onSourceUpdateWithParameter == null
                && _propertyTarget != null
                && _propertyTarget.IsAlive
                && _propertyTarget.Target != null)
            {
                if (_propertyTarget.Target is INotifyPropertyChanged inpc2)
                {
                    var listener = new PropertyChangedEventListener(this, inpc2, false);
                    _listeners.Add(listener);
                    PropertyChangedEventManager.AddListener(inpc2, listener, _targetPropertyName);
                }
                else
                {
                    CheckControlTarget();
                }
            }
        }

        private bool CanBeConverted(PropertyInfo? sourceProperty, PropertyInfo? targetProperty)
        {
            if (sourceProperty == null || targetProperty == null)
            {
                return true;
            }

            var sourceType = sourceProperty.PropertyType;
            var targetType = targetProperty.PropertyType;

            return sourceType == targetType
                   || (IsValueType(sourceType) && IsValueType(targetType));
        }

        private void DetachAllSourceHandlers()
        {
            if (_propertySource == null
                || !_propertySource.IsAlive
                || _propertySource.Target == null)
            {
                return;
            }

            foreach (var eventName in SourceHandlers.Keys)
            {
                var type = _propertySource.Target.GetType();
                var @event = type.GetRuntimeEvent(eventName);
                if (@event == null)
                {
                    return;
                }

                @event.RemoveEventHandler(_propertySource.Target, SourceHandlers[eventName].Delegate);
            }

            SourceHandlers.Clear();
        }

        private void DetachAllTargetHandlers()
        {
            if (_propertySource == null
                || !_propertySource.IsAlive
                || _propertySource.Target == null)
            {
                return;
            }

            foreach (var eventName in TargetHandlers.Keys)
            {
                var type = _propertyTarget!.Target.GetType();
                var @event = type.GetRuntimeEvent(eventName);
                if (@event == null)
                {
                    return;
                }

                @event.RemoveEventHandler(_propertyTarget.Target, TargetHandlers[eventName].Delegate);
            }

            TargetHandlers.Clear();
        }

        private IList<PropertyAndName> GetPropertyChain(
            object? topInstance,
            IList<PropertyAndName>? instances,
            MemberExpression? expression,
            string propertyName,
            bool resolveTopField,
            bool top = true)
        {
            if (instances == null)
            {
                instances = new List<PropertyAndName>();
            }

            var expr = expression!.Expression as MemberExpression;
            if (expr == null)
            {
                if (top)
                {
                    instances.Add(
                        new PropertyAndName
                        {
                            Instance = topInstance,
                            Name = propertyName
                        });
                }

                return instances;
            }

            var list = GetPropertyChain(topInstance, instances, expr, propertyName, resolveTopField, false);

            if (list.Count == 0)
            {
                list.Add(
                    new PropertyAndName
                    {
                        Instance = topInstance
                    });
            }

            if (top
                && list.Count > 0
                && list.First().Instance != topInstance)
            {
                list.Insert(
                    0,
                    new PropertyAndName
                    {
                        Instance = topInstance
                    });
            }

            var lastInstance = list.Last();

            if (lastInstance.Instance != null)
            {
                if (expr.Member is PropertyInfo prop)
                {
                    try
                    {
                        var newInstance = prop.GetMethod.Invoke(lastInstance.Instance, new object[] { });

                        lastInstance.Name = prop.Name;

                        list.Add(
                            new PropertyAndName
                            {
                                Instance = newInstance
                            });
                    }
                    catch (TargetInvocationException)
                    {
                        // ignored
                    }
                }
                else
                {
                    if (lastInstance.Instance == topInstance && resolveTopField)
                    {
                        var field = expr.Member as FieldInfo;
                        if (field != null)
                        {
                            try
                            {
                                var newInstance = field.GetValue(lastInstance.Instance);

                                lastInstance.Name = field.Name;

                                list.Add(
                                    new PropertyAndName
                                    {
                                        Instance = newInstance
                                    });
                            }
                            catch (ArgumentException)
                            {
                                throw new InvalidOperationException(
                                    "Are you trying to use SetBinding with a local variable? Try to use new Binding instead");
                            }
                        }
                    }
                }

                if (top)
                {
                    list.Last().Name = propertyName;
                }
            }

            return list;
        }

        [SuppressMessage(
            "StyleCop.CSharp.OrderingRules",
            "SA1204:Static elements should appear before instance elements",
            Justification = "Revieved.")]
        private static string? GetPropertyName<T>(Expression<Func<T>>? propertyExpression)
        {
            if (propertyExpression == null)
            {
                return null;
            }

            var body = propertyExpression.Body as MemberExpression;
            if (body == null)
            {
                throw new ArgumentException("Invalid argument", nameof(propertyExpression));
            }

            var property = body.Member as PropertyInfo;
            if (property == null)
            {
                throw new ArgumentException("Argument is not a property", nameof(propertyExpression));
            }

            return property.Name;
        }

        [return: MaybeNull]
        private TTarget GetSourceValue()
        {
            if (_sourceProperty == null)
            {
                return default;
            }

            var sourceValue = (TSource) _sourceProperty.GetValue(_propertySource!.Target, null);

            try
            {
                return _converter.Convert(sourceValue);
            }
            catch (Exception)
            {
                if (!Equals(FallbackValue, default(TSource)))
                {
                    return _converter.Convert(FallbackValue);
                }

                var targetValue = (TTarget) _targetProperty!.GetValue(_propertyTarget!.Target, null);
                return targetValue;
            }
        }

        [return: MaybeNull]
        private TSource GetTargetValue()
        {
            var targetValue = (TTarget) _targetProperty!.GetValue(_propertyTarget!.Target, null);

            try
            {
                return _converter.ConvertBack(targetValue);
            }
            catch (Exception)
            {
                var sourceValue = (TSource) _sourceProperty!.GetValue(_propertySource!.Target, null);
                return sourceValue;
            }
        }

        private void HandleSourceEvent<TEventArgs>(object sender, TEventArgs args)
        {
            if (_propertyTarget != null
                && _propertyTarget.IsAlive
                && _propertyTarget.Target != null
                && _propertySource != null
                && _propertySource.IsAlive
                && _propertySource.Target != null
                && !_settingTargetToSource)
            {
                var valueLocal = GetSourceValue();
                var targetValue = _targetProperty!.GetValue(_propertyTarget.Target, null);

                if (Equals(valueLocal, targetValue))
                {
                    return;
                }

                if (_targetProperty != null)
                {
                    _settingSourceToTarget = true;
                    SetTargetValue(valueLocal!);
                    _settingSourceToTarget = false;
                }
            }

            _onSourceUpdate?.Execute();
            _onSourceUpdateWithParameter?.Execute(_sourcePropertyFunc!.Invoke());

            RaiseValueChanged();
        }

        private void HandleTargetEvent<TEventArgs>(object source, TEventArgs args)
        {
            if (_propertyTarget != null
                && _propertyTarget.IsAlive
                && _propertyTarget.Target != null
                && _propertySource != null
                && _propertySource.IsAlive
                && _propertySource.Target != null
                && !_settingSourceToTarget)
            {
                var valueLocal = GetTargetValue();
                var sourceValue = _sourceProperty!.GetValue(_propertySource.Target, null);

                if (Equals(valueLocal, sourceValue))
                {
                    return;
                }

                _settingTargetToSource = true;
                SetSourceValue(valueLocal!);
                _settingTargetToSource = false;
            }

            RaiseValueChanged();
        }

        private bool IsSourceDefaultValue()
        {
            if (_sourceProperty == null)
            {
                return true;
            }

            var sourceValue = (TSource) _sourceProperty.GetValue(_propertySource!.Target, null);
            return Equals(default(TSource), sourceValue);
        }

        private bool IsValueType(Type type)
        {
            return !type.GetTypeInfo().IsClass;
        }

        private void RaiseValueChanged()
        {
            var handler = ValueChanged;
            handler?.Invoke(this, EventArgs.Empty);
        }

        private void SetSourceValue([MaybeNull] TSource value)
        {
            _sourceProperty!.SetValue(_propertySource!.Target, value, null);
        }

        private bool SetSpecialValues()
        {
            if (_isFallbackValueActive)
            {
                _targetProperty!.SetValue(_propertyTarget!.Target, _converter.Convert(FallbackValue), null);
                return true;
            }

            if (!Equals(default(TTarget), TargetNullValue))
            {
                if (IsSourceDefaultValue())
                {
                    _targetProperty!.SetValue(_propertyTarget!.Target, _converter.Convert(TargetNullValue), null);
                    return true;
                }
            }

            return false;
        }

        private void SetTargetValue([MaybeNull] TTarget value)
        {
            if (!SetSpecialValues())
            {
                _targetProperty!.SetValue(_propertyTarget!.Target, value, null);
            }
        }

        internal class ObjectSwappedEventListener : IWeakEventListener
        {
            private readonly WeakReference _bindingReference;

            public ObjectSwappedEventListener(
                Binding<TSource, TTarget> binding,
                INotifyPropertyChanged instance)
            {
                _bindingReference = new WeakReference(binding);
                InstanceReference = new WeakReference(instance);
            }

            public WeakReference InstanceReference { get; }

            public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
            {
                if (InstanceReference.Target == sender
                    && e is PropertyChangedEventArgs
                    && _bindingReference.IsAlive
                    && _bindingReference.Target != null)
                {
                    var binding = (Binding<TSource, TTarget>) _bindingReference.Target;

                    binding.Detach();

                    binding.Attach(binding.Source, binding.Target, binding.Mode);

                    return true;
                }

                return false;
            }
        }

        internal class PropertyAndName
        {
            public object? Instance;
            public string? Name;
        }

        internal class PropertyChangedEventListener : IWeakEventListener
        {
            private readonly WeakReference _bindingReference;
            private readonly bool _updateFromSourceToTarget;

            public PropertyChangedEventListener(
                Binding<TSource, TTarget> binding,
                INotifyPropertyChanged instance,
                bool updateFromSourceToTarget)
            {
                _updateFromSourceToTarget = updateFromSourceToTarget;
                _bindingReference = new WeakReference(binding);
                InstanceReference = new WeakReference(instance);
            }

            /// <summary>
            ///     Gets a reference to the instance that this listener listens to.
            /// </summary>
            public WeakReference InstanceReference { get; }

            public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
            {
                if (_bindingReference.IsAlive
                    && _bindingReference.Target != null)
                {
                    var binding = (Binding<TSource, TTarget>) _bindingReference.Target;

                    if (_updateFromSourceToTarget)
                    {
                        if (binding._propertySource != null
                            && binding._propertySource.IsAlive
                            && sender == binding._propertySource.Target)
                        {
                            if (!binding._settingTargetToSource)
                            {
                                binding.ForceUpdateValueFromSourceToTarget();
                            }

                            binding._settingTargetToSource = false;
                        }
                    }
                    else
                    {
                        if (binding._propertyTarget != null
                            && binding._propertyTarget.IsAlive
                            && sender == binding._propertyTarget.Target)
                        {
                            if (!binding._settingSourceToTarget)
                            {
                                binding.ForceUpdateValueFromTargetToSource();
                            }

                            binding._settingSourceToTarget = false;
                        }
                    }

                    return true;
                }

                return false;
            }
        }

        /// <summary>
        ///     Extends delegate with additional properties.
        /// </summary>
        [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Reviewed.")]
        public class DelegateInfo
        {
            /// <summary>
            ///     The delegate instance.
            /// </summary>
            public Delegate? Delegate;

            /// <summary>
            ///     Indicates that this delegate should be used as default.
            /// </summary>
            public bool IsDefault;
        }

        private class WeakConverter
        {
            private WeakFunc<TSource, TTarget>? _convert;
            private WeakFunc<TTarget, TSource>? _convertBack;

            [return: MaybeNull]
            public TTarget Convert(TSource value)
            {
                if (_convert is { IsAlive: true })
                {
                    return _convert.Execute(value);
                }

                try
                {
                    return ConvertSafely<TSource, TTarget>(value);
                }
                catch (Exception)
                {
                    return default;
                }
            }

            [return: MaybeNull]
            public TSource ConvertBack(TTarget value)
            {
                if (_convertBack is { IsAlive: true })
                {
                    return _convertBack.Execute(value);
                }

                try
                {
                    return ConvertSafely<TTarget, TSource>(value);
                }
                catch (Exception)
                {
                    return default;
                }
            }

            public void SetConvert(Func<TSource, TTarget> convert)
            {
                _convert = new WeakFunc<TSource, TTarget>(convert);
            }

            public void SetConvertBack(Func<TTarget, TSource> convertBack)
            {
                _convertBack = new WeakFunc<TTarget, TSource>(convertBack);
            }

            [return: MaybeNull]
            private static TTo ConvertSafely<TFrom, TTo>(TFrom value)
            {
                try
                {
                    var notNullableFromType = Nullable.GetUnderlyingType(typeof(TFrom));
                    object? notNullableValue = value;
                    if (notNullableFromType != null)
                    {
                        if (value == null)
                        {
                            return default;
                        }

                        notNullableValue = System.Convert.ChangeType(value, notNullableFromType);
                    }

                    var notNullableToType = Nullable.GetUnderlyingType(typeof(TTo)) ?? typeof(TTo);
                    return (TTo) System.Convert.ChangeType(notNullableValue, notNullableToType);
                }
                catch (Exception)
                {
                    return default;
                }
            }
        }
    }
}
