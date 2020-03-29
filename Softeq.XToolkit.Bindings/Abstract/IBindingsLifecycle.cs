// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.Bindings.Abstract
{
    /// <summary>
    ///     Contract of bindings lifecycle.
    /// </summary>
    public interface IBindingsLifecycle
    {
        /// <summary>
        ///     Occurs when bindings can be set.
        /// </summary>
        void DoAttachBindings();

        /// <summary>
        ///     Occurs when bindings can be detached.
        /// </summary>
        void DoDetachBindings();
    }
}
