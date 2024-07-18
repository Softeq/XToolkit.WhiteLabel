// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics;
using Android.OS;
using Android.Runtime;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Droid;
using Softeq.XToolkit.Common.Droid;
using Softeq.XToolkit.Common.Threading;
using Softeq.XToolkit.WhiteLabel.Bootstrapper;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;

namespace Softeq.XToolkit.WhiteLabel.Droid
{
    /// <summary>
    ///     Based on <see cref="T:Android.App.Application"/> class for maintaining global application state
    ///     and integration WhiteLabel components.
    /// </summary>
    public abstract class MainApplicationBase : Android.App.Application
    {
        protected MainApplicationBase(IntPtr handle, JniHandleOwnership transfer)
            : base(handle, transfer)
        {
        }

        public override void OnCreate()
        {
            InitStrictMode();

            base.OnCreate();

            InitializeExternalDependencies();

            InitializeWhiteLabelRuntime();
        }

        [Conditional("DEBUG")]
        protected void InitStrictMode()
        {
            var builder = new StrictMode.ThreadPolicy.Builder()
                    .DetectCustomSlowCalls()
                    .DetectNetwork()
                    .PenaltyLog();

            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                builder = builder.DetectResourceMismatches();
            }

            StrictMode.SetThreadPolicy(builder.Build());

            StrictMode.SetVmPolicy(
                new StrictMode.VmPolicy.Builder()
                    .DetectActivityLeaks()
                    .DetectLeakedClosableObjects()
                    .PenaltyLog()
                    .Build());
        }

        protected virtual void InitializeExternalDependencies()
        {
            Microsoft.Maui.ApplicationModel.Platform.Init(this);
        }

        protected abstract IBootstrapper CreateBootstrapper();

        protected virtual void InitializeWhiteLabelRuntime()
        {
            // Init Bindings
            BindingExtensions.Initialize(new DroidBindingFactory());

            // Init platform helpers
            Execute.Initialize(new DroidMainThreadExecutor());

            // Init dependencies
            var bootstrapper = CreateBootstrapper();
            var container = bootstrapper.Initialize();
            Dependencies.Initialize(container);

            // Notify dependencies ready to be used
            OnContainerInitialized(container);
        }

        protected virtual void OnContainerInitialized(IContainer container)
        {
        }
    }
}
