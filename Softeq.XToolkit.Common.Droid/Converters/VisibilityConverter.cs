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
    public class VisibilityConverter : IConverter<ViewStates, bool>
    {
        private readonly ViewStates _hideViewState;

        private VisibilityConverter(ViewStates hideViewState)
        {
            _hideViewState = hideViewState;
        }

        [Obsolete("Use `VisibilityConverter.Invisible` instead.")]
        public static IConverter<ViewStates, bool> Instance => Invisible;

        /// <summary>
        ///     Gets configured converter that uses <see cref="T:Android.Views.ViewStates.Invisible"/> when value is <c>false</c>.
        /// </summary>
        public static IConverter<ViewStates, bool> Invisible { get; } = new VisibilityConverter(ViewStates.Invisible);

        /// <summary>
        ///     Gets configured converter that uses <see cref="T:Android.Views.ViewStates.Gone"/> when value is <c>false</c>.
        /// </summary>
        public static IConverter<ViewStates, bool> Gone { get; } = new VisibilityConverter(ViewStates.Gone);

        /// <inheritdoc />
        public ViewStates ConvertValue(bool value, object? parameter = null, string? language = null)
        {
            return value ? ViewStates.Visible : _hideViewState;
        }

        /// <inheritdoc />
        public bool ConvertValueBack(ViewStates value, object? parameter = null, string? language = null)
        {
            return value == ViewStates.Visible;
        }
    }
}
