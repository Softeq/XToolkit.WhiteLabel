// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Reflection;

namespace Softeq.XToolkit.Common.WeakSubscription
{
    public class WeakEventSubscription<TSource, TEventArgs> : IDisposable
        where TSource : class
    {
        private readonly WeakReference _targetReference;
        private readonly WeakReference<TSource> _sourceReference;

        private readonly MethodInfo _eventHandlerMethodInfo;

        private readonly EventInfo _sourceEventInfo;

        // we store a copy of our Delegate/EventHandler in order to prevent it being
        // garbage collected while the `client` still has ownership of this subscription
        private readonly Delegate _ourEventHandler;

        private bool _subscribed;

        public WeakEventSubscription(
            TSource source,
            string sourceEventName,
            EventHandler<TEventArgs> targetEventHandler)
            : this(source, typeof(TSource).GetEvent(sourceEventName), targetEventHandler)
        {
        }

        protected WeakEventSubscription(
            TSource source,
            EventInfo sourceEventInfo,
            EventHandler<TEventArgs> targetEventHandler)
        {
            if (source == null)
                throw new ArgumentNullException();

            if (sourceEventInfo == null)
                throw new ArgumentNullException(nameof(sourceEventInfo),
                                                "missing source event info in WeakEventSubscription");

            _eventHandlerMethodInfo = targetEventHandler.GetMethodInfo();
            _targetReference = new WeakReference(targetEventHandler.Target);
            _sourceReference = new WeakReference<TSource>(source);
            _sourceEventInfo = sourceEventInfo;

            _ourEventHandler = CreateEventHandler();

            AddEventHandler();
        }

        protected virtual Delegate CreateEventHandler()
        {
            return new EventHandler<TEventArgs>(OnSourceEvent);
        }

        protected virtual object GetTargetObject()
        {
            return _targetReference.Target;
        }

        //This is the method that will handle the event of source.
        protected void OnSourceEvent(object sender, TEventArgs e)
        {
            var target = GetTargetObject();
            if (target != null)
            {
                _eventHandlerMethodInfo.Invoke(target, new[] { sender, e });
            }
            else
            {
                RemoveEventHandler();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                RemoveEventHandler();
            }
        }

        private void RemoveEventHandler()
        {
            if (!_subscribed)
                return;

            TSource source;
            if (_sourceReference.TryGetTarget(out source))
            {
                _sourceEventInfo.GetRemoveMethod().Invoke(source, new object[] { _ourEventHandler });
                _subscribed = false;
            }
        }

        private void AddEventHandler()
        {
            if (_subscribed)
                throw new Exception("Should not call _subscribed twice");

            TSource source;
            if (_sourceReference.TryGetTarget(out source))
            {
                _sourceEventInfo.GetAddMethod().Invoke(source, new object[] { _ourEventHandler });
                _subscribed = true;
            }
        }
    }
}
