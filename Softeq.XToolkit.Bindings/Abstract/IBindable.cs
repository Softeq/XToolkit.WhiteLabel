// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.Bindings.Abstract
{
    /// <summary>
    ///     A contract for each Bindable object that contains DataContext and set of bindings.
    /// </summary>
    public interface IBindable : IBindingsOwner
    {
        /// <summary>
        ///     Gets data context for an element when it participates in data binding.
        /// </summary>
        object DataContext { get; }

        void SetDataContext(object dataContext);
    }
}
