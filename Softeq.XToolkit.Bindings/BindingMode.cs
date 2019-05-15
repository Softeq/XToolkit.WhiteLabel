// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.Bindings
{
    /// <summary>
    ///     The mode of the <see cref="Binding{TSource,TTarget}" />.
    /// </summary>
    public enum BindingMode
    {
        /// <summary>
        ///     A default binding is a one way binding.
        /// </summary>
        Default = 0,

        /// <summary>
        ///     A one time binding. The binding's value will be set when the
        ///     binding is created but subsequent changes will be ignored/
        /// </summary>
        OneTime = 1,

        /// <summary>
        ///     A one way binding, where the changes to the source
        ///     property will update the target property, but changes to the
        ///     target property don't affect the source property.
        /// </summary>
        OneWay = 2,

        /// <summary>
        ///     A two way binding, where the changes to the source
        ///     property will update the target property, and vice versa.
        /// </summary>
        TwoWay = 3
    }
}