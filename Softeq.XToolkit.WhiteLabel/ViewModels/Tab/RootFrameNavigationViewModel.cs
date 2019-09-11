// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Model;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.ViewModels.Tab
{
    public class RootFrameNavigationViewModel : RootFrameNavigationViewModelBase
    {
        private TabItem _tab;
        private string _badgeText;
        private bool _isBadgeVisible;

        public RootFrameNavigationViewModel(
            IFrameNavigationService frameNavigationService)
            : base(frameNavigationService)
        {
        }

        public RootFrameNavigationViewModel Initialize(TabItem tab)
        {
            _tab = tab;
            return this;
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

        public string ImageKey => _tab.ImageKey;

        public override void NavigateToFirstPage()
        {
            if (FrameNavigationService.IsEmptyBackStack)
            {
                FrameNavigationService.NavigateToViewModel(_tab.RootViewModelType, true);
            }
            else
            {
                FrameNavigationService.NavigateToFirstPage();
            }
        }
    }
}
