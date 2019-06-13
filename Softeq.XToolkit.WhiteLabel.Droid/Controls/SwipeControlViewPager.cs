// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Content;
using Android.Support.V4.View;
using Android.Util;

namespace Softeq.XToolkit.WhiteLabel.Droid.Controls
{
    public class SwipeControlViewPager : ViewPager
    {
        public bool SwipesEnabled { get; set; }

        public SwipeControlViewPager(Context context, IAttributeSet attrs) : base(context, attrs) { }

        public override bool OnTouchEvent(Android.Views.MotionEvent e)
        {
            return SwipesEnabled && base.OnTouchEvent(e);
        }

        public override bool OnInterceptTouchEvent(Android.Views.MotionEvent ev)
        {
            return SwipesEnabled && base.OnInterceptTouchEvent(ev);
        }
    }
}
