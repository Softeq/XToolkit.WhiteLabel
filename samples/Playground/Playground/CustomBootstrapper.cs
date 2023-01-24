// Developed by Softeq Development Corporation
// http://www.softeq.com

using Playground.Services;
using Playground.ViewModels.Frames;
using Playground.ViewModels.RIB.CreateAccount;
using Playground.ViewModels.RIB.Detail;
using Playground.ViewModels.RIB.Login;
using Playground.ViewModels.RIB.Main;
using Playground.ViewModels.RIB.Platform.Navigation;
using Playground.ViewModels.RIB.Root;
using Playground.ViewModels.RIB.Services;
using Playground.ViewModels.RIB.Services.Interfaces;
using Playground.ViewModels.TestApproach;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;

namespace Playground
{
    public static class CustomBootstrapper
    {
        public static void Configure(IContainerBuilder builder)
        {
            // Playground
            builder.Singleton<DataService, IDataService>();
            builder.Singleton<CommonNavigationServiceWithFlow, ICommonNavigationService>();

            builder.PerDependency<LoginService, ILoginService>();
            builder.PerDependency<LoginBuilder, ILoginBuildable>();
            builder.PerDependency<MainBuilder, IMainBuildable>();

            builder.PerDependency<LoginRouter>();
            builder.PerDependency<CreateAccountRouter>();
            builder.PerDependency<MainRouter>();
            builder.PerDependency<DetailRouter>();
            builder.PerDependency<RootRouter>();

            builder.PerDependency<LoginInteractor>();

            builder.Singleton<RibNavigationService, IRibNavigationService>();

            builder.PerDependency<TopShellViewModel>();
        }
    }
}
