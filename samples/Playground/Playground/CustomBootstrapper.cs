// Developed by Softeq Development Corporation
// http://www.softeq.com

using Playground.Services;
using Playground.ViewModels.Frames;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;

namespace Playground
{
    public static class CustomBootstrapper
    {
        public static void Configure(IContainerBuilder builder)
        {
            // Playground
            builder.Singleton<DataService, IDataService>();

            builder.PerDependency<TopShellViewModel>();
        }
    }
}
