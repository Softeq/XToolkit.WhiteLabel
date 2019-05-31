// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using NSubstitute;
using Softeq.XToolkit.Common.Interfaces;
using Xunit;

namespace Softeq.XToolkit.Tests.Core.Common
{
    public class ContractTests
    {
        [Fact]
        public void IInternalSettingsTest()
        {
            var mock = Substitute.For<IInternalSettings>();
            mock.AddOrUpdateValue("key", 1);
            mock.Clear();
            mock.Contains("key");
            mock.GetValueOrDefault("key", 1);
            mock.Remove("key");
        }

        [Fact]
        public void ILoggerTest()
        {
            var mock = Substitute.For<ILogger>();
            mock.Debug(default(string));
            mock.Info(default(string));
            mock.Warn(default(string));
            mock.Warn(default(string));
            mock.Error(default(string));
            mock.Error(default(Exception));
        }
    }
}