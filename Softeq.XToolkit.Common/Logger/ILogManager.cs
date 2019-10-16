// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.Common.Logger
{
    public interface ILogManager
    {
        /// <summary>
        ///     Retrieves or creates a logger.
        /// </summary>
        /// <typeparam name="T">The type of the class to retrieve.</typeparam>
        /// <returns>The logger with the type specified.</returns>
        ILogger GetLogger<T>();

        /// <summary>
        ///     Retrieves or creates a named logger.
        /// </summary>
        /// <param name="name">The name of the logger to retrieve.</param>
        /// <returns>The logger with the name specified.</returns>
        ILogger GetLogger(string name);
    }
}
