// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Softeq.XToolkit.WhiteLabel.Dialogs;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;

namespace Softeq.XToolkit.WhiteLabel.iOS.Dialogs
{
    public class IosConfirmDialog : ViewControllerDialogBase, IDialog<bool>
    {
        private readonly ConfirmDialogConfig _config;

        public IosConfirmDialog(IViewLocator viewLocator, ConfirmDialogConfig config)
            : base(viewLocator)
        {
            _config = config;

            Title = config.Title;
            Message = config.Message;
        }

        public Task<bool> ShowAsync()
        {
            var dialogResult = new TaskCompletionSource<bool>();

            if (_config.IsDestructive)
            {
                AddAction(
                    AlertAction.Destructive(
                        _config.AcceptButtonText,
                        () => dialogResult.TrySetResult(true)));
            }
            else
            {
                AddAction(
                    AlertAction.Default(
                        _config.AcceptButtonText,
                        () => dialogResult.TrySetResult(true)));
            }

            if (_config.CancelButtonText != null)
            {
                AddAction(
                    AlertAction.Cancel(
                        _config.CancelButtonText,
                        () => dialogResult.TrySetResult(false)));
            }

            Present();

            return dialogResult.Task;
        }
    }
}
