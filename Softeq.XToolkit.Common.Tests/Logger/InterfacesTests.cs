// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using NSubstitute;
using Softeq.XToolkit.Common.Logger;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Logger
{
    public class InterfacesTests
    {
        [Fact]
        public void ILogger()
        {
            var mock = Substitute.For<ILogger>();
            mock.Debug(default(string));
            mock.Info(default(string));
            mock.Warn(default(string));
            mock.Warn(default(string));
            mock.Error(default(string));
            mock.Error(default(Exception));
        }

        [Fact]
        public void ILogManager()
        {
            var mock = Substitute.For<ILogManager>();
            mock.GetLogger(default);
            mock.GetLogger<string>();
        }
    }
}
