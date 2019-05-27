// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;

namespace Softeq.XToolkit.WhiteLabel.Extensions
{
    public static class ReflectionExtensions
    {
        public static IEnumerable<Type> View(this IEnumerable<Type> types, params Type[] viewEndWith)
        {
            return types.Where(type => viewEndWith.Any(type.IsSubclassOf) &&
                type.BaseType != null && type.BaseType.IsGenericType);
        }
    }
}