// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Foundation;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Controls
{
    [Register("GradientFadeView")]
    public class GradientFadeView : GradientView
    {
        private bool _isFromTopToBottom;
        private UIColor _mainColor = default!;

        public GradientFadeView(IntPtr handle) : base(handle)
        {
        }

        public bool IsFromTopToBottom
        {
            get => _isFromTopToBottom;
            set
            {
                if (_isFromTopToBottom != value)
                {
                    _isFromTopToBottom = value;
                    UpdateColors();
                }
            }
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            _mainColor = BackgroundColor;
            UpdateColors();
            BackgroundColor = UIColor.Clear;
        }

        public void UpdateColors()
        {
            var topColor = IsFromTopToBottom ? _mainColor.CGColor : _mainColor.ColorWithAlpha(0).CGColor;
            var bottomColor = !IsFromTopToBottom ? _mainColor.CGColor : _mainColor.ColorWithAlpha(0).CGColor;
            GradientLayer.Colors = new[] { topColor, bottomColor };
        }
    }
}