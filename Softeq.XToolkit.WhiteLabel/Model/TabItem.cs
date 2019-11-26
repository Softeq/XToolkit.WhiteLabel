// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;

namespace Softeq.XToolkit.WhiteLabel.Model
{
    public abstract class TabItem
    {
        protected TabItem(string title, string imageName)
        {
            Title = title;
            ImageKey = imageName;
        }

        public string Title { get; }

        public string ImageKey { get; }

        public abstract TabViewModel CreateViewModel();
    }

    public class TabItem<TFirstViewModel> : TabItem where TFirstViewModel : ViewModelBase
    {
        public TabItem(string title, string imageName) : base(title, imageName)
        {
        }

        public override TabViewModel CreateViewModel()
        {
            var frameNavigationService = Dependencies.Container.Resolve<IFrameNavigationService>();
            var tabViewModel = new TabViewModel<TFirstViewModel>(frameNavigationService);
            tabViewModel.Initialize(this);
            return tabViewModel;
        }
    }
}