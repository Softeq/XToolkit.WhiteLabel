// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Runtime.CompilerServices;
using Android.Content;
using Android.OS;

namespace Softeq.XToolkit.Common.Droid.Extensions
{
    /// <summary>
    ///     Extension methods for <see cref="T:Android.Content.Context"/>.
    ///     Partly based on Xamarin.Forms.Android sources:
    ///     https://github.com/xamarin/Xamarin.Forms/blob/release-4.8.0-sr1.2/Xamarin.Forms.Platform.Android/ContextExtensions.cs.
    /// </summary>
    public static class ContextExtensions
    {
        /// <summary>
        ///     Caching this display density here means that all pixel calculations are going to be
        ///     based on the density of the first Context these extensions are run against.
        /// </summary>
        private static float _displayDensity = float.MinValue;

        /// <summary>
        ///    This method converts device specific pixels to density independent pixels.
        /// </summary>
        /// <param name="context">Context to get resources and device specific display metrics.</param>
        /// <param name="pixels">A value in px (pixels) unit. Which we need to convert into dp.</param>
        /// <returns>A float value to represent dp equivalent to px value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double PxToDp(this Context context, double pixels)
        {
            SetupMetrics(context);

            return pixels / _displayDensity;
        }

        /// <summary>
        ///     This method converts dp unit to equivalent pixels, depending on device density.
        /// </summary>
        /// <param name="context">Context to get resources and device specific display metrics.</param>
        /// <param name="dp">A value in dp (density independent pixels) unit. Which we need to convert into pixels.</param>
        /// <returns>A float value to represent px equivalent to dp depending on device density.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DpToPx(this Context context, double dp)
        {
            SetupMetrics(context);

            return (float) Math.Round(dp * _displayDensity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SetupMetrics(Context context)
        {
            if (Math.Abs(_displayDensity - float.MinValue) > float.Epsilon)
            {
                return;
            }

            using (var metrics = context.Resources!.DisplayMetrics)
            {
                _displayDensity = metrics!.Density;
            }
        }

        /// <summary>
        ///     Get the height of the status bar in pixels.
        ///     Call in OnCreate() method of your Activity.
        /// </summary>
        /// <param name="context">Target context.</param>
        /// <returns>Returns height in pixels.</returns>
        public static int GetStatusBarHeight(this Context context)
        {
            var resources = context.Resources;
            if (resources == null)
            {
                return 0;
            }

            var resourceId = resources.GetIdentifier("status_bar_height", "dimen", "android");
            if (resourceId > 0)
            {
                return resources.GetDimensionPixelSize(resourceId);
            }

            return 0;
        }

        /// <summary>
        ///     Check if the device is currently in power save mode. For Lollipop (API 21)+.
        /// </summary>
        /// <param name="context">Target context.</param>
        /// <returns>
        ///     <see langword="true"/> if the device is currently in power save mode.
        ///     For Lollipop (API 21)+, otherwise <see langword="false"/>.
        /// </returns>
        public static bool IsInPowerSavingMode(this Context context)
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
            {
                return false;
            }

            var powerManager = (PowerManager) context.GetSystemService(Context.PowerService)!;
            return powerManager.IsPowerSaveMode;
        }

        /// <summary>
        ///     Provides a check of whether there is a component that can launch given activity intent.
        ///     <para>
        ///         There might be cases when <see cref="M:Android.Content.Intent.ResolveActivity(Android.Content.PM.PackageManager)"/>
        ///         returns a component but it can't be used to launch <see cref="T:Android.Content.Intent"/>
        ///         (for instance not exported activity from different package).
        ///     </para>
        /// </summary>
        /// <returns>
        ///     <see langword="true"/>, if intent action was handled,
        ///     <see langword="false"/> otherwise (if it can not be handled).
        /// </returns>
        /// <param name="context">Target context.</param>
        /// <param name="intent">Intent that should be handled.</param>
        public static bool TryStartActivity(this Context context, Intent intent)
        {
            var canResolveActivity = intent.ResolveActivity(context.PackageManager!) != null;
            if (canResolveActivity)
            {
                context.StartActivity(intent);
            }

            return canResolveActivity;
        }
    }
}
