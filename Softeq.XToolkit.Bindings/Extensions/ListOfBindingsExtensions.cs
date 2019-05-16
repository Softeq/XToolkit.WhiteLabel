// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;

namespace Softeq.XToolkit.Bindings.Extensions
{
    public static class ListOfBindingsExtensions
    {
        public static void DetachAllAndClear(this IList<Binding> bindings)
        {
            while (bindings.Count > 0)
            {
                bindings[0].Detach();
                bindings.RemoveAt(0);
            }
        }
    }
}