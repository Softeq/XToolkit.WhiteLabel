// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Globalization;
using Xamarin.Forms;

namespace Softeq.XToolkit.WhiteLabel.Forms.Converters
{
    /// <summary>
    ///     Convert integer value to true if it is equal to zero, false otherwise (check if number is zero).
    ///     False can be passed as a parameter to inverse comparison and check if number is not zero instead.
    ///     Can be used to change visibility of element based on list items count.
    ///     ConvertBack is not implemented.
    /// </summary>
    public class IsNumberZeroConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var compareWithZero = GetParameter(parameter);
            var isZero = (int) value == 0;
            return isZero == compareWithZero ? true : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }

        private bool GetParameter(object parameter)
        {
            if (parameter is bool)
            {
                return (bool) parameter;
            }
            else if (parameter is string)
            {
                return bool.Parse((string) parameter);
            }

            return true;
        }
    }
}
