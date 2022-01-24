// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Xamarin.Essentials;

namespace Softeq.XToolkit.WhiteLabel.Services
{
    /// <summary>
    ///     Xamarin.Essentials implementation for <see cref="ILauncherService"/> interface.
    /// </summary>
    public class EssentialsLauncherService : ILauncherService
    {
        private readonly ILogger _logger;

        /// <summary>
        ///     Initializes a new instance of the <see cref="EssentialsLauncherService"/> class.
        /// </summary>
        /// <param name="logManager">Loggers factory.</param>
        public EssentialsLauncherService(ILogManager logManager)
        {
            _logger = logManager.GetLogger<EssentialsLauncherService>();
        }

        /// <inheritdoc />
        public void OpenUrl(string url)
        {
            Launcher
                .TryOpenAsync(url)
                .FireAndForget(_logger);
        }

        /// <inheritdoc />
        public void OpenAppSettings()
        {
            AppInfo.ShowSettingsUI();
        }

        /// <inheritdoc />
        public virtual void OpenVideo(string videoUrl)
        {
            OpenUrl(videoUrl);
        }

        /// <inheritdoc />
        public virtual void OpenEmail(string email)
        {
            Email
                .ComposeAsync(string.Empty, string.Empty, email)
                .FireAndForget(_logger);
        }

        /// <inheritdoc />
        public virtual void OpenPhoneNumber(string number)
        {
            PhoneDialer.Open(number);
        }
    }
}
