// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using AndroidX.Fragment.App;

namespace Softeq.XToolkit.WhiteLabel.Droid.Internal
{
    internal static class ViewModelStore
    {
        private const string ViewModelStoreTag = "WL_ViewModelStore";

        internal static IViewModelStore Of(Fragment fragment)
        {
            return Of(fragment.Activity);
        }

        internal static IViewModelStore Of(FragmentActivity activity)
        {
            return Get(activity.SupportFragmentManager);
        }

        private static IViewModelStore Get(FragmentManager fragmentManager)
        {
            ViewModelStoreFragment viewModelStore;

            if (!fragmentManager.IsDestroyed)
            {
                viewModelStore = (ViewModelStoreFragment) fragmentManager.FindFragmentByTag(ViewModelStoreTag);

                if (viewModelStore == null)
                {
                    viewModelStore = ViewModelStoreFragment.NewInstance();

                    fragmentManager
                        .BeginTransaction()
                        .Add(viewModelStore, ViewModelStoreTag)
                        .CommitNow();
                }
            }
            else
            {
                throw new Exception("view model store has been destroyed");
            }

            return viewModelStore;
        }
    }
}
