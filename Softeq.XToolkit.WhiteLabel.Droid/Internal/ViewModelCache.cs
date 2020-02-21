// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Droid.Internal
{
    internal static class ViewModelCache
    {
        private static readonly Dictionary<string, Dictionary<string, IViewModelBase>> _cache;

        static ViewModelCache()
        {
            _cache = new Dictionary<string, Dictionary<string, IViewModelBase>>();
        }

        internal static TViewModel? Get<TViewModel>(string containerId, string key)
            where TViewModel : class, IViewModelBase
        {
            var container = _cache.GetValueOrDefault(containerId);

            if (container != null)
            {
                return (TViewModel) container.GetValueOrDefault(key);
            }

            return null;
        }

        internal static void Add(string containerId, string key, IViewModelBase viewModel)
        {
            if (!_cache.ContainsKey(containerId))
            {
                _cache[containerId] = new Dictionary<string, IViewModelBase>();
            }

            _cache[containerId][key] = viewModel;
        }

        internal static void Remove(string containerId, string key)
        {
            if (_cache.ContainsKey(containerId))
            {
                _cache[containerId].Remove(key);
            }
        }

        internal static void Remove(string containerId, IReadOnlyCollection<string> keys)
        {
            if (_cache.ContainsKey(containerId))
            {
                var container = _cache[containerId];

                foreach (var key in keys)
                {
                    container.Remove(key);
                }
            }
        }

        internal static void Clear(string containerId)
        {
            _cache.Remove(containerId);
        }
    }
}
