// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common.Files;
using Softeq.XToolkit.Common.Interfaces;

namespace Softeq.XToolkit.WhiteLabel
{
    public static class WhiteLabelDependencies
    {
        public static IFilesProvider InternalStorageProvider => ServiceLocator.Resolve<InternalStorageProvider>();
    }
}
