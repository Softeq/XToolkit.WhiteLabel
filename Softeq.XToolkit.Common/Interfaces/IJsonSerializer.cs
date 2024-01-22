// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.IO;
using System.Threading.Tasks;

namespace Softeq.XToolkit.Common.Interfaces
{
    /// <summary>
    ///     Represents methods for serialization and deserialization objects into and from the JSON format.
    /// </summary>
    public interface IJsonSerializer
    {
        /// <summary>
        ///     Serializes the specified object to a JSON string.
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <returns>A JSON string representation of the object.</returns>
        string Serialize(object value);

        /// <summary>
        ///     Deserializes the JSON to a .NET object.
        /// </summary>
        /// <param name="value">The string that contains the JSON structure to deserialize.</param>
        /// <returns>The deserialized object.</returns>
        /// <typeparam name="TResult">Object type.</typeparam>
        TResult? Deserialize<TResult>(string value);

        /// <summary>
        ///     Asynchronously serializes the specified object to a JSON string.
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="stream">The Stream used to write the JSON structure.</param>
        /// <returns>
        ///     A task that represents the asynchronous serialize operation.
        ///     A JSON string representation of the object.
        /// </returns>
        Task SerializeAsync(object value, Stream stream);

        /// <summary>
        ///     Asynchronously deserializes the JSON to the specified .NET type.
        /// </summary>
        /// <param name="stream">The JSON Stream to deserialize.</param>
        /// <returns>
        ///     A task that represents the asynchronous deserialize operation.
        ///     The value of the <c>TResult</c> parameter contains the deserialized object from the JSON string.
        /// </returns>
        /// <typeparam name="TResult">Object type.</typeparam>
        Task<TResult?> DeserializeAsync<TResult>(Stream stream);
    }
}
