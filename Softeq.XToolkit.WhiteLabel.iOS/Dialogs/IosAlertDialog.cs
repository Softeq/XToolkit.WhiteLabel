// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Softeq.XToolkit.WhiteLabel.Dialogs;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;

namespace Softeq.XToolkit.WhiteLabel.iOS.Dialogs
{
    public class IosAlertDialog : ViewControllerDialogBase
    {
        private readonly AlertDialogConfig _config;

        public IosAlertDialog(IViewLocator viewLocator, AlertDialogConfig config)
            : base(viewLocator)
        {
            _config = config;

            Title = config.Title;
            Message = config.Message;
        }

        public Task ShowAsync()
        {
            var dialogResult = new TaskCompletionSource<bool>();

            AddAction(
                AlertAction.Cancel(
                    _config.CancelButtonText,
                    () => dialogResult.TrySetResult(true)));

            Present();

            return dialogResult.Task;
        }
    }
}
