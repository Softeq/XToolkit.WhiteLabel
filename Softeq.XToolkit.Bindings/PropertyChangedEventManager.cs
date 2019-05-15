// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Softeq.XToolkit.Common;

namespace Softeq.XToolkit.Bindings
{
    /// <summary>
    ///     Provides an implementation so that you can use the
    ///     "weak event listener" pattern to attach listeners
    ///     for the <see cref="PropertyChanged" /> event.
    /// </summary>
    public class PropertyChangedEventManager
    {
        private static PropertyChangedEventManager _manager;
        private static readonly object SyncLock = new object();
        private Dictionary<string, List<ListenerInfo>> _list;

        /// <summary>
        ///     Get the current instance of <see cref="PropertyChangedEventManager" />
        /// </summary>
        private static PropertyChangedEventManager Instance => _manager ??
                                                               (_manager = new PropertyChangedEventManager());

        /// <summary>
        ///     Adds the specified listener to the list of listeners on the specified source.
        /// </summary>
        /// <param name="source">The object with the event.</param>
        /// <param name="listener">The object to add as a listener.</param>
        /// <param name="propertyName">
        ///     The name of the property that exists on
        ///     source upon which to listen for changes.
        /// </param>
        public static void AddListener(
            INotifyPropertyChanged source,
            IWeakEventListener listener,
            string propertyName)
        {
            Instance.PrivateAddListener(source, listener, propertyName);
        }

        /// <summary>
        ///     Removes the specified listener from the list of listeners on the
        ///     specified source.
        /// </summary>
        /// <param name="listener">The object to remove as a listener.</param>
        public static void RemoveListener(IWeakEventListener listener)
        {
            Instance.PrivateRemoveListener(listener);
        }

        /// <summary>
        ///     Private method to add the specified listener to the list of listeners
        ///     on the specified source.
        /// </summary>
        /// <param name="source">The object with the event.</param>
        /// <param name="listener">The object to add as a listener.</param>
        /// <param name="propertyName">
        ///     The name of the property that exists
        ///     on source upon which to listen for changes.
        /// </param>
        private void PrivateAddListener(
            INotifyPropertyChanged source,
            IWeakEventListener listener,
            string propertyName)
        {
            if (source == null)
            {
                return;
            }

            lock (SyncLock)
            {
                if (_list == null)
                {
                    _list = new Dictionary<string, List<ListenerInfo>>();
                }

                var sourceExists = _list.Any(
                    list => list.Value.Any(
                        entry => entry.InstanceReference != null
                                 && entry.InstanceReference.IsAlive
                                 && entry.InstanceReference.Target != null
                                 && entry.InstanceReference.Target.Equals(source)));

                if (_list.ContainsKey(propertyName))
                {
                    _list[propertyName]
                        .Add(
                            new ListenerInfo(
                                listener,
                                source));
                }
                else
                {
                    var list = new List<ListenerInfo>
                    {
                        new ListenerInfo(listener, source)
                    };
                    _list.Add(propertyName, list);
                }

                if (!sourceExists)
                {
                    // Now, start listening to source
                    StartListening(source);
                }
            }
        }

        /// <summary>
        ///     Private method to remove the specified listener from the list of listeners
        ///     on the specified source.
        /// </summary>
        /// <param name="listener">The object to remove as a listener.</param>
        private void PrivateRemoveListener(IWeakEventListener listener)
        {
            if (_list != null)
            {
                lock (SyncLock)
                {
                    string propertyName = null;
                    ListenerInfo toRemove = null;

                    foreach (var list in _list)
                    {
                        foreach (var info in list.Value)
                        {
                            if (info.Listener == listener)
                            {
                                propertyName = list.Key;
                                toRemove = info;
                                break;
                            }
                        }

                        if (!string.IsNullOrEmpty(propertyName))
                        {
                            break;
                        }
                    }

                    if (string.IsNullOrEmpty(propertyName))
                    {
                        return;
                    }

                    _list[propertyName].Remove(toRemove);

                    if (_list[propertyName].Count == 0)
                    {
                        _list.Remove(propertyName);
                    }

                    var checkInstance =
                        _list.Any(
                            l => l.Value.Any(
                                i => i.InstanceReference != null
                                     && i.InstanceReference.IsAlive
                                     && i.InstanceReference.Target != null
                                     && i.InstanceReference.Target.Equals(toRemove.InstanceReference.Target)));

                    if (!checkInstance)
                    {
                        StopListening((INotifyPropertyChanged) toRemove.InstanceReference.Target);
                    }
                }
            }
        }

        /// <summary>
        ///     The method that handles the <see cref="INotifyPropertyChanged.PropertyChanged" /> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">
        ///     A <see cref="PropertyChangedEventArgs" /> that
        ///     contains the event data.
        /// </param>
        private void PropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (!_list.ContainsKey(args.PropertyName))
            {
                return;
            }

            var list = _list[args.PropertyName];
            if (list != null)
            {
                var recipients =
                    list.Where(
                            i => i.InstanceReference != null
                                 && i.InstanceReference.IsAlive
                                 && i.InstanceReference.Target == sender
                                 && i.Listener != null)
                        .ToList();

                // We have the listeners. Deal with them
                foreach (var item in recipients)
                {
                    item.Listener.ReceiveWeakEvent(GetType(), sender, args);
                }
            }
        }

        /// <summary>
        ///     Begin listening for the <see cref="PropertyChanged" /> event on
        ///     the provided source.
        /// </summary>
        /// <param name="source">
        ///     The object on which to start listening
        ///     for <see cref="PropertyChanged" />.
        /// </param>
        private void StartListening(INotifyPropertyChanged source)
        {
            if (source != null)
            {
                source.PropertyChanged += PropertyChanged;
            }
        }

        /// <summary>
        ///     Stop listening for the <see cref="PropertyChanged" /> event on the
        ///     provided source.
        /// </summary>
        /// <param name="source">
        ///     The object on which to start listening for
        ///     <see cref="PropertyChanged" />.
        /// </param>
        private void StopListening(INotifyPropertyChanged source)
        {
            if (source != null)
            {
                source.PropertyChanged -= PropertyChanged;
            }
        }

        private class ListenerInfo
        {
            public ListenerInfo(IWeakEventListener listener, INotifyPropertyChanged inpc)
            {
                Listener = listener;
                InstanceReference = new WeakReference(inpc);
            }

            public WeakReference InstanceReference { get; }

            public IWeakEventListener Listener { get; }
        }
    }
}