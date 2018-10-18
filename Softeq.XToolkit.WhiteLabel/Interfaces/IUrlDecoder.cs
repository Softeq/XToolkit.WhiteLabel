// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.WhiteLabel.Interfaces
{
    public interface IUrlDecoder
    {
        string EncodeUrl(string input);
        string DecodeUrl(string input);
    }
}