// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;

namespace Softeq.XToolkit.Common.Interfaces
{
    public interface IFullScreenPhotosService
    {
        void DisplayImages(IList<string> urls, int startIndex = 0);
    }
}
