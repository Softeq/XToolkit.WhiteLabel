// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;

namespace Softeq.XToolkit.WhiteLabel.Droid.Internal
{
    internal static class ViewModelCache
    {
        private static readonly Dictionary<string, Dictionary<string, object>> _cache =
            new Dictionary<string, Dictionary<string, object>>();

        internal static TViewModel? Get<TViewModel>(string containerId, string key)
            where TViewModel : class
        {
            if (_cache.TryGetValue(containerId, out var container))
            {
                return (TViewModel) container.GetValueOrDefault(key);
            }

            return null;
        }

        internal static void Add(string containerId, string key, object viewModel)
        {
            if (!_cache.ContainsKey(containerId))
            {
                _cache[containerId] = new Dictionary<string, object>();
            }

            _cache[containerId][key] = viewModel;
        }

        internal static void Remove(string containerId, string key)
        {
            if (_cache.TryGetValue(containerId, out var container))
            {
                container.Remove(key);
            }
        }

        internal static void Remove(string containerId, IEnumerable<string> keys)
        {
            if (_cache.TryGetValue(containerId, out var container))
            {
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
