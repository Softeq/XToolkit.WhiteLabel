// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;

namespace Softeq.XToolkit.WhiteLabel.Model
{
    public interface ITabItem
    {
        string Title { get; }
        string ImageKey { get; }

        TabViewModel CreateTabViewModel();
    }

    public class TabItem<T> : ITabItem where T : ViewModelBase
    {
        public TabItem(string title, string imageName)
        {
            Title = title;
            ImageKey = imageName;
        }

        public string Title { get; }

        public string ImageKey { get; }

        public TabViewModel CreateTabViewModel()
        {
            var frameNavigationService = Dependencies.Container.Resolve<IFrameNavigationService>();
            var tabViewModel = new TabViewModel<T>(frameNavigationService);
            tabViewModel.Initialize(this);
            return tabViewModel;
        }
    }
}