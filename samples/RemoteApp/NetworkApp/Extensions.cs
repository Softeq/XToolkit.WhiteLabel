using System;
using System.Collections.Generic;

namespace NetworkApp
{
    public static class Extensions
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
