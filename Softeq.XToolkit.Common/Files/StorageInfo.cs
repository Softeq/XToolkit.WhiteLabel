// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.Common.Files
{
    /// <summary>
    ///     Contains info about storage.
    /// </summary>
    public readonly struct StorageInfo
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="StorageInfo"/> struct.
        /// </summary>
        /// <param name="type">Type of storage.</param>
        /// <param name="totalBytes">Total storage bytes.</param>
        /// <param name="freeBytes">Free storage bytes.</param>
        public StorageInfo(StorageType type, ulong totalBytes, ulong freeBytes)
        {
            Type = type;
            TotalBytes = totalBytes;
            FreeBytes = freeBytes;
        }

        /// <summary>
        ///     Gets storage type.
        /// </summary>
        public StorageType Type { get; }

        /// <summary>
        ///     Gets total storage bytes.
        /// </summary>
        public ulong TotalBytes { get; }

        /// <summary>
        ///     Gets available free storage bytes.
        /// </summary>
        public ulong FreeBytes { get; }

        /// <inheritdoc />
        public override string ToString() => $"Type: {Type}, Total: {TotalBytes}, Free: {FreeBytes}";
    }

    /// <summary>
    ///     Storage type.
    /// </summary>
#pragma warning disable SA1201
    public enum StorageType
#pragma warning restore SA1201
    {
        /// <summary>
        ///     Unknown storage.
        /// </summary>
        Unknown = 0,

        /// <summary>
        ///     Internal storage.
        /// </summary>
        Internal = 1,

        /// <summary>
        ///     External storage.
        /// </summary>
        External = 2
    }
}
