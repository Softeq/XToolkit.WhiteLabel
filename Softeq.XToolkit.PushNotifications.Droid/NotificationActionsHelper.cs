// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.App;
using Android.Content;
using AndroidX.Core.App;
using RemoteInput = AndroidX.Core.App.RemoteInput;

namespace Softeq.XToolkit.PushNotifications.Droid
{
    /// <summary>
    ///     A helper class with utils methods for creating notification actions and handling remote input messages.
    /// </summary>
    public static class NotificationActionsHelper
    {
        /// <summary>
        ///     Creates a simple notification action. Note: you can pass these parameters directly
        ///     to <see cref="T:AndroidX.Core.App.NotificationCompat.Builder.AddAction(NotificationCompat.Action)"/>
        ///     without creating an instance of action.
        /// </summary>
        /// <param name="iconResId">Resource id for the icon.</param>
        /// <param name="name">Name for the action button.</param>
        /// <param name="pendingIntent">Pending intent for this action button.</param>
        /// <returns>A notification action.</returns>
        public static NotificationCompat.Action CreateAction(int iconResId, string name, PendingIntent pendingIntent)
        {
            return new NotificationCompat.Action.Builder(iconResId, name, pendingIntent)
                .Build();
        }

        /// <summary>
        ///     Creates a direct reply action. Make sure to create correct pendingIntent.
        /// </summary>
        /// <param name="iconResId">Resource id for the icon.</param>
        /// <param name="name">Name for the action button.</param>
        /// <param name="pendingIntent">Pending intent for this action button.</param>
        /// <param name="remoteInputResultKey">Result key for remote input (needed to obtain user reply).</param>
        /// <param name="remoteInputLabel">A label to be displayed in remote input field.</param>
        /// <returns>A notification action with <see cref="T:AndroidX.Core.App.RemoteInput"/> attached.</returns>
        public static NotificationCompat.Action CreateDirectReplyAction(
            int iconResId,
            string name,
            PendingIntent pendingIntent,
            string remoteInputResultKey,
            string remoteInputLabel)
        {
            var remoteInput = CreateRemoteInput(remoteInputResultKey, remoteInputLabel);
            return new NotificationCompat.Action.Builder(iconResId, name, pendingIntent)
                .AddRemoteInput(remoteInput)
                .Build();
        }

        /// <summary>
        /// Creates an instance of <see cref="T:AndroidX.Core.App.RemoteInput"/> with specified parameters.
        /// </summary>
        /// <param name="resultKey">Result key needed to obtain user reply.</param>
        /// <param name="label">A label to be displayed in remote input field.</param>
        /// <returns>An instance of <see cref="T:AndroidX.Core.App.RemoteInput"/>.</returns>
        public static RemoteInput CreateRemoteInput(string resultKey, string label)
        {
            return new RemoteInput.Builder(resultKey).SetLabel(label).Build();
        }

        /// <summary>
        /// Obtains user reply from the <see cref="T:AndroidX.Core.App.RemoteInput"/>.
        /// </summary>
        /// <param name="intent">
        ///     Intent which opened Activity/BroadcastReceiver/Service and contains data
        ///     from <see cref="T:AndroidX.Core.App.RemoteInput"/>.
        /// </param>
        /// <param name="resultKey">Result key that was used to create the <see cref="T:AndroidX.Core.App.RemoteInput"/>.</param>
        /// <returns>A string message with user reply or null.</returns>
        public static string GetRemoteInputMessage(Intent intent, string resultKey)
        {
            var reply = string.Empty;
            var remoteInput = RemoteInput.GetResultsFromIntent(intent);
            if (remoteInput != null)
            {
                reply = remoteInput.GetCharSequence(resultKey);
            }

            return reply;
        }
    }
}
