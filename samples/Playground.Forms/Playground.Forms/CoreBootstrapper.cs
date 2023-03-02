// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Reflection;
using Playground.Forms.Remote;
using Playground.Forms.Services;
using Playground.Forms.ViewModels;
using Playground.Forms.ViewModels.Components;
using Playground.Forms.ViewModels.Dialogs;
using Playground.Forms.ViewModels.Dialogs.Modal;
using Playground.Forms.ViewModels.MasterDetailNavigation;
using Playground.Forms.ViewModels.MasterDetailNavigation.DrillNavigation;
using Playground.Forms.ViewModels.SimpleNavigation;
using Softeq.XToolkit.Permissions;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Forms;
using Softeq.XToolkit.WhiteLabel.Forms.Navigation;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Xamarin.Forms;

namespace Playground.Forms
{
    public abstract class CoreBootstrapper : FormsBootstrapper
    {
        protected override IList<Assembly> SelectAssemblies()
        {
            return new List<Assembly>
            {
                typeof(App).Assembly
            };
        }

        protected override bool IsExtractToAssembliesCache(Type type)
        {
            return typeof(Page).IsAssignableFrom(type);
        }

        protected override void ConfigureIoc(IContainerBuilder builder)
        {
            RemoteBootstrapper.ConfigureIoc(builder);

            builder.PerDependency<MainPageViewModel>();
            builder.PerDependency<FirstPageViewModel>();
            builder.PerDependency<SecondPageViewModel>();
            builder.PerDependency<RootMasterDetailPageViewModel>();
            builder.PerDependency<MasterPageViewModel>();
            builder.PerDependency<DetailPageViewModel>();
            builder.PerDependency<SelectedItemPageViewModel>();
            builder.PerDependency<DialogsRootPageViewModel>();
            builder.PerDependency<ModalPageViewModel>();
            builder.PerDependency<SecondModalPageViewModel>();
            builder.PerDependency<RootFrameNavigationPageViewModel<DrillLevel1PageViewModel>>();
            builder.PerDependency<DrillLevel1PageViewModel>();
            builder.PerDependency<DrillLevel2PageViewModel>();
            builder.PerDependency<AsyncCommandsPageViewModel>();
            builder.PerDependency<PermissionsPageViewModel>();
            builder.PerDependency<ValidationPageViewModel>();
            builder.PerDependency<PaginationSearchPageViewModel>();
            builder.PerDependency<LauncherPageViewModel>();
            builder.PerDependency<LocationPageViewModel>();

            builder.PerDependency<ViewModelFactoryService, IViewModelFactoryService>();

            builder.Singleton<PlaygroundViewLocator, IFormsViewLocator>(IfRegistered.Replace);
            builder.Singleton<PermissionsService, IPermissionsService>();
        }
    }
}