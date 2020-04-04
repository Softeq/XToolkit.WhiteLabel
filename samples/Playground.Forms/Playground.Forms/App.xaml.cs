// Developed by Softeq Development Corporation
// http://www.softeq.com

using Playground.Forms.ViewModels;
using Playground.Forms.Views;
using Softeq.XToolkit.WhiteLabel;
using Softeq.XToolkit.WhiteLabel.Bootstrapper;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Xamarin.Forms;

namespace Playground.Forms
{
    public partial class App
    {
        public App(IBootstrapper bootstrapper) : base(bootstrapper)
        {
            InitializeComponent();
            MainPage = new NavigationPage(new StartPage());
        }

        protected override void OnStarted()
        {
            base.OnStarted();

            var navigationService = Dependencies.Container.Resolve<IPageNavigationService>();
            navigationService.Initialize(Current.MainPage.Navigation);
            navigationService
                .For<MainPageViewModel>()
                .WithParam(x => x.Title, "Main Page")
                .Navigate(true);
        }
    }
}
