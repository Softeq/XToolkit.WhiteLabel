// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Softeq.XToolkit.Common.Files
{
    /// <summary>
    ///     Provides info about available storages.
    /// </summary>
    public interface IStorageInfoProvider
    {
        /// <summary>
        ///     Gets info about available storages.
        /// </summary>
        List<StorageInfo> Current { get; }
    }
}
