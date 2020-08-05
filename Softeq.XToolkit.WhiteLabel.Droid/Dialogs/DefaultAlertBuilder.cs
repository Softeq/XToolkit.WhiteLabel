// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Droid.Providers;
using Softeq.XToolkit.WhiteLabel.Threading;
using AlertDialog = AndroidX.AppCompat.App.AlertDialog;

namespace Softeq.XToolkit.WhiteLabel.Droid.Dialogs
{
    [Obsolete("Use DroidAlertDialog or DroidConfirmDialog instead.")]
    public class DefaultAlertBuilder : AlertDialogBase, IAlertBuilder
    {
        public Task<bool> ShowAlertAsync(string title, string message, string okButtonText,
            string? cancelButtonText = null)
        {
            var tcs = new TaskCompletionSource<bool>();

            Execute.BeginOnUIThread(() =>
            {
                var context = Dependencies.Container.Resolve<IActivityProvider>().Current;

                var builder = new AlertDialog.Builder(context)
                    .SetTitle(title)
                    .SetMessage(message);

                SetPositiveButton(builder, okButtonText, tcs, true);

                if (cancelButtonText != null)
                {
                    SetNegativeButton(builder, cancelButtonText, tcs, false);
                }

                var dialog = builder.Create();
                var dismissCommand = new RelayCommand<TaskCompletionSource<bool>>(x => { x.TrySetResult(false); });
                dialog.SetCommand(nameof(dialog.DismissEvent), dismissCommand, tcs);
                dialog.Show();
            });

            return tcs.Task;
        }
    }
}
