// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Android.Support.V7.App;
using Plugin.CurrentActivity;

namespace Softeq.XToolkit.WhiteLabel.Droid.Dialogs
{
    public abstract class AlertDialogBase
    {
        protected virtual AlertDialog.Builder GetBuilder()
        {
            var context = CrossCurrentActivity.Current.Activity;
            return new AlertDialog.Builder(context)
                .SetCancelable(false);
        }

        protected virtual void Present(AlertDialog.Builder builder)
        {
            var dialog = builder.Create();
            dialog.Show();
        }

        protected virtual void SetPositiveButton<T>(
            AlertDialog.Builder builder,
            string text,
            TaskCompletionSource<T> dialogResult,
            T value)
        {
            builder.SetPositiveButton(text, (sender, _) =>
            {
                dialogResult.TrySetResult(value);
                var dialog = (AlertDialog) sender;
                dialog.Dismiss();
            });
        }

        protected virtual void SetNegativeButton<T>(
            AlertDialog.Builder builder,
            string text,
            TaskCompletionSource<T> dialogResult,
            T value)
        {
            builder.SetNegativeButton(text, (sender, _) =>
            {
                dialogResult.TrySetResult(value);
                var dialog = (AlertDialog) sender;
                dialog.Dismiss();
            });
        }

        protected virtual void SetNeutralButton<T>(
            AlertDialog.Builder builder,
            string text,
            TaskCompletionSource<T> dialogResult,
            T value)
        {
            builder.SetNeutralButton(text, (sender, _) =>
            {
                dialogResult.TrySetResult(value);
                var dialog = (AlertDialog) sender;
                dialog.Dismiss();
            });
        }
    }
}
