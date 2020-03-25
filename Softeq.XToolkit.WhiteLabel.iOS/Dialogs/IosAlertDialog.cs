// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Softeq.XToolkit.WhiteLabel.Dialogs;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;

namespace Softeq.XToolkit.WhiteLabel.iOS.Dialogs
{
    public class IosAlertDialog : AlertViewControllerBase, IDialog<object?>
    {
        private readonly AlertDialogConfig _config;

        public IosAlertDialog(IViewLocator viewLocator, AlertDialogConfig config)
            : base(viewLocator)
        {
            _config = config;

            Title = config.Title;
            Message = config.Message;
        }

        public Task<object?> ShowAsync()
        {
            var dialogResult = new TaskCompletionSource<object?>();

            AddAction(
                AlertAction.Cancel(
                    _config.CloseButtonText,
                    () => dialogResult.TrySetResult(null)));

            Present();

            return dialogResult.Task;
        }
    }
}
