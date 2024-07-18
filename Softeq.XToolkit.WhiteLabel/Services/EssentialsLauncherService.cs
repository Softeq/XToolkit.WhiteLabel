// Developed by Softeq Development Corporation
// http://www.softeq.com

using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.ApplicationModel.Communication;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Common.Threading;
using Softeq.XToolkit.WhiteLabel.Interfaces;

namespace Softeq.XToolkit.WhiteLabel.Services
{
    /// <summary>
    ///     Xamarin.Essentials thread-safe implementation for <see cref="ILauncherService"/> interface.
    /// </summary>
    public class EssentialsLauncherService : ILauncherService
    {
        private readonly ILogger _logger;

        /// <summary>
        ///     Initializes a new instance of the <see cref="EssentialsLauncherService"/> class.
        /// </summary>
        /// <param name="logManager">An instance of loggers factory.</param>
        public EssentialsLauncherService(ILogManager logManager)
        {
            _logger = logManager.GetLogger<EssentialsLauncherService>();
        }

        /// <inheritdoc />
        public void OpenUrl(string url)
        {
            Execute.BeginOnUIThread(() =>
            {
                Launcher
                    .TryOpenAsync(url)
                    .FireAndForget(_logger);
            });
        }

        /// <inheritdoc />
        public void OpenAppSettings()
        {
            Execute.BeginOnUIThread(() =>
            {
                AppInfo.ShowSettingsUI();
            });
        }

        /// <inheritdoc />
        public virtual void OpenVideo(string videoUrl)
        {
            OpenUrl(videoUrl);
        }

        /// <inheritdoc />
        public virtual void OpenEmail(string email)
        {
            OpenUrl($"mailto:{email}");
        }

        /// <inheritdoc />
        public virtual void OpenPhoneNumber(string number)
        {
            Execute.BeginOnUIThread(() =>
            {
                PhoneDialer.Open(number);
            });
        }
    }
}
