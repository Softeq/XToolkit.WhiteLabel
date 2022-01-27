// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Android.Views;
using Softeq.XToolkit.Common.Converters;

namespace Softeq.XToolkit.Common.Droid.Converters
{
    /// <summary>
    ///     Converts <see cref="T:System.Boolean"/> to Android specific <see cref="T:Android.Views.ViewStates"/> and back.
    /// </summary>
    [Obsolete("Use `VisibilityConverter.Gone` instead.")]
    public class GoneConverter : IConverter<ViewStates, bool>
    {
        public static IConverter<ViewStates, bool> Instance { get; } = new GoneConverter();

        /// <inheritdoc />
        public ViewStates ConvertValue(bool value, object? parameter = null, string? language = null)
        {
            return value ? ViewStates.Visible : ViewStates.Gone;
        }

        /// <inheritdoc />
        public bool ConvertValueBack(ViewStates value, object? parameter = null, string? language = null)
        {
            return value == ViewStates.Visible;
        }
    }
}
