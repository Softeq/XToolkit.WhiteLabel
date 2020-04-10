// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Globalization;
using Xamarin.Forms;

namespace Softeq.XToolkit.WhiteLabel.Forms.Converters
{
    /// <summary>
    ///     Converts integer value multiplying it by parameter.
    ///     Can be used, for instance, to set fixed height of a list as a product of cell height and list items count
    ///     so that it only occupies specific place on the page.
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
