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
        private readonly TabItem _model;

        private string _badgeText;
        private bool _isBadgeVisible;

        public RootFrameNavigationViewModel(IFrameNavigationService frameNavigationService, TabItem model)
            : base(frameNavigationService)
        {
            _model = model;
        }

        public string BadgeText
        {
            get => _badgeText;
            set => Set(ref _badgeText, value);
        }

        public bool IsBadgeVisible
        {
            get => _isBadgeVisible;
            set => Set(ref _isBadgeVisible, value);
        }

        public string Title => _model.Title;

        public string ImageKey => _model.ImageKey;

        public override void NavigateToFirstPage()
        {
            FrameNavigationService.NavigateToViewModel(_model.RootViewModelType, true);
        }
    }
}
