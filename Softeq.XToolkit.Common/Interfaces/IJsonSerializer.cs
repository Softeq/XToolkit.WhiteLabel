// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.Common.Interfaces
{
    public interface IJsonSerializer
    {
        string Serialize(object value);
        T Deserialize<T>(string value);
    }
}