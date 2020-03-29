// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Common.Commands;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Extensions
{
    public static class UiViewExtensions
    {
        public static void SetEndEditingCommand(this UIButton button, UIView view)
        {
            button.SetCommand(new RelayCommand<UIView>(x => x.EndEditing(true)), view);
        }
    }
}