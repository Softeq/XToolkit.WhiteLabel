// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.IO;
using System.Threading.Tasks;

namespace Softeq.XToolkit.WhiteLabel.ImagePicker
{
    public enum ImageExtension
    {
        Unknown,
        Png,
        Jpg
    }

    /// <summary>
    /// Image picker result.
    /// </summary>
    public abstract class ImagePickerResult : IDisposable
    {
        /// <summary>
        /// Quality of image.
        /// </summary>
        public float Quality { get; set; }

        /// <summary>
        /// Platform-specific object of the image.
        /// </summary>
        public IDisposable ImageObject { get; set; }

        /// <summary>
        /// Extension of image.
        /// </summary>
        public ImageExtension ImageExtension { get; set; }

        /// <summary>
        /// File extension for image.
        /// </summary>
        public string Extension
        {
            get
            {
                var converter = new ImageExtensionToStringConverter();
                return converter.ConvertValue(ImageExtension);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ImagePickerResult()
        {
            Dispose(false);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                ImageObject?.Dispose();
            }
        }

        /// <summary>
        /// Platform-specific a decode to managed stream.
        /// </summary>
        /// <returns></returns>
        public abstract Task<Stream> GetStream();
    }
}