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
using Softeq.XToolkit.WhiteLabel.Extensions;
using Softeq.XToolkit.WhiteLabel.Services.Logger;

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

        protected override void ConfigureIoc(ContainerBuilder builder)
        {
            // core
            CustomBootstrapper.Configure(builder);

            builder.Singleton<ConsoleLogManager, ILogManager>();
            //builder.PerDependency<DefaultAlertBuilder, IAlertBuilder>();
            //builder.PerDependency<DroidFragmentDialogService, IDialogsService>();
            //builder.Singleton<DroidInternalSettings, IInternalSettings>();
            //builder.Singleton<LauncherService, ILauncherService>();
        }
    }
}
