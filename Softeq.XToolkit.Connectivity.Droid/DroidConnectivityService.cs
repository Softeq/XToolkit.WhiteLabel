// Developed by Softeq Development Corporation
// http://www.softeq.com

using AndroidX.Lifecycle;
using Microsoft.Maui.Networking;
using Softeq.XToolkit.Common.Droid;
using Softeq.XToolkit.Common.Threading;
using Softeq.XToolkit.Common.Weak;

namespace Softeq.XToolkit.Connectivity.Droid
{
    /// <summary>
    ///     Droid implementation of <see cref="DroidConnectivityService"/>.
    /// </summary>
    public class DroidConnectivityService : EssentialsConnectivityService
    {
        private readonly AppLifecycleObserver _lifecycleObserver;
        private readonly WeakAction _startAction;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DroidConnectivityService"/> class.
        /// </summary>
        /// <param name="connectivity">
        ///     Custom instance of <see cref="T:Microsoft.Maui.Networking.IConnectivity"/>
        ///     or you can use <see cref="Default"/> static method.
        /// </param>
        public DroidConnectivityService(IConnectivity connectivity) : base(connectivity)
        {
            _startAction = new WeakAction(OnAppStart);
            _lifecycleObserver = new AppLifecycleObserver(startAction: _startAction);
            Execute.BeginOnUIThread(() =>
            {
                ProcessLifecycleOwner.Get().Lifecycle.AddObserver(_lifecycleObserver);
            });
        }

        ~DroidConnectivityService()
        {
            Dispose(false);
        }

        /// <summary>
        ///     Releases the unmanaged and optionally the managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to dispose managed state.</param>
        /// <seealso cref="Dispose"/>
        /// <seealso cref="T:System.IDisposable"/>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Execute.BeginOnUIThread(() =>
                {
                    ProcessLifecycleOwner.Get().Lifecycle.RemoveObserver(_lifecycleObserver);
                });
            }
        }

        private void OnAppStart()
        {
            Connectivity.ConnectivityChanged -= CurrentConnectivityChanged;
            Connectivity.ConnectivityChanged += CurrentConnectivityChanged;
            CurrentConnectivityChanged(this, new ConnectivityChangedEventArgs(Connectivity.NetworkAccess, Connectivity.ConnectionProfiles));
        }
    }
}
