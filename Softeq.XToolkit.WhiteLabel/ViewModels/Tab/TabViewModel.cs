// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Model;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.ViewModels.Tab
{
    public abstract class TabViewModel<TKey> : RootFrameNavigationViewModelBase
    {
        private string _badgeText;
        private bool _isBadgeVisible;
        private TabItem<TKey> _tab;

        protected TabViewModel(IFrameNavigationService frameNavigationService)
            : base(frameNavigationService)
        {
        }

        public string BadgeText
        {
            get => _badgeText;
            set => Set(ref _badgeText, value);
        }

        public bool IsBadgeVisible
        {
            get => _isBadgeVisible;
            set => Set(ref _isBadgeVisible, value);
        }

        public string Title => _tab.Title;

        public TKey Key => _tab.Key;

        public bool CanGoBack => FrameNavigationService.CanGoBack;

        public void GoBack()
        {
            FrameNavigationService.GoBack();
        }

        internal void Initialize(TabItem<TKey> tab)
        {
            _tab = tab;
        }
    }

    public class TabViewModel<TFirstViewModel, TKey> : TabViewModel<TKey> where TFirstViewModel : ViewModelBase
    {
        public TabViewModel(IFrameNavigationService frameNavigationService)
            : base(frameNavigationService)
        {
        }

        public override void NavigateToFirstPage()
        {
            // Check fast-backward nav by tab selected
            if (FrameNavigationService.IsEmptyBackStack)
            {
                FrameNavigationService.NavigateToViewModel<TFirstViewModel>(true);
            }
            else
            {
                FrameNavigationService.NavigateToFirstPage();
            }
        }
    }
}