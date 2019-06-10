// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Content;

namespace Softeq.XToolkit.Common.Droid.Extensions
{
    public static class IntentExtensions
    {
        public static bool HandleIntentAction(this Intent intent, Context context)
        {
            // There might be cases when ResolveActivity returns a component 
            // but it can't be used to launch intent (for instance not exported activity from different package)
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
