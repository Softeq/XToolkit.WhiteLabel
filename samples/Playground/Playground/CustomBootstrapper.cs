// Developed by Softeq Development Corporation
// http://www.softeq.com

using Playground.Services;
using Playground.ViewModels.Frames;
using Softeq.XToolkit.Permissions;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Services;

namespace Playground
{
    public static class CustomBootstrapper
    {
        public static void Configure(IContainerBuilder builder)
        {
            builder.Singleton<EssentialsAppInfoService, IAppInfoService>();

            // Playground
            builder.Singleton<DataService, IDataService>();
            builder.Singleton<PermissionsService, IPermissionsService>();

            builder.PerDependency<TopShellViewModel>();
        }
    }
}
