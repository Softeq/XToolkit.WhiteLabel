// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Droid.Providers;
using AlertDialog = AndroidX.AppCompat.App.AlertDialog;

namespace Softeq.XToolkit.WhiteLabel.Droid.Dialogs
{
    public abstract class AlertDialogBase
    {
        private readonly IContextProvider _contextProvider = Dependencies.Container.Resolve<IContextProvider>();

        protected virtual AlertDialog.Builder GetBuilder()
        {
            var context = _contextProvider.CurrentActivity;
            return new AlertDialog.Builder(context)
                .SetCancelable(false);
        }

        protected virtual void Present(AlertDialog.Builder builder)
        {
            var activity = _contextProvider.CurrentActivity;
            if (activity != null && !activity.IsFinishing && !activity.IsDestroyed)
            {
                var dialog = builder.Create();
                dialog.Show();
            }
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

        protected virtual void HandleDismiss<T>(
            AlertDialog dialog,
            TaskCompletionSource<T> dialogResult,
            T value)
        {
            var dismissCommand = new RelayCommand<TaskCompletionSource<T>>(x =>
            {
                x.TrySetResult(value);
            });
            dialog.SetCommand(nameof(dialog.DismissEvent), dismissCommand, dialogResult);
        }
    }
}
