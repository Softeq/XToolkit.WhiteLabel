using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Services
{
    public static class KeyboardService
    {
        /// <summary>
        /// Hide the keyboard globally.
        /// </summary>
        public static void HideKeyboard()
        {
            UIApplication.SharedApplication.SendAction(new ObjCRuntime.Selector("resignFirstResponder"), null, null, null);
        }
    }
}
