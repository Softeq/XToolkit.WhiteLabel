using Autofac;
using Softeq.XToolkit.Common.Interfaces;
using Softeq.XToolkit.WhiteLabel.Extensions;
using Softeq.XToolkit.WhiteLabel.iOS;
using Softeq.XToolkit.WhiteLabel.Services.Logger;

namespace Playground.iOS
{
    internal class Bootstrapper : IosBootstrapper
    {
        protected override void ConfigureIoc(ContainerBuilder builder)
        {
            // core
            CustomBootstrapper.Configure(builder);

            builder.PerDependency<ConsoleLogManager, ILogManager>();
            //builder.PerDependency<StoryboardDialogsService, IDialogsService>();
            //builder.PerLifetimeScope<IosInternalSettings, IInternalSettings>();
        }
    }
}
