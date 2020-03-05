// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics;
using NSubstitute;
using Softeq.XToolkit.Common.Logger;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Logger
{
    public class ConsoleLoggerTests : IDisposable
    {
        private readonly ConsoleTraceListener _listener;

        public ConsoleLoggerTests()
        {
            _listener = Substitute.ForPartsOf<ConsoleTraceListener>();
            _listener.WhenForAnyArgs(x => x.WriteLine(default(string), default(string))).DoNotCallBase();
            _listener.WhenForAnyArgs(x => x.WriteLine(default(object), default(string))).DoNotCallBase();

            Trace.Listeners.Add(_listener);
        }

        public void Dispose()
        {
            Trace.Listeners.Remove(_listener);
        }

        [Theory]
        [ClassData(typeof(ConsoleLoggerMessageTestData))]
        public void Debug_AnyMessage_WritesMessage(string category, string message)
        {
            var logger = new ConsoleLogger(category);

            logger.Debug(message);

            _listener.Received(1).WriteLine(message, category);
        }

        [Theory]
        [ClassData(typeof(ConsoleLoggerMessageTestData))]
        public void Info_AnyMessage_WritesMessage(string category, string message)
        {
            var logger = new ConsoleLogger(category);

            logger.Info(message);

            _listener.Received(1).WriteLine(message, category);
        }

        [Theory]
        [ClassData(typeof(ConsoleLoggerMessageTestData))]
        public void Warn_AnyMessage_WritesMessage(string category, string message)
        {
            var logger = new ConsoleLogger(category);

            logger.Warn(message);

            _listener.Received(1).WriteLine(message, category);
        }

        [Theory]
        [ClassData(typeof(ConsoleLoggerMessageTestData))]
        public void Error_AnyMessage_WritesMessage(string category, string message)
        {
            var logger = new ConsoleLogger(category);

            logger.Error(message);

            _listener.Received(1).WriteLine(message, category);
        }

        [Theory]
        [ClassData(typeof(ConsoleLoggerExceptionTestData))]
        public void Error_AnyException_WritesMessage(string category, Exception message)
        {
            var logger = new ConsoleLogger(category);

            logger.Error(message);

            _listener.Received(1).WriteLine(message, category);
        }

        [Theory]
        [ClassData(typeof(ConsoleLoggerExceptionTestData))]
        public void Warn_AnyException_WritesMessage(string category, Exception message)
        {
            var logger = new ConsoleLogger(category);

            logger.Warn(message);

            _listener.Received(1).WriteLine(message, category);
        }
    }
}
