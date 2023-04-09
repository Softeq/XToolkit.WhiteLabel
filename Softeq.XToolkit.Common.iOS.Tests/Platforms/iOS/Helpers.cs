// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using UIKit;

namespace Softeq.XToolkit.Common.iOS.Tests;

public static class Helpers
{
    public static Task<TResult> RunOnUIThreadAsync<TResult>(Func<TResult> func)
    {
        var tcs = new TaskCompletionSource<TResult>();
        UIApplication.SharedApplication.BeginInvokeOnMainThread(() =>
        {
            try
            {
                var result = func();
                tcs.SetResult(result);
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }
        });
        return tcs.Task;
    }
}
