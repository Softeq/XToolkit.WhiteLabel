// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Common.Weak;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Controls
{
    internal class BindableTabBarItem : UITabBarItem, IBindable
    {
        private Binding _textBinding;
        private WeakReferenceEx<TabViewModel> _viewModelRef;
        private Binding _visibilityBinding;

        internal BindableTabBarItem(string title, UIImage image, UIImage selectedImage)
            : base(title, image, selectedImage)
        {
        }

        public object DataContext => _viewModelRef?.Target;

        public List<Binding> Bindings => throw new System.NotImplementedException();

        public void SetDataContext(object dataContext)
        {
            _viewModelRef = new WeakReferenceEx<TabViewModel>((TabViewModel) dataContext);

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