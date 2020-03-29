// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Extensions
{
    public static class ReflectionExtensions
    {
        // TODO YP: add tests and refactor
        public static IEnumerable<Type> View(this IEnumerable<Type> types, params Type[] viewEndWith)
        {
            return types.Where(type => viewEndWith.Any(type.IsSubclassOf) &&
                type.BaseType != null && type.BaseType.IsGenericType &&
                type.BaseType.GetGenericArguments()[0].AssemblyQualifiedName != null &&
                type.BaseType.GetGenericArguments()[0].IsSubclassOf(typeof(ViewModelBase)));
        }
    }
}
