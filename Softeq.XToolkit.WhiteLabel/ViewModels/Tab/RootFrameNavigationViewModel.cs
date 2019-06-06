// Developed by Softeq Development Corporation
// http://www.softeq.com

﻿using System;
using Softeq.XToolkit.WhiteLabel.Model;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.ViewModels.Tab
{
    public class RootFrameNavigationViewModel : RootFrameNavigationViewModelBase
    {
        public RootFrameNavigationViewModel(IFrameNavigationService frameNavigationService, TabItem model)
            : base(frameNavigationService)
        {
            Model = model;
        }
       
        public TabItem Model { get; set; }

        public override void NavigateToFirstPage()
        {
            FrameNavigationService.NavigateToViewModel(Model.RootViewModelType, true);
        }
    }
}
