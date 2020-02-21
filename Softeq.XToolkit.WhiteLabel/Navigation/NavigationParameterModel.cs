// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public class NavigationParameterModel
    {
        public NavigationParameterModel(object? value, PropertyInfoModel propertyInfo)
        {
            Value = value;
            PropertyInfo = propertyInfo;
        }

        public object? Value { get; }

        public PropertyInfoModel PropertyInfo { get; }
    }
}