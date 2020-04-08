// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Globalization;
using Xamarin.Forms;

namespace Softeq.XToolkit.WhiteLabel.Forms.Converters
{
    /// <summary>
    ///     Converts integer value multiplying it by parameter.
    /// </summary>
    public class MultiplyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var multiplicator = System.Convert.ToDouble(parameter);
            var initial = System.Convert.ToDouble(value);
            return (initial * multiplicator).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dividor = System.Convert.ToDouble(parameter);
            var initial = System.Convert.ToDouble(value);
            return (initial / dividor).ToString();
        }
    }
}
