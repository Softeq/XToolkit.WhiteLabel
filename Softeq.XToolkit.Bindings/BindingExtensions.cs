﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Softeq.XToolkit.Common.Command;
using Softeq.XToolkit.Common.Interfaces;

namespace Softeq.XToolkit.Bindings
{
    /// <summary>
    ///     Defines extension methods used to add data bindings and commands between Xamarin
    ///     Android and iOS elements.
    /// </summary>
    public static class BindingExtensions
    {
        private static IBindingFactory _bindingFactory;

        public static void Initialize(IBindingFactory bindingFactory)
        {
            _bindingFactory = bindingFactory;
        }

        public static Binding Bind<T1, T2>(this object obj, Expression<Func<T1>> sourcePropertyExpression,
            Expression<Func<T2>> targetPropertyExpression, IConverter<T2, T1> converter)
        {
            return Bind(obj, sourcePropertyExpression, targetPropertyExpression, BindingMode.OneWay, converter);
        }

        public static Binding<T1, T1> Bind<T1>(this object obj, Expression<Func<T1>> sourcePropertyExpression,
            Action whenSourceChanges)
        {
            return obj.SetBinding(sourcePropertyExpression).WhenSourceChanges(whenSourceChanges);
        }

        public static Binding<T1, T1> Bind<T1>(this object obj, Expression<Func<T1>> sourcePropertyExpression,
            Func<T1, Task> whenSourceChanges)
        {
            return obj.SetBinding(sourcePropertyExpression).WhenSourceChanges(whenSourceChanges);
        }

        public static Binding<T1, T1> Bind<T1>(this object obj, Expression<Func<T1>> sourcePropertyExpression,
            Action<T1> action,
            BindingMode bindingMode = BindingMode.OneWay)
        {
            return obj.SetBinding(sourcePropertyExpression, bindingMode).WhenSourceChanges(action);
        }

        public static Binding Bind<T1, T2>(this object obj,
            Expression<Func<T1>> sourcePropertyExpression,
            Expression<Func<T2>> targetPropertyExpression = null,
            BindingMode mode = BindingMode.OneWay,
            IConverter<T2, T1> converter = null)
        {
            return obj.SetBinding(sourcePropertyExpression, targetPropertyExpression, mode)
                .SetConverter(converter);
        }

        /// <summary>
        ///     Sets a data binding between two properties. If the source implements INotifyPropertyChanged, the source property
        ///     raises the PropertyChanged event
        ///     and the BindingMode is OneWay or TwoWay, the target property will be synchronized with the source property. If
        ///     the target implements INotifyPropertyChanged, the target property raises the PropertyChanged event and the
        ///     BindingMode is
        ///     TwoWay, the source property will also be synchronized with the target property.
        /// </summary>
        /// <remarks>
        ///     This class allows for a different TSource and TTarget and is able to perform simple
        ///     type conversions automatically. This is useful if the source property and the target
        ///     property are of different type.
        ///     If the type conversion is complex, please use the <see cref="Binding{TSource, TTarget}.ConvertSourceToTarget" />
        ///     and <see cref="Binding{TSource, TTarget}.ConvertTargetToSource" /> methods to configure the binding.
        ///     It is very possible that TSource and TTarget are the same type in which case no conversion occurs.
        /// </remarks>
        /// <typeparam name="TSource">The type of the property that is being databound before conversion.</typeparam>
        /// <typeparam name="TTarget">The type of the property that is being databound after conversion.</typeparam>
        /// <param name="target">
        ///     The target of the binding. If this object implements INotifyPropertyChanged and the
        ///     BindingMode is TwoWay, the source will be notified of changes to the source property.
        /// </param>
        /// <param name="targetPropertyExpression">
        ///     An expression pointing to the target property. It can be
        ///     a simple expression "() => [target].MyProperty" or a composed expression "() =>
        ///     [target].SomeObject.SomeOtherObject.SomeProperty".
        /// </param>
        /// <param name="source">
        ///     The source of the binding. If this object implements INotifyPropertyChanged and the
        ///     BindingMode is OneWay or TwoWay, the target will be notified of changes to the target property.
        /// </param>
        /// <param name="sourcePropertyExpression">
        ///     An expression pointing to the source property. It can be
        ///     a simple expression "() => [source].MyProperty" or a composed expression "() =>
        ///     [source].SomeObject.SomeOtherObject.SomeProperty".
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
        ///     The value to use when the binding is unable to return a value. This can happen if one of the
        ///     items on the Path (except the source property itself) is null, or if the Converter throws an exception.
        /// </param>
        /// <param name="targetNullValue">The value used when the source property is null (or equals to default(TSource)).</param>
        /// <returns>The new Binding instance.</returns>
        public static Binding<TSource, TTarget> SetBinding<TSource, TTarget>(
            this object source,
            Expression<Func<TSource>> sourcePropertyExpression,
            object target,
            Expression<Func<TTarget>> targetPropertyExpression = null,
            BindingMode mode = BindingMode.Default,
            TSource fallbackValue = default(TSource),
            TSource targetNullValue = default(TSource))
        {
            return _bindingFactory.CreateBinding(
                source,
                sourcePropertyExpression,
                null,
                target,
                targetPropertyExpression,
                mode,
                fallbackValue,
                targetNullValue);
        }

        /// <summary>
        ///     Creates a <see cref="Binding{TSource, TSource}" /> with a source property but without a target.
        ///     This type of bindings is useful for the <see cref="SetCommand{T}(object, string, RelayCommand{T}, Binding)" />,
        ///     <see cref="SetCommand{T}(object, RelayCommand{T}, Binding)" />,
        ///     <see cref="SetCommand{T, TEventArgs}(object, string, RelayCommand{T}, Binding)" />
        ///     and <see cref="SetCommand{T, TEventArgs}(object, RelayCommand{T}, Binding)" /> methods, to use as CommandParameter
        ///     binding.
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
        ///     The value to use when the binding is unable to return a value. This can happen if one of the
        ///     items on the Path (except the source property itself) is null, or if the Converter throws an exception.
        /// </param>
        /// <param name="targetNullValue">The value used when the source property is null (or equals to default(TSource)).</param>
        /// <typeparam name="TSource">The type of the bound property.</typeparam>
        /// <returns>The created binding instance.</returns>
        public static Binding<TSource, TSource> SetBinding<TSource>(
            this object source,
            Expression<Func<TSource>> sourcePropertyExpression,
            BindingMode mode = BindingMode.Default,
            TSource fallbackValue = default(TSource),
            TSource targetNullValue = default(TSource))
        {
            return _bindingFactory.CreateBinding<TSource, TSource>(
                source,
                sourcePropertyExpression,
                true,
                null,
                null,
                mode,
                fallbackValue,
                targetNullValue);
        }

        /// <summary>
        ///     Sets a data binding between two properties of the same object. If the source implements INotifyPropertyChanged, has
        ///     observable properties
        ///     and the BindingMode is OneWay or TwoWay, the target property will be notified of changes to the source property. If
        ///     the target implements INotifyPropertyChanged, has observable properties and the BindingMode is
        ///     TwoWay, the source will also be notified of changes to the target's properties.
        /// </summary>
        /// <typeparam name="TSource">The type of the source property that is being databound.</typeparam>
        /// <typeparam name="TTarget">
        ///     The type of the target property that is being databound. If the source type
        ///     is not the same as the target type, an automatic conversion will be attempted. However only
        ///     simple types can be converted. For more complex conversions, use the
        ///     <see cref="Binding{TSource, TTarget}.ConvertSourceToTarget" />
        ///     and <see cref="Binding{TSource, TTarget}.ConvertTargetToSource" /> methods to define custom converters.
        /// </typeparam>
        /// <param name="targetPropertyExpression">
        ///     An expression pointing to the target property. It can be
        ///     a simple expression "() => [target].MyProperty" or a composed expression "() =>
        ///     [target].SomeObject.SomeOtherObject.SomeProperty".
        /// </param>
        /// <param name="source">
        ///     The source of the binding. If this object implements INotifyPropertyChanged and the
        ///     BindingMode is OneWay or TwoWay, the target will be notified of changes to the target property.
        /// </param>
        /// <param name="sourcePropertyExpression">
        ///     An expression pointing to the source property. It can be
        ///     a simple expression "() => [source].MyProperty" or a composed expression "() =>
        ///     [source].SomeObject.SomeOtherObject.SomeProperty".
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
        ///     The value to use when the binding is unable to return a value. This can happen if one of the
        ///     items on the Path (except the source property itself) is null, or if the Converter throws an exception.
        /// </param>
        /// <param name="targetNullValue">The value used when the source property is null (or equals to default(TSource)).</param>
        /// <returns>The new Binding instance.</returns>
        public static Binding<TSource, TTarget> SetBinding<TSource, TTarget>(
            this object source,
            Expression<Func<TSource>> sourcePropertyExpression,
            Expression<Func<TTarget>> targetPropertyExpression = null,
            BindingMode mode = BindingMode.Default,
            TSource fallbackValue = default(TSource),
            TSource targetNullValue = default(TSource))
        {
            return _bindingFactory.CreateBinding(
                source,
                sourcePropertyExpression,
                null,
                targetPropertyExpression,
                mode,
                fallbackValue,
                targetNullValue);
        }

        /// <summary>
        ///     Sets a data binding between two properties. If the source implements INotifyPropertyChanged, the source property
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
        ///     simple types can be converted. For more complex conversions, use the
        ///     <see cref="Binding{TSource, TTarget}.ConvertSourceToTarget" />
        ///     and <see cref="Binding{TSource, TTarget}.ConvertTargetToSource" /> methods to define custom converters.
        /// </typeparam>
        /// <param name="target">
        ///     The target of the binding. If this object implements INotifyPropertyChanged and the
        ///     BindingMode is TwoWay, the source will be notified of changes to the source property.
        /// </param>
        /// <param name="targetPropertyName">The name of the target property. This must be a simple name, without dots.</param>
        /// <param name="source">
        ///     The source of the binding. If this object implements INotifyPropertyChanged and the
        ///     BindingMode is OneWay or TwoWay, the target will be notified of changes to the target property.
        /// </param>
        /// <param name="sourcePropertyName">The name of the source property. This must be a simple name, without dots.</param>
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
        ///     The value to use when the binding is unable to return a value. This can happen if one of the
        ///     items on the Path (except the source property itself) is null, or if the Converter throws an exception.
        /// </param>
        /// <param name="targetNullValue">The value used when the source property is null (or equals to default(TSource)).</param>
        /// <returns>The new Binding instance.</returns>
        public static Binding<TSource, TTarget> SetBinding<TSource, TTarget>(
            this object source,
            string sourcePropertyName,
            object target,
            string targetPropertyName = null,
            BindingMode mode = BindingMode.Default,
            TSource fallbackValue = default(TSource),
            TSource targetNullValue = default(TSource))
        {
            return _bindingFactory.CreateBinding<TSource, TTarget>(
                source,
                sourcePropertyName,
                target,
                targetPropertyName,
                mode,
                fallbackValue,
                targetNullValue);
        }

        /// <summary>
        ///     Sets a data binding between two properties of the same object. If the source implements INotifyPropertyChanged, has
        ///     observable properties
        ///     and the BindingMode is OneWay or TwoWay, the target property will be notified of changes to the source property. If
        ///     the target implements INotifyPropertyChanged, has observable properties and the BindingMode is
        ///     TwoWay, the source will also be notified of changes to the target's properties.
        /// </summary>
        /// <typeparam name="TSource">The type of the source property that is being databound.</typeparam>
        /// <typeparam name="TTarget">
        ///     The type of the target property that is being databound. If the source type
        ///     is not the same as the target type, an automatic conversion will be attempted. However only
        ///     simple types can be converted. For more complex conversions, use the
        ///     <see cref="Binding{TSource, TTarget}.ConvertSourceToTarget" />
        ///     and <see cref="Binding{TSource, TTarget}.ConvertTargetToSource" /> methods to define custom converters.
        /// </typeparam>
        /// <param name="targetPropertyName">The name of the target property. This must be a simple name, without dots.</param>
        /// <param name="source">
        ///     The source of the binding. If this object implements INotifyPropertyChanged and the
        ///     BindingMode is OneWay or TwoWay, the target will be notified of changes to the target property.
        /// </param>
        /// <param name="sourcePropertyName">The name of the source property. This must be a simple name, without dots.</param>
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
        ///     The value to use when the binding is unable to return a value. This can happen if one of the
        ///     items on the Path (except the source property itself) is null, or if the Converter throws an exception.
        /// </param>
        /// <param name="targetNullValue">The value used when the source property is null (or equals to default(TSource)).</param>
        /// <returns>The new Binding instance.</returns>
        public static Binding<TSource, TTarget> SetBinding<TSource, TTarget>(
            this object source,
            string sourcePropertyName,
            string targetPropertyName = null,
            BindingMode mode = BindingMode.Default,
            TSource fallbackValue = default(TSource),
            TSource targetNullValue = default(TSource))
        {
            return _bindingFactory.CreateBinding<TSource, TTarget>(
                source,
                sourcePropertyName,
                null,
                targetPropertyName,
                mode,
                fallbackValue,
                targetNullValue);
        }

        /// <summary>
        ///     Sets a generic RelayCommand to an object and actuates the command when a specific event is raised. This method
        ///     can only be used when the event uses a standard EventHandler.
        /// </summary>
        /// <typeparam name="T">The type of the CommandParameter that will be passed to the RelayCommand.</typeparam>
        /// <param name="element">The element to which the command is added.</param>
        /// <param name="eventName">The name of the event that will be subscribed to to actuate the command.</param>
        /// <param name="command">The command that must be added to the element.</param>
        /// <param name="commandParameterBinding">
        ///     A <see cref="Binding{T, T}">Binding</see> instance subscribed to
        ///     the CommandParameter that will passed to the RelayCommand. Depending on the Binding, the CommandParameter
        ///     will be observed and changes will be passed to the command, for example to update the CanExecute.
        /// </param>
        public static void SetCommand<T>(
            this object element,
            string eventName,
            ICommand command,
            Binding commandParameterBinding)
        {
            var t = element.GetType();
            var e = t.GetEventInfoForControl(eventName);

            var castedBinding = (Binding<T, T>)commandParameterBinding;

            //var handler = e.GetCommandHandler(eventName, t, command, castedBinding);
            var handler = _bindingFactory.GetCommandHandler(e, eventName, t, command, castedBinding);
            e.AddEventHandler(
                element,
                handler);

            if (commandParameterBinding == null)
            {
                return;
            }

            var enabledProperty = t.GetRuntimeProperty("Enabled");
            if (enabledProperty != null)
            {
                enabledProperty.SetValue(element, command.CanExecute(castedBinding.Value));

                command.CanExecuteChanged += (s, args) => enabledProperty.SetValue(
                    element,
                    command.CanExecute(castedBinding.Value));

                commandParameterBinding.ValueChanged += (s, args) => enabledProperty.SetValue(
                    element,
                    command.CanExecute(castedBinding.Value));
            }
        }

        /// <summary>
        ///     Sets a generic RelayCommand to an object and actuates the command when a specific event is raised. This method
        ///     should be used when the event uses an EventHandler&lt;TEventArgs&gt;.
        /// </summary>
        /// <typeparam name="T">The type of the CommandParameter that will be passed to the RelayCommand.</typeparam>
        /// <typeparam name="TEventArgs">The type of the event's arguments.</typeparam>
        /// <param name="element">The element to which the command is added.</param>
        /// <param name="command">The command that must be added to the element.</param>
        /// <param name="eventName">The name of the event that will be subscribed to to actuate the command.</param>
        /// <param name="commandParameterBinding">
        ///     A <see cref="Binding&lt;TSource, TTarget&gt;">Binding</see> instance subscribed to
        ///     the CommandParameter that will passed to the RelayCommand. Depending on the Binding, the CommandParameter
        ///     will be observed and changes will be passed to the command, for example to update the CanExecute.
        /// </param>
        public static void SetCommand<T, TEventArgs>(
            this object element,
            string eventName,
            ICommand command,
            Binding commandParameterBinding)
        {
            var castedBinding = (Binding<T, T>)commandParameterBinding;

            var t = element.GetType();
            var e = t.GetEventInfoForControl(eventName);

            EventHandler<TEventArgs> handler = (s, args) =>
            {
                var param = castedBinding == null ? default(T) : castedBinding.Value;
                command.Execute(param);
            };

            e.AddEventHandler(
                element,
                handler);

            if (commandParameterBinding == null)
            {
                return;
            }

            var enabledProperty = t.GetRuntimeProperty("Enabled");
            if (enabledProperty != null)
            {
                enabledProperty.SetValue(element, command.CanExecute(castedBinding.Value));

                commandParameterBinding.ValueChanged += (s, args) => enabledProperty.SetValue(
                    element,
                    command.CanExecute(castedBinding.Value));

                command.CanExecuteChanged += (s, args) => enabledProperty.SetValue(
                    element,
                    command.CanExecute(castedBinding.Value));
            }
        }

        /// <summary>
        ///     Sets a non-generic RelayCommand to an object and actuates the command when a specific event is raised. This method
        ///     can only be used when the event uses a standard EventHandler.
        /// </summary>
        /// <param name="element">The element to which the command is added.</param>
        /// <param name="eventName">The name of the event that will be subscribed to to actuate the command.</param>
        /// <param name="command">The command that must be added to the element.</param>
        public static void SetCommand(
            this object element,
            string eventName,
            ICommand command)
        {
            var t = element.GetType();
            var e = t.GetEventInfoForControl(eventName);

            var handler = _bindingFactory.GetCommandHandler(e, eventName, t, command);

            e.AddEventHandler(
                element,
                handler);

            var enabledProperty = t.GetRuntimeProperty("Enabled");
            if (enabledProperty != null)
            {
                enabledProperty.SetValue(element, command.CanExecute(null));

                command.CanExecuteChanged += (s, args) => enabledProperty.SetValue(
                    element,
                    command.CanExecute(null));
            }
        }

        public static void SetCommand(this object element, string eventName, Action action)
        {
            SetCommand(element, eventName, new RelayCommand(action));
        }

        /// <summary>
        ///     Sets a non-generic RelayCommand to an object and actuates the command when a specific event is raised. This method
        ///     should be used when the event uses an EventHandler&lt;TEventArgs&gt;.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event's arguments.</typeparam>
        /// <param name="element">The element to which the command is added.</param>
        /// <param name="eventName">The name of the event that will be subscribed to to actuate the command.</param>
        /// <param name="command">The command that must be added to the element.</param>
        public static void SetCommand<TEventArgs>(
            this object element,
            string eventName,
            ICommand command)
        {
            var t = element.GetType();
            var e = t.GetEventInfoForControl(eventName);

            EventHandler<TEventArgs> handler = (s, args) =>
            {
                if (command.CanExecute(null))
                {
                    command.Execute(null);
                }
            };

            e.AddEventHandler(
                element,
                handler);

            var enabledProperty = t.GetRuntimeProperty("Enabled");
            if (enabledProperty != null)
            {
                enabledProperty.SetValue(element, command.CanExecute(null));

                command.CanExecuteChanged += (s, args) => enabledProperty.SetValue(
                    element,
                    command.CanExecute(null));
            }
        }

        /// <summary>
        ///     Sets a generic RelayCommand to an object and actuates the command when a specific event is raised. This method
        ///     can only be used when the event uses a standard EventHandler.
        /// </summary>
        /// <typeparam name="T">The type of the CommandParameter that will be passed to the RelayCommand.</typeparam>
        /// <param name="element">The element to which the command is added.</param>
        /// <param name="command">The command that must be added to the element.</param>
        /// <param name="eventName">The name of the event that will be subscribed to to actuate the command.</param>
        /// <param name="commandParameter">
        ///     The command parameter that will be passed to the RelayCommand when it
        ///     is executed. This is a fixed value. To pass an observable value, use one of the SetCommand
        ///     overloads that uses a Binding as CommandParameter.
        /// </param>
        public static void SetCommand<T>(
            this object element,
            string eventName,
            ICommand command,
            T commandParameter)
        {
            var t = element.GetType();
            var e = t.GetEventInfoForControl(eventName);

            var handler = _bindingFactory.GetCommandHandler(e, eventName, t, command, commandParameter);

            e.AddEventHandler(
                element,
                handler);

            var enabledProperty = t.GetRuntimeProperty("Enabled");
            if (enabledProperty != null)
            {
                enabledProperty.SetValue(element, command.CanExecute(commandParameter));

                command.CanExecuteChanged += (s, args) => enabledProperty.SetValue(
                    element,
                    command.CanExecute(commandParameter));
            }
        }

        /// <summary>
        ///     Sets a generic RelayCommand to an object and actuates the command when a specific event is raised. This method
        ///     should be used when the event uses an EventHandler&lt;TEventArgs&gt;.
        /// </summary>
        /// <typeparam name="T">The type of the CommandParameter that will be passed to the RelayCommand.</typeparam>
        /// <typeparam name="TEventArgs">The type of the event's arguments.</typeparam>
        /// <param name="element">The element to which the command is added.</param>
        /// <param name="command">The command that must be added to the element.</param>
        /// <param name="eventName">The name of the event that will be subscribed to to actuate the command.</param>
        /// <param name="commandParameter">
        ///     The command parameter that will be passed to the RelayCommand when it
        ///     is executed. This is a fixed value. To pass an observable value, use one of the SetCommand
        ///     overloads that uses a Binding as CommandParameter.
        /// </param>
        public static void SetCommand<T, TEventArgs>(
            this object element,
            string eventName,
            ICommand command,
            T commandParameter)
        {
            var t = element.GetType();
            var e = t.GetEventInfoForControl(eventName);

            EventHandler<TEventArgs> handler = (s, args) => command.Execute(commandParameter);

            e.AddEventHandler(
                element,
                handler);

            var enabledProperty = t.GetRuntimeProperty("Enabled");
            if (enabledProperty != null)
            {
                enabledProperty.SetValue(element, command.CanExecute(commandParameter));

                command.CanExecuteChanged += (s, args) => enabledProperty.SetValue(
                    element,
                    command.CanExecute(commandParameter));
            }
        }

        public static void SetCommand<T>(
            this object element,
            ICommand command,
            Binding commandParameterBinding)
        {
            SetCommand(element, string.Empty, command, commandParameterBinding);
        }

        public static void SetCommand<T, TEventArgs>(
            this object element,
            ICommand command,
            Binding commandParameterBinding)
        {
            SetCommand<T, TEventArgs>(element, string.Empty, command, commandParameterBinding);
        }

        public static void SetCommand(
            this object element,
            ICommand command)
        {
            SetCommand(element, string.Empty, command);
        }

        public static void SetCommand<TEventArgs>(
            this object element,
            ICommand command)
        {
            SetCommand<TEventArgs>(element, string.Empty, command);
        }

        public static void SetCommand<T>(
            this object element,
            ICommand command,
            T commandParameter)
        {
            SetCommand(element, string.Empty, command, commandParameter);
        }

        public static void SetCommand<T, TEventArgs>(
            this object element,
            ICommand command,
            T commandParameter)
        {
            SetCommand<T, TEventArgs>(element, string.Empty, command, commandParameter);
        }

        public static void SetCommandWithArgs<T>(
            this object element,
            string eventName,
            ICommand<T> command)
        {
            var t = element.GetType();
            var e = t.GetEventInfoForControl(eventName);

            var handler = _bindingFactory.GetCommandHandlerWithArgs(e, eventName, t, command);

            e.AddEventHandler(
                element,
                handler);
        }

        internal static EventInfo GetEventInfoForControl(this Type type, string eventName)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                eventName = _bindingFactory.GetDefaultEventNameForControl(type);
            }

            if (string.IsNullOrEmpty(eventName))
            {
                throw new ArgumentException("Event not found", "eventName");
            }

            var info = type.GetRuntimeEvent(eventName);

            if (info == null)
            {
                throw new ArgumentException("Event not found: " + eventName, "eventName");
            }

            return info;
        }
    }
}
