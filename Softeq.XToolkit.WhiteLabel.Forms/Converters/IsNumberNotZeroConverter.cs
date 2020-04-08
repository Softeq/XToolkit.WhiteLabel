// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Globalization;
using Xamarin.Forms;

namespace Softeq.XToolkit.WhiteLabel.Forms.Converters
{
    /// <summary>
    ///     Convert integer value to true if it is not equal to zero, false otherwise.
    ///     Can be used to changed visibility of element based on list count.
    ///     ConvertBack is not implemented.
    /// </summary>
    public class IsNumberNotZeroConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int) value != 0 ? true : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }
}
