// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Content;
using Android.Views;
using Android.Views.InputMethods;

namespace Softeq.XToolkit.WhiteLabel.Droid.Services
{
    public static class KeyboardService
    {
        public static void ShowSoftKeyboard(View view)
        {
            if (view == null)
            {
                return;
            }

            view.RequestFocus();
            var imm = (InputMethodManager) view.Context.GetSystemService(Context.InputMethodService);
            imm.ShowSoftInput(view, ShowFlags.Forced);
            imm.ToggleSoftInput(ShowFlags.Forced, HideSoftInputFlags.ImplicitOnly);
        }

        public static void HideSoftKeyboard(View view)
        {
            if (view == null)
            {
                return;
            }

            var imm = (InputMethodManager) view.Context.GetSystemService(Context.InputMethodService);
            imm.HideSoftInputFromWindow(view.WindowToken, HideSoftInputFlags.None);
        }

        public static void HideSoftKeyboard()
        {
            var currentActivity = MainApplicationBase.CurrentActivity;
            var activityContentView = currentActivity.FindViewById(Android.Resource.Id.Content);
            HideSoftKeyboard(activityContentView);
        }
    }
}
