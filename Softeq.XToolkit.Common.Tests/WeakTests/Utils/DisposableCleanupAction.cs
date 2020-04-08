// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.Common.Tests.WeakTests.Utils
{
    public sealed class DisposableCleanupAction : IDisposable
    {
        private readonly Action _cleanUpAction;

        public DisposableCleanupAction(Action cleanUpAction)
        {
            _cleanUpAction = cleanUpAction;
        }

        public void Dispose()
        {
            _cleanUpAction.Invoke();
        }
    }
}
