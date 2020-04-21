// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics;
using Android.OS;
using Android.Runtime;
using Plugin.CurrentActivity;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Droid;
using Softeq.XToolkit.WhiteLabel.Bootstrapper;
using Softeq.XToolkit.WhiteLabel.Droid.Providers;
using Softeq.XToolkit.WhiteLabel.Threading;

namespace Softeq.XToolkit.WhiteLabel.Droid
{
    /// <summary>
    ///     Based on Android.App.Application class for maintaining global application state
    ///     and integration WhiteLabel components.
    /// </summary>
    public abstract class MainApplicationBase : Android.App.Application
    {
        protected MainApplicationBase(IntPtr handle, JniHandleOwnership transfer)
            : base(handle, transfer)
        {
        }

        protected abstract IBootstrapper Bootstrapper { get; }

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
            CrossCurrentActivity.Current.Init(this);
        }

        protected virtual void InitializeWhiteLabelRuntime()
        {
            // Init Bindings
            BindingExtensions.Initialize(new DroidBindingFactory());

            // Init platform helpers
            PlatformProvider.Current = new DroidPlatformProvider();

            // Init dependencies
            Bootstrapper.Initialize();
        }
    }
}
