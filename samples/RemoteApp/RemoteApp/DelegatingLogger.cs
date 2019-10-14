using System;
using Softeq.XToolkit.Common.Logger;

namespace RemoteApp
{
    public class DelegatingLogger : ILogger
    {
        public event Action<string> Written;

        public void Debug(string message) => OnWritten(message);

        public void Error(string message) => OnWritten($"Error: {message}");

        public void Error(Exception exception) => OnWritten($"Error: {exception.GetType().Name}: {exception.Message}");

        public void Info(string message) => OnWritten($"Info: {message}");

        public void Warn(string message) => OnWritten($"Warn: {message}");

        public void Warn(Exception exception) => OnWritten($"Warn: {exception.GetType().Name}: {exception.Message}");

        protected virtual void OnWritten(string obj)
        {
            Written?.Invoke(obj);
        }
    }
}
