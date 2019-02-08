// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Windows.Input;
using Softeq.XToolkit.Common.Command;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Controls
{
    public class ActionsSheetControl : IActionSheet
    {
        private readonly IViewLocator _viewLocator;
        private string _actionHeader;
        private IList<CommandAction> _actions;

        public ActionsSheetControl(IViewLocator viewLocator)
        {
            _viewLocator = viewLocator;
            OpenCommand = new RelayCommand(Open);
        }

        public ICommand OpenCommand { get; }

        public void SetHeader(string header)
        {
            _actionHeader = header;
        }

        public void SetActions(IList<CommandAction> actions)
        {
            _actions = actions;
        }

        public UIColor TintColor { get; set; } = UIColor.Clear;

        private void Open()
        {
            var controller = new SupportRotationAlertController(_actionHeader, null,
                UIAlertControllerStyle.ActionSheet);

            foreach (var action in _actions)
            {
                controller.AddAction(UIAlertAction.Create(action.Title, Convert(action.CommandActionStyle),
                    action.Command.Execute));
            }

            if (!Equals(TintColor, UIColor.Clear))
            {
                controller.View.TintColor = TintColor;
            }

            _viewLocator.GetTopViewController().PresentViewController(controller, true, null);

            if (!Equals(TintColor, UIColor.Clear))
            {
                controller.View.TintColor = TintColor;
            }
        }

        private static UIAlertActionStyle Convert(CommandActionStyle actionStyle)
        {
            switch (actionStyle)
            {
                case CommandActionStyle.Default:
                    return UIAlertActionStyle.Default;
                case CommandActionStyle.Cancel:
                    return UIAlertActionStyle.Cancel;
                case CommandActionStyle.Destructive:
                    return UIAlertActionStyle.Destructive;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}