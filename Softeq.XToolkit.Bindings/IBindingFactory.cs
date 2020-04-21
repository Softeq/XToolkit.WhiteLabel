// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Input;

namespace Softeq.XToolkit.Bindings
{
    public interface IBindingFactory
    {
        /// <summary>
        ///     Creates a new instance of the <see cref="Binding{TSource,TTarget}"/> class
        ///     for which the source and target properties are located in different objects.
        /// </summary>
        /// <param name="source">
        ///     The source of the binding. If this object implements <see cref="T:System.ComponentModel.INotifyPropertyChanged"/> and the
        ///     <see cref="BindingMode"/> is OneWay or TwoWay, the target will be notified of changes to the target property.
        /// </param>
        /// <param name="sourcePropertyExpression">The expression of the source property for the binding.</param>
        /// <param name="resolveTopField">Flag for resolve top field.</param>
        /// <param name="target">
        ///     The target of the binding. If this object implements <see cref="T:System.ComponentModel.INotifyPropertyChanged"/> and the
        ///     <see cref="BindingMode"/> is TwoWay, the source will be notified of changes to the source property.
        /// </param>
        /// <param name="targetPropertyExpression">The expression of the target property for the binding.</param>
        /// <param name="mode">The mode of the binding.</param>
        /// <param name="fallbackValue">
        ///     The value to use when the binding is unable to return a value. This can happen if one of the
        ///     items on the Path (except the source property itself) is null, or if the Converter throws an exception.
        /// </param>
        /// <param name="targetNullValue">
        ///     The value to use when the binding is unable to return a value.
        /// </param>
        /// <typeparam name="TSource">The type of the source property that is being databound.</typeparam>
        /// <typeparam name="TTarget">
        ///     The type of the target property that is being databound.
        ///     If the source type is not the same as the target type, an automatic conversion will be attempted.
        ///     However only simple types can be converted. For more complex conversions,
        ///     use the <see cref="Binding{TSource,TTarget}.ConvertSourceToTarget" />
        ///     and <see cref="Binding{TSource,TTarget}.ConvertTargetToSource" /> methods to define custom converters.
        /// </typeparam>
        /// <returns>New Binding.</returns>
        Binding<TSource, TTarget> CreateBinding<TSource, TTarget>(
            object source,
            Expression<Func<TSource>> sourcePropertyExpression,
            bool? resolveTopField,
            object? target = null,
            Expression<Func<TTarget>>? targetPropertyExpression = null,
            BindingMode mode = BindingMode.Default,
            TSource fallbackValue = default,
            TSource targetNullValue = default);

        /// <summary>
        ///     Creates a new instance of the <see cref="Binding{TSource,TTarget}"/> class
        ///     for which the source and target properties are located in different objects.
        /// </summary>
        /// <param name="source">
        ///     The source of the binding. If this object implements <see cref="T:System.ComponentModel.INotifyPropertyChanged"/> and the
        ///     <see cref="BindingMode"/> is OneWay or TwoWay, the target will be notified of changes to the target property.
        /// </param>
        /// <param name="sourcePropertyExpression">The expression of the source property for the binding.</param>
        /// <param name="target">
        ///     The target of the binding. If this object implements <see cref="T:System.ComponentModel.INotifyPropertyChanged"/> and the
        ///     <see cref="BindingMode"/> is TwoWay, the source will be notified of changes to the source property.
        /// </param>
        /// <param name="targetPropertyExpression">The expression of the target property for the binding.</param>
        /// <param name="mode">The mode of the binding.</param>
        /// <param name="fallbackValue">
        ///     The value to use when the binding is unable to return a value. This can happen if one of the
        ///     items on the Path (except the source property itself) is null, or if the Converter throws an exception.
        /// </param>
        /// <param name="targetNullValue">
        ///     The value to use when the binding is unable to return a value.
        /// </param>
        /// <typeparam name="TSource">The type of the source property that is being databound.</typeparam>
        /// <typeparam name="TTarget">
        ///     The type of the target property that is being databound.
        ///     If the source type is not the same as the target type, an automatic conversion will be attempted.
        ///     However only simple types can be converted. For more complex conversions,
        ///     use the <see cref="Binding{TSource,TTarget}.ConvertSourceToTarget" />
        ///     and <see cref="Binding{TSource,TTarget}.ConvertTargetToSource" /> methods to define custom converters.
        /// </typeparam>
        /// <returns>New Binding.</returns>
        Binding<TSource, TTarget> CreateBinding<TSource, TTarget>(
            object source,
            Expression<Func<TSource>> sourcePropertyExpression,
            object? target = null,
            Expression<Func<TTarget>>? targetPropertyExpression = null,
            BindingMode mode = BindingMode.Default,
            TSource fallbackValue = default,
            TSource targetNullValue = default);

        /// <summary>
        ///     Creates a new instance of the <see cref="Binding{TSource,TTarget}"/> class
        ///     for which the source and target properties are located in different objects.
        /// </summary>
        /// <param name="source">
        ///     The source of the binding. If this object implements <see cref="T:System.ComponentModel.INotifyPropertyChanged"/> and the
        ///     <see cref="BindingMode"/> is OneWay or TwoWay, the target will be notified of changes to the target property.
        /// </param>
        /// <param name="sourcePropertyName">The name of the source property for the binding.</param>
        /// <param name="target">
        ///     The target of the binding. If this object implements <see cref="T:System.ComponentModel.INotifyPropertyChanged"/> and the
        ///     <see cref="BindingMode"/> is TwoWay, the source will be notified of changes to the source property.
        /// </param>
        /// <param name="targetPropertyName">The name of the target property for the binding.</param>
        /// <param name="mode">The mode of the binding.</param>
        /// <param name="fallbackValue">
        ///     The value to use when the binding is unable to return a value. This can happen if one of the
        ///     items on the Path (except the source property itself) is null, or if the Converter throws an exception.
        /// </param>
        /// <param name="targetNullValue">
        ///     The value to use when the binding is unable to return a value.
        /// </param>
        /// <typeparam name="TSource">The type of the source property that is being databound.</typeparam>
        /// <typeparam name="TTarget">
        ///     The type of the target property that is being databound.
        ///     If the source type is not the same as the target type, an automatic conversion will be attempted.
        ///     However only simple types can be converted. For more complex conversions,
        ///     use the <see cref="Binding{TSource,TTarget}.ConvertSourceToTarget" />
        ///     and <see cref="Binding{TSource,TTarget}.ConvertTargetToSource" /> methods to define custom converters.
        /// </typeparam>
        /// <returns>New Binding.</returns>
        Binding<TSource, TTarget> CreateBinding<TSource, TTarget>(
            object source,
            string sourcePropertyName,
            object? target = null,
            string? targetPropertyName = null,
            BindingMode mode = BindingMode.Default,
            TSource fallbackValue = default,
            TSource targetNullValue = default);

        /// <summary>
        ///    Creates a new instance of the <see cref="T:System.EventHandler"/> that will wrap the command.
        /// </summary>
        /// <param name="info">An <see cref="T:System.Reflection.EventInfo"/> object that describe the event.</param>
        /// <param name="eventName">Name of event.</param>
        /// <param name="elementType">Type of event.</param>
        /// <param name="command">An <see cref="T:System.Windows.Input.ICommand"/> instance that will be wrapped in the event handler.</param>
        /// <param name="commandParameter">Optional parameter that will be used for the command.</param>
        /// <returns>New delegate.</returns>
        Delegate GetCommandHandler(
            EventInfo info,
            string eventName,
            Type elementType,
            ICommand command,
            object? commandParameter = null);

        /// <summary>
        ///    Creates a new instance of the <see cref="T:System.EventHandler"/> that will wrap the command.
        /// </summary>
        /// <param name="info">An <see cref="T:System.Reflection.EventInfo"/> object that describe the event.</param>
        /// <param name="eventName">Name of event.</param>
        /// <param name="elementType">Type of event.</param>
        /// <param name="command">An <see cref="T:System.Windows.Input.ICommand"/> instance that will be wrapped in the event handler.</param>
        /// <param name="castedBinding">
        ///     A <see cref="Binding{TSource, TTarget}" /> instance subscribed to the CommandParameter
        ///     that will passed to the <see cref="T:System.Windows.Input.ICommand"/>. Depending on the Binding, the CommandParameter will be observed
        ///     and changes will be passed to the command, for example to update the CanExecute.
        /// </param>
        /// <typeparam name="T">Type of command parameter.</typeparam>
        /// <returns>New delegate.</returns>
        Delegate GetCommandHandler<T>(
            EventInfo info,
            string eventName,
            Type elementType,
            ICommand command,
            Binding<T, T> castedBinding);

        /// <summary>
        ///    Creates a new instance of the <see cref="T:System.EventHandler`1"/> that will wrap the command.
        /// </summary>
        /// <param name="info">An <see cref="T:System.Reflection.EventInfo"/> object that describe the event.</param>
        /// <param name="eventName">Name of event.</param>
        /// <param name="elementType">Type of event.</param>
        /// <param name="command">
        ///     An <see cref="T:System.Windows.Input.ICommand"/> instance that will be wrapped in the event handler.
        ///     Object of type <typeparamref name="TEventArgs"/> will be passed to command as parameter.
        /// </param>
        /// <typeparam name="TEventArgs">Type of EventArgs.</typeparam>
        /// <returns>New delegate.</returns>
        Delegate GetCommandHandler<TEventArgs>(
            EventInfo info,
            string eventName,
            Type elementType,
            ICommand command);

        /// <summary>
        ///    Creates a new instance of the <see cref="T:System.EventHandler`1"/> that will wrap the command.
        /// </summary>
        /// <param name="info">An <see cref="T:System.Reflection.EventInfo"/> object that describe the event.</param>
        /// <param name="eventName">Name of event.</param>
        /// <param name="elementType">Type of event.</param>
        /// <param name="command">An <see cref="T:System.Windows.Input.ICommand"/> instance that will be wrapped in the event handler.</param>
        /// <param name="commandParameter">Optional parameter that will be used for the command.</param>
        /// <typeparam name="T">Type of command parameter.</typeparam>
        /// <typeparam name="TEventArgs">Type of EventArgs.</typeparam>
        /// <returns>New delegate.</returns>
        Delegate GetCommandHandler<T, TEventArgs>(
            EventInfo info,
            string eventName,
            Type elementType,
            ICommand command,
            T commandParameter);

        /// <summary>
        ///    Creates a new instance of the <see cref="T:System.EventHandler`1"/> that will wrap the command.
        /// </summary>
        /// <param name="info">An <see cref="T:System.Reflection.EventInfo"/> object that describe the event.</param>
        /// <param name="eventName">Name of event.</param>
        /// <param name="elementType">Type of event.</param>
        /// <param name="command">An <see cref="T:System.Windows.Input.ICommand"/> instance that will be wrapped in the event handler.</param>
        /// <param name="castedBinding">
        ///     A <see cref="Binding{TSource, TTarget}" /> instance subscribed to the CommandParameter
        ///     that will passed to the <see cref="T:System.Windows.Input.ICommand"/>. Depending on the Binding, the CommandParameter will be observed
        ///     and changes will be passed to the command, for example to update the CanExecute.
        /// </param>
        /// <typeparam name="T">Type of command parameter.</typeparam>
        /// <typeparam name="TEventArgs">Type of EventArgs.</typeparam>
        /// <returns>New delegate.</returns>
        Delegate GetCommandHandler<T, TEventArgs>(
            EventInfo info,
            string eventName,
            Type elementType,
            ICommand command,
            Binding<T, T> castedBinding);

        string GetDefaultEventNameForControl(Type type);
    }
}
