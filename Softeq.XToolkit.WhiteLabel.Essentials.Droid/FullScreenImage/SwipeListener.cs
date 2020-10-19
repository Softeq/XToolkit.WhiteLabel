// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Android.Views;

namespace Softeq.XToolkit.WhiteLabel.Essentials.Droid.FullScreenImage
{
    // TODO WL: Export to WL.Droid
    public class SwipeListener : GestureDetector.SimpleOnGestureListener
    {
        private const int SwipeThreshold = 100;
        private const int SwipeVelocityThreshold = 100;

        public event EventHandler? SwipedRight;
        public event EventHandler? SwipedLeft;
        public event EventHandler? SwipedBottom;
        public event EventHandler? SwipedTop;

        public override bool OnDown(MotionEvent? e)
        {
            return true;
        }

        public override bool OnFling(MotionEvent? e1, MotionEvent? e2, float velocityX, float velocityY)
        {
            var result = false;

            if (e1 == null || e2 == null)
            {
                return result;
            }

            try
            {
                var diffY = e2.GetY() - e1.GetY();
                var diffX = e2.GetX() - e1.GetX();

                if (Math.Abs(diffX) > Math.Abs(diffY))
                {
                    if (Math.Abs(diffX) > SwipeThreshold && Math.Abs(velocityX) > SwipeVelocityThreshold)
                    {
                        if (diffX > 0)
                        {
                            SwipedRight?.Invoke(this, EventArgs.Empty);
                        }
                        else
                        {
                            SwipedLeft?.Invoke(this, EventArgs.Empty);
                        }

                        result = true;
                    }
                }
                else if (Math.Abs(diffY) > SwipeThreshold && Math.Abs(velocityY) > SwipeVelocityThreshold)
                {
                    if (diffY > 0)
                    {
                        SwipedBottom?.Invoke(this, EventArgs.Empty);
                    }
                    else
                    {
                        SwipedTop?.Invoke(this, EventArgs.Empty);
                    }

                    result = true;
                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception);
            }

            return result;
        }
    }
}
