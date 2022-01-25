// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.WhiteLabel.Interfaces
{
    /// <summary>
    ///     Launches an application specified by the passed uri.
    /// </summary>
    public interface ILauncherService
    {
        /// <summary>
        ///     Launches the app specified by the uri scheme.
        /// </summary>
        /// <param name="url">string uri scheme.</param>
        void OpenUrl(string url);

        /// <summary>
        ///     Open the settings menu or page for the application.
        /// </summary>
        void OpenAppSettings();

        /// <summary>
        ///     Opens the default video client.
        /// </summary>
        /// <param name="videoUrl">Url to the video.</param>
        void OpenVideo(string videoUrl);

        /// <summary>
        ///     Opens the default email client to allow the user to send the message.
        /// </summary>
        /// <param name="email">Recipient's email.</param>
        void OpenEmail(string email);

        /// <summary>
        ///     Open the phone dialer to a specific phone number.
        /// </summary>
        /// <param name="number">Phone number to initialize the dialer with.</param>
        void OpenPhoneNumber(string number);
    }
}
