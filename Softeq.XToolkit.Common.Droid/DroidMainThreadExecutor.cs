// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading;
using Android.App;
using Softeq.XToolkit.Common.Threading;

namespace Softeq.XToolkit.Common.Droid
{
    /// <summary>
    ///     A <see cref="IMainThreadExecutor" /> implementation for the Xamarin Android platform.
    /// </summary>
    public class DroidMainThreadExecutor : MainThreadExecutorBase
    {
        /// <inheritdoc />
        public override bool IsMainThread => SynchronizationContext.Current != null;

        protected override void PlatformBeginInvokeOnMainThread(Action action)
        {
            Application.SynchronizationContext.Post(_ => action(), null);
        }
    }
}
