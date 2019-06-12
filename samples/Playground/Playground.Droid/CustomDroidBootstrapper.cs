using Autofac;
using Softeq.XToolkit.WhiteLabel.Droid;

namespace Playground.Droid
{
    internal class CustomDroidBootstrapper : DroidBootstrapper
    {
        protected override void ConfigureIoc(ContainerBuilder builder)
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
