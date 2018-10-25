// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Softeq.XToolkit.WhiteLabel.Droid.Controls
{
    [Register("com.softeq.xtoolkit.whitelabel.droid.BusyOverlayView")]
    public class BusyOverlayView : LinearLayout
    {
        public BusyOverlayView(Context context) : base(context)
        {
            Init(context);
        }

        public BusyOverlayView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init(context);
        }

        public BusyOverlayView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            Init(context);
        }

        public BusyOverlayView(IntPtr handle, JniHandleOwnership owner) : base(handle, owner)
        {
        }

        public void SetOverlayBackgroundResource(int resourceId)
        {
            var view = FindViewById<View>(Resource.Id.control_busy_overlay_container);
            view.SetBackgroundResource(resourceId);
        }

        private void Init(Context context)
        {
            Inflate(context, Resource.Layout.control_busy_overlay, this);
        }
    }
}
