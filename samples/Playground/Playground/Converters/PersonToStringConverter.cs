// Developed by Softeq Development Corporation
// http://www.softeq.com

using Playground.Models;
using Softeq.XToolkit.Common.Converters;

namespace Playground.Converters
{
    public class PersonToStringConverter : IConverter<string, Person>
    {
        public static PersonToStringConverter Create => new PersonToStringConverter();

        public string ConvertValue(Person person, object parameter = null, string language = null)
        {
            return person?.FullName ?? "null";
        }

        public Person ConvertValueBack(string value, object parameter = null, string language = null)
        {
            return default;
        }
    }
}
