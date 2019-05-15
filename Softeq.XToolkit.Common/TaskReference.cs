// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;

namespace Softeq.XToolkit.Common
{
	public class TaskReference
    {
        private readonly Func<Task> _func;
        
        public TaskReference(Func<Task> func)
        {
            _func = func;
        }
        
        public async Task RunAsync()
        {
            await _func();
        }
    }

    public class VoidTaskReference<P1>
    {
        private readonly Func<P1, Task> _func;

        public VoidTaskReference(Func<P1, Task> func)
        {
            _func = func;
        }

        public Task RunAsync(P1 param1)
        {
            return _func(param1);
        }
    }

    public class VoidTaskReference<P1, P2>
    {
        private readonly Func<P1, P2, Task> _func;

        public VoidTaskReference(Func<P1, P2, Task> func)
        {
            _func = func;
        }

        public Task RunAsync(P1 param1, P2 param2)
        {
            return _func(param1, param2);
        }
    }

    public class VoidTaskReference<P1, P2, P3>
    {
        private readonly Func<P1, P2, P3, Task> _func;

        public VoidTaskReference(Func<P1, P2, P3, Task> func)
        {
            _func = func;
        }

        public Task RunAsync(P1 param1, P2 param2, P3 param3)
        {
            return _func(param1, param2, param3);
        }
    }

    public class VoidTaskReference<P1, P2, P3, P4>
    {
        private readonly Func<P1, P2, P3, P4, Task> _func;

        public VoidTaskReference(Func<P1, P2, P3, P4, Task> func)
        {
            _func = func;
        }

        public Task RunAsync(P1 param1, P2 param2, P3 param3, P4 param4)
        {
            return _func(param1, param2, param3, param4);
        }
    }

    public class VoidTaskReference<P1, P2, P3, P4, P5>
    {
        private readonly Func<P1, P2, P3, P4, P5, Task> _func;

        public VoidTaskReference(Func<P1, P2, P3, P4, P5, Task> func)
        {
            _func = func;
        }

        public Task RunAsync(P1 param1, P2 param2, P3 param3, P4 param4, P5 param5)
        {
            return _func(param1, param2, param3, param4, param5);
        }
    }

    public class TaskReference<T>
    {
        private readonly Func<Task<T>> _func;
        
        public TaskReference(Func<Task<T>> func)
        {
            _func = func;
        }
        
        public Task<T> RunAsync()
        {
            return _func();
        }
    }

    public class TaskReference<P1, T>
    {
        private readonly Func<P1, Task<T>> _func;

        public TaskReference(Func<P1, Task<T>> func)
        {
            _func = func;
        }

        public Task<T> RunAsync(P1 param1)
        {
            return _func(param1);
        }
    }

    public class TaskReference<P1, P2, T>
    {
        private readonly Func<P1, P2, Task<T>> _func;

        public TaskReference(Func<P1, P2, Task<T>> func)
        {
            _func = func;
        }

        public Task<T> RunAsync(P1 param1, P2 param2)
        {
            return _func(param1, param2);
        }
    }

    public class TaskReference<P1, P2, P3, T>
    {
        private readonly Func<P1, P2, P3, Task<T>> _func;

        public TaskReference(Func<P1, P2, P3, Task<T>> func)
        {
            _func = func;
        }

        public Task<T> RunAsync(P1 param1, P2 param2, P3 param3)
        {
            return _func(param1, param2, param3);
        }
    }

    public class TaskReference<P1, P2, P3, P4, T>
    {
        private readonly Func<P1, P2, P3, P4, Task<T>> _func;

        public TaskReference(Func<P1, P2, P3, P4, Task<T>> func)
        {
            _func = func;
        }

        public Task<T> RunAsync(P1 param1, P2 param2, P3 param3, P4 param4)
        {
            return _func(param1, param2, param3, param4);
        }
    }

    public class TaskReference<P1, P2, P3, P4, P5, T>
    {
        private readonly Func<P1, P2, P3, P4, P5, Task<T>> _func;

        public TaskReference(Func<P1, P2, P3, P4, P5, Task<T>> func)
        {
            _func = func;
        }

        public Task<T> RunAsync(P1 param1, P2 param2, P3 param3, P4 param4, P5 param5)
        {
            return _func(param1, param2, param3, param4, param5);
        }
    }
}