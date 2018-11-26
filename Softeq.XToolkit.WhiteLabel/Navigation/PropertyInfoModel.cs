using System;
using System.Reflection;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public class PropertyInfoModel
    {
        private readonly string _typeName;
        private readonly string _propertyName;

        public PropertyInfoModel(MemberInfo p)
        {
            _typeName = p.DeclaringType.AssemblyQualifiedName;
            _propertyName = p.Name;
        }

        public PropertyInfo ToProperty()
        {
            return Type.GetType(_typeName).GetProperty(_propertyName);
        }
    }
}