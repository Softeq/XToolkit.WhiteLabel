// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Softeq.XToolkit.Common;
using Softeq.XToolkit.Common.Interfaces;

namespace Softeq.XToolkit.Bindings
{
    /// <summary>
    ///     Creates a binding between two properties. If the source implements INotifyPropertyChanged, the source property
    ///     raises the PropertyChanged event
    ///     and the BindingMode is OneWay or TwoWay, the target property will be synchronized with the source property. If
    ///     the target implements INotifyPropertyChanged, the target property raises the PropertyChanged event and the
    ///     BindingMode is
    ///     TwoWay, the source property will also be synchronized with the target property.
    /// </summary>
    /// <typeparam name="TSource">The type of the source property that is being databound.</typeparam>
    /// <typeparam name="TTarget">
    ///     The type of the target property that is being databound. If the source type
    ///     is not the same as the target type, an automatic conversion will be attempted. However only
    ///     simple types can be converted. For more complex conversions, use the <see cref="ConvertSourceToTarget" />
    ///     and <see cref="ConvertTargetToSource" /> methods to define custom converters.
    /// </typeparam>
    public abstract class Binding<TSource, TTarget> : Binding
    {
        private readonly SimpleConverter _converter = new SimpleConverter();
        private readonly List<IWeakEventListener> _listeners = new List<IWeakEventListener>();
        private readonly Expression<Func<TSource>> _sourcePropertyExpression;
        private readonly string _sourcePropertyName;
        private readonly Expression<Func<TTarget>> _targetPropertyExpression;
        private readonly string _targetPropertyName;
        public readonly Dictionary<string, DelegateInfo> SourceHandlers = new Dictionary<string, DelegateInfo>();
        public readonly Dictionary<string, DelegateInfo> TargetHandlers = new Dictionary<string, DelegateInfo>();
        private bool _isFallbackValueActive;
        private WeakAction _onSourceUpdate;
        private bool _resolveTopField;
        private bool _settingSourceToTarget;
        private bool _settingTargetToSource;
        private PropertyInfo _sourceProperty;
        private PropertyInfo _targetProperty;
        protected WeakReference PropertySource;
        protected WeakReference PropertyTarget;
        private IConverter<TTarget, TSource> _valueConverter;
        private WeakAction<TSource> _onSourceUpdateWithParameter;
        private readonly Func<TSource> _sourcePropertyFunc;
        private WeakFunc<TSource, Task> _sourceUpdateFunctionWithParameter;

        /// <summary>
        ///     Initializes a new instance of the Binding class for which the source and target properties
        ///     are located in different objects.
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
        ///     created) but that subsequent changes will be ignored. OneWay means that the target property will be set, and
        ///     if the PropertyChanged event is raised by the source, the target property will be updated. TwoWay means that the
        ///     source
        ///     property will also be updated if the target raises the PropertyChanged event. Default means OneWay if only the
        ///     source
        ///     implements INPC, and TwoWay if both the source and the target implement INPC.
        /// </param>
        /// <param name="fallbackValue">
        ///     Tthe value to use when the binding is unable to return a value. This can happen if one of the
        ///     items on the Path (except the source property itself) is null, or if the Converter throws an exception.
        /// </param>
        /// <param name="targetNullValue">
        ///     The value to use when the binding is unable to return a value. This can happen if one of the
        ///     items on the Path (except the source property itself) is null, or if the Converter throws an exception.
        /// </param>
        protected Binding(
            object source,
            string sourcePropertyName,
            object target = null,
            string targetPropertyName = null,
            BindingMode mode = BindingMode.Default,
            TSource fallbackValue = default(TSource),
            TSource targetNullValue = default(TSource))
        {
            Mode = mode;
            FallbackValue = fallbackValue;
            TargetNullValue = targetNullValue;

            TopSource = new WeakReference(source);
            PropertySource = new WeakReference(source);
            _sourcePropertyName = sourcePropertyName;

            if (target == null)
            {
                TopTarget = TopSource;
                PropertyTarget = PropertySource;
            }
            else
            {
                TopTarget = new WeakReference(target);
                PropertyTarget = new WeakReference(target);
            }

            _targetPropertyName = targetPropertyName;
            Attach();
        }

        /// <summary>
        ///     Initializes a new instance of the Binding class for which the source and target properties
        ///     are located in different objects.
        /// </summary>
        /// <param name="source">
        ///     The source of the binding. If this object implements INotifyPropertyChanged and the
        ///     BindingMode is OneWay or TwoWay, the target will be notified of changes to the target property.
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
        ///     The mode of the binding. OneTime means that the target property will be set once (when the binding is
        ///     created) but that subsequent changes will be ignored. OneWay means that the target property will be set, and
        ///     if the PropertyChanged event is raised by the source, the target property will be updated. TwoWay means that the
        ///     source
        ///     property will also be updated if the target raises the PropertyChanged event. Default means OneWay if only the
        ///     source
        ///     implements INPC, and TwoWay if both the source and the target implement INPC.
        /// </param>
        /// <param name="fallbackValue">
        ///     Tthe value to use when the binding is unable to return a value. This can happen if one of the
        ///     items on the Path (except the source property itself) is null, or if the Converter throws an exception.
        /// </param>
        /// <param name="targetNullValue">
        ///     The value to use when the binding is unable to return a value. This can happen if one of the
        ///     items on the Path (except the source property itself) is null, or if the Converter throws an exception.
        /// </param>
        protected Binding(
            object source,
            Expression<Func<TSource>> sourcePropertyExpression,
            object target = null,
            Expression<Func<TTarget>> targetPropertyExpression = null,
            BindingMode mode = BindingMode.Default,
            TSource fallbackValue = default(TSource),
            TSource targetNullValue = default(TSource))
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

        protected Binding(
            object source,
            Expression<Func<TSource>> sourcePropertyExpression,
            bool? resolveTopField,
            object target = null,
            Expression<Func<TTarget>> targetPropertyExpression = null,
            BindingMode mode = BindingMode.Default,
            TSource fallbackValue = default(TSource),
            TSource targetNullValue = default(TSource))
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
        ///     Gets or sets the value to use when the binding is unable to return a value. This can happen if one of the
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
        public TTarget Value
        {
            get
            {
                if (PropertySource == null
                    || !PropertySource.IsAlive)
                {
                    return default(TTarget);
                }

                var type = PropertySource.Target.GetType();
                var property = type.GetRuntimeProperty(_sourcePropertyName);
                return (TTarget) property.GetValue(PropertySource.Target, null);
            }
        }

        /// <summary>
        ///     Defines a custom conversion method for a binding. To be used when the
        ///     binding's source property is of a different type than the binding's
        ///     target property, and the conversion cannot be done automatically (simple
        ///     values).
        /// </summary>
        /// <param name="convert">
        ///     A func that will be called with the source
        ///     property's value, and will return the target property's value.
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
        ///     Defines a custom conversion method for a two-way binding. To be used when the
        ///     binding's target property is of a different type than the binding's
        ///     source property, and the conversion cannot be done automatically (simple
        ///     values).
        /// </summary>
        /// <param name="convertBack">
        ///     A func that will be called with the source
        ///     property's value, and will return the target property's value.
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

        public Binding SetConverter(IConverter<TTarget, TSource> converter)
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
        ///     Instructs the Binding instance to stop listening to value changes and to
        ///     remove all listeneners.
        /// </summary>
        public override void Detach()
        {
            foreach (var listener in _listeners)
            {
                PropertyChangedEventManager.RemoveListener(listener);
            }

            _listeners.Clear();

            DetachSourceHandlers();
            DetachTargetHandlers();
        }

        /// <summary>
        ///     Forces the Binding's value to be reevaluated. The target value will
        ///     be set to the source value.
        /// </summary>
        public override void ForceUpdateValueFromSourceToTarget()
        {
            if (_onSourceUpdate == null
                && _onSourceUpdateWithParameter == null
                && (PropertySource == null
                    || !PropertySource.IsAlive
                    || PropertySource.Target == null))
            {
                return;
            }

            if (_targetProperty != null)
            {
                try
                {
                    var value = GetSourceValue();
                    var targetValue = _targetProperty.GetValue(PropertyTarget.Target);

                    if (!Equals(value, targetValue))
                    {
                        _settingSourceToTarget = true;
                        SetTargetValue(value);
                        _settingSourceToTarget = false;
                    }
                }
                catch
                {
                    if (!Equals(FallbackValue, default(TSource)))
                    {
                        _settingSourceToTarget = true;
                        _targetProperty.SetValue(PropertyTarget.Target, FallbackValue, null);
                        _settingSourceToTarget = false;
                    }
                }
            }

            if (_onSourceUpdate != null
                && _onSourceUpdate.IsAlive)
            {
                _onSourceUpdate.Execute();
            }

            if (_onSourceUpdateWithParameter != null
                && _onSourceUpdateWithParameter.IsAlive)
            {
                _onSourceUpdateWithParameter.Execute(_sourcePropertyFunc.Invoke());
            }

            RaiseValueChanged();
        }

        /// <summary>
        ///     Forces the Binding's value to be reevaluated. The source value will
        ///     be set to the target value.
        /// </summary>
        public override void ForceUpdateValueFromTargetToSource()
        {
            if (PropertyTarget == null
                || !PropertyTarget.IsAlive
                || PropertyTarget.Target == null
                || PropertySource == null
                || !PropertySource.IsAlive
                || PropertySource.Target == null)
            {
                return;
            }

            if (_targetProperty != null)
            {
                var value = GetTargetValue();
                var sourceValue = _sourceProperty.GetValue(PropertySource.Target);

                if (!Equals(value, sourceValue))
                {
                    _settingTargetToSource = true;
                    SetSourceValue(value);
                    _settingTargetToSource = false;
                }
            }

            RaiseValueChanged();
        }

        /// <summary>
        ///     Define when the binding should be evaluated when the bound source object
        ///     is a control. Because Xamarin controls are not DependencyObjects, the
        ///     bound property will not automatically update the binding attached to it. Instead,
        ///     use this method to define which of the control's events should be observed.
        /// </summary>
        /// <param name="eventName">
        ///     The name of the event that should be observed
        ///     to update the binding's value.
        /// </param>
        /// <returns>The Binding instance.</returns>
        /// <exception cref="InvalidOperationException">
        ///     When this method is called
        ///     on a OneTime binding. Such bindings cannot be updated. This exception can
        ///     also be thrown when the source object is null or has already been
        ///     garbage collected before this method is called.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     When the eventName parameter is null
        ///     or is an empty string.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     When the requested event does not exist on the
        ///     source control.
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
                && (PropertySource == null
                    || !PropertySource.IsAlive
                    || PropertySource.Target == null))
            {
                throw new InvalidOperationException("Source is not ready");
            }

            if (string.IsNullOrEmpty(eventName))
            {
                throw new ArgumentNullException("eventName");
            }

            var type = PropertySource.Target.GetType();
            var ev = type.GetRuntimeEvent(eventName);
            if (ev == null)
            {
                throw new ArgumentException(
                    "Event not found: " + eventName,
                    "eventName");
            }

            EventHandler handler = HandleSourceEvent;

            var defaultHandlerInfo = SourceHandlers.Values.FirstOrDefault(i => i.IsDefault);

            if (defaultHandlerInfo != null)
            {
                DetachSourceHandlers();
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

            ev.AddEventHandler(
                PropertySource.Target,
                handler);

            return this;
        }

        /// <summary>
        ///     Define when the binding should be evaluated when the bound source object
        ///     is a control. Because Xamarin controls are not DependencyObjects, the
        ///     bound property will not automatically update the binding attached to it. Instead,
        ///     use this method to define which of the control's events should be observed.
        /// </summary>
        /// <remarks>
        ///     Use this method when the event requires a specific EventArgs type
        ///     instead of the standard EventHandler.
        /// </remarks>
        /// <typeparam name="TEventArgs">The type of the EventArgs used by this control's event.</typeparam>
        /// <param name="eventName">
        ///     The name of the event that should be observed
        ///     to update the binding's value.
        /// </param>
        /// <returns>The Binding instance.</returns>
        /// <exception cref="InvalidOperationException">
        ///     When this method is called
        ///     on a OneTime binding. Such bindings cannot be updated. This exception can
        ///     also be thrown when the source object is null or has already been
        ///     garbage collected before this method is called.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     When the eventName parameter is null
        ///     or is an empty string.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     When the requested event does not exist on the
        ///     source control.
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
                && (PropertySource == null
                    || !PropertySource.IsAlive
                    || PropertySource.Target == null))
            {
                throw new InvalidOperationException("Source is not ready");
            }

            if (string.IsNullOrEmpty(eventName))
            {
                throw new ArgumentNullException("eventName");
            }

            var type = PropertySource.Target.GetType();
            var ev = type.GetRuntimeEvent(eventName);
            if (ev == null)
            {
                throw new ArgumentException(
                    "Event not found: " + eventName,
                    "eventName");
            }

            EventHandler<TEventArgs> handler = HandleSourceEvent;

            var defaultHandlerInfo = SourceHandlers.Values.FirstOrDefault(i => i.IsDefault);

            if (defaultHandlerInfo != null)
            {
                DetachSourceHandlers();
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

            ev.AddEventHandler(
                PropertySource.Target,
                handler);

            return this;
        }

        /// <summary>
        ///     Define when the binding should be evaluated when the bound target object
        ///     is a control. Because Xamarin controls are not DependencyObjects, the
        ///     bound property will not automatically update the binding attached to it. Instead,
        ///     use this method to define which of the control's events should be observed.
        /// </summary>
        /// <param name="eventName">
        ///     The name of the event that should be observed
        ///     to update the binding's value.
        /// </param>
        /// <returns>The Binding instance.</returns>
        /// <exception cref="InvalidOperationException">
        ///     When this method is called
        ///     on a OneTime or a OneWay binding. This exception can
        ///     also be thrown when the source object is null or has already been
        ///     garbage collected before this method is called.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     When the eventName parameter is null
        ///     or is an empty string.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     When the requested event does not exist on the
        ///     target control.
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

            if (PropertyTarget == null
                || !PropertyTarget.IsAlive
                || PropertyTarget.Target == null)
            {
                throw new InvalidOperationException("Target is not ready");
            }

            if (string.IsNullOrEmpty(eventName))
            {
                throw new ArgumentNullException("eventName");
            }

            var type = PropertyTarget.Target.GetType();

            var ev = type.GetRuntimeEvent(eventName);
            if (ev == null)
            {
                throw new ArgumentException(
                    "Event not found: " + eventName,
                    "eventName");
            }

            EventHandler handler = HandleTargetEvent;

            var defaultHandlerInfo = TargetHandlers.Values.FirstOrDefault(i => i.IsDefault);

            if (defaultHandlerInfo != null)
            {
                DetachTargetHandlers();
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

            ev.AddEventHandler(
                PropertyTarget.Target,
                handler);

            return this;
        }

        /// <summary>
        ///     Define when the binding should be evaluated when the bound target object
        ///     is a control. Because Xamarin controls are not DependencyObjects, the
        ///     bound property will not automatically update the binding attached to it. Instead,
        ///     use this method to define which of the control's events should be observed.
        /// </summary>
        /// <remarks>
        ///     Use this method when the event requires a specific EventArgs type
        ///     instead of the standard EventHandler.
        /// </remarks>
        /// <typeparam name="TEventArgs">The type of the EventArgs used by this control's event.</typeparam>
        /// <param name="eventName">
        ///     The name of the event that should be observed
        ///     to update the binding's value.
        /// </param>
        /// <returns>The Binding instance.</returns>
        /// <exception cref="InvalidOperationException">
        ///     When this method is called
        ///     on a OneTime or OneWay binding. This exception can
        ///     also be thrown when the target object is null or has already been
        ///     garbage collected before this method is called.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     When the eventName parameter is null
        ///     or is an empty string.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     When the requested event does not exist on the
        ///     target control.
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

            if (PropertyTarget == null
                || !PropertyTarget.IsAlive
                || PropertyTarget.Target == null)
            {
                throw new InvalidOperationException("Target is not ready");
            }

            if (string.IsNullOrEmpty(eventName))
            {
                throw new ArgumentNullException("eventName");
            }

            var type = PropertyTarget.Target.GetType();

            var ev = type.GetRuntimeEvent(eventName);
            if (ev == null)
            {
                throw new ArgumentException(
                    "Event not found: " + eventName,
                    "eventName");
            }

            EventHandler<TEventArgs> handler = HandleTargetEvent;

            var defaultHandlerInfo = TargetHandlers.Values.FirstOrDefault(i => i.IsDefault);

            if (defaultHandlerInfo != null)
            {
                DetachTargetHandlers();
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

            ev.AddEventHandler(
                PropertyTarget.Target,
                handler);

            return this;
        }

        /// <summary>
        ///     Defines an action that will be executed every time that the binding value
        ///     changes.
        /// </summary>
        /// <param name="callback">
        ///     The action that will be executed when the binding changes.
        ///     IMPORTANT: Note that closures are not supported at the moment
        ///     due to the use of WeakActions (see http://stackoverflow.com/questions/25730530/).
        /// </param>
        /// <returns>The Binding instance.</returns>
        /// <exception cref="InvalidOperationException">
        ///     When WhenSourceChanges is called on
        ///     a binding which already has a target property set.
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
                _onSourceUpdateWithParameter.Execute(_sourcePropertyFunc.Invoke());
            }

            return this;
        }

        public Binding<TSource, TTarget> WhenSourceChanges(Func<TSource, Task> func)
        {
            _sourceUpdateFunctionWithParameter = new WeakFunc<TSource, Task>(func);

            return WhenSourceChanges(source =>
            {
                _sourceUpdateFunctionWithParameter.Execute(_sourcePropertyFunc.Invoke());
            });
        }

        protected abstract Binding<TSource, TTarget> CheckControlSource();

        protected abstract Binding<TSource, TTarget> CheckControlTarget();

        private void Attach(
            object source,
            object target,
            BindingMode mode)
        {
            Attach(source, target, mode, _resolveTopField);
        }

        private void Attach(
            object source,
            object target,
            BindingMode mode,
            bool resolveTopField)
        {
            _resolveTopField = resolveTopField;

            var sourceChain = GetPropertyChain(
                source,
                null,
                _sourcePropertyExpression.Body as MemberExpression,
                _sourcePropertyName,
                resolveTopField);

            var lastSourceInChain = sourceChain.Last();
            sourceChain.Remove(lastSourceInChain);

            PropertySource = new WeakReference(lastSourceInChain.Instance);

            if (mode != BindingMode.OneTime)
            {
                foreach (var instance in sourceChain)
                {
                    var inpc = instance.Instance as INotifyPropertyChanged;
                    if (inpc != null)
                    {
                        var listener = new ObjectSwappedEventListener(
                            this,
                            inpc);
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

                PropertyTarget = new WeakReference(lastTargetInChain.Instance);

                if (mode != BindingMode.OneTime)
                {
                    foreach (var instance in targetChain)
                    {
                        var inpc = instance.Instance as INotifyPropertyChanged;
                        if (inpc != null)
                        {
                            var listener = new ObjectSwappedEventListener(
                                this,
                                inpc);
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
            if (PropertyTarget != null
                && PropertyTarget.IsAlive
                && PropertyTarget.Target != null
                && !string.IsNullOrEmpty(_targetPropertyName))
            {
                var targetType = PropertyTarget.Target.GetType();
                _targetProperty = targetType.GetRuntimeProperty(_targetPropertyName);

                if (_targetProperty == null)
                {
                    throw new InvalidOperationException("Property not found: " + _targetPropertyName);
                }
            }

            if (PropertySource == null
                || !PropertySource.IsAlive
                || PropertySource.Target == null)
            {
                SetSpecialValues();
                return;
            }

            var sourceType = PropertySource.Target.GetType();
            _sourceProperty = sourceType.GetRuntimeProperty(_sourcePropertyName);

            if (_sourceProperty == null)
            {
                throw new InvalidOperationException("Property not found: " + _sourcePropertyName);
            }

            // OneTime binding

            if (CanBeConverted(_sourceProperty, _targetProperty))
            {
                var value = GetSourceValue();

                if (_targetProperty != null
                    && PropertyTarget != null
                    && PropertyTarget.IsAlive
                    && PropertyTarget.Target != null)
                {
                    _settingSourceToTarget = true;
                    SetTargetValue(value);
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
                    _onSourceUpdateWithParameter.Execute(_sourcePropertyFunc.Invoke());
                }
            }

            if (Mode == BindingMode.OneTime)
            {
                return;
            }

            // Check OneWay binding

            if (PropertySource.Target is INotifyPropertyChanged inpc)
            {
                var listener = new PropertyChangedEventListener(
                    this,
                    inpc,
                    true);

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
                && PropertyTarget != null
                && PropertyTarget.IsAlive
                && PropertyTarget.Target != null)
            {
                var inpc2 = PropertyTarget.Target as INotifyPropertyChanged;

                if (inpc2 != null)
                {
                    var listener = new PropertyChangedEventListener(
                        this,
                        inpc2,
                        false);

                    _listeners.Add(listener);
                    PropertyChangedEventManager.AddListener(inpc2, listener, _targetPropertyName);
                }
                else
                {
                    CheckControlTarget();
                }
            }
        }

        private bool CanBeConverted(PropertyInfo sourceProperty, PropertyInfo targetProperty)
        {
            if (targetProperty == null)
            {
                return true;
            }

            var sourceType = sourceProperty.PropertyType;
            var targetType = targetProperty.PropertyType;

            return sourceType == targetType
                   || IsValueType(sourceType) && IsValueType(targetType);
        }

        private void DetachSourceHandlers()
        {
            if (PropertySource == null
                || !PropertySource.IsAlive
                || PropertySource.Target == null)
            {
                return;
            }

            foreach (var eventName in SourceHandlers.Keys)
            {
                var type = PropertySource.Target.GetType();
                var ev = type.GetRuntimeEvent(eventName);
                if (ev == null)
                {
                    return;
                }

                ev.RemoveEventHandler(PropertySource.Target, SourceHandlers[eventName].Delegate);
            }

            SourceHandlers.Clear();
        }

        private void DetachTargetHandlers()
        {
            if (PropertySource == null
                || !PropertySource.IsAlive
                || PropertySource.Target == null)
            {
                return;
            }

            foreach (var eventName in TargetHandlers.Keys)
            {
                var type = PropertyTarget.Target.GetType();
                var ev = type.GetRuntimeEvent(eventName);
                if (ev == null)
                {
                    return;
                }

                ev.RemoveEventHandler(PropertyTarget.Target, TargetHandlers[eventName].Delegate);
            }

            TargetHandlers.Clear();
        }

        private static IList<PropertyAndName> GetPropertyChain(
            object topInstance,
            IList<PropertyAndName> instances,
            MemberExpression expression,
            string propertyName,
            bool resolveTopField,
            bool top = true)
        {
            if (instances == null)
            {
                instances = new List<PropertyAndName>();
            }

            var ex = expression.Expression as MemberExpression;

            if (ex == null)
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

            var list = GetPropertyChain(topInstance, instances, ex, propertyName, resolveTopField, false);

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
                var prop = ex.Member as PropertyInfo;

                if (prop != null)
                {
                    try
                    {
                        var newInstance = prop.GetMethod.Invoke(
                            lastInstance.Instance,
                            new object[]
                            {
                            });

                        lastInstance.Name = prop.Name;

                        list.Add(
                            new PropertyAndName
                            {
                                Instance = newInstance
                            });
                    }
                    catch (TargetInvocationException)
                    {
                    }
                }
                else
                {
                    if (lastInstance.Instance == topInstance
                        && resolveTopField)
                    {
                        var field = ex.Member as FieldInfo;
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
                                    "Are you trying to use SetBinding with a local variable? "
                                    + "Try to use new Binding instead");
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

        private static string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                return null;
            }

            var body = propertyExpression.Body as MemberExpression;

            if (body == null)
            {
                throw new ArgumentException("Invalid argument", "propertyExpression");
            }

            var property = body.Member as PropertyInfo;

            if (property == null)
            {
                throw new ArgumentException("Argument is not a property", "propertyExpression");
            }

            return property.Name;
        }

        private TTarget GetSourceValue()
        {
            if (_sourceProperty == null)
            {
                return default(TTarget);
            }

            var sourceValue = (TSource) _sourceProperty.GetValue(PropertySource.Target, null);

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

                var targetValue = (TTarget) _targetProperty.GetValue(PropertyTarget.Target, null);
                return targetValue;
            }
        }

        private TSource GetTargetValue()
        {
            var targetValue = (TTarget) _targetProperty.GetValue(PropertyTarget.Target, null);

            try
            {
                return _converter.ConvertBack(targetValue);
            }
            catch (Exception)
            {
                var sourceValue = (TSource) _sourceProperty.GetValue(PropertySource.Target, null);
                return sourceValue;
            }
        }

        private void HandleSourceEvent<TEventArgs>(object sender, TEventArgs args)
        {
            if (PropertyTarget != null
                && PropertyTarget.IsAlive
                && PropertyTarget.Target != null
                && PropertySource != null
                && PropertySource.IsAlive
                && PropertySource.Target != null
                && !_settingTargetToSource)
            {
                var valueLocal = GetSourceValue();
                var targetValue = _targetProperty.GetValue(PropertyTarget.Target, null);

                if (Equals(valueLocal, targetValue))
                {
                    return;
                }

                if (_targetProperty != null)
                {
                    _settingSourceToTarget = true;
                    SetTargetValue(valueLocal);
                    _settingSourceToTarget = false;
                }
            }

            _onSourceUpdate?.Execute();
            _onSourceUpdateWithParameter?.Execute(_sourcePropertyFunc.Invoke());

            RaiseValueChanged();
        }

        private void HandleTargetEvent<TEventArgs>(object source, TEventArgs args)
        {
            if (PropertyTarget != null
                && PropertyTarget.IsAlive
                && PropertyTarget.Target != null
                && PropertySource != null
                && PropertySource.IsAlive
                && PropertySource.Target != null
                && !_settingSourceToTarget)
            {
                var valueLocal = GetTargetValue();
                var sourceValue = _sourceProperty.GetValue(PropertySource.Target, null);

                if (Equals(valueLocal, sourceValue))
                {
                    return;
                }

                _settingTargetToSource = true;
                SetSourceValue(valueLocal);
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

            var sourceValue = (TSource) _sourceProperty.GetValue(PropertySource.Target, null);
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

        private void SetSourceValue(TSource value)
        {
            _sourceProperty.SetValue(PropertySource.Target, value, null);
        }

        private bool SetSpecialValues()
        {
            if (_isFallbackValueActive)
            {
                _targetProperty.SetValue(PropertyTarget.Target, FallbackValue, null);
                return true;
            }

            if (!Equals(default(TTarget), TargetNullValue))
            {
                if (IsSourceDefaultValue())
                {
                    _targetProperty.SetValue(PropertyTarget.Target, _converter.Convert(TargetNullValue), null);
                    return true;
                }
            }

            return false;
        }

        private void SetTargetValue(TTarget value)
        {
            if (!SetSpecialValues())
            {
                _targetProperty.SetValue(PropertyTarget.Target, value, null);
            }
        }

        /// <summary>
        ///     Occurs when the value of the databound property changes.
        /// </summary>
        public override event EventHandler ValueChanged;

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
                var propArgs = e as PropertyChangedEventArgs;

                if (InstanceReference.Target == sender
                    && propArgs != null
                    && _bindingReference != null
                    && _bindingReference.IsAlive
                    && _bindingReference.Target != null)
                {
                    var binding = (Binding<TSource, TTarget>) _bindingReference.Target;

                    binding.Detach();

                    binding.Attach(
                        binding.Source,
                        binding.Target,
                        binding.Mode);

                    return true;
                }

                return false;
            }
        }

        internal class PropertyAndName
        {
            public object Instance;
            public string Name;
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
                if (_bindingReference != null
                    && _bindingReference.IsAlive
                    && _bindingReference.Target != null)
                {
                    var binding = (Binding<TSource, TTarget>) _bindingReference.Target;

                    if (_updateFromSourceToTarget)
                    {
                        if (binding.PropertySource != null
                            && binding.PropertySource.IsAlive
                            && sender == binding.PropertySource.Target)
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
                        if (binding.PropertyTarget != null
                            && binding.PropertyTarget.IsAlive
                            && sender == binding.PropertyTarget.Target)
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

        public class DelegateInfo
        {
            public Delegate Delegate;
            public bool IsDefault;
        }

        private class SimpleConverter
        {
            private WeakFunc<TSource, TTarget> _convert;
            private WeakFunc<TTarget, TSource> _convertBack;

            public TTarget Convert(TSource value)
            {
                if (_convert != null
                    && _convert.IsAlive)
                {
                    return _convert.Execute(value);
                }

                try
                {
                    return (TTarget) System.Convert.ChangeType(value, typeof(TTarget));
                }
                catch (Exception)
                {
                    return default(TTarget);
                }
            }

            public TSource ConvertBack(TTarget value)
            {
                if (_convertBack != null
                    && _convertBack.IsAlive)
                {
                    return _convertBack.Execute(value);
                }

                try
                {
                    return (TSource) System.Convert.ChangeType(value, typeof(TSource));
                }
                catch (Exception)
                {
                    return default(TSource);
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
        }
    }
}