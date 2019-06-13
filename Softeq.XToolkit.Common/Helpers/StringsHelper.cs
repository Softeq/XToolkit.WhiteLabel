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
        /// <summary>
        /// Capitalize first letter of specified string.
        /// </summary>
        /// <returns>The specified string with upper-case first symbol.</returns>
        /// <param name="value">Initial string.</param>
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

        /// <summary>
        /// Removes the empty lines.
        /// </summary>
        /// <returns>Specified string without empty lines.</returns>
        /// <param name="input">Specified string.</param>
        public static string RemoveEmptyLines(this string input)
        {
            return Regex.Replace(input, @"[\r\n]*^\s*$[\r\n]*", "", RegexOptions.Multiline);
        }

        /// <summary>
        /// Converts the string representation of a number to its double-precision floating-point number equivalent.
        /// </summary>
        /// <returns><c>true</c>, if parse double was tryed, <c>false</c> otherwise.</returns>
        /// <param name="text">A string containing a number to convert.</param>
        /// <param name="result">When this method returns, contains the double-precision floating-point number equivalent of the s parameter, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the s parameter is null or Empty, is not a number in a valid format, or represents a number less than MinValue or greater than MaxValue. This parameter is passed uninitialized; any value originally supplied in result will be overwritten..</param>
        public static bool TryParseDouble(this string text, out double? result)
        {
            if (double.TryParse(text, NumberStyles.Number, CultureInfo.CurrentCulture, out var number))
            {
                result = number;
                return true;
            }
            result = null;
            return false;
        }
    }
}