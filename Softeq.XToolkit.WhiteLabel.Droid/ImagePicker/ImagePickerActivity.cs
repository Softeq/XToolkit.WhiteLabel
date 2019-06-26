
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using Plugin.CurrentActivity;
using Path = System.IO.Path;

namespace Softeq.XToolkit.WhiteLabel.Droid.ImagePicker
{
    [Activity(Label = "ImagePickerActivity")]
    internal class ImagePickerActivity : Activity
    {
        public const string ModeKey = "Mode";
        public const int CameraMode = 1;
        public const int GalleryMode = 2;

        private Android.Net.Uri _fileUri;
        private Intent _pickIntent;

        public static event EventHandler<Bitmap> ImagePicked;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            try
            {
                switch (Intent.GetIntExtra(ModeKey, default(int)))
                {
                    case CameraMode:
                        CaptureCamera();
                        break;
                    case GalleryMode:
                        PickImage();
                        break;
                }
            }
            catch
            {
                OnImagePicked(null);
            }
            finally
            {
                _pickIntent.Dispose();
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            Bitmap bitmap = null;
            switch (requestCode)
            {
                case CameraMode:
                    bitmap = MediaStore.Images.Media.GetBitmap(CrossCurrentActivity.Current.AppContext.ContentResolver, _fileUri);
                    break;
                case GalleryMode:
                    if (data?.Data != null)
                    {
                        bitmap = MediaStore.Images.Media.GetBitmap(CrossCurrentActivity.Current.AppContext.ContentResolver, data.Data);
                    }
                    break;
            }
            OnImagePicked(bitmap);
        }

        private void OnImagePicked(Bitmap bitmap)
        {
            ImagePicked?.Invoke(this, bitmap);
            Finish();
        }

        private void CaptureCamera()
        {
            _pickIntent = new Intent(MediaStore.ActionImageCapture);
            var file = new Java.IO.File(CrossCurrentActivity.Current.AppContext.GetExternalFilesDir(Android.OS.Environment.DirectoryPictures),
                string.Format($"{new Guid().ToString()}.jpg"));
            file.CreateNewFile();

            _fileUri = FileProvider.GetUriForFile(CrossCurrentActivity.Current.AppContext,
                                                  CrossCurrentActivity.Current.AppContext.PackageName + ".fileprovider",
                                                  file);

            _pickIntent.PutExtra(MediaStore.ExtraOutput, _fileUri);
            StartActivityForResult(_pickIntent, CameraMode);
        }

        private void PickImage()
        {
            _pickIntent = new Intent(Intent.ActionPick);
            _pickIntent.SetType("image/*");
            StartActivityForResult(_pickIntent, GalleryMode);
        }
    }
}
