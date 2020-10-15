// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using NSubstitute;
using Polly;
using Softeq.XToolkit.Remote.Executor;
using Xunit;

namespace Softeq.XToolkit.Remote.Tests.Executor
{
    public class DefaultExecutorBuilderTests
    {
        private readonly DefaultExecutorBuilder _builder;
        private readonly Func<Exception, bool> _shouldRetry;

        public DefaultExecutorBuilderTests()
        {
            _builder = new DefaultExecutorBuilder();
            _shouldRetry = Substitute.For<Func<Exception, bool>>();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void WithTimeout_IncorrectTimeout_ThrowsArgumentOutOfRangeException(int timeout)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _builder.WithTimeout(timeout);
            });
        }

        [Theory]
        [InlineData(1)]
        public void WithTimeout_CorrectTimeout_ReturnsCorrectInstance(int timeout)
        {
            var result = _builder.WithTimeout(timeout);

            Assert.Same(_builder, result);
        }

        [Theory]
        [InlineData(-1)]
        public void WithRetry_IncorrectRetryCount_ThrowsArgumentOutOfRangeException(int retryCount)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _builder.WithRetry(retryCount, _shouldRetry);
            });
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void WithRetry_CorrectRetryCount_ReturnsCorrectInstance(int retryCount)
        {
            var result = _builder.WithRetry(retryCount, _shouldRetry);

            Assert.Same(_builder, result);
        }

        [Fact]
        public void WithRetry_NullShouldRetry_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _builder.WithRetry(1, null!);
            });
        }

        [Fact]
        public void Build_WithoutAnyPolicies_ReturnsPolicy()
        {
            var result = _builder.Build();

            Assert.IsAssignableFrom<IAsyncPolicy>(result);
        }

        [Fact]
        public void Build_WithPolicies_ReturnsPolicy()
        {
            _builder
                .WithTimeout(1)
                .WithRetry(1, _shouldRetry);

            var result = _builder.Build();

            Assert.IsAssignableFrom<IAsyncPolicy>(result);
        }
    }
}
