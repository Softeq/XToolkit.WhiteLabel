// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Content;

namespace Softeq.XToolkit.Common.Droid.Extensions
{
    /// <summary>
    ///     Extension methods for <see cref="T:Android.Content.Intent"/>.
    /// </summary>
    public static class IntentExtensions
    {
        /// <summary>
        ///     Provides a check of whether there is a component that can launch given activity intent.
        ///     <para>
        ///         There might be cases when <see cref="M:Android.Content.Intent.ResolveActivity(Android.Content.PM.PackageManager)"/>
        ///         returns a component but it can't be used to launch <see cref="T:Android.Content.Intent"/>
        ///         (for instance not exported activity from different package).
        ///     </para>
        /// </summary>
        /// <returns>
        ///     <see langword="true"/>, if intent action was handled, <see langword="false"/> otherwise (if it can not be handled).</returns>
        /// <param name="intent">Intent that should be handled.</param>
        /// <param name="context">Current context.</param>
        public static bool HandleIntentAction(this Intent intent, Context context)
        {
            try
            {
                if (intent.ResolveActivity(context.PackageManager) != null)
                {
                    context.StartActivity(intent);
                }
                else
                {
                    throw new ActivityNotFoundException();
                }
            }
            catch (ActivityNotFoundException)
            {
                return false;
            }

            return true;
        }
    }
}
