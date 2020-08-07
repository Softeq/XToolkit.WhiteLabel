// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Globalization;
using Xamarin.Forms;

namespace Softeq.XToolkit.WhiteLabel.Forms.Converters
{
    /// <summary>
    ///     Converts a string to UpperCase string.
    ///     ConvertBack is not implemented.
    /// </summary>
    public class StringToUpperConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as string)?.ToUpper() ?? string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}