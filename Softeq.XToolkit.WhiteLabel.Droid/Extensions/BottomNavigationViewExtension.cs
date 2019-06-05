
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;

namespace Softeq.XToolkit.WhiteLabel.Droid.Extensions
{
    public static class BottomNavigationViewExtension 
    {
        public static int GetIndex(this BottomNavigationView bottomNavtigationView, IMenuItem menuItem)
        {
            var menu = bottomNavtigationView.Menu;
            for(int i = 0; i < bottomNavtigationView.Menu.Size(); i++)
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
