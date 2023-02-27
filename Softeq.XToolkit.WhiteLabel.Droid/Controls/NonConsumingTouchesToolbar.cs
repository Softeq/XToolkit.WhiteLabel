// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using AndroidX.AppCompat.Widget;

namespace Softeq.XToolkit.WhiteLabel.Droid.Controls
{
    /// <summary>
    ///     A Toolbar that passes touches to the views below (default Toolbar consumes all touches itself).
    /// </summary>
    public class NonConsumingTouchesToolbar : Toolbar
    {
        public NonConsumingTouchesToolbar(Context context)
            : base(context)
        {
        }

        public NonConsumingTouchesToolbar(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
        }

        public NonConsumingTouchesToolbar(Context context, IAttributeSet attrs, int defStyleAttr)
            : base(context, attrs, defStyleAttr)
        {
        }

        protected NonConsumingTouchesToolbar(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public override bool OnTouchEvent(MotionEvent? e)
        {
            base.OnTouchEvent(e);
            return false; // We return false to pass touches to the views below
        }
    }
}
