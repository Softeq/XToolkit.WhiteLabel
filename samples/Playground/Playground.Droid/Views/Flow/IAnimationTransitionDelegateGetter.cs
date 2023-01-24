// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Android.Views;

namespace Playground.Droid.Views.Flow
{
    public interface IAnimationTransitionDelegateGetter
    {
        Tuple<int[], int, int> GetCurrentAnimationDelegate();

        void ResetAnimationDelegate();

        Tuple<View, string> GetCurrentAnimationDelegate2();
    }
}
