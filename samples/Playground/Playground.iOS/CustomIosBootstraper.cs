// Developed by Softeq Development Corporation
// http://www.softeq.com

using Autofac;
using Softeq.XToolkit.WhiteLabel.Extensions;
using Softeq.XToolkit.WhiteLabel.iOS;
using Softeq.XToolkit.WhiteLabel.iOS.Services;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.iOS
{
    internal class CustomIosBootstrapper : IosBootstrapper
    {
        protected override void ConfigureIoc(ContainerBuilder builder)
        {
            // core
            CustomBootstrapper.Configure(builder);

            builder.PerDependency<StoryboardDialogsService, IDialogsService>();
            
            //builder.PerLifetimeScope<IosInternalSettings, IInternalSettings>();
        }
    }
}
