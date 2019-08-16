// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Playground.Utils
{
    internal static class DebugUtils
    {
        internal static void ForceCGCollect()
        {
            Console.WriteLine($"=== The highest generation is {GC.MaxGeneration}");

            GC.Collect(0, GCCollectionMode.Forced, false);
            GC.WaitForPendingFinalizers();
            GC.Collect(0, GCCollectionMode.Forced, false);
            GC.WaitForPendingFinalizers();
            GC.Collect(1);
            GC.WaitForPendingFinalizers();
            GC.Collect(2);

            Console.WriteLine($"=== Total managed memory: {GC.GetTotalMemory(true).ToString("N0")} bytes.");
        }
    }
}
