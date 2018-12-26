// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Android.App;
using Plugin.CurrentActivity;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.WhiteLabel.Commands;
using Softeq.XToolkit.WhiteLabel.Droid.Dialogs;
using Softeq.XToolkit.WhiteLabel.Threading;

namespace Softeq.XToolkit.WhiteLabel.Droid.Services
{
    public class DefaultAlertBuilder : IAlertBuilder
    {
        public Task<bool> ShowAlertAsync(string title, string message, string okButtonText,
            string cancelButtonText = null)
        {
            var tcs = new TaskCompletionSource<bool>();

            Execute.BeginOnUIThread(() =>
            {
                var context = CrossCurrentActivity.Current.Activity;

                var builder = new AlertDialog.Builder(context)
                    .SetTitle(title)
                    .SetMessage(message)
                    .SetPositiveButton(okButtonText, (o, e) =>
                    {
                        tcs.TrySetResult(true);
                        var alertDialog = (AlertDialog) o;
                        alertDialog.Dismiss();
                    });

                if (!string.IsNullOrEmpty(cancelButtonText))
                {
                    builder.SetNegativeButton(cancelButtonText, (o, e) =>
                    {
                        tcs.TrySetResult(false);
                        var alertDialog = (AlertDialog) o;
                        alertDialog.Dismiss();
                    });
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