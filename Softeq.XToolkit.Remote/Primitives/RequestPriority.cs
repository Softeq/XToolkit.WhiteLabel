using System;

namespace Softeq.XToolkit.Remote.Primitives
{
    /// <summary>
    /// This enumeration defines the default base priorities associated with the different HttpMessageHandler instances.
    /// </summary>
    [Obsolete]
    public enum RequestPriority
    {
        /// <summary>
        /// A speculative priority where we aren't sure.
        /// </summary>
        Speculative = 10,

        /// <summary>
        /// This is a instance which is initiated by the user.
        /// </summary>
        UserInitiated = 100,

        /// <summary>
        /// This is background based task.
        /// </summary>
        Background = 20,

        /// <summary>
        /// This is a explicit task.
        /// </summary>
        Explicit = 0,
    }
}
