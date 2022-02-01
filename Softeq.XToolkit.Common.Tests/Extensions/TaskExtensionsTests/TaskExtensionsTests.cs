// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using NSubstitute;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Common.Tests.Extensions.TaskExtensionsTests.Stubs;

namespace Softeq.XToolkit.Common.Tests.Extensions.TaskExtensionsTests
{
    public partial class TaskExtensionsTests
    {
        private readonly ILogger _logger;
        private readonly TaskStub<object> _taskStub;
        private readonly Action<Exception> _onException;

        public TaskExtensionsTests()
        {
            _logger = Substitute.For<ILogger>();
            _taskStub = new TaskStub<object>();
            _onException = Substitute.For<Action<Exception>>();
        }

        private TimeSpan LongTimeout => TimeSpan.FromDays(1);

        private TimeSpan ShortTimeout => TimeSpan.FromMilliseconds(1);
    }
}
