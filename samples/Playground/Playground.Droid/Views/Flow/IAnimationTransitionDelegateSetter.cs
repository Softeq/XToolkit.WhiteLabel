// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Views;

namespace Playground.Droid.Views.Flow
{
    public interface IAnimationTransitionDelegateSetter
    {
        void SetCurrentTransitionDelegate(int[] transitioningDelegate, int width, int height);
        void SetCurrentTransitionDelegate(View fromView, string name);
    }
}
