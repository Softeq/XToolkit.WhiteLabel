﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Android.App;
using Android.OS;
using Android.Widget;
using Playground.ViewModels.Controls;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.WhiteLabel.Droid;

namespace Playground.Droid.Views.Controls
{
    [Activity(Label = "Photo Browser")]
    public class PhotoBrowserActivity : ActivityBase<PhotoBrowserViewModel>
    {
        private Button? _cameraButton;
        private Button? _galleryButton;
        private ImageView? _imageView;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_imagepicker);

            _cameraButton = FindViewById<Button>(Resource.Id.buttonCamera);
            _galleryButton = FindViewById<Button>(Resource.Id.buttonGallery);
            _imageView = FindViewById<ImageView>(Resource.Id.imageView1);

            _cameraButton!.SetCommand(ViewModel.CameraCommand);
            _galleryButton!.SetCommand(ViewModel.GalleryCommand);
        }

        protected override void DoAttachBindings()
        {
            base.DoAttachBindings();

            this.Bind(() => ViewModel.Image, OnImage);
        }

        private void OnImage(IDisposable? image)
        {
            _imageView!.SetImageBitmap((Android.Graphics.Bitmap) image!);
        }
    }
}
