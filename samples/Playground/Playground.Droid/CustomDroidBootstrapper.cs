// Developed by Softeq Development Corporation
// http://www.softeq.com

using Autofac;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Droid;

namespace Playground.Droid
{
    internal class CustomDroidBootstrapper : DroidBootstrapper
    {
        protected override void ConfigureIoc(ICustomContainerBuilder builder)
        {
            // core
            CustomBootstrapper.Configure(builder);

            //builder.PerDependency<DefaultAlertBuilder, IAlertBuilder>();
            //builder.PerDependency<DroidFragmentDialogService, IDialogsService>();
            //builder.Singleton<DroidInternalSettings, IInternalSettings>();
            //builder.Singleton<LauncherService, ILauncherService>();
        }
    }
}
