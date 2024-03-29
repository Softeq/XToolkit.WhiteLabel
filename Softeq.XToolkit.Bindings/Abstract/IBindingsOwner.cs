// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;

namespace Softeq.XToolkit.Bindings.Abstract
{
    /// <summary>
    ///     Common contract for everyone who wants to use bindings.
    /// </summary>
    public interface IBindingsOwner
    {
        /// <summary>
        ///     Gets list where keeping bindings.
        /// </summary>
        List<Binding> Bindings { get; }
    }
}
