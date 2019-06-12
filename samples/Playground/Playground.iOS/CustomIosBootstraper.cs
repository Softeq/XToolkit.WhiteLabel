using Autofac;
using Softeq.XToolkit.WhiteLabel.iOS;

namespace Playground.iOS
{
    internal class Bootstrapper : IosBootstrapper
    {
        protected override void ConfigureIoc(ContainerBuilder builder)
        {
            // core
            CustomBootstrapper.Configure(builder);

            //builder.PerDependency<StoryboardDialogsService, IDialogsService>();
            //builder.PerLifetimeScope<IosInternalSettings, IInternalSettings>();
        }
    }
}
