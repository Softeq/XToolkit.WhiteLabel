using System;
using System.Collections.Generic;
using System.Linq;

namespace Softeq.XToolkit.WhiteLabel.Extensions
{
    public static class ReflectionExtensions
    {
        public static IEnumerable<Type> View(this IEnumerable<Type> types, params string[] viewEndWith)
        {
            return types.Where(type => type.FullName != null && viewEndWith.Any(type.FullName.EndsWith) &&
                                       type.BaseType != null && type.BaseType.IsGenericType);
        }
    }
}