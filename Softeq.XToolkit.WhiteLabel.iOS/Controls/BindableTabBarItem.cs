// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Common;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Controls
{
    internal class BindableTabBarItem : UITabBarItem
    {
        private WeakReferenceEx<TabViewModel> _viewModelRef;
        private Binding _textBinding;
        private Binding _visibilityBinding;

        internal BindableTabBarItem(string title, UIImage image, UIImage selectedImage) : base(title, image, selectedImage)
        {
        }

        internal void SetViewModel(TabViewModel viewModel)
        {
            _viewModelRef = new WeakReferenceEx<TabViewModel>(viewModel);

            _textBinding?.Detach();
            _textBinding = this.SetBinding(() => _viewModelRef.Target.BadgeText).WhenSourceChanges(UpdateBadge);

            _visibilityBinding?.Detach();
            _visibilityBinding = this.SetBinding(() => _viewModelRef.Target.IsBadgeVisible).WhenSourceChanges(UpdateBadge);
        }

        private void UpdateBadge()
        {
            if (_viewModelRef.Target.IsBadgeVisible)
            {
                BadgeValue = _viewModelRef.Target.BadgeText;
            }
            else
            {
                BadgeValue = null;
            }
        }
    }
}
