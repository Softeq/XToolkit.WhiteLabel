// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common.Interfaces;
using Softeq.XToolkit.PushNotifications.Abstract;

namespace Softeq.XToolkit.PushNotifications
{
    public class PushTokenStorageService : IPushTokenStorageService
    {
        private readonly string _pushTokenKey = $"{nameof(IPushTokenStorageService)}_push_token_key";
        private readonly string _isTokenRegisteredInSystemKey = $"{nameof(IPushTokenStorageService)}_token_registered_in_system_key";
        private readonly string _isTokenSavedOnServerKey = $"{nameof(IPushTokenStorageService)}_token_saved_on_server_key";

        private readonly IInternalSettings _internalSettings;

        public PushTokenStorageService(IInternalSettings internalSettings)
        {
            _internalSettings = internalSettings;
        }

        /// <inheritdoc />
        public string PushToken
        {
            get => _internalSettings.GetValueOrDefault(_pushTokenKey, default(string));
            set => _internalSettings.AddOrUpdateValue(_pushTokenKey, value);
        }

        /// <inheritdoc />
        public bool IsTokenRegisteredInSystem
        {
            get => _internalSettings.GetValueOrDefault(_isTokenRegisteredInSystemKey, default(bool));
            set => _internalSettings.AddOrUpdateValue(_isTokenRegisteredInSystemKey, value);
        }

        /// <inheritdoc />
        public bool IsTokenSavedOnServer
        {
            get => _internalSettings.GetValueOrDefault(_isTokenSavedOnServerKey, default(bool));
            set => _internalSettings.AddOrUpdateValue(_isTokenSavedOnServerKey, value);
        }
    }
}
