// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.Bindings.Abstract
{
    /// <summary>
    ///     Contract for each Bindable View that provides a mechanism for control DataContext.
    /// </summary>
    public interface IBindable : IBindingsOwner, IBindingsLifecycle
    {
        /// <summary>
        ///     Data context for an element when it participates in data binding.
        /// </summary>
        object DataContext { get; set; } // TODO YP: public set only for support current projects
    }
}
