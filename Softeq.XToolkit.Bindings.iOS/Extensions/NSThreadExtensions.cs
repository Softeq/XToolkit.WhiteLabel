// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Foundation;

namespace Softeq.XToolkit.Bindings.iOS.Extensions
{
    public static class NSThreadExtensions
    {
        public static void ExecuteOnMainThread(Action action)
        {
            if (NSThread.Current.IsMainThread)
            {
                action();
            }
            else
            {
                NSOperationQueue.MainQueue.AddOperation(action);
                NSOperationQueue.MainQueue.WaitUntilAllOperationsAreFinished();
            }
        }
    }
}
