// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Android.Support.V4.App;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Droid.Internal
{
    // TODO YP:
    // - clean up after back from host activity;

    internal static class ViewModelCache
    {
        private static readonly Dictionary<string, IViewModelBase> _cache;

        static ViewModelCache()
        {
            _cache = new Dictionary<string, IViewModelBase>();
        }

        internal static void Add(Fragment fragment, IViewModelBase viewModel)
        {
            var fragmentName = fragment.GetType().Name;

            if (!_cache.ContainsKey(fragmentName))
            {
                _cache.Add(fragmentName, viewModel);
            }
        }

        internal static IViewModelBase Get(Fragment fragment)
        {
            var fragmentName = fragment.GetType().Name;

            _cache.TryGetValue(fragmentName, out var viewModel);

            return viewModel;
        }

        internal static void Remove(Fragment fragment)
        {
            _cache.Remove(fragment.GetType().Name);
        }
    }
}
