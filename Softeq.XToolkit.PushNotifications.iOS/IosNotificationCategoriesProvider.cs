using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;
using UserNotifications;

namespace Softeq.XToolkit.PushNotifications.iOS
{
    public class IosNotificationCategoriesProvider : INotificationCategoriesProvider
    {
        public IList<UNNotificationCategory> NotificationCategories { get; } = new List<UNNotificationCategory>();

        protected Dictionary<string, IList<UNNotificationAction>> _actionsForCategories = new Dictionary<string, IList<UNNotificationAction>>();

        public virtual void HandlePushNotificationCustomAction(PushNotificationModel pushNotification, string actionId, string textInput) { }

        /// <summary>
        /// A method to create and save an Action for a specific Category Id. See <see cref="UNNotificationAction.FromIdentifier"/>
        /// NOTE: should be called before AddCategory for this Category Id in order to have action attached to the category
        /// </summary>
        /// <param name="categoryId">Identifier for the Category connected with this Action</param>
        /// <param name="actionId">The string that you use internally to identify the Action. This string must be unique among all of your app's supported Actions</param>
        /// <param name="title">Localized string that will be displayed on Action button (probably localized)</param>
        /// <param name="options">Additional options describing how the Action behaves</param>
        protected void AddAction(string categoryId, string actionId, string title, UNNotificationActionOptions options)
        {
            var action = UNNotificationAction.FromIdentifier(actionId, title, options);
            SaveAction(categoryId, action);
        }

        /// <summary>
        /// A method to create and save an Action with Text Input for a specific Category Id. See <see cref="UNTextInputNotificationAction.FromIdentifier"/>
        /// NOTE: should be called before AddCategory for this Category Id in order to have action attached to the category
        /// </summary>
        /// <param name="categoryId">Identifier for the Category connected with this Action</param>
        /// <param name="actionId">The string that you use internally to identify the Action. This string must be unique among all of your app's supported Actions</param>
        /// <param name="title">Localized string that will be displayed on Action button</param>
        /// <param name="options">Additional options describing how the Action behaves</param>
        /// <param name="textInputButtonTitle">The localized title of the text input button that is displayed to the user</param>
        /// <param name="textInputPlaceholder">The localized placeholder text to display in the text input field</param>
        protected void AddTextInputAction(string categoryId, string actionId, string title, UNNotificationActionOptions options,
            string textInputButtonTitle, string textInputPlaceholder)
        {
            var action = UNTextInputNotificationAction.FromIdentifier(actionId, title, options,
                textInputButtonTitle ?? string.Empty, textInputPlaceholder ?? string.Empty);
            SaveAction(categoryId, action);
        }

        /// <summary>
        /// A method to create and save a Category that will be registered for your app with all the actions
        /// that were previously added for this Category Id. See <see cref="UNNotificationCategory.FromIdentifier"/>
        /// </summary>
        /// <param name="categoryId">The unique identifier for the Category. Should not be empty</param>
        /// <param name="intentIdentifiers">The intent identifier strings that you want to associate with notifications of this type.
        /// The Intents framework defines constants for each type of intent that you can associate with your notifications</param>
        /// <param name="options">Additional options for handling notifications of this type</param>
        protected void AddCategory(string categoryId, string[] intentIdentifiers, UNNotificationCategoryOptions options)
        {
            IList<UNNotificationAction> actions;
            _actionsForCategories.TryGetValue(categoryId, out actions);

            var messageCategory = UNNotificationCategory.FromIdentifier(categoryId,
                actions?.ToArray() ?? new UNNotificationAction[] { }, intentIdentifiers, options);
            NotificationCategories.Add(messageCategory);
        }

        /// <summary>
        /// A method to create and save a Category that will be registered for your app with all the actions that were previously
        /// added for this Category Id and with hiddenPreviewsBodyPlaceholder. See <see cref="UNNotificationCategory.FromIdentifier"/>
        /// </summary>
        /// <param name="categoryId">The unique identifier for the Category. Should not be empty</param>
        /// <param name="intentIdentifiers">The intent identifier strings that you want to associate with notifications of this type.
        /// The Intents framework defines constants for each type of intent that you can associate with your notifications</param>
        /// <param name="hiddenPreviewsBodyPlaceholder">A placeholder string to display when the user has disabled notification previews for the app.
        /// Include the characters %u in the string to represent the number of notifications with the same thread identifier</param>
        /// <param name="options">Additional options for handling notifications of this type</param>
        protected void AddCategory(string categoryId, string[] intentIdentifiers, string hiddenPreviewsBodyPlaceholder,
            UNNotificationCategoryOptions options)
        {
            IList<UNNotificationAction> actions;
            _actionsForCategories.TryGetValue(categoryId, out actions);

            var messageCategory = UNNotificationCategory.FromIdentifier(categoryId,
                actions?.ToArray() ?? new UNNotificationAction[] { }, intentIdentifiers, hiddenPreviewsBodyPlaceholder, options);
            NotificationCategories.Add(messageCategory);
        }

        /// <summary>
        /// A method to create and save a Category that will be registered for your app with all the actions
        /// that were previously added for this Category Id and with hiddenPreviewsBodyPlaceholder and categorySummaryFormat.
        /// CategorySummaryFormat is only supported since iOS 12. For iOS 11 category will be created ignoring this option.
        /// See <see cref="UNNotificationCategory.FromIdentifier"/>
        /// </summary>
        /// <param name="categoryId">The unique identifier for the Category. Should not be empty</param>
        /// <param name="intentIdentifiers">The intent identifier strings that you want to associate with notifications of this type.
        /// The Intents framework defines constants for each type of intent that you can associate with your notifications</param>
        /// <param name="hiddenPreviewsBodyPlaceholder">A placeholder string to display when the user has disabled notification previews for the app.
        /// Include the characters %u in the string to represent the number of notifications with the same thread identifier</param>
        /// /// <param name="categorySummaryFormat">Category’s summary format string.
        /// Include the characters %u in the string to represent the number of notifications with the same thread identifier</param>
        /// <param name="options">Additional options for handling notifications of this type</param>
        protected void AddCategory(string categoryId, string[] intentIdentifiers, string hiddenPreviewsBodyPlaceholder,
            string categorySummaryFormat, UNNotificationCategoryOptions options)
        {
            IList<UNNotificationAction> actions;
            _actionsForCategories.TryGetValue(categoryId, out actions);

            UNNotificationCategory messageCategory;

            if (UIDevice.CurrentDevice.CheckSystemVersion(12, 0))
            {
                messageCategory = UNNotificationCategory.FromIdentifier(categoryId,
                    actions?.ToArray() ?? new UNNotificationAction[] { }, intentIdentifiers,
                    hiddenPreviewsBodyPlaceholder, new NSString(categorySummaryFormat), options);
            }
            else
            {
                messageCategory = UNNotificationCategory.FromIdentifier(categoryId,
                    actions?.ToArray() ?? new UNNotificationAction[] { }, intentIdentifiers,
                    hiddenPreviewsBodyPlaceholder, options);
            }

            NotificationCategories.Add(messageCategory);
        }

        // TODO: might want to add other constructors for category (with hiddenPreviewsBodyPlaceholder and categorySummaryFormat(12+))

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
