using System.Collections.Generic;
using System.Linq;
using UserNotifications;

namespace Softeq.XToolkit.PushNotifications.iOS
{
    public class IosNotificationCategoriesProvider : INotificationCategoriesProvider
    {
        public IList<UNNotificationCategory> NotificationCategories { get; } = new List<UNNotificationCategory>();

        protected Dictionary<string, IList<UNNotificationAction>> _actionsForCategories = new Dictionary<string, IList<UNNotificationAction>>();

        public virtual void HandlePushNotificationCustomAction(PushNotificationModel pushNotification, string actionId, string textInput) { }

        protected void AddAction(string categoryId, string actionId, string title, UNNotificationActionOptions options)
        {
            var action = UNNotificationAction.FromIdentifier(actionId, title, options);
            SaveAction(categoryId, action);
        }

        protected void AddTextInputAction(string categoryId, string actionId, string title, UNNotificationActionOptions options,
            string textInputButtonTitle, string textInputPlaceholder)
        {
            var action = UNTextInputNotificationAction.FromIdentifier(actionId, title, options,
                textInputButtonTitle ?? string.Empty, textInputPlaceholder ?? string.Empty);
            SaveAction(categoryId, action);
        }

        protected void AddCategory(string categoryId, string[] intentIdentifiers, UNNotificationCategoryOptions options)
        {
            IList<UNNotificationAction> actions;
            _actionsForCategories.TryGetValue(categoryId, out actions);

            var messageCategory = UNNotificationCategory.FromIdentifier(categoryId,
                actions?.ToArray() ?? new UNNotificationAction[] { }, intentIdentifiers, options);
            NotificationCategories.Add(messageCategory);
        }

        private void SaveAction(string categoryId, UNNotificationAction action)
        {
            if (_actionsForCategories.ContainsKey(categoryId))
            {
                _actionsForCategories[categoryId].Add(action);
            }
            else
            {
                _actionsForCategories.Add(categoryId, new List<UNNotificationAction> { action });
            }
        }
    }
}
