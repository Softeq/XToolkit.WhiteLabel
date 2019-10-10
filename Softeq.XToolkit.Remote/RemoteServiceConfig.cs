using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Softeq.XToolkit.Remote
{
    public class RemoteServiceConfig
    {
        public RemoteServiceConfig(string baseUrl)
        {
            BaseUrl = baseUrl;
        }

        public string BaseUrl { get; }
        public bool AutoRedirectRequests { get; set; } = true;
        public Func<DelegatingHandler> DelegatingHandler { get; set; }
        public IDictionary<string, string> DefaultHeaders { get; set; }
    }
}
