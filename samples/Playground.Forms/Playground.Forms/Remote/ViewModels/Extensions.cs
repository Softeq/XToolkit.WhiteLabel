// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;

namespace Playground.Forms.Remote.ViewModels
{
    internal static class Extensions
    {
        public static void AddDuplicates<T>(
            this IList<T> list,
            int count,
            Func<int, T> itemFactory)
        {
            for (int i = 0; i < count; i++)
            {
                list.Add(itemFactory(i));
            }
        }
    }
}
