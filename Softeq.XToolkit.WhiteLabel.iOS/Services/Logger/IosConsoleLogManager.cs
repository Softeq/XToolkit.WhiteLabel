// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common.Interfaces;

namespace Softeq.XToolkit.WhiteLabel.iOS.Services.Logger
{
    public class IosConsoleLogManager : ILogManager
    {
        public ILogger GetLogger<T>()
        {
            var type = typeof(T);
            var name = type.Name;
            return CreateInstance(name);
        }

        public ILogger GetLogger(string name)
        {
            return CreateInstance(name);
        }

        private ILogger CreateInstance(string name)
        {
            return new IosConsoleLogger();
        }
    }
}