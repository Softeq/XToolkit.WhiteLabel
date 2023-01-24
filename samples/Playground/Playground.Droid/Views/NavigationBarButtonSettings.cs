// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Windows.Input;
using Android.Graphics.Drawables;

namespace Playground.Droid.Views
{
    public class NavigationBarButtonSettings
    {
        public NavigationBarButtonSettings(
            string title,
            bool isBold,
            Drawable image,
            ICommand command)
        {
            Title = title;
            IsBold = isBold;
            Image = image;
            Command = command;
        }

        public string Title { get; }
        public bool IsBold { get; }
        public Drawable Image { get; }
        public ICommand Command { get; }
    }
}
