// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.Common.Files
{
    public readonly struct StorageInfo : IEquatable<StorageInfo>
    {
        public StorageInfo(StorageType type, ulong totalBytes, ulong freeBytes)
        {
            Type = type;
            TotalBytes = totalBytes;
            FreeBytes = freeBytes;
        }

        public StorageType Type { get; }

        public ulong TotalBytes { get; }

        public ulong FreeBytes { get; }

        /// <inheritdoc />
        public bool Equals(StorageInfo other) =>
            Type == other.Type &&
            FreeBytes == other.FreeBytes &&
            TotalBytes == other.TotalBytes;

        /// <inheritdoc />
        public static bool operator ==(StorageInfo lhs, StorageInfo rhs) => lhs.Equals(rhs);

        /// <inheritdoc />
        public static bool operator !=(StorageInfo lhs, StorageInfo rhs) => !(lhs == rhs);

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is StorageInfo info && Equals(info);
        }

        public override int GetHashCode() => (TotalBytes, FreeBytes, Type).GetHashCode();

        public override string ToString() => $"Type: {Type}, Total: {TotalBytes}, Free: {FreeBytes}";
    }

    public enum StorageType
    {
        Unknown = 0,
        Internal = 1,
        External = 2
    }
}
