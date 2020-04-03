// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common.Interfaces;

namespace Softeq.XToolkit.PushNotifications
{
    /// <summary>
    ///     The default implementation of storing push token.
    /// </summary>
    public class PushTokenStorageService : IPushTokenStorageService
    {
        private const string PushTokenKey = nameof(IPushTokenStorageService) + "_push_token_key";
        private const string IsTokenRegisteredInSystemKey = nameof(IPushTokenStorageService) + "_token_registered_in_system_key";
        private const string IsTokenSavedOnServerKey = nameof(IPushTokenStorageService) + "_token_saved_on_server_key";

        private readonly IInternalSettings _internalSettings;

        /// <summary>
        ///     Initializes a new instance of the <see cref="PushTokenStorageService"/> class.
        /// </summary>
        /// <param name="internalSettings">Instance of internal storage service.</param>
        public PushTokenStorageService(IInternalSettings internalSettings)
        {
            _internalSettings = internalSettings;
        }

        /// <inheritdoc/>
        public string PushToken
        {
            get => _internalSettings.GetValueOrDefault(PushTokenKey, default(string));
            set => _internalSettings.AddOrUpdateValue(PushTokenKey, value);
        }

        /// <inheritdoc/>
        public bool IsTokenRegisteredInSystem
        {
            get => _internalSettings.GetValueOrDefault(IsTokenRegisteredInSystemKey, default(bool));
            set => _internalSettings.AddOrUpdateValue(IsTokenRegisteredInSystemKey, value);
        }

        /// <inheritdoc/>
        public bool IsTokenSavedOnServer
        {
            get => _internalSettings.GetValueOrDefault(IsTokenSavedOnServerKey, default(bool));
            set => _internalSettings.AddOrUpdateValue(IsTokenSavedOnServerKey, value);
        }
    }
}
