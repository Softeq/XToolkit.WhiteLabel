// Developed by Softeq Development Corporation
// http://www.softeq.com

﻿using Autofac;
using Softeq.XToolkit.WhiteLabel.Droid;
using Softeq.XToolkit.WhiteLabel.Droid.Dialogs;
using Softeq.XToolkit.WhiteLabel.Droid.Services;
using Softeq.XToolkit.WhiteLabel.Extensions;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.Droid
{
    internal class CustomDroidBootstrapper : DroidBootstrapper
    {
        protected override void ConfigureIoc(ContainerBuilder builder)
        {
            // core
            CustomBootstrapper.Configure(builder);

            builder.Singleton<DroidFragmentDialogService, IDialogsService>();
            builder.Singleton<DefaultAlertBuilder, IAlertBuilder>();
            
            //builder.Singleton<DroidInternalSettings, IInternalSettings>();
            //builder.Singleton<LauncherService, ILauncherService>();
        }
    }
}
