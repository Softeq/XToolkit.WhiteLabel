// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Model;

namespace Softeq.XToolkit.WhiteLabel.Interfaces
{
    public interface IAppService
    {
        Platform Platform { get; }
        string GetVersion(bool withBuildNumber);
    }
}