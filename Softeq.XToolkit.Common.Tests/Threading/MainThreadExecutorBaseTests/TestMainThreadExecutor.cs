// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Common.Threading;

namespace Softeq.XToolkit.Common.Tests.Threading.MainThreadExecutorBaseTests
{
    public class TestMainThreadExecutor : MainThreadExecutorBase
    {
        private bool _isMainThread;

        public override bool IsMainThread => _isMainThread;

        public void IsMainThread_Returns(bool isMainThread)
        {
            _isMainThread = isMainThread;
        }

        protected override void PlatformBeginInvokeOnMainThread(Action action)
        {
            action();
        }
    }
}
