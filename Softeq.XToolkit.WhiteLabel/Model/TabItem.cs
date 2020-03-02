// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;

namespace Softeq.XToolkit.WhiteLabel.Model
{
    public abstract class TabItem<TKey>
    {
        protected TabItem(string title, TKey key)
        {
            Title = title;
            Key = key;
        }

        public string Title { get; }

        public TKey Key { get; }

        public abstract TabViewModel<TKey> CreateViewModel();
    }

    public class TabItem<TFirstViewModel, TKey> : TabItem<TKey> where TFirstViewModel : ViewModelBase
    {
        public TabItem(string title, TKey key) : base(title, key)
        {
        }

        public override TabViewModel<TKey> CreateViewModel()
        {
            var frameNavigationService = Dependencies.Container.Resolve<IFrameNavigationService>();
            var tabViewModel = new TabViewModel<TFirstViewModel, TKey>(frameNavigationService);
            tabViewModel.Initialize(this);
            return tabViewModel;
        }
    }
}