using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Softeq.XToolkit.WhiteLabel.Dialogs;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Threading;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Dialogs
{
    public abstract class ViewControllerDialogBase : IDisposable
    {
        private readonly IViewLocator _viewLocator;
        private UIAlertController? _alertController; // TODO YP: can be move to use local
        private readonly List<(string, UIAlertActionStyle, Action<UIAlertAction>)> _actions;

        protected ViewControllerDialogBase(IViewLocator viewLocator)
        {
            _viewLocator = viewLocator;
            _actions = new List<(string, UIAlertActionStyle, Action<UIAlertAction>)>();

            Title = string.Empty;
            Message = string.Empty;
            Style = UIAlertControllerStyle.Alert;
        }

        protected string Title { get; set; }
        protected string Message { get; set; }
        protected UIAlertControllerStyle Style { get; set; }

        public void Dispose()
        {
            _alertController?.Dispose();
        }

        protected virtual void Show()
        {
            Execute.BeginOnUIThread(() =>
            {
                _alertController = UIAlertController.Create(Title, Message, Style);

                ApplyActions(_alertController, _actions);

                var topViewController = _viewLocator.GetTopViewController();
                topViewController.PresentViewController(_alertController, true, null);
            });
        }

        protected virtual void ApplyActions(UIAlertController? alertController, List<(string, UIAlertActionStyle, Action<UIAlertAction>)> actions)
        {
            foreach (var (text, style, action) in actions)
            {
                alertController.AddAction(UIAlertAction.Create(text, style, action));
            }
        }

        protected virtual void AddAction(string text,
            Action<UIAlertAction> action,
            UIAlertActionStyle style = UIAlertActionStyle.Default)
        {
            _actions.Add((text, style, action));
        }
    }

    public class IosAlertDialog : ViewControllerDialogBase
    {
        private readonly AlertDialogConfig _config;

        public IosAlertDialog(IViewLocator viewLocator, AlertDialogConfig config)
            : base(viewLocator)
        {
            _config = config;

            Title = config.Title;
            Message = config.Message;
        }

        public Task ShowAsync()
        {
            var dialogResult = new TaskCompletionSource<bool>();

            AddAction(
                _config.CancelButtonText,
                _ => dialogResult.TrySetResult(true),
                UIAlertActionStyle.Cancel);

            Show();

            return dialogResult.Task;
        }
    }

    public class IosConfirmDialog : ViewControllerDialogBase
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

            AddAction(
                _config.AcceptButtonText,
                _ => dialogResult.TrySetResult(true),
                _config.IsDestructive
                    ? UIAlertActionStyle.Destructive
                    : UIAlertActionStyle.Default);

            if (_config.CancelButtonText != null)
            {
                AddAction(
                    _config.CancelButtonText,
                    _ => dialogResult.TrySetResult(false),
                    UIAlertActionStyle.Cancel);
            }

            Show();

            return dialogResult.Task;
        }
    }

}
