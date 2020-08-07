// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;
using Softeq.XToolkit.WhiteLabel.Threading;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Dialogs
{
    public abstract class AlertViewControllerBase
    {
        private readonly IViewLocator _viewLocator;
        protected readonly IList<AlertAction> Actions;

        protected AlertViewControllerBase(IViewLocator viewLocator)
        {
            _viewLocator = viewLocator;
            Actions = new List<AlertAction>();

            Title = string.Empty;
            Message = null;
            Style = UIAlertControllerStyle.Alert;
        }

        protected string? Title { get; set; }
        protected string? Message { get; set; }
        protected UIAlertControllerStyle Style { get; set; }

        protected virtual void AddAction(AlertAction alertAction)
        {
            Actions.Add(alertAction);
        }

        protected virtual void Present()
        {
            Execute.BeginOnUIThread(() =>
            {
                var alertController = CreateAlertController();

                ApplyActions(alertController, Actions);

                var topViewController = _viewLocator.GetTopViewController();
                topViewController.PresentViewController(alertController, true, null);

                Actions.Clear();
            });
        }

        protected virtual UIAlertController CreateAlertController()
        {
            return UIAlertController.Create(Title, Message, Style);
        }

        protected virtual void ApplyActions(
            UIAlertController alertController,
            IList<AlertAction> alertActions)
        {
            foreach (var alertAction in alertActions)
            {
                alertController.AddAction(UIAlertAction.Create(
                    alertAction.Text,
                    alertAction.Style,
                    _ => alertAction.Action?.Invoke()));
            }
        }
    }
}
