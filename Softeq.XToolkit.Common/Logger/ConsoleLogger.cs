// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics;

namespace Softeq.XToolkit.Common.Logger
{
    /// <summary>
    ///     Console implementation for <see cref="ILogger"/> interface.
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        private readonly string _category;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConsoleLogger"/> class.
        /// </summary>
        /// <param name="category">Category name.</param>
        public ConsoleLogger(string category)
        {
            _category = category;
        }

        /// <inheritdoc />
        public void Debug(string message)
        {
            WriteMessage(message);
        }

        /// <inheritdoc />
        public void Info(string message)
        {
            WriteMessage(message);
        }

        /// <inheritdoc />
        public void Warn(string message)
        {
            WriteMessage(message);
        }

        /// <inheritdoc />
        public void Warn(Exception ex)
        {
            WriteMessage(ex);
        }

        /// <inheritdoc />
        public void Error(string message)
        {
            WriteMessage(message);
        }

        /// <inheritdoc />
        public void Error(Exception ex)
        {
            WriteMessage(ex);
        }

        /// <summary>
        ///     Base method to log <see cref="System.Exception" /> message.
        /// </summary>
        /// <param name="ex">Exception to log.</param>
        protected virtual void WriteMessage(Exception ex)
        {
            Trace.WriteLine(ex, _category);
        }

        /// <summary>
        ///     Base method to log string message.
        /// </summary>
        /// <param name="message">Message to log.</param>
        protected virtual void WriteMessage(string message)
        {
            Trace.WriteLine(message, _category);
        }
    }
}
