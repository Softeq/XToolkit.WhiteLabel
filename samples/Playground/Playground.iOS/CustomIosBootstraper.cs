// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.iOS;
using Softeq.XToolkit.WhiteLabel.iOS.Services;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.iOS
{
    internal class CustomIosBootstrapper : IosBootstrapper
    {
        protected override void ConfigureIoc(IContainerBuilder builder)
        {
            // core
            CustomBootstrapper.Configure(builder);

            builder.PerDependency<StoryboardDialogsService, IDialogsService>();
            
            //builder.PerLifetimeScope<IosInternalSettings, IInternalSettings>();
        }
    }
}
