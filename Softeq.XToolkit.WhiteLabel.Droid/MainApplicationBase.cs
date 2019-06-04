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
using Softeq.XToolkit.WhiteLabel.Droid.Navigation;
using Softeq.XToolkit.WhiteLabel.Droid.Providers;
using Softeq.XToolkit.WhiteLabel.Extensions;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Threading;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;

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
            AssemblySource.Instance.AddRange(SelectAssemblies());

            //init dependencies
            StartScopeForIoc();

            //init ui thread helper
            PlatformProvider.Current = new DroidPlatformProvider();
        }

        protected abstract IList<Assembly> SelectAssemblies();

        protected void RegisterInternalServices(ContainerBuilder builder)
        {
            builder.Singleton(c => CrossCurrentActivity.Current)
                .PreserveExistingDefaults();
            builder.Singleton<ActivityPageNavigationService, IPlatformNavigationService>()
                .PreserveExistingDefaults();
            builder.PerDependency<FrameNavigationService, IFrameNavigationService>()
                .PreserveExistingDefaults();
            builder.Singleton<ViewLocator, IViewLocator>()
                .PreserveExistingDefaults();
            builder.PerDependency<RootFrameNavigationViewModel>()
                .PreserveExistingDefaults();
        }

        protected virtual void StartScopeForIoc()
        {
            var containerBuilder = new ContainerBuilder();
            ConfigureIoc(containerBuilder);
            RegisterInternalServices(containerBuilder);
            Dependencies.IocContainer.StartScope(containerBuilder);
        }

        protected abstract void ConfigureIoc(ContainerBuilder builder);
    }
}