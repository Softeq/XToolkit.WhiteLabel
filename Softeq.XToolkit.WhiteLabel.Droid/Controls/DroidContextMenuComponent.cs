// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Droid.Controls
{
    public class DroidContextMenuComponent
    {
        private readonly IDictionary<int, CommandAction> _commandActions = new Dictionary<int, CommandAction>();

        public DroidContextMenuComponent(IReadOnlyList<CommandAction> commandActions)
        {
            commandActions.Apply(commandAction => _commandActions.Add(View.GenerateViewId(), commandAction));
        }

        public PopupMenu BuildMenu(Context context, View anchorView)
        {
            var popup = new PopupMenu(context, anchorView);
            var order = 0;
            foreach (var commandAction in _commandActions)
            {
                popup.Menu.Add(0, commandAction.Key, order++, commandAction.Value.Title);
            }

            popup.MenuItemClick += Popup_MenuItemClick;
            return popup;
        }

        private void Popup_MenuItemClick(object sender, PopupMenu.MenuItemClickEventArgs e)
        {
            ExecuteCommand(e.Item.ItemId, null);
        }

        public void ExecuteCommand(int menuItemId, object parameter)
        {
            if (_commandActions.TryGetValue(menuItemId, out var commandAction))
            {
                commandAction.Command.Execute(parameter);
            }
        }
    }
}
