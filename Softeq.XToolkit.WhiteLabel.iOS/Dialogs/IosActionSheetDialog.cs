// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Softeq.XToolkit.WhiteLabel.Dialogs;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Dialogs
{
    public class IosActionSheetDialog : AlertViewControllerBase, IDialog<string>
    {
        private readonly ActionSheetDialogConfig _config;

        public IosActionSheetDialog(IViewLocator viewLocator, ActionSheetDialogConfig config)
            : base(viewLocator)
        {
            _config = config;

            Title = config.Title;
            Message = null;
            Style = UIAlertControllerStyle.ActionSheet;
        }

        public Task<string> ShowAsync()
        {
            if (_config.OptionButtons == null)
            {
                throw new ArgumentNullException(nameof(_config.OptionButtons));
            }

            var dialogResult = new TaskCompletionSource<string>();

            if (_config.DestructButtonText != null)
            {
                AddAction(
                    AlertAction.Destructive(
                        _config.DestructButtonText,
                        () => dialogResult.TrySetResult(_config.DestructButtonText)));
            }

            foreach (var optionText in _config.OptionButtons)
            {
                AddAction(
                    AlertAction.Default(
                        optionText, () => dialogResult.TrySetResult(optionText)));
            }

            if (_config.CancelButtonText != null)
            {
                AddAction(
                    AlertAction.Cancel(
                        _config.CancelButtonText,
                        () => dialogResult.TrySetResult(_config.CancelButtonText)));
            }

            Present();

            return dialogResult.Task;
        }
    }
}
