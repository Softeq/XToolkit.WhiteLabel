﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using ObjCRuntime;
using Playground.ViewModels.Controls;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.WhiteLabel.iOS;
using UIKit;

namespace Playground.iOS.ViewControllers.Controls
{
    public partial class PhotoBrowserViewController : ViewControllerBase<PhotoBrowserViewModel>
    {
        public PhotoBrowserViewController(NativeHandle handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            GalleryButton.SetCommand(ViewModel.GalleryCommand);
            CameraButton.SetCommand(ViewModel.CameraCommand);
        }

        protected override void DoAttachBindings()
        {
            base.DoAttachBindings();

            this.Bind(() => ViewModel.Image, OnImage);
        }

        private void OnImage(IDisposable? image)
        {
            ImageView.Image = (UIImage) image!;
        }
    }
}
