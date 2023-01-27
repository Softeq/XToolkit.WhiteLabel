// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Android.OS;
using Java.IO;
using Softeq.XToolkit.Common.Files;

namespace Softeq.XToolkit.Common.Droid.Files
{
    public class DroidStorageInfoProvider : IStorageInfoProvider
    {
        /// <inheritdoc />
        /// <remarks>
        ///     More modern implementation via <see cref="T:Android.OS.Storage.StorageManager"/> yon can see here:
        ///     <see cref="T:Softeq.XToolkit.WhiteLabel.Droid.Providers.ModernDroidStorageInfoProvider"/>.
        /// </remarks>
        public List<StorageInfo> Current
        {
            get
            {
                var directory = Environment.IsExternalStorageEmulated
                    ? Environment.ExternalStorageDirectory!
                    : Environment.DataDirectory;

                var internalStorageInfo = GetInfoByDirectory(StorageType.Internal, directory!);

                return new List<StorageInfo> { internalStorageInfo };
            }
        }

        private static StorageInfo GetInfoByDirectory(StorageType type, File directory)
        {
            var stats = new StatFs(directory.Path);
            return new StorageInfo(type, (ulong)stats.TotalBytes, (ulong)stats.AvailableBytes);
        }
    }
}
