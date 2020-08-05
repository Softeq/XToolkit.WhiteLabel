// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.Views;
using Google.Android.Material.Snackbar;
using Java.Lang;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.WhiteLabel.Droid.Extensions;
using Softeq.XToolkit.WhiteLabel.Droid.Providers;
using Softeq.XToolkit.WhiteLabel.Droid.ViewComponents;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Threading;

namespace Softeq.XToolkit.WhiteLabel.Droid.Services
{
    public class DroidToastService
    {
        private readonly ILogger _logger;
        private readonly Queue<ToastModel> _queue;
        private readonly ToastSettings _toastSettings;
        private readonly IActivityProvider _activityProvider;

        private bool _isBusy;

        public DroidToastService(
            ToastSettings toastSettings,
            ILogManager logManager,
            IActivityProvider activityProvider)
        {
            _toastSettings = toastSettings;
            _activityProvider = activityProvider;
            _logger = logManager.GetLogger<DroidToastService>();
            _queue = new Queue<ToastModel>();
        }

        public void Enqueue(ToastModel model)
        {
            _queue.Enqueue(model);
            StartHandleImp().FireAndForget(_logger);
        }

        private async Task StartHandleImp()
        {
            if (_isBusy)
            {
                return;
            }

            _isBusy = true;

            while (_queue.Count > 0)
            {
                var action = _queue.Dequeue();
                await HandleTask(action).ConfigureAwait(false);
            }

            _isBusy = false;
        }

        private Task HandleTask(ToastModel model)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();

            Execute.BeginOnUIThread(() =>
            {
                var toastContainerComponent = default(ToastContainerComponent);

                var activity = _activityProvider.Current;

                if (activity is ActivityBase activityBase)
                {
                    toastContainerComponent =
                        activityBase.ViewComponents.FirstOrDefault(x => x.Key == nameof(ToastContainerComponent)) as
                            ToastContainerComponent;
                }

                var view = toastContainerComponent == null
                    ? activity.FindViewById<ViewGroup>(Android.Resource.Id.Content).GetChildAt(0)
                    : activity.FindViewById<ViewGroup>(toastContainerComponent.ContainerId);

                if (view == null)
                {
                    taskCompletionSource.TrySetResult(false);
                    return;
                }

                var snackbar = Snackbar
                    .Make(view, model.Label, Snackbar.LengthLong)
                    .SetAction(model.CommandAction.Title, x => { model.CommandAction.Command.Execute(x); });
                var snackbarView = snackbar.View;
                snackbarView.SetBackgroundResource(_toastSettings.NotificationBgColor);
                snackbar.SetActionTextColor(snackbarView.GetColor(_toastSettings.NotificationTextColor));
                var callback = new SnackbarCallback(taskCompletionSource, model.AltCommandAction);
                snackbar.AddCallback(callback);
                snackbar.Show();
            });

            return taskCompletionSource.Task;
        }

        private class SnackbarCallback : BaseTransientBottomBar.BaseCallback
        {
            private readonly TaskCompletionSource<bool> _taskCompletionSource;

            private CommandAction? _commandAction;

            public SnackbarCallback(
                TaskCompletionSource<bool> taskCompletionSource,
                CommandAction commandAction)
            {
                _taskCompletionSource = taskCompletionSource;
                _commandAction = commandAction;
            }

            public override void OnDismissed(Object transientBottomBar, int eventCode)
            {
                base.OnDismissed(transientBottomBar, eventCode);

                _commandAction?.Command.Execute(this);
                _commandAction = null;

                _taskCompletionSource.TrySetResult(true);
            }
        }
    }

    public class ToastModel
    {
        public ToastModel(string label, CommandAction commandAction, CommandAction altCommandAction)
        {
            Label = label;
            CommandAction = commandAction;
            AltCommandAction = altCommandAction;
        }

        public string Label { get; set; }

        public CommandAction CommandAction { get; set; }

        public CommandAction AltCommandAction { get; set; }
    }

    public class ToastSettings
    {
        public int NotificationBgColor { get; set; }
        public int NotificationTextColor { get; set; }
    }
}
