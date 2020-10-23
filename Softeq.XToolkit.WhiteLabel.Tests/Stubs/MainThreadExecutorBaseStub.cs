// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Common.Threading;

namespace Softeq.XToolkit.WhiteLabel.Tests.Stubs
{
    internal class MainThreadExecutorBaseStub : MainThreadExecutorBase
    {
        public override bool IsMainThread => true;

        protected override void PlatformBeginInvokeOnMainThread(Action action)
        {
            action();
        }
    }
}
