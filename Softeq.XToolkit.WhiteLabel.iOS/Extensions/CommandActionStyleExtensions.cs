// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.ComponentModel;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Extensions
{
    public static class CommandActionStyleExtensions
    {
        public static UIAlertActionStyle ToNative(this CommandActionStyle actionStyle)
        {
            return actionStyle switch
            {
                CommandActionStyle.Default => UIAlertActionStyle.Default,
                CommandActionStyle.Cancel => UIAlertActionStyle.Cancel,
                CommandActionStyle.Destructive => UIAlertActionStyle.Destructive,
                _ => throw new InvalidEnumArgumentException(
                    nameof(actionStyle), (int)actionStyle, actionStyle.GetType())
            };
        }
    }
}
