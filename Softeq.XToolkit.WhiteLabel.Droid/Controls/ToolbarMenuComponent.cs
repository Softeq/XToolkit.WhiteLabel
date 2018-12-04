// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Android.Views;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Droid.Controls
{
    public class ToolbarMenuComponent
    {
        private readonly IDictionary<int, CommandAction> _commandActions = new Dictionary<int, CommandAction>();

        public bool BuildMenu(IList<CommandAction> commandActions, IMenu menu)
        {
            _commandActions.Clear();
            menu.Clear();

            foreach (var commandAction in commandActions)
            {
                var id = View.GenerateViewId();
                _commandActions.Add(id, commandAction);

                var menuItem = menu.Add(0, id, 0, commandAction.Title);

                switch (commandAction.CommandActionStyle)
                {
                    case CommandActionStyle.Destructive:
                        menuItem.SetShowAsActionFlags(ShowAsAction.Always | ShowAsAction.WithText);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            return true;
        }

        public bool HandleClick(IMenuItem menuItem)
        {
            if (_commandActions.TryGetValue(menuItem.ItemId, out var commandAction))
            {
                commandAction.Command.Execute(this);
                return true;
            }

            return false;
        }
    }
}