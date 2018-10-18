// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Common.Interfaces;

namespace Softeq.XToolkit.WhiteLabel.iOS.Services.Logger
{
    public class IosConsoleLogger : ILogger
    {
        public void Debug(string message)
        {
            WriteMessage(message);
        }

        public void Info(string message)
        {
            WriteMessage(message);
        }

        public void Warn(string message)
        {
            WriteMessage(message);
        }

        public void Warn(Exception ex)
        {
            WriteMessage(ex);
        }

        public void Error(string message)
        {
            WriteMessage(message);
        }

        public void Error(Exception ex)
        {
            WriteMessage(ex);
        }

        private void WriteMessage(Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
            System.Diagnostics.Debug.WriteLine(ex.StackTrace);
        }

        private void WriteMessage(string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }
    }
}