// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.Common
{
    public static class WeakReferenceEx
    {
        public static WeakReferenceEx<T> Create<T>(T target) where T : class
        {
            return new WeakReferenceEx<T>(target);
        }
    }

    public class WeakReferenceEx<T> where T : class
    {
        private readonly WeakReference<T> _weakReference;

        public WeakReferenceEx(T obj)
        {
            _weakReference = new WeakReference<T>(obj);
        }

        public T Target
        {
            get
            {
                T target;
                return _weakReference.TryGetTarget(out target) ? target : null;
            }
        }
    }
}