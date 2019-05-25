// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.IO;
using System.Threading.Tasks;

namespace Softeq.XToolkit.Common.Interfaces
{
    public interface IJsonSerializer
    {
        string Serialize(object value);
        T Deserialize<T>(string value);
        Task<T> DeserializeAsync<T>(Stream stream);
        Task SerializeAsync(object obj, Stream stream);
    }
}