// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.WhiteLabel.Model
{
    public class TabItem
    {
        public TabItem(string title, string imageName, Type firstViewModelType)
        {
            Title = title;
            ImageKey = imageName;
            FirstViewModelType = firstViewModelType;
        }

        public string Title { get; }

        public string ImageKey { get; }

        public Type FirstViewModelType { get; }

        public static TabItem CreateFor<TViewModel>(string title, string imageName)
        {
            return new TabItem(title, imageName, typeof(TViewModel));
        }
    }
}
