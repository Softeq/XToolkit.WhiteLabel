// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Globalization;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.V4.Content;
using Java.IO;
using Plugin.CurrentActivity;
using AUri = Android.Net.Uri;
using Debug = System.Diagnostics.Debug;
using ImageOrientation = Android.Media.Orientation;
using IOException = System.IO.IOException;
using Stream = System.IO.Stream;

namespace Softeq.XToolkit.WhiteLabel.Droid.ImagePicker
{
    [Activity]
    internal class ImagePickerActivity : Activity
    {
        public const string ModeKey = "Mode";
        public const int CameraMode = 1;
        public const int GalleryMode = 2;

        private const string ImagesFolder = "Pictures";
        private const string CameraFileUriKey = "CameraFileUri";

        private AUri? _fileUri;
        private Intent _pickIntent = default!;

        public static event EventHandler<Bitmap?>? ImagePicked;

        protected override void OnCreate(Bundle savedInstanceState)
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
                switch (Intent.GetIntExtra(ModeKey, default))
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

        protected override void OnSaveInstanceState(Bundle outState)
        {
            if (_fileUri != null)
            {
                outState.PutParcelable(CameraFileUriKey, _fileUri);
            }

            base.OnSaveInstanceState(outState);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            AUri? uri = null;
            Bitmap? bitmap = null;

            if (resultCode != Result.Ok)
            {
                OnImagePicked(null);
                return;
            }

            switch (requestCode)
            {
                case CameraMode:
                    uri = _fileUri;
                    break;
                case GalleryMode:
                    uri = data?.Data;
                    break;
            }

            if (uri != null)
            {
                bitmap = MediaStore.Images.Media.GetBitmap(CrossCurrentActivity.Current.AppContext.ContentResolver, uri);

                if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
                {
                    using (var stream = GetContentStream(CrossCurrentActivity.Current.AppContext, uri))
                    {
                        bitmap = FixRotation(bitmap, new ExifInterface(stream)).Result;
                    }
                }
                else
                {
                    bitmap = FixRotation(bitmap, new ExifInterface(uri.ToString())).Result;
                }
            }

            OnImagePicked(bitmap);
        }

        private void OnImagePicked(Bitmap? bitmap)
        {
            ImagePicked?.Invoke(this, bitmap);
            Finish();
        }

        private void CaptureCamera()
        {
            _pickIntent = new Intent(MediaStore.ActionImageCapture);
            _fileUri = GetOutputMediaFile(CrossCurrentActivity.Current.AppContext, ImagesFolder, null);

            _pickIntent.PutExtra(MediaStore.ExtraOutput, _fileUri);
            StartActivityForResult(_pickIntent, CameraMode);
        }

        private void PickImage()
        {
            _pickIntent = new Intent(Intent.ActionPick);
            _pickIntent.SetType("image/*");
            StartActivityForResult(_pickIntent, GalleryMode);
        }

        private Task<Bitmap> FixRotation(Bitmap originalImage, ExifInterface exif)
        {
            return Task.Run(() =>
            {
                var rotation = GetRotation(exif);
                if (rotation == 0)
                {
                    return originalImage;
                }

                return RotateImage(originalImage, rotation);
            });
        }

        private int GetRotation(ExifInterface exif)
        {
            if (exif == null)
            {
                return 0;
            }

            try
            {
                var orientation =
                    (ImageOrientation) exif.GetAttributeInt(ExifInterface.TagOrientation, (int) ImageOrientation.Normal);

                switch (orientation)
                {
                    case ImageOrientation.Rotate90:
                        return 90;
                    case ImageOrientation.Rotate180:
                        return 180;
                    case ImageOrientation.Rotate270:
                        return 270;
                    default:
                        return 0;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                throw ex;
#else
                return 0;
#endif
            }
        }

        private Bitmap RotateImage(Bitmap originalImage, float degrees)
        {
            var matrix = new Matrix();
            matrix.PostRotate(degrees);
            var rotatedImage = Bitmap.CreateBitmap(originalImage, 0, 0, originalImage.Width, originalImage.Height, matrix, true);
            originalImage.Recycle();
            originalImage.Dispose();
            GC.Collect();
            return rotatedImage;
        }

        private Stream GetContentStream(Context context, AUri uri)
        {
            var stream = Stream.Null;
            try
            {
                stream = context.ContentResolver.OpenInputStream(uri);
            }
            catch (FileNotFoundException ex)
            {
                Debug.WriteLine("Unable to save picked file from disk " + ex);
            }

            return stream;
        }

        private AUri GetOutputMediaFile(Context context, string subdir, string? name)
        {
            subdir = subdir ?? string.Empty;

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

                return FileProvider.GetUriForFile(context, $"{context.PackageName}.fileprovider",
                    new File(GetUniquePath(mediaStorageDir.Path, name!)));
            }
        }

        private static string GetUniquePath(string folder, string name)
        {
            var ext = System.IO.Path.GetExtension(name);
            if (ext == string.Empty)
            {
                ext = ".jpg";
            }

            name = System.IO.Path.GetFileNameWithoutExtension(name);

            var nname = name + ext;
            var i = 1;
            while (System.IO.File.Exists(System.IO.Path.Combine(folder, nname)))
            {
                nname = name + "_" + (i++) + ext;
            }

            return System.IO.Path.Combine(folder, nname);
        }
    }
}
