// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Model;

namespace Softeq.XToolkit.WhiteLabel.Interfaces
{
    /// <summary>
    ///     Provides information about your application.
    /// </summary>
    public interface IAppInfoService
    {
        /// <summary>
        ///     Gets application Platform.
        /// </summary>
        Platform Platform { get; }

        /// <summary>
        ///     Gets application Name.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     Gets package Name/Application Identifier (com.company.test_app).
        /// </summary>
        string PackageName { get; }

        /// <summary>
        ///     Gets application Version (1.0.0).
        /// </summary>
        string Version { get; }

        /// <summary>
        ///     Gets application Build Number (1).
        /// </summary>
        string Build { get; }
    }
}
