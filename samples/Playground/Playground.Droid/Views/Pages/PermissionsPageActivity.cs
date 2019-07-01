// Developed by Softeq Development Corporation
// http://www.softeq.com

﻿using System;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using Playground.ViewModels.Pages;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.WhiteLabel.Droid;
using Softeq.XToolkit.Permissions;
using Plugin.Permissions;
using Softeq.XToolkit.Common.Interfaces;
using Softeq.XToolkit.Bindings.Extensions;
using Android.Graphics.Drawables;

namespace Playground.Droid.Views.Pages
{
    [Activity(Theme = "@style/AppTheme")]
    public class PermissionsPageActivity : ActivityBase<PermissionsPageViewModel>
    {
        private Button _cameraButton;
        private Button _storageButton;
        private Button _locationButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_permissions);

            _cameraButton = FindViewById<Button>(Resource.Id.buttonCamera);
            _cameraButton.SetCommand(ViewModel.RequestCameraCommand);

            _storageButton = FindViewById<Button>(Resource.Id.buttonStorage);
            _storageButton.SetCommand(ViewModel.RequestStorageCommand);

            _locationButton = FindViewById<Button>(Resource.Id.buttonLocationInUse);
            _locationButton.SetCommand(ViewModel.RequestLocationCommand);
        }

        protected override void DoAttachBindings()
        {
            base.DoAttachBindings();
            var converter = new ColorConverter();

            this.Bind(() => ViewModel.CameraGranted, () => _cameraButton.Background, converter);
            this.Bind(() => ViewModel.StorageGranted, () => _storageButton.Background, converter);
            this.Bind(() => ViewModel.LocationGranted, () => _locationButton.Background, converter);
        }

        private class ColorConverter : IConverter<Drawable, bool>
        {
            public Drawable ConvertValue(bool TIn, object parameter = null, string language = null)
            {
                return new ColorDrawable(TIn ? Color.Green : Color.Red);
            }

            public bool ConvertValueBack(Drawable value, object parameter = null, string language = null)
            {
                return ((ColorDrawable)value).Color == Color.Green;
            }
        }
    }
}
