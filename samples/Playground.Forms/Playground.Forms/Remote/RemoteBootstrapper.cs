// Developed by Softeq Development Corporation
// http://www.softeq.com

using Playground.Forms.Remote.ViewModels;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;

namespace Playground.Forms.Remote
{
    public static class RemoteBootstrapper
    {
        public static void ConfigureIoc(IContainerBuilder builder)
        {
            builder.PerDependency<RemotePageViewModel>();
        }
    }
}
