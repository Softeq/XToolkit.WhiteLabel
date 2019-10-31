// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Reflection;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public class PropertyInfoModel
    {
        public PropertyInfoModel(string propertyName, string typeName)
        {
            PropertyName = propertyName;
            TypeName = typeName;
        }

        public string PropertyName { get; }
        
        public string TypeName { get; }

        public static PropertyInfoModel FromProperty(MemberInfo memberInfo)
        {
            return new PropertyInfoModel(memberInfo.DeclaringType.AssemblyQualifiedName, memberInfo.Name);
        }

        public PropertyInfo ToProperty()
        {
            return Type.GetType(TypeName).GetProperty(PropertyName);
        }
    }
}
