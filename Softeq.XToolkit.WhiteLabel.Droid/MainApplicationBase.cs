// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Android.App;
using Android.Runtime;
using Android.Support.V4.App;
using Autofac;
using Plugin.CurrentActivity;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Droid;
using Softeq.XToolkit.WhiteLabel.Bootstrapper;
using Softeq.XToolkit.WhiteLabel.Droid.Providers;
using Softeq.XToolkit.WhiteLabel.Threading;

namespace Softeq.XToolkit.WhiteLabel.Droid
{
    public abstract class MainApplicationBase : Application
    {
        protected MainApplicationBase(IntPtr handle, JniHandleOwnership transfer)
            : base(handle, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();

            CrossCurrentActivity.Current.Init(this);

            //init factory for bindings
            BindingExtensions.Initialize(new DroidBindingFactory());

            //init assembly sources for Activator.cs
            AssemblySourceCache.Install();
            AssemblySourceCache.ExtractTypes = assembly =>
                assembly.GetExportedTypes()
                    .Where(t => typeof(FragmentActivity).IsAssignableFrom(t)
                                || typeof(Android.Support.V4.App.Fragment).IsAssignableFrom(t)
                                || typeof(Android.Support.V4.App.DialogFragment).IsAssignableFrom(t));
            var assemblies = SelectAssemblies();
            AssemblySource.Instance.AddRange(assemblies);

            //init dependencies
            Bootstrapper.Init(assemblies);

            //init ui thread helper
            PlatformProvider.Current = new DroidPlatformProvider();
        }

        protected abstract IBootstrapper Bootstrapper { get; }

        protected abstract IList<Assembly> SelectAssemblies();
    }
}