// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Reflection;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public class PropertyInfoModel
    {
        public string TypeName;
        public string PropertyName;

        public static PropertyInfoModel FromProperty(MemberInfo memberInfo)
        {
            return new PropertyInfoModel()
            {
                TypeName = memberInfo.DeclaringType.AssemblyQualifiedName,
                PropertyName = memberInfo.Name
            };
        }

        public PropertyInfo ToProperty()
        {
            return Type.GetType(TypeName).GetProperty(PropertyName);
        }
    }
}