// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Softeq.XToolkit.WhiteLabel.Dialogs;
using Softeq.XToolkit.WhiteLabel.Threading;

namespace Softeq.XToolkit.WhiteLabel.Droid.Dialogs
{
    public class DroidAlertDialog : AlertDialogBase, IDialog<object?>
    {
        private readonly AlertDialogConfig _config;

        public DroidAlertDialog(AlertDialogConfig config)
        {
            _config = config;
        }

        public Task<object?> ShowAsync()
        {
            var dialogResult = new TaskCompletionSource<object?>();

            Execute.BeginOnUIThread(() =>
            {
                var builder = GetBuilder()
                    .SetTitle(_config.Title)
                    .SetMessage(_config.Message);

                SetPositiveButton(builder, _config.CloseButtonText, dialogResult, null);

                Present(builder);
            });

            return dialogResult.Task;
        }
    }
}
