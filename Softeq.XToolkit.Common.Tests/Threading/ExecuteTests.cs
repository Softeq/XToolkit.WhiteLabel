// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using NSubstitute;
using Softeq.XToolkit.Common.Threading;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Threading
{
    [SuppressMessage("ReSharper", "SA1312", Justification = "More readable")]
    [SuppressMessage("ReSharper", "ConvertToLambdaExpression", Justification = "More readable")]
    public class ExecuteTests
    {
        private readonly IMainThreadExecutor _executor;
        private readonly Action _action;

        public ExecuteTests()
        {
            _executor = Substitute.For<IMainThreadExecutor>();
            _executor.When(x => x.BeginOnUIThread(Arg.Any<Action>())).Do(x => ((Action) x[0]).Invoke());
            _executor.When(x => x.OnUIThreadAsync(Arg.Any<Action>())).Do(x => ((Action) x[0]).Invoke());
            _executor.When(x => x.OnUIThread(Arg.Any<Action>())).Do(x => ((Action) x[0]).Invoke());

            _action = Substitute.For<Action>();

            Execute.Initialize(_executor);
        }

        [Fact(Skip = "Currently executor always != null")]
        public void CurrentExecutor_Default_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var _ = Execute.CurrentExecutor;
            });
        }

        [Fact]
        public void CurrentExecutor_Initialized_ReturnsCorrectInstance()
        {
            Assert.Same(_executor, Execute.CurrentExecutor);
        }

        [Fact]
        public void Initialize_Null_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                Execute.Initialize(null!);
            });
        }

        [Fact]
        public void BeginOnUIThread_Null_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                Execute.BeginOnUIThread(null!);
            });
        }

        [Fact]
        public void BeginOnUIThread_ActionExtension_Executes()
        {
            _action.BeginOnUIThread();

            _action.Received().Invoke();
        }

        [Fact]
        public async Task OnUIThreadAsync_Null_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
            {
                return Execute.OnUIThreadAsync(null!);
            });
        }

        [Fact]
        public async Task OnUIThreadAsync_ActionExtension_Executes()
        {
            await _action.OnUIThreadAsync();

            _action.Received().Invoke();
        }

        [Fact]
        public void OnUIThread_Null_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                Execute.OnUIThread(null!);
            });
        }

        [Fact]
        public void OnUIThread_ActionExtension_Executes()
        {
            _action.OnUIThread();

            _action.Received().Invoke();
        }
    }
}
