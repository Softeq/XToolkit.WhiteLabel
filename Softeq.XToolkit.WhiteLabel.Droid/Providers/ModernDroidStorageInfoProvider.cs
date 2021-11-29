// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using Android.App.Usage;
using Android.Content;
using Android.OS;
using Android.OS.Storage;
using Java.IO;
using Java.Util;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Common.Files;

namespace Softeq.XToolkit.WhiteLabel.Droid.Providers
{
    public class ModernDroidStorageInfoProvider : IStorageInfoProvider
    {
        private readonly IContextProvider _contextProvider;

        public ModernDroidStorageInfoProvider(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public List<StorageInfo> Current
        {
            get
            {
                var storageManager = StorageManager.FromContext(_contextProvider.AppContext);
                if (storageManager == null)
                {
                    throw new InvalidOperationException($"Can't resolve '{nameof(StorageManager)}'");
                }

                var storageStatsManager = _contextProvider.AppContext.GetSystemService(Context.StorageStatsService) as StorageStatsManager;
                if (storageStatsManager == null)
                {
                    throw new InvalidOperationException($"Can't resolve '{nameof(StorageStatsManager)}'");
                }

                return _contextProvider.AppContext.GetExternalFilesDirs(null)
                    .EmptyIfNull()
                    .Select(directory => GetInfoByDirectory(directory, storageManager, storageStatsManager))
                    .ToList();
            }
        }

        // YP: Workaround over the storageManager.getStorageVolumes + getDirectory (API 30)
        // https://stackoverflow.com/a/57126727/5925490
        private static StorageInfo GetInfoByDirectory(
            File directory,
            StorageManager storageManager,
            StorageStatsManager storageStatsManager)
        {
            var type = StorageType.Unknown;
            var storageVolume = storageManager.GetStorageVolume(directory);

            if (storageVolume != null)
            {
                type = storageVolume.IsRemovable ? StorageType.External : StorageType.Internal;

                // YP: skip non-primary volumes, because should be used `storageVolume.getStorageUuid` (API 31)
                if (storageVolume.IsPrimary)
                {
                    return GetInfoByVolume(type, storageVolume.Uuid, storageStatsManager);
                }
            }

            var stats = new StatFs(directory.Path);
            return new StorageInfo(type, (ulong)stats.TotalBytes, (ulong)stats.AvailableBytes);
        }

        private static StorageInfo GetInfoByVolume(
            StorageType type,
            string? storageVolumeUuid,
            StorageStatsManager storageStatsManager)
        {
            var storageUuid = string.IsNullOrEmpty(storageVolumeUuid)
                ? StorageManager.UuidDefault
                : UUID.FromString(storageVolumeUuid!);

            if (storageUuid == null)
            {
                throw new InvalidOperationException($"Invalid storage volume UUID: '{storageVolumeUuid}'.");
            }

            var totalBytes = storageStatsManager.GetTotalBytes(storageUuid);
            var freeBytes = storageStatsManager.GetFreeBytes(storageUuid);

            return new StorageInfo(type, (ulong)totalBytes, (ulong)freeBytes);
        }
    }
}
