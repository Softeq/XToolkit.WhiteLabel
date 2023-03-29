// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.WhiteLabel.Navigation;

/// <summary>
///     Represents methods for serialization and deserialization of objects into and from the text format.
/// </summary>
/// <remarks>
///     Related to navigation and uses internally to save some of ViewModel states (currently only on Android).
/// </remarks>
public interface INavigationSerializer
{
    /// <summary>
    ///     Serializes the specified object to a string.
    /// </summary>
    /// <param name="value">The object to serialize.</param>
    /// <returns>A string representation of the object.</returns>
    string Serialize(object value);

    /// <summary>
    ///     Deserializes to a .NET object.
    /// </summary>
    /// <param name="value">The string that contains data to deserialize.</param>
    /// <returns>The deserialized object.</returns>
    /// <typeparam name="TResult">Result object type.</typeparam>
    TResult? Deserialize<TResult>(string value);

    /// <summary>
    ///     Deserializes the custom object to a .NET object of the <paramref name="returnType"/>.
    /// </summary>
    /// <param name="value">The custom object that contains data to deserialize.</param>
    /// <param name="returnType">Result object type.</param>
    /// <remarks>In case:
    ///     <para>- to serialize Model as `object` we will receive json string.</para>
    ///     <para>
    ///         - to deserialize json string to Model back when the type was `object`
    ///         we should explicitly deserialize/unwrap the received object (object of serializer like JsonElement, etc.)
    ///         to convert this object to Model on runtime.
    ///     </para>
    /// </remarks>
    /// <returns>The deserialized object of type <paramref name="returnType"/>.</returns>
    object? Deserialize(object? value, Type returnType);
}
