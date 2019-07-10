// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common.Interfaces;

namespace Softeq.XToolkit.PushNotifications
{
    public class PushTokenStorageService : IPushTokenStorageService
    {
        private readonly IInternalSettings _internalSettings;

        public PushTokenStorageService(IInternalSettings internalSettings)
        {
            _internalSettings = internalSettings;
        }

        private readonly string PushTokenKey = $"{nameof(IPushTokenStorageService)}_push_token_key";
        public string PushToken
        {
            get => _internalSettings.GetValueOrDefault(PushTokenKey, default(string));
            set => _internalSettings.AddOrUpdateValue(PushTokenKey, value);
        }

        private readonly string IsTokenRegisteredInSystemKey = $"{nameof(IPushTokenStorageService)}_token_registered_in_system_key";
        public bool IsTokenRegisteredInSystem
        {
            get => _internalSettings.GetValueOrDefault(IsTokenRegisteredInSystemKey, default(bool));
            set => _internalSettings.AddOrUpdateValue(IsTokenRegisteredInSystemKey, value);
        }

        private readonly string IsTokenSavedOnServerKey = $"{nameof(IPushTokenStorageService)}_token_saved_on_server_key";
        public bool IsTokenSavedOnServer
        {
            get => _internalSettings.GetValueOrDefault(IsTokenSavedOnServerKey, default(bool));
            set => _internalSettings.AddOrUpdateValue(IsTokenSavedOnServerKey, value);
        }
    }
}
