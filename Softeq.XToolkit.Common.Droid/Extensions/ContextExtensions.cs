// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Runtime.CompilerServices;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Views;

namespace Softeq.XToolkit.Common.Droid.Extensions
{
	public static class ContextExtensions
    {
        private const string StatusBarHeight = "status_bar_height";
        private const string Dimen = "dimen";
        private const string Android = "android";
        private static float _displayDensity = float.MinValue;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double FromPixels(this Context self, double pixels)
        {
            SetupMetrics(self);

            return pixels / _displayDensity;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ToPixels(this Context self, double dp)
        {
            SetupMetrics(self);

            return (float) Math.Round(dp * _displayDensity);
        }

        public static double GetThemeAttributeDp(this Context self, int resource)
        {
            using (var value = new TypedValue())
            {
                if (!self.Theme.ResolveAttribute(resource, value, true))
                {
                    return -1;
                }

                var pixels = (double) TypedValue.ComplexToDimension(value.Data, self.Resources.DisplayMetrics);

                return self.FromPixels(pixels);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SetupMetrics(Context context)
        {
            if (Math.Abs(_displayDensity - float.MinValue) > float.Epsilon)
            {
                return;
            }

            using (var metrics = context.Resources.DisplayMetrics)
            {
                _displayDensity = metrics.Density;
            }
        }

        public static void RemoveFromParent(this View view)
        {
            ((ViewGroup) view?.Parent)?.RemoveView(view);
        }

        public static int GetStatusBarHeight(Context context)
        {
            var result = 0;
            var resourceId = context.Resources.GetIdentifier(StatusBarHeight, Dimen, Android);
            if (resourceId > 0)
            {
                result = context.Resources.GetDimensionPixelSize(resourceId);
            }

            return result;
        }

		public static bool IsInPowerSavingMode(this Context context)
		{
			if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
			{
				var powerManager = (PowerManager)(context.GetSystemService(Context.PowerService));
				return powerManager.IsPowerSaveMode;
			}

			return false;
		}
    }
}