// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System;

namespace Softeq.XToolkit.Common.Helpers
{
    public static class StringsHelper
    {
        public static string CapitalizeFirstLetter(string value)
        {
            var array = value.ToCharArray();
            array[0] = char.ToUpper(array[0]);
            var sb = new StringBuilder();
            foreach (var item in array)
            {
                sb.Append(item);
            }

            return sb.ToString();
        }

        public static string RemoveEmptyLines(this string input)
        {
            return Regex.Replace(input, @"[\r\n]*^\s*$[\r\n]*", "", RegexOptions.Multiline);
        }

        public static bool TryParseDouble(this string text, out double? result)
        {
            if (double.TryParse(text, NumberStyles.Number, CultureInfo.CurrentCulture, out var number))
            {
                result = number;
                return true;
            }
            result = null;
            return string.IsNullOrEmpty(text);
        }
    }
}