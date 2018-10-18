// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Net;
using Softeq.XToolkit.WhiteLabel.Interfaces;

namespace Softeq.XToolkit.WhiteLabel.iOS.Services
{
    public class UrlDecoder : IUrlDecoder
    {
        public string DecodeUrl(string input)
        {
            return WebUtility.UrlDecode(input);
        }

        public string EncodeUrl(string input)
        {
            return WebUtility.UrlEncode(input);
        }
    }
}