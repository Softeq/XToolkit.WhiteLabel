// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using AndroidX.Fragment.App;

namespace Softeq.XToolkit.WhiteLabel.Droid.Internal
{
    internal static class ViewModelStore
    {
        private const string ViewModelStoreTag = "WL_ViewModelStore";

        internal static IViewModelStore Of(FragmentManager fragmentManager)
        {
            return Get(fragmentManager);
        }

        private static IViewModelStore Get(FragmentManager fragmentManager)
        {
            ViewModelStoreFragment viewModelStore;

            if (!fragmentManager.IsDestroyed)
            {
                viewModelStore = (ViewModelStoreFragment) fragmentManager.FindFragmentByTag(ViewModelStoreTag);

                if (viewModelStore == null)
                {
                    viewModelStore = new ViewModelStoreFragment();

                    fragmentManager
                        .BeginTransaction()
                        .Add(viewModelStore, ViewModelStoreTag)
                        .Commit();
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
