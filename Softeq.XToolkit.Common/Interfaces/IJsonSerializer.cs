// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.IO;
using System.Threading.Tasks;

namespace Softeq.XToolkit.Common.Interfaces
{
    /// <summary>
    /// Represents methods for serialization and deserialization objects into and from the JSON format
    /// </summary>
    public interface IJsonSerializer
    {
        /// <summary>
        /// Serializes the specified object to a JSON string.
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <returns>A JSON string representation of the object.</returns>
        string Serialize(object value);

        /// <summary>
        /// Deserializes the JSON to a .NET object.
        /// </summary>
        /// <param name="value">The Stream that contains the JSON structure to deserialize.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        /// <typeparam name="T">Object type.</typeparam>
        T Deserialize<T>(string value);

        /// <summary>
        /// Deserializes the JSON to a .NET object.
        /// </summary>
        /// <param name="stream">Stream The JSON to deserialize.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        /// <typeparam name="T">Object type.</typeparam>
        Task<T> DeserializeAsync<T>(Stream stream);

        /// <summary>
        /// Asynchronously Serializes the specified object to a JSON string.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="stream">The Stream used to write the JSON structure.</param>
        /// <returns>A JSON string representation of the object.</returns>
        Task SerializeAsync(object obj, Stream stream);
    }
}