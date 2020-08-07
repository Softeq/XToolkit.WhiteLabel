// Developed by Softeq Development Corporation
// http://www.softeq.com

﻿namespace RemoteServices.Profile.Models
{
    public class ProfileConfig
    {
        public ProfileConfig(string baseUrl)
        {
            BaseUrl = baseUrl;
        }

        public string BaseUrl { get; }
    }
}
