// Developed by Softeq Development Corporation
// http://www.softeq.com

﻿using System;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation.NavigationHelpers;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public static class FrameNavigationServiceExtension
    {
        public static NavigateHelper<T> For<T>(this IFrameNavigationService frameNavigationService) 
            where T : IViewModelBase
        {
            return new NavigateHelper<T>(frameNavigationService);
        }
    }
}
