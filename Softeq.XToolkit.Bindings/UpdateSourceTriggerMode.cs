// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.Bindings
{
    /// <summary>
    ///     Defines how a <see cref="Binding" /> is updated by a source control.
    /// </summary>
    public enum UpdateTriggerMode
    {
        /// <summary>
        ///     Defines that the binding should be updated when the control
        ///     loses the focus.
        /// </summary>
        LostFocus,

        /// <summary>
        ///     Defines that the binding should be updated when the control's
        ///     bound property changes.
        /// </summary>
        PropertyChanged
    }
}