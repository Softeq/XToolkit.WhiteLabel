// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Softeq.XToolkit.Remote.Executor;
using Softeq.XToolkit.Remote.Primitives;
using Xunit;

namespace Softeq.XToolkit.Remote.Tests
{
    public class RemoteServiceTests
    {
        private const string ResultData = "<TEST_DATA>";

        private readonly IRemoteService<IStubApiInterface> _remoteService;

        public RemoteServiceTests()
        {
            var testException = new Exception("TEST EXCEPTION");

            var apiService = Substitute.For<IStubApiInterface>();
            apiService.GetData(Arg.Any<CancellationToken>()).Returns(ResultData);
            apiService.DoException().Throws(testException);
            apiService.DoException(Arg.Any<CancellationToken>()).Throws(testException);

            var executorBuilder = new DefaultExecutorBuilderFactory();

            _remoteService = new RemoteService<IStubApiInterface>(apiService, executorBuilder);
        }

        [Fact]
        [SuppressMessage("ReSharper", "RedundantArgumentDefaultValue", Justification = "For test")]
        public async Task MakeRequest_WithNullOptions_GetsResult()
        {
            var data = await _remoteService.MakeRequest((s, ct) => s.GetData(ct), null);

            Assert.Equal(ResultData, data);
        }

        [Fact]
        public async Task MakeRequest_WithSomeOptions_GetsResult()
        {
            var options = new RequestOptions { RetryCount = 2 };

            var data = await _remoteService.MakeRequest((s, ct) => s.GetData(ct), options);

            Assert.Equal(ResultData, data);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [SuppressMessage("ReSharper", "MethodSupportsCancellation", Justification = "For test")]
        public async Task MakeRequest_WithException_ThrowsException(bool generic)
        {
            var task = generic
                ? _remoteService.MakeRequest((s, ct) => s.DoException())
                : _remoteService.MakeRequest((s, ct) => s.DoException(ct));

            await Assert.ThrowsAsync<Exception>(() => task);
        }
    }
}
