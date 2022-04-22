// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common.Converters;

namespace Softeq.XToolkit.Common.Tests.Converters
{
    public class ConverterHelper
    {
        public static IConverter<string, bool> CreateTwoWayConverter()
        {
            return new TwoWayConverter();
        }

        public static IConverter<string, bool> CreateOneWayConverter()
        {
            return new OneWayConverter();
        }

        private class TwoWayConverter : IConverter<string, bool>
        {
            public string ConvertValue(bool value, object parameter = null, string language = null)
            {
                return value.ToString();
            }

            public bool ConvertValueBack(string value, object parameter = null, string language = null)
            {
                return bool.Parse(value);
            }
        }

        private class OneWayConverter : ConverterBase<string, bool>
        {
            public override string ConvertValue(bool value, object parameter = null, string language = null)
            {
                return value.ToString();
            }
        }
    }
}
