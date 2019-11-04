// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.Common.Logger
{
    public class ConsoleLogger : ILogger
    {
        private readonly string _category;

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

        private void WriteMessage(Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex, _category);
        }

        private void WriteMessage(string message)
        {
            System.Diagnostics.Debug.WriteLine(message, _category);
        }
    }
}
