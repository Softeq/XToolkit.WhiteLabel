// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Playground.RemoteData.Profile.Models
{
    public class ProfileApiConfig
    {
        public ProfileApiConfig(string baseUrl)
        {
            BaseUrl = baseUrl;
        }

        public string BaseUrl { get; }
    }
}
