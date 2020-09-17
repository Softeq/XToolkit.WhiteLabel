// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using NSubstitute;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Threading.MainThreadExecutorBaseTests
{
    [SuppressMessage("ReSharper", "ConvertToLambdaExpression", Justification = "More readable")]
    public class MainThreadExecutorBaseTests
    {
        private readonly TestMainThreadExecutor _executor;
        private readonly Action _action;

        public MainThreadExecutorBaseTests()
        {
            _executor = new TestMainThreadExecutor();
            _action = Substitute.For<Action>();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void BeginOnUIThread_NullAction_ThrowsNullReferenceException(bool isMainThread)
        {
            _executor.IsMainThread_Returns(isMainThread);

            Assert.Throws<NullReferenceException>(() =>
            {
                _executor.BeginOnUIThread(null!);
            });
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void BeginOnUIThread_Action_Executes(bool isMainThread)
        {
            _executor.IsMainThread_Returns(isMainThread);

            _executor.BeginOnUIThread(_action);

            _action.Received().Invoke();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task OnUIThreadAsync_NullAction_ThrowsNullReferenceException(bool isMainThread)
        {
            _executor.IsMainThread_Returns(isMainThread);

            await Assert.ThrowsAsync<NullReferenceException>(() =>
            {
                return _executor.OnUIThreadAsync(null!);
            });
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task OnUIThreadAsync_Action_Executes(bool isMainThread)
        {
            _executor.IsMainThread_Returns(isMainThread);

            await _executor.OnUIThreadAsync(_action);

            _action.Received().Invoke();
        }

        [Fact]
        public async Task OnUIThreadAsync_ActionWithCancellation_ThrowsTaskCanceledException()
        {
            _executor.IsMainThread_Returns(false);
            _action.When(x => x.Invoke()).Do(x => throw new TaskCanceledException("Test"));

            await Assert.ThrowsAsync<TaskCanceledException>(() =>
            {
                return _executor.OnUIThreadAsync(_action);
            });
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void OnUIThread_NullAction_ThrowsNullReferenceException(bool isMainThread)
        {
            _executor.IsMainThread_Returns(isMainThread);

            Assert.Throws<NullReferenceException>(() =>
            {
                _executor.OnUIThread(null!);
            });
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void OnUIThread_Action_Executes(bool isMainThread)
        {
            _executor.IsMainThread_Returns(isMainThread);

            _executor.OnUIThread(_action);

            _action.Received().Invoke();
        }
    }
}
