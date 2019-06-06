using System;
namespace Softeq.XToolkit.WhiteLabel.Model
{
    public class TabItem
    {
        public TabItem(string title, string imageName, Type rootViewModelType)
        {
            Title = title;
            ImageName = imageName;
            RootViewModelType = rootViewModelType;
        }

        public string Title { get; }

        public string ImageName { get; }

        public Type RootViewModelType { get; }
    }
}
