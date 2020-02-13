// Developed for PAWS-HALO by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Extensions
{
    public static class CommandActionStyleExtensions
    {
        public static UIAlertActionStyle ToNative(this CommandActionStyle actionStyle)
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
