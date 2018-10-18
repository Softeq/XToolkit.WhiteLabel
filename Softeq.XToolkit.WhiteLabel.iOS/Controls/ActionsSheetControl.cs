// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Common;
using Softeq.XToolkit.Common.Command;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Controls
{
    public class ActionsSheetControl
    {
        private readonly WeakReferenceEx<UIViewController> _parentRef;
        private readonly WeakReferenceEx<IActionSheet> _viewModelRef;

        public ActionsSheetControl(IActionSheet actionSheet, UIViewController parent)
        {
            _parentRef = WeakReferenceEx.Create(parent);
            _viewModelRef = WeakReferenceEx.Create(actionSheet);

            actionSheet.OpenCommand = new RelayCommand(Open);
        }

        public UIColor TintColor { get; set; } = UIColor.Clear;

        private void Open()
        {
            if (_parentRef.Target == null || _viewModelRef.Target == null)
            {
                return;
            }

            var controller = new SupportRotationAlertController(_viewModelRef.Target.ActionHeader, null,
                UIAlertControllerStyle.ActionSheet);

            foreach (var action in _viewModelRef.Target.Actions)
            {
                controller.AddAction(UIAlertAction.Create(action.Title, Convert(action.CommandActionStyle),
                    action.Command.Execute));
            }

            if (!Equals(TintColor, UIColor.Clear))
            {
                controller.View.TintColor = TintColor;
            }

            _parentRef.Target.PresentViewController(controller, true, null);

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