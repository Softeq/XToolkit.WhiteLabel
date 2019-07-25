// Developed by Softeq Development Corporation
// http://www.softeq.com

﻿using System;

namespace Softeq.XToolkit.WhiteLabel.Model
{
    public class TabItem
    {
        public TabItem(string title, string imageName, Type rootViewModelType)
        {
            Title = title;
            ImageKey = imageName;
            RootViewModelType = rootViewModelType;
        }

        public string Title { get; }

        public string ImageKey { get; }

        public Type RootViewModelType { get; }
    }
}
