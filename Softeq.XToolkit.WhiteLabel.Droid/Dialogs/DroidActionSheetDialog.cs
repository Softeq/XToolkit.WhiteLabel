// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Softeq.XToolkit.WhiteLabel.Dialogs;
using Softeq.XToolkit.WhiteLabel.Threading;

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
                    SetPositiveButton(builder, _config.CancelButtonText, dialogResult, _config.CancelButtonText);
                }

                Present(builder);
            });

            return dialogResult.Task;
        }
    }
}
