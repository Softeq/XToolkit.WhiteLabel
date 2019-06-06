// Developed by Softeq Development Corporation
// http://www.softeq.com

﻿
using System;
using Android.Support.Design.Widget;
using Android.Views;

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
