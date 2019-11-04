// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.WhiteLabel.Model;

namespace Softeq.XToolkit.WhiteLabel.Interfaces
{
    /// <summary>
    ///     Provides information about your application.
    /// </summary>
    public interface IAppInfoService
    {
        /// <summary>
        ///     Application Platform.
        /// </summary>
        Platform Platform { get; }

        /// <summary>
        ///     Application Name.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     Package Name/Application Identifier (com.company.testapp).
        /// </summary>
        string PackageName { get; }

        /// <summary>
        ///     Application Version (1.0.0)
        /// </summary>
        string Version { get; }

        /// <summary>
        ///     Application Build Number (1)
        /// </summary>
        string Build { get; }

        [Obsolete("Use properties.")]
        string GetVersion(bool withBuildNumber);
    }
}
