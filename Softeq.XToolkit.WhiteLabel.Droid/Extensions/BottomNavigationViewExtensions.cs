// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Android.Support.Design.Widget;
using Android.Views;

namespace Softeq.XToolkit.WhiteLabel.Droid.Extensions
{
    public static class BottomNavigationViewExtensions
    {
        public static int GetIndex(this BottomNavigationView bottomNavigationView, IMenuItem menuItem)
        {
            var menu = bottomNavigationView.Menu;
            for (var i = 0; i < bottomNavigationView.Menu.Size(); i++)
            {
                var item = menu.GetItem(i);
                if (ReferenceEquals(menuItem, item))
                {
                    return i;
                }
            }
            throw new Exception("MenuItem not found");
        }
    }
}
