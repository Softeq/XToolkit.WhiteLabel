﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Globalization;
using Xamarin.Forms;

namespace Softeq.XToolkit.WhiteLabel.Forms.Converters
{
    /// <summary>
    ///     Convert equality between value and parameter to boolean result:
    ///     returns true if value equals parameter, false otherwise.
    ///     Can be used to compare property value with specific enum value
    ///     (for instance, to check if specific state is selected to show/hide some elements)
    ///     ConvertBack is not implemented.
    /// </summary>
    public class EqualityToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Equals(value, parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }
}
