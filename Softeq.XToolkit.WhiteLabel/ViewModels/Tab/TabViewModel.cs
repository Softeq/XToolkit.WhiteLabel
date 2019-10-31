// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Model;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.ViewModels.Tab
{
    public class TabViewModel : RootFrameNavigationViewModelBase
    {
        private TabItem _tab = default!;
        private string? _badgeText;
        private bool _isBadgeVisible;

        public TabViewModel(
            IFrameNavigationService frameNavigationService)
            : base(frameNavigationService)
        {
        }

        internal TabViewModel Initialize(TabItem tab)
        {
            _tab = tab;
            return this;
        }

        public string? BadgeText
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
            // Check fast-backward nav by tab selected
            if (FrameNavigationService.IsEmptyBackStack)
            {
                FrameNavigationService.NavigateToViewModel(_tab.FirstViewModelType, true);
            }
            else
            {
                FrameNavigationService.NavigateToFirstPage();
            }
        }
    }
}
