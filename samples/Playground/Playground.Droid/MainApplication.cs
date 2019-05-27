// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Reflection;
using Android.App;
using Android.Runtime;
using Autofac;
using Softeq.XToolkit.Common.Interfaces;
using Softeq.XToolkit.WhiteLabel.Droid;
using Softeq.XToolkit.WhiteLabel.Droid.Services.Logger;
using Softeq.XToolkit.WhiteLabel.Extensions;

namespace Playground.Droid
{
#if DEBUG
    [Application(Debuggable = true)]
#else
    [Application(Debuggable = false)]
#endif
    public class MainApplication : AutoRegistrationMainApplication
    {
        protected MainApplication(IntPtr handle, JniHandleOwnership transer)
            : base(handle, transer)
        {
        }

        protected override IList<Assembly> SelectAssemblies() => new List<Assembly>
        {
            GetType().Assembly
        };

#pragma warning disable RECS0133
        protected override void ConfigureIoc(ContainerBuilder builder)
#pragma warning restore RECS0133
        {
            // core
            Bootstrapper.Configure(builder);

            builder.PerLifetimeScope<DroidConsoleLogManager, ILogManager>();
            //builder.PerDependency<DefaultAlertBuilder, IAlertBuilder>();
            //builder.PerDependency<DroidFragmentDialogService, IDialogsService>();
            //builder.PerLifetimeScope<DroidInternalSettings, IInternalSettings>();
            //builder.PerLifetimeScope<LauncherService, ILauncherService>();
        }
    }
}
