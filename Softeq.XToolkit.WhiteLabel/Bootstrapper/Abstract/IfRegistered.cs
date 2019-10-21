// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract
{
    public enum IfRegistered
    {
        /// <summary>
        ///     Keeps old default or keyed registration ignoring new registration: ensures Register-Once semantics.
        /// </summary>
        Keep,

        /// <summary>
        ///     Replaces old registration with new one.
        /// </summary>
        Replace,

        /// <summary>
        ///     Adds the new implementation or null (Made.Of),
        ///     otherwise keeps the previous registration of the same implementation type.
        /// </summary>
        AppendNewImplementation
    }
}
