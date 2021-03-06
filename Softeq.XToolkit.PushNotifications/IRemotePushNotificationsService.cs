﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;

namespace Softeq.XToolkit.PushNotifications
{
    public interface IRemotePushNotificationsService
    {
        /// <summary>
        ///     Send prepared token for push notifications to server.
        /// </summary>
        /// <param name="pushNotificationsToken">Push notifications token.</param>
        /// <returns>
        ///     Task with <see langword="true"/> result if token was sent successfully
        ///     or <see langword="false"/> result otherwise.
        /// </returns>
        Task<bool> SendPushNotificationsToken(string pushNotificationsToken);

        /// <summary>
        ///     Request removing token for push notifications from server.
        /// </summary>
        /// <param name="pushNotificationsToken">Push notifications token.</param>
        /// <returns>
        ///     Task with <see langword="true"/> result if token was removed successfully
        ///     or <see langword="false"/> result otherwise.
        /// </returns>
        Task<bool> RemovePushNotificationsToken(string pushNotificationsToken);
    }
}
