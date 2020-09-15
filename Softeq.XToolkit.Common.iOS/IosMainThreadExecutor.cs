// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Foundation;
using Softeq.XToolkit.Common.Threading;

namespace Softeq.XToolkit.Common.iOS
{
    /// <summary>
    ///     A <see cref="IMainThreadExecutor" /> implementation for the Xamarin iOS platform.
    /// </summary>
    public class IosMainThreadExecutor : MainThreadExecutorBase
    {
        /// <inheritdoc />
        public override bool IsMainThread => NSThread.Current.IsMainThread;

        protected override void PlatformBeginInvokeOnMainThread(Action action)
        {
            NSRunLoop.Main.BeginInvokeOnMainThread(action);
        }
    }
}
