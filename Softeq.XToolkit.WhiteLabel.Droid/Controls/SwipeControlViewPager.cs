// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Content;
using Android.Util;
using Android.Views;
using AndroidX.ViewPager.Widget;

namespace Softeq.XToolkit.WhiteLabel.Droid.Controls
{
    public class SwipeControlViewPager : ViewPager
    {
        public SwipeControlViewPager(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public bool SwipesEnabled { get; set; }

        public override bool OnTouchEvent(MotionEvent e)
        {
            return SwipesEnabled && base.OnTouchEvent(e);
        }

        public override bool OnInterceptTouchEvent(MotionEvent ev)
        {
            return SwipesEnabled && base.OnInterceptTouchEvent(ev);
        }
    }
}
