﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.PushNotifications
{
    public enum ForegroundNotificationOptions
    {
        /// <summary>
        ///     Foreground notifications will not be shown in system.
        /// </summary>
        DoNotShow,

        /// <summary>
        ///     Foreground notifications will be shown in system, badge value not applied.
        /// </summary>
        Show,

        /// <summary>
        ///     Foreground notifications will be shown in system with badge value applied.
        /// </summary>
        ShowWithBadge
    }

#pragma warning disable SA1649
    public static class ForegroundNotificationOptionsExtensions
#pragma warning restore SA1649
    {
        /// <summary>
        ///     Helper method to identify if we should show notifications in foreground for these options.
        /// </summary>
        /// <param name="foregroundOptions">Value of current foreground options.</param>
        /// <returns>
        ///     <see langword="true"/> if notifications should be shown in foreground,
        ///     <see langword="false"/> otherwise.
        /// </returns>
        public static bool ShouldShow(this ForegroundNotificationOptions foregroundOptions) =>
            foregroundOptions == ForegroundNotificationOptions.Show
            || foregroundOptions == ForegroundNotificationOptions.ShowWithBadge;
    }
}
