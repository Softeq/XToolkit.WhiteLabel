using System;
using System.Collections.Concurrent;

namespace NetworkApp
{
    // wcoder: https://gist.github.com/wcoder/44341a4ecafaaae861c4
    public static class SimpleIoc
    {
        private static readonly ConcurrentDictionary<Type, object> _dependencyMap = new ConcurrentDictionary<Type, object>();

        public static T Resolve<T>() => (T) Resolve(typeof(T));

        public static void Register<T>(T instance) => Register<T>((object) instance);

        private static void Register<T>(object value) => Register(typeof(T), value);

        private static void Register(Type type, object value) => _dependencyMap.TryAdd(type, value);

        private static object Resolve(Type type)
        {
            if (Get(type) is object fastInst)
            {
                return fastInst;
            }

            var ctor = type.GetConstructors()[0];
            var args = ctor.GetParameters();
            var deps = new object[args.Length];

            for (int i = 0; i < args.Length; i++)
            {
                deps[i] = Resolve(args[i].ParameterType);
            }

            return ctor.Invoke(deps);
        }

        private static object Get(Type type)
        {
            _dependencyMap.TryGetValue(type, out object inst);
            return inst;
        }
    }
}
