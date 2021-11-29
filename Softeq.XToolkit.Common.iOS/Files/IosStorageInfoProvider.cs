// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Foundation;
using ObjCRuntime;
using Softeq.XToolkit.Common.Files;
using UIKit;

namespace Softeq.XToolkit.Common.iOS.Files
{
    public class IosStorageInfoProvider : IStorageInfoProvider
    {
        /// <inheritdoc />
        public List<StorageInfo> Current
        {
            get
            {
                var homePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var fileSystemAttributes = GetFileSystemAttributesForPath(homePath);

                StorageInfo info;

                Debug.WriteLine($"System Total: {fileSystemAttributes.Size}");
                if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                {
                    var volumeCapacities = GetVolumeAvailableCapacities(homePath);
                    info = new StorageInfo(StorageType.Internal, fileSystemAttributes.Size, volumeCapacities.AvailableCapacityForImportantUsage);

                    Debug.WriteLine($"AvailableCapacity: {volumeCapacities.AvailableCapacity}");
                    Debug.WriteLine($"AvailableCapacityForImportantUsage: {volumeCapacities.AvailableCapacityForImportantUsage}");
                    Debug.WriteLine($"AvailableCapacityForOpportunisticUsage: {volumeCapacities.AvailableCapacityForOpportunisticUsage}");
                }
                else
                {
                    Debug.WriteLine($"System Free: {fileSystemAttributes.FreeSize}");
                    info = new StorageInfo(StorageType.Internal, fileSystemAttributes.Size, fileSystemAttributes.FreeSize);
                }

                return new List<StorageInfo> { info };
            }
        }

        public NSFileSystemAttributes GetFileSystemAttributesForPath(string path)
        {
            var attributes = NSFileManager.DefaultManager.GetFileSystemAttributes(path, out NSError error);

            if (error != null)
            {
                throw new NSErrorException(error);
            }

            return attributes;
        }

        /// <summary>
        /// Total available capacity in bytes for "Important" resources,
        /// including space expected to be cleared by purging non-essential and cached resources.
        ///
        /// "Important" means something that the user or application clearly expects to be present on the local system,
        /// but is ultimately replaceable. This would include items that the user has explicitly requested via the UI,
        /// and resources that an application requires in order to provide functionality.
        ///
        /// Examples:
        ///     A video that the user has explicitly requested to watch but has not yet finished watching or
        ///     an audio file that the user has requested to download.
        ///     This value should not be used in determining if there is room for an irreplaceable resource.
        ///     In the case of irreplaceable resources, always attempt to save the resource regardless of available capacity and
        ///     handle failure as gracefully as possible.
        /// </summary>
        /// <remarks>
        ///     More details:
        ///     <see href="https://developer.apple.com/documentation/foundation/nsurlresourcekey/checking_volume_storage_capacity?language=objc" />.
        /// </remarks>
        /// <param name="path">Target path. Default is file system root.</param>
        /// <returns>
        ///     Available capacity values:
        ///     - AvailableCapacity - available capacity in bytes, minimal.
        ///     - AvailableCapacityForImportantUsage - capacity in bytes for storing important resources, maximum available space.
        ///     - AvailableCapacityForOpportunisticUsage - capacity in bytes for storing nonessential resources.
        /// </returns>
        /// <exception cref="NSErrorException">Native error during request info from the system.</exception>
        [Introduced(PlatformName.iOS, 11, 0)]
        public (ulong AvailableCapacity, ulong AvailableCapacityForImportantUsage, ulong AvailableCapacityForOpportunisticUsage)
            GetVolumeAvailableCapacities(string path = "/")
        {
            var keys = new[]
            {
                NSUrl.VolumeAvailableCapacityKey,
                NSUrl.VolumeAvailableCapacityForImportantUsageKey,
                NSUrl.VolumeAvailableCapacityForOpportunisticUsageKey
            };

            var dict = NSUrl.FromFilename(path).GetResourceValues(keys, out NSError error);
            if (error != null)
            {
                throw new NSErrorException(error);
            }

            var availableCapacity = GetNumber(dict, NSUrl.VolumeAvailableCapacityKey);
            var capacityForImportantUsage = GetNumber(dict, NSUrl.VolumeAvailableCapacityForImportantUsageKey);
            var capacityForOpportunisticUsage = GetNumber(dict, NSUrl.VolumeAvailableCapacityForOpportunisticUsageKey);

            return (availableCapacity, capacityForImportantUsage, capacityForOpportunisticUsage);
        }

        protected static ulong GetNumber(NSDictionary dict, NSString key)
        {
            var value = dict.ObjectForKey(key) as NSNumber;
            return value?.UInt64Value ?? 0;
        }
    }
}
