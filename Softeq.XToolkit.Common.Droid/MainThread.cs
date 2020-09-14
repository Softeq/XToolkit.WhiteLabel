// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Android.OS;

namespace Softeq.XToolkit.Common.Droid
{
    /// <summary>
    ///     Main Thread helpers.
    ///     Based on Xamarin.Essentials:
    ///     https://github.com/xamarin/Essentials/blob/1.5.3.2/Xamarin.Essentials/MainThread/MainThread.android.cs.
    /// </summary>
    public static class MainThread
    {
        private static volatile Handler? _handler;

        /// <summary>
        ///     Gets a value indicating whether it is the current main UI thread.
        /// </summary>
        public static bool IsMainThread
        {
            get
            {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
                {
                    return Looper.MainLooper != null && Looper.MainLooper.IsCurrentThread;
                }

                return Looper.MyLooper() == Looper.MainLooper;
            }
        }

        /// <summary>
        ///     Invokes an action on the main thread of the application.
        /// </summary>
        /// <param name="action">Action to execute.</param>
        public static void BeginInvokeOnMainThread(Action action)
        {
            if (IsMainThread)
            {
                action.Invoke();
            }
            else
            {
                PlatformBeginInvokeOnMainThread(action);
            }
        }

        private static void PlatformBeginInvokeOnMainThread(Action action)
        {
            if (_handler?.Looper != Looper.MainLooper)
            {
                _handler = new Handler(Looper.MainLooper!);
            }

            _handler!.Post(action);
        }
    }
}
