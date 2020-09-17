// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Softeq.XToolkit.Common.Threading;
using Softeq.XToolkit.WhiteLabel.Dialogs;

namespace Softeq.XToolkit.WhiteLabel.Droid.Dialogs
{
    public class DroidActionSheetDialog : AlertDialogBase, IDialog<string>
    {
        private readonly ActionSheetDialogConfig _config;

        public DroidActionSheetDialog(ActionSheetDialogConfig config)
        {
            _config = config;
        }

        public Task<string> ShowAsync()
        {
            var dialogResult = new TaskCompletionSource<string>();

            Execute.BeginOnUIThread(() =>
            {
                var builder = GetBuilder()
                    .SetTitle(_config.Title);

                builder.SetItems(_config.OptionButtons, (sender, args) =>
                {
                    dialogResult.TrySetResult(_config.OptionButtons[args.Which]);
                });

                if (_config.DestructButtonText != null)
                {
                    SetNegativeButton(builder, _config.DestructButtonText, dialogResult, _config.DestructButtonText);
                }

                if (_config.CancelButtonText != null)
                {
                    builder.SetCancelable(true);

                    SetPositiveButton(builder, _config.CancelButtonText, dialogResult, _config.CancelButtonText);
                }

                var dialog = builder.Create();

                if (_config.CancelButtonText != null)
                {
                    HandleDismiss(dialog, dialogResult, _config.CancelButtonText);
                }

                dialog.Show();
            });

            return dialogResult.Task;
        }
    }
}
