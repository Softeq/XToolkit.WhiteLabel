// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Plugin.CurrentActivity;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Droid;
using Softeq.XToolkit.WhiteLabel.Bootstrapper;
using Softeq.XToolkit.WhiteLabel.Droid.Providers;
using Softeq.XToolkit.WhiteLabel.Threading;

namespace Softeq.XToolkit.WhiteLabel.Droid
{
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

            InitBootstrapper();

            CrossCurrentActivity.Current.Init(this);

            // init Bindings
            BindingExtensions.Initialize(new DroidBindingFactory());

            // init UI thread helper
            PlatformProvider.Current = new DroidPlatformProvider();
        }

        protected abstract IBootstrapper Bootstrapper { get; }

        protected abstract IList<Assembly> SelectAssemblies();

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

        private void InitBootstrapper()
        {
            // init assembly sources
            AssemblySourceCache.Install();
            AssemblySourceCache.ExtractTypes = assembly =>
                assembly.GetExportedTypes()
                    .Where(t => typeof(FragmentActivity).IsAssignableFrom(t)
                        || typeof(DialogFragment).IsAssignableFrom(t)
                        || typeof(Fragment).IsAssignableFrom(t));
            var assemblies = SelectAssemblies();
            AssemblySource.Instance.AddRange(assemblies);

            // init dependencies
            Bootstrapper.Init(assemblies);
        }
    }
}
