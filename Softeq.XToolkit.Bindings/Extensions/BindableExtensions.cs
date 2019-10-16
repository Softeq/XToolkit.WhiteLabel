// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq.Expressions;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Common.Converters;

namespace Softeq.XToolkit.Bindings.Extensions
{
    public static class BindableExtensions
    {
        /// <summary>
        ///     Sets a data binding between two properties of the same object. If the source implements INotifyPropertyChanged,
        ///     has observable properties and the BindingMode is OneWay or TwoWay, the target property will be notified of changes
        ///     to the source property. If the target implements INotifyPropertyChanged, has observable properties and
        ///     the BindingMode is TwoWay, the source will also be notified of changes to the target's properties.
        /// </summary>
        /// <typeparam name="TSource">The type of the source property that is being databound.</typeparam>
        /// <typeparam name="TTarget">
        ///     The type of the target property that is being databound. If the source type
        ///     is not the same as the target type, an automatic conversion will be attempted. However only
        ///     simple types can be converted. For more complex conversions, use the
        ///     <see cref="Binding{TSource, TTarget}.ConvertSourceToTarget" />
        ///     and <see cref="Binding{TSource, TTarget}.ConvertTargetToSource" /> methods to define custom converters.
        /// </typeparam>
        /// <param name="source">
        ///     The source of the binding. If this object implements INotifyPropertyChanged and the
        ///     BindingMode is OneWay or TwoWay, the target will be notified of changes to the target property.
        /// </param>
        /// <param name="sourcePropertyExpression">
        ///     An expression pointing to the source property. It can be
        ///     a simple expression "() => [source].MyProperty" or a composed expression "() =>
        ///     [source].SomeObject.SomeOtherObject.SomeProperty".
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
        ///     source property will also be updated if the target raises the PropertyChanged event.
        ///     Default means OneWay if only the source implements INPC, and TwoWay if both the source and the target implement INPC.
        /// </param>
        /// <returns>The new Binding instance.</returns>
        public static Binding Bind<TSource, TTarget>(this IBindingsOwner source,
            Expression<Func<TSource>> sourcePropertyExpression,
            Expression<Func<TTarget>> targetPropertyExpression,
            BindingMode mode = BindingMode.Default)
        {
            var binding = source.SetBinding(sourcePropertyExpression, targetPropertyExpression, mode);

            SetBindingTo(source, binding);

            return binding;
        }

        /// <summary>
        ///     Sets a data binding between two properties of the same object. If the source implements INotifyPropertyChanged,
        ///     has observable properties and the BindingMode is OneWay or TwoWay, the target property will be notified of changes
        ///     to the source property. If the target implements INotifyPropertyChanged, has observable properties and
        ///     the BindingMode is TwoWay, the source will also be notified of changes to the target's properties.
        ///
        ///     The method provides the ability for advanced configuration of internal
        ///     <see cref="Binding{TSource, TTarget}" /> object.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="source">
        ///     The source of the binding. If this object implements INotifyPropertyChanged and the
        ///     BindingMode is OneWay or TwoWay, the target will be notified of changes to the target property.
        /// </param>
        /// <param name="sourcePropertyExpression">
        ///     An expression pointing to the source property. It can be
        ///     a simple expression "() => [source].MyProperty" or a composed expression "() =>
        ///     [source].SomeObject.SomeOtherObject.SomeProperty".
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
        ///     source property will also be updated if the target raises the PropertyChanged event.
        ///     Default means OneWay if only the source implements INPC, and TwoWay if both the source and the target implement INPC.
        /// </param>
        /// <param name="configure">
        ///     Callback for advanced configuration of internal <see cref="Binding{TSource, TTarget}" /> object.
        ///     Example of using:
        ///     "binding => binding.ObserveTargetEvent[UISearchBarTextChangedEventArgs](nameof(SearchBar.TextChanged))"
        /// </param>
        /// <returns>The new Binding instance.</returns>
        public static Binding Bind<TSource, TTarget>(this IBindingsOwner source,
            Expression<Func<TSource>> sourcePropertyExpression,
            Expression<Func<TTarget>> targetPropertyExpression,
            BindingMode mode,
            Func<Binding<TSource, TTarget>, Binding> configure)
        {
            var binding = source.SetBinding(sourcePropertyExpression, targetPropertyExpression, mode);

            var resultBinding = configure(binding);

            SetBindingTo(source, resultBinding);

            return resultBinding;
        }

        /// <summary>
        ///     Sets a data binding between two properties of the same object. If the source implements INotifyPropertyChanged,
        ///     has observable properties and the BindingMode is OneWay or TwoWay, the target property will be notified of changes
        ///     to the source property. If the target implements INotifyPropertyChanged, has observable properties and
        ///     the BindingMode is TwoWay, the source will also be notified of changes to the target's properties.
        /// </summary>
        /// <typeparam name="TSource">The type of the source property that is being databound.</typeparam>
        /// <typeparam name="TTarget">
        ///     The type of the target property that is being databound. If the source type
        ///     is not the same as the target type, an automatic conversion will be attempted. However only
        ///     simple types can be converted. For more complex conversions, use the
        ///     <see cref="Binding{TSource, TTarget}.ConvertSourceToTarget" />
        ///     and <see cref="Binding{TSource, TTarget}.ConvertTargetToSource" /> methods to define custom converters.
        /// </typeparam>
        /// <param name="source">
        ///     The source of the binding. If this object implements INotifyPropertyChanged and the
        ///     BindingMode is OneWay or TwoWay, the target will be notified of changes to the target property.
        /// </param>
        /// <param name="sourcePropertyExpression">
        ///     An expression pointing to the source property. It can be
        ///     a simple expression "() => [source].MyProperty" or a composed expression "() =>
        ///     [source].SomeObject.SomeOtherObject.SomeProperty".
        /// </param>
        /// <param name="targetPropertyExpression">
        ///     An expression pointing to the target property. It can be
        ///     a simple expression "() => [target].MyProperty" or a composed expression "() =>
        ///     [target].SomeObject.SomeOtherObject.SomeProperty".
        /// </param>
        /// <param name="converter">
        ///     The instance of the converter.
        /// </param>
        /// <returns>The new Binding instance.</returns>
        public static Binding Bind<TSource, TTarget>(this IBindingsOwner source,
            Expression<Func<TSource>> sourcePropertyExpression,
            Expression<Func<TTarget>> targetPropertyExpression,
            IConverter<TTarget, TSource> converter)
        {
            var binding = source
                .SetBinding(sourcePropertyExpression, targetPropertyExpression)
                .SetConverter(converter);

            SetBindingTo(source, binding);

            return binding;
        }

        /// <summary>
        ///     Sets a data binding between two properties of the same object. If the source implements INotifyPropertyChanged,
        ///     has observable properties and the BindingMode is OneWay or TwoWay, the target property will be notified of changes
        ///     to the source property. If the target implements INotifyPropertyChanged, has observable properties and
        ///     the BindingMode is TwoWay, the source will also be notified of changes to the target's properties.
        /// </summary>
        /// <typeparam name="TSource">The type of the source property that is being databound.</typeparam>
        /// <typeparam name="TTarget">
        ///     The type of the target property that is being databound. If the source type
        ///     is not the same as the target type, an automatic conversion will be attempted. However only
        ///     simple types can be converted. For more complex conversions, use the
        ///     <see cref="Binding{TSource, TTarget}.ConvertSourceToTarget" />
        ///     and <see cref="Binding{TSource, TTarget}.ConvertTargetToSource" /> methods to define custom converters.
        /// </typeparam>
        /// <param name="source">
        ///     The source of the binding. If this object implements INotifyPropertyChanged and the
        ///     BindingMode is OneWay or TwoWay, the target will be notified of changes to the target property.
        /// </param>
        /// <param name="sourcePropertyExpression">
        ///     An expression pointing to the source property. It can be
        ///     a simple expression "() => [source].MyProperty" or a composed expression "() =>
        ///     [source].SomeObject.SomeOtherObject.SomeProperty".
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
        ///     source property will also be updated if the target raises the PropertyChanged event. Default means OneWay if only
        ///     the source implements INPC, and TwoWay if both the source and the target implement INPC.
        /// </param>
        /// <param name="converter">
        ///     The instance of the converter.
        /// </param>
        /// <returns>The new Binding instance.</returns>
        public static Binding Bind<TSource, TTarget>(this IBindingsOwner source,
            Expression<Func<TSource>> sourcePropertyExpression,
            Expression<Func<TTarget>> targetPropertyExpression,
            BindingMode mode,
            IConverter<TTarget, TSource> converter)
        {
            var binding = source
                .SetBinding(sourcePropertyExpression, targetPropertyExpression, mode)
                .SetConverter(converter);

            SetBindingTo(source, binding);

            return binding;
        }

        /// <summary>
        ///     Creates a <see cref="Binding{TSource, TSource}" /> with a source property but without a target.
        ///     This type of bindings is useful for the <see cref="T:SetCommand{T}(object, string, RelayCommand{T}, Binding)" />,
        ///     <see cref="T:SetCommand{T}(object, RelayCommand{T}, Binding)" />,
        ///     <see cref="T:SetCommand{T, TEventArgs}(object, string, RelayCommand{T}, Binding)" />
        ///     and <see cref="T:SetCommand{T, TEventArgs}(object, RelayCommand{T}, Binding)" /> methods, to use as
        ///     CommandParameter binding.
        /// </summary>
        /// <typeparam name="TSource">The type of the bound property.</typeparam>
        /// <param name="source">
        ///     The source of the binding. If this object implements INotifyPropertyChanged and the
        ///     BindingMode is OneWay or TwoWay, the target will be notified of changes to the target property.
        /// </param>
        /// <param name="sourcePropertyExpression">
        ///     An expression pointing to the source property. It can be
        ///     a simple expression "() => [source].MyProperty" or a composed expression "() =>
        ///     [source].SomeObject.SomeOtherObject.SomeProperty".
        /// </param>
        /// <param name="action">
        ///     The action that will be executed when the binding changes.
        ///     IMPORTANT: Note that closures are not supported at the moment
        ///     due to the use of WeakActions (see http://stackoverflow.com/questions/25730530/).
        /// </param>
        /// <returns>The created binding instance.</returns>
        public static Binding Bind<TSource>(this IBindingsOwner source,
            Expression<Func<TSource>> sourcePropertyExpression,
            Action<TSource> action)
        {
            var binding = source.SetBinding(sourcePropertyExpression).WhenSourceChanges(action);

            SetBindingTo(source, binding);

            return binding;
        }

        /// <summary>
        ///     Creates a <see cref="Binding{TSource, TSource}" /> with a source property but without a target.
        ///     This type of bindings is useful for the <see cref="T:SetCommand{T}(object, string, RelayCommand{T}, Binding)" />,
        ///     <see cref="T:SetCommand{T}(object, RelayCommand{T}, Binding)" />,
        ///     <see cref="T:SetCommand{T, TEventArgs}(object, string, RelayCommand{T}, Binding)" />
        ///     and <see cref="T:SetCommand{T, TEventArgs}(object, RelayCommand{T}, Binding)" /> methods,
        ///     to use as CommandParameter binding.
        /// </summary>
        /// <typeparam name="TSource">The type of the bound property.</typeparam>
        /// <param name="source">
        ///     The source of the binding. If this object implements INotifyPropertyChanged and the
        ///     BindingMode is OneWay or TwoWay, the target will be notified of changes to the target property.
        /// </param>
        /// <param name="sourcePropertyExpression">
        ///     An expression pointing to the source property. It can be
        ///     a simple expression "() => [source].MyProperty" or a composed expression "() =>
        ///     [source].SomeObject.SomeOtherObject.SomeProperty".
        /// </param>
        /// <param name="action">
        ///     The action that will be executed when the binding changes.
        ///     IMPORTANT: Note that closures are not supported at the moment
        ///     due to the use of WeakActions (see http://stackoverflow.com/questions/25730530/).
        /// </param>
        /// <param name="mode">
        ///     The mode of the binding. OneTime means that the target property will be set once (when the binding is
        ///     created) but that subsequent changes will be ignored. OneWay means that the target property will be set, and
        ///     if the PropertyChanged event is raised by the source, the target property will be updated. TwoWay means that the
        ///     source property will also be updated if the target raises the PropertyChanged event. Default means OneWay if only
        ///     the source implements INPC, and TwoWay if both the source and the target implement INPC.
        /// </param>
        /// <returns>The created binding instance.</returns>
        public static Binding Bind<TSource>(this IBindingsOwner source,
            Expression<Func<TSource>> sourcePropertyExpression,
            Action<TSource> action,
            BindingMode mode)
        {
            var binding = source.SetBinding(sourcePropertyExpression, mode).WhenSourceChanges(action);

            SetBindingTo(source, binding);

            return binding;
        }

        /// <summary>
        ///     Detach all bindings of the source.
        /// </summary>
        /// <param name="source">
        ///     The source of the bindings.
        /// </param>
        public static void DetachBindings(this IBindingsOwner source)
        {
            source.Bindings?.DetachAllAndClear();
        }

        /// <summary>
        ///     Set DataContext with reloads existing bindings.
        /// </summary>
        /// <param name="self">Bindable View.</param>
        /// <param name="dataContext">DataContext.</param>
        public static void ReloadDataContext(this IBindableView self, object dataContext)
        {
            self.DoDetachBindings();

            self.SetDataContext(dataContext);

            self.DoAttachBindings();
        }

        private static void SetBindingTo(IBindingsOwner bindableOwner, Binding binding)
        {
            if (bindableOwner.Bindings == null)
            {
                throw new ArgumentNullException(
                    $"{bindableOwner.GetType()}.{nameof(bindableOwner.Bindings)}",
                    "List of Bindings can't be null. Please declare on the top level.");
            }

            bindableOwner.Bindings.Add(binding);
        }
    }
}
