using System;

namespace Softeq.XToolkit.Common.Tests.Helpers
{
    public class ReferenceHolder<T> where T : class
    {
        private readonly WeakReference _weakReference;

        public T Value { get; private set; }

        public bool IsObjectAlive => _weakReference.IsAlive;

        public ReferenceHolder(T value)
        {
            Value = value;
            _weakReference = new WeakReference(Value);
        }

        public void ClearReference()
        {
            Value = null;
        }
    }
}
