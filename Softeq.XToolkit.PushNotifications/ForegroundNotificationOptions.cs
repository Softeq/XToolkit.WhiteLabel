namespace Softeq.XToolkit.PushNotifications
{
    public enum ForegroundNotificationOptions
    {
        /// <summary>
        /// Foreground notifications will not be shown in system
        /// </summary>
        DoNotShow,

        /// <summary>
        /// Foreground notifications will be shown in system, badge value not applied
        /// </summary>
        Show,

        /// <summary>
        /// Foreground notifications will be shown in system with badge value applied
        /// </summary>
        ShowWithBadge
    }

    public static class ForegroundNotificationOptionsExtensions
    {
        /// <summary>
        /// Helper method to identify if we should show notifications in foreground for these options
        /// </summary>
        /// <param name="foregroundOptions">Value of current foreground options</param>
        /// <returns>True if notifications should be shown in foreground, False otherwise</returns>
        public static bool DoShow(this ForegroundNotificationOptions foregroundOptions) =>
            foregroundOptions == ForegroundNotificationOptions.Show
            || foregroundOptions == ForegroundNotificationOptions.ShowWithBadge;
    }
}
