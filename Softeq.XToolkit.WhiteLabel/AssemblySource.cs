// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.Common.Extensions;

namespace Softeq.XToolkit.WhiteLabel
{
    /// <summary>
    ///     A source of assemblies that are inspectable by the framework.
    /// </summary>
    public static class AssemblySource
    {
        /// <summary>
        ///     The singleton instance of the AssemblySource used by the framework.
        /// </summary>
        public static readonly ObservableRangeCollection<Assembly> Instance = new ObservableRangeCollection<Assembly>();

        /// <summary>
        ///     Finds a type which matches one of the elements in the sequence of names.
        /// </summary>
        public static Func<IEnumerable<string>, Type> FindTypeByNames = names =>
        {
            if (names == null)
            {
                return null;
            }

            var type = names
                .Join(Instance.SelectMany(a => a.GetExportedTypes()), n => n, t => t.FullName, (n, t) => t)
                .FirstOrDefault();

            return type;
        };
    }

    /// <summary>
    ///     A caching subsystem for <see cref="AssemblySource" />.
    /// </summary>
    public static class AssemblySourceCache
    {
        private static bool _isInstalled;
        private static readonly IDictionary<string, Type> TypeNameCache = new Dictionary<string, Type>();

        /// <summary>
        ///     Extracts the types from the spezified assembly for storing in the cache.
        /// </summary>
        public static Func<Assembly, IEnumerable<Type>> ExtractTypes = assembly =>
            assembly.GetExportedTypes()
                .Where(t => typeof(INotifyPropertyChanged).IsAssignableFrom(t));

        /// <summary>
        ///     Installs the caching subsystem.
        /// </summary>
        public static void Install()
        {
            if (_isInstalled)
            {
                return;
            }

            _isInstalled = true;

            AssemblySource.Instance.CollectionChanged += (s, e) =>
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        e.NewItems.OfType<Assembly>()
                            .SelectMany(a => ExtractTypes(a))
                            .Apply(t => TypeNameCache.Add(t.FullName, t));
                        break;
                    case NotifyCollectionChangedAction.Remove:
                    case NotifyCollectionChangedAction.Replace:
                    case NotifyCollectionChangedAction.Reset:
                        TypeNameCache.Clear();
                        AssemblySource.Instance
                            .SelectMany(a => ExtractTypes(a))
                            .Apply(t => TypeNameCache.Add(t.FullName, t));
                        break;
                }
            };

            AssemblySource.FindTypeByNames = names =>
            {
                if (names == null)
                {
                    return null;
                }

                var type = names.Select(n => TypeNameCache.GetValueOrDefault(n)).FirstOrDefault(t => t != null);
                return type;
            };
        }
    }
}