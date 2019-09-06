// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Common.Interfaces;

namespace Playground.Converters
{
    public class InverseBooleanConverter : IConverter<bool, bool>
    {
        public bool ConvertValue(bool value, object parameter = null, string language = null)
        {
            return !value;
        }

        public bool ConvertValueBack(bool value, object parameter = null, string language = null)
        {
            return !value;
        }
    }
}
