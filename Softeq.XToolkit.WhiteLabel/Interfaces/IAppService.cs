// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.WhiteLabel.Model;

namespace Softeq.XToolkit.WhiteLabel.Interfaces
{
    public interface IAppService
    {
        Platform Platform { get; }

        string Name { get; }

        string PackageName { get; }

        string Version { get; }

        string Build { get; }

        [Obsolete("Use properties.")]
        string GetVersion(bool withBuildNumber);
    }
}