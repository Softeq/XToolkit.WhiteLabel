// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.Common.Interfaces
{
    public interface ILogger
    {
        /// <summary>
        /// Log a message object with the Debug level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        void Debug(string message);

        /// <summary>
        /// Log a message object with the Info level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        void Info(string message);

        /// <summary>
        /// Log a message object with the Warn level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        void Warn(string message);

        /// <summary>
        /// Log a message object with the Warn level.
        /// </summary>
		/// <param name="exception">The exception to log, including its stack trace.</param>
        void Warn(Exception exception);

        /// <summary>
        /// Log a message object with the Error level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        void Error(string message);

        /// <summary>
        /// Log a message object with the Error level.
        /// </summary>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        void Error(Exception exception);
    }
}