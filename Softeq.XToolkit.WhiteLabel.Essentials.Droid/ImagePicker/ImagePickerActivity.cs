// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Globalization;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using AndroidX.Core.Content;
using Java.IO;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.WhiteLabel.Droid.Providers;
using AUri = Android.Net.Uri;
using IOException = System.IO.IOException;

namespace Softeq.XToolkit.WhiteLabel.Essentials.Droid.ImagePicker
{
#pragma warning disable SA1649
    public static class ImagePickerMode
#pragma warning restore SA1649
    {
        public const int Camera = 1;
        public const int Gallery = 2;
        public const int ImageCropOnly = 3;
    }

    [Activity]
    internal class ImagePickerActivity : Activity
    {
        public const string ModeKey = "Mode";

        private const string ImagesFolder = "Pictures";
        private const string CameraFileUriKey = "CameraFileUri";

        private readonly IImagePickerActivityResultHandler _handler = Dependencies.Container.Resolve<IImagePickerActivityResultHandler>();

        private AUri? _fileUri;
        private Intent _pickIntent = default!;

        public static event EventHandler<Bitmap?>? ImagePicked;

        private Context CurrentContext
        {
            get
            {
                var context = Dependencies.Container.Resolve<IContextProvider>().AppContext;
                return context;
            }
        }

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Activity is recreated after having been stopped by the system
            if (savedInstanceState != null && !savedInstanceState.IsEmpty)
            {
                if (savedInstanceState.GetParcelable(CameraFileUriKey) is AUri fileUri)
                {
                    _fileUri = fileUri;
                }

                return;
            }

            try
            {
                switch (Intent!.GetIntExtra(ModeKey, default))
                {
                    case ImagePickerMode.Camera:
                        CaptureCamera();
                        break;
                    case ImagePickerMode.Gallery:
                        PickImage();
                        break;
                    case ImagePickerMode.ImageCropOnly:
                        HandleImagePickerGalleryResultAsync(Intent.Data).FireAndForget();
                        break;
                }
            }
            catch
            {
                OnImagePicked(null);
            }
            finally
            {
                _pickIntent?.Dispose();
            }
        }

        private async Task HandleImagePickerGalleryResultAsync(AUri? fileUri)
        {
            var bitmap = await _handler
                .HandleImagePickerGalleryResultAsync(this, fileUri)
                .ConfigureAwait(false);
            OnImagePicked(bitmap);
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            if (_fileUri != null)
            {
                outState.PutParcelable(CameraFileUriKey, _fileUri);
            }

            base.OnSaveInstanceState(outState);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent? data)
        {
            HandleOnActivityResult(requestCode, resultCode, data).FireAndForget();
        }

        private async Task HandleOnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent? data)
        {
            if (requestCode == ImagePickerMode.Camera)
            {
                if (resultCode == Result.Ok)
                {
                    var bitmap = await _handler
                        .HandleImagePickerCameraResultAsync(this, _fileUri)
                        .ConfigureAwait(false);
                    OnImagePicked(bitmap);
                }
                else
                {
                    OnImagePicked(null);
                }
            }
            else if (requestCode == ImagePickerMode.Gallery)
            {
                if (resultCode == Result.Ok)
                {
                    HandleImagePickerGalleryResultAsync(data?.Data).FireAndForget();
                }
                else
                {
                    OnImagePicked(null);
                }
            }
            else
            {
                _handler.HandleCustomResultAsync(requestCode, resultCode, data).FireAndForget();
            }
        }

        private void OnImagePicked(Bitmap? bitmap)
        {
            ImagePicked?.Invoke(this, bitmap);
            Finish();
        }

        private void CaptureCamera()
        {
            _pickIntent = new Intent(MediaStore.ActionImageCapture);
            _fileUri = GetOutputMediaFile(CurrentContext, ImagesFolder, null);

            _pickIntent.PutExtra(MediaStore.ExtraOutput, _fileUri);
            StartActivityForResult(_pickIntent, ImagePickerMode.Camera);
        }

        private void PickImage()
        {
            _pickIntent = new Intent(Intent.ActionOpenDocument);
            _pickIntent.AddCategory(Intent.CategoryOpenable);
            _pickIntent.AddFlags(ActivityFlags.GrantReadUriPermission);
            _pickIntent.SetType("image/*");
            StartActivityForResult(_pickIntent, ImagePickerMode.Gallery);
        }

        private AUri GetOutputMediaFile(Context context, string? subdir, string? name)
        {
            subdir ??= string.Empty;

            if (string.IsNullOrWhiteSpace(name))
            {
                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture);
                name = $"IMG_{timestamp}.jpg";
            }

            var directory = context.CacheDir;
            using (var mediaStorageDir = new File(directory, subdir))
            {
                if (!mediaStorageDir.Exists())
                {
                    if (!mediaStorageDir.Mkdirs())
                    {
                        throw new IOException("Couldn't create directory, have you added the WRITE_EXTERNAL_STORAGE permission?");
                    }

                    using (var nomedia = new File(mediaStorageDir, ".nomedia"))
                    {
                        nomedia.CreateNewFile();
                    }
                }

                return FileProvider.GetUriForFile(
                    context,
                    $"{context.PackageName}.fileprovider",
                    new File(GetUniquePath(mediaStorageDir.Path, name!)));
            }
        }

        private string GetUniquePath(string folder, string name)
        {
            var ext = System.IO.Path.GetExtension(name);
            if (ext == string.Empty)
            {
                ext = ".jpg";
            }

            name = System.IO.Path.GetFileNameWithoutExtension(name);

            var fullName = name + ext;
            var i = 1;
            while (System.IO.File.Exists(System.IO.Path.Combine(folder, fullName)))
            {
                fullName = name + "_" + (i++) + ext;
            }

            return System.IO.Path.Combine(folder, fullName);
        }
    }
}
