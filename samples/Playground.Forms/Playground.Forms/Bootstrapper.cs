// Developed by Softeq Development Corporation
// http://www.softeq.com

using Playground.Forms.Services;
using Playground.Forms.ViewModels;
using Playground.Forms.ViewModels.Dialogs;
using Playground.Forms.ViewModels.Dialogs.Modal;
using Playground.Forms.ViewModels.MasterDetailNavigation;
using Playground.Forms.ViewModels.SimpleNavigation;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Forms;
using Softeq.XToolkit.WhiteLabel.Forms.Navigation;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Xamarin.Forms;

namespace Playground.Forms
{
    public abstract class Bootstrapper : FormsBootstrapper
    {
        protected override void ConfigureIoc(IContainerBuilder builder)
        {
            builder.PerDependency<MainPageViewModel>();
            builder.PerDependency<FirstLevelViewModel>();
            builder.PerDependency<SecondLevelViewModel>();
            builder.PerDependency<RootMasterViewModel>();
            builder.PerDependency<MasterViewModel>();
            builder.PerDependency<DetailViewModel>();
            builder.PerDependency<SelectedItemViewModel>();
            builder.PerDependency<DialogsRootViewModel>();
            builder.PerDependency<ModalPageViewModel>();
            builder.PerDependency<SecondModalPageViewModel>();

            builder.PerDependency<ViewModelFactoryService, IViewModelFactoryService>();

            builder.Singleton<PlaygroundViewLocator, IFormsViewLocator>(IfRegistered.Replace);

            builder.RegisterBuildCallback(OnContainerReady);
        }

        private static void OnContainerReady(IContainer container)
        {
            var navigationService = container.Resolve<IPageNavigationService>();
            navigationService.Initialize(Application.Current.MainPage.Navigation);
            navigationService
                .For<MainPageViewModel>()
                .WithParam(x => x.Title, "Main Page")
                .Navigate(true);
        }
    }
}
