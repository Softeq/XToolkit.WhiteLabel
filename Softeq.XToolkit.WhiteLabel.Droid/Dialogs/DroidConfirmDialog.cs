// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Softeq.XToolkit.Common.Threading;
using Softeq.XToolkit.WhiteLabel.Dialogs;

namespace Softeq.XToolkit.WhiteLabel.Droid.Dialogs
{
    public class DroidConfirmDialog : AlertDialogBase, IDialog<bool>
    {
        private readonly ConfirmDialogConfig _config;

        public DroidConfirmDialog(ConfirmDialogConfig config)
        {
            _config = config;
        }

        public Task<bool> ShowAsync()
        {
            var dialogResult = new TaskCompletionSource<bool>();

            Execute.BeginOnUIThread(() =>
            {
                var builder = GetBuilder()
                    .SetTitle(_config.Title)
                    .SetMessage(_config.Message);

                if (_config.IsDestructive) // YP: correct ordering dialog buttons for styling
                {
                    SetNegativeButton(builder, _config.AcceptButtonText, dialogResult, true);

                    if (_config.CancelButtonText != null)
                    {
                        SetNeutralButton(builder, _config.CancelButtonText, dialogResult, false);
                    }
                }
                else
                {
                    SetPositiveButton(builder, _config.AcceptButtonText, dialogResult, true);

                    if (_config.CancelButtonText != null)
                    {
                        SetNegativeButton(builder, _config.CancelButtonText, dialogResult, false);
                    }
                }

                Present(builder);
            });

            return dialogResult.Task;
        }
    }
}
