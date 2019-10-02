﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common.Interfaces;

namespace Softeq.XToolkit.WhiteLabel.Services.Logger
{
    public class ConsoleLogManager : ILogManager
    {
        /// <inheritdoc />
        public ILogger GetLogger<T>()
        {
            var type = typeof(T);
            var name = type.Name;
            return CreateInstance(name);
        }

        /// <inheritdoc />
        public ILogger GetLogger(string name)
        {
            return CreateInstance(name);
        }

        private static ILogger CreateInstance(string name)
        {
            return new ConsoleLogger(name);
        }
    }
}