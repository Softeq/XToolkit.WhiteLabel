using System;
using System.Collections.Generic;
using System.Linq;

namespace Softeq.XToolkit.WhiteLabel.Extensions
{
    public static class ReflectionExtensions
    {
        public static IEnumerable<Type> View(this IEnumerable<Type> types, string viewEndWith)
        {
            return types.Where(type => type.FullName != null && type.FullName.EndsWith(viewEndWith) &&
                                       type.BaseType != null && type.BaseType.IsGenericType);
        }
    }
}