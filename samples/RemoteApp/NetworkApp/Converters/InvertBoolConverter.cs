// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Globalization;
using Xamarin.Forms;

namespace NetworkApp.Converters
{
    public class InvertBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool val)
            {
                return !val;
            }

            throw new ArgumentException(value.ToString(), nameof(value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
#pragma warning disable RECS0083 // Shows NotImplementedException throws in the quick task bar
            throw new NotImplementedException();
#pragma warning restore RECS0083 // Shows NotImplementedException throws in the quick task bar
        }
    }
}
