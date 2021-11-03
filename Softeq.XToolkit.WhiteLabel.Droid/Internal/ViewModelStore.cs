// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using AndroidX.Fragment.App;

namespace Softeq.XToolkit.WhiteLabel.Droid.Internal
{
    public static class ViewModelStore
    {
        private const string ViewModelStoreTag = "WL_ViewModelStore";

        public static void SetupFor(FragmentManager fragmentManager)
        {
            Get(fragmentManager);
        }

        internal static IInstanceStorage Of(FragmentManager fragmentManager)
        {
            return Get(fragmentManager);
        }

        internal static string GenerateKeyForType(Type type)
        {
            return type.Name;
        }

        private static IInstanceStorage Get(FragmentManager fragmentManager)
        {
            if (fragmentManager.IsDestroyed)
            {
                throw new InvalidOperationException("View Model store has been destroyed");
            }

            var viewModelStore = (ViewModelStorageFragment?) fragmentManager.FindFragmentByTag(ViewModelStoreTag);

            if (viewModelStore == null)
            {
                viewModelStore = new ViewModelStorageFragment();

                fragmentManager
                    .BeginTransaction()
                    .Add(viewModelStore, ViewModelStoreTag)
                    .Commit();
            }

            return viewModelStore;
        }
    }
}
