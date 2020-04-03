// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;

namespace Softeq.XToolkit.Common.Tasks
{
    /// <summary>
    ///     Func&lt;Task&gt; wrapper
    /// </summary>
    public class TaskReference
    {
        private readonly Func<Task> _func;

        /// <summary>
        ///     Create wrapper over specified function
        /// </summary>
        /// <param name="func">Wrapped function.</param>
        public TaskReference(Func<Task> func)
        {
            _func = func;
        }

        /// <summary>
        ///     Execute wrapped function
        /// </summary>
        /// <returns>Task returned from wrapped function</returns>
        public async Task RunAsync()
        {
            await _func();
        }
    }

    /// <summary>
    ///     Func&lt;P1, Task&gt; wrapper
    /// </summary>
    /// <typeparam name="T1">First parameter type</typeparam>
    public class VoidTaskReference<T1>
    {
        private readonly Func<T1, Task> _func;

        /// <summary>
        ///     Create wrapper over specified function
        /// </summary>
        /// <param name="func">Wrapped function.</param>
        public VoidTaskReference(Func<T1, Task> func)
        {
            _func = func;
        }

        /// <summary>
        ///     Execute wrapped function
        /// </summary>
        /// <returns>Task returned from wrapped function</returns>
        /// <param name="param1">First parameter</param>
        public Task RunAsync(T1 param1)
        {
            return _func(param1);
        }
    }

    /// <summary>
    ///     Func&lt;P1, P2, Task&gt; wrapper
    /// </summary>
    /// <typeparam name="T1">First parameter type</typeparam>
    /// <typeparam name="T2">Second parameter type</typeparam>
    public class VoidTaskReference<T1, T2>
    {
        private readonly Func<T1, T2, Task> _func;

        /// <summary>
        ///     Create wrapper over specified function
        /// </summary>
        /// <param name="func">Wrapped function.</param>
        public VoidTaskReference(Func<T1, T2, Task> func)
        {
            _func = func;
        }

        /// <summary>
        ///     Execute wrapped function
        /// </summary>
        /// <returns>Task returned from wrapped function</returns>
        /// <param name="param1">First parameter</param>
        /// <param name="param2">Second parameter</param>
        public Task RunAsync(T1 param1, T2 param2)
        {
            return _func(param1, param2);
        }
    }

    /// <summary>
    ///     Func&lt;P1, P2, P3, Task&gt; wrapper
    /// </summary>
    /// <typeparam name="T1">First parameter type</typeparam>
    /// <typeparam name="T2">Second parameter type</typeparam>
    /// <typeparam name="T3">Third parameter type</typeparam>
    public class VoidTaskReference<T1, T2, T3>
    {
        private readonly Func<T1, T2, T3, Task> _func;

        /// <summary>
        ///     Create wrapper over specified function
        /// </summary>
        /// <param name="func">Wrapped function.</param>
        public VoidTaskReference(Func<T1, T2, T3, Task> func)
        {
            _func = func;
        }

        /// <summary>
        ///     Execute wrapped function
        /// </summary>
        /// <returns>Task returned from wrapped function</returns>
        /// <param name="param1">First parameter</param>
        /// <param name="param2">Second parameter</param>
        /// <param name="param3">Third parameter</param>
        public Task RunAsync(T1 param1, T2 param2, T3 param3)
        {
            return _func(param1, param2, param3);
        }
    }

    /// <summary>
    ///     Func&lt;P1, P2, P3, P4, Task&gt; wrapper
    /// </summary>
    /// <typeparam name="T1">First parameter type</typeparam>
    /// <typeparam name="T2">Second parameter type</typeparam>
    /// <typeparam name="T3">Third parameter type</typeparam>
    /// <typeparam name="T4">Fourth parameter type</typeparam>
    public class VoidTaskReference<T1, T2, T3, T4>
    {
        private readonly Func<T1, T2, T3, T4, Task> _func;

        /// <summary>
        ///     Create wrapper over specified function
        /// </summary>
        /// <param name="func">Wrapped function.</param>
        public VoidTaskReference(Func<T1, T2, T3, T4, Task> func)
        {
            _func = func;
        }

        /// <summary>
        ///     Execute wrapped function
        /// </summary>
        /// <returns>Task returned from wrapped function</returns>
        /// <param name="param1">First parameter</param>
        /// <param name="param2">Second parameter</param>
        /// <param name="param3">Third parameter</param>
        /// <param name="param4">Fourth parameter</param>
        public Task RunAsync(T1 param1, T2 param2, T3 param3, T4 param4)
        {
            return _func(param1, param2, param3, param4);
        }
    }

    /// <summary>
    ///     Func&lt;P1, P2, P3, P4, P5, Task&gt; wrapper.
    /// </summary>
    /// <typeparam name="T1">First parameter type.</typeparam>
    /// <typeparam name="T2">Second parameter type.</typeparam>
    /// <typeparam name="T3">Third parameter type.</typeparam>
    /// <typeparam name="T4">Fourth parameter type.</typeparam>
    /// <typeparam name="T5">Fifth parameter type.</typeparam>
    public class VoidTaskReference<T1, T2, T3, T4, T5>
    {
        private readonly Func<T1, T2, T3, T4, T5, Task> _func;

        /// <summary>
        ///     Create wrapper over specified function.
        /// </summary>
        /// <param name="func">Wrapped function.</param>
        public VoidTaskReference(Func<T1, T2, T3, T4, T5, Task> func)
        {
            _func = func;
        }

        /// <summary>
        ///     Execute wrapped function.
        /// </summary>
        /// <returns>Task returned from wrapped function.</returns>
        /// <param name="param1">First parameter.</param>
        /// <param name="param2">Second parameter.</param>
        /// <param name="param3">Third parameter.</param>
        /// <param name="param4">Fourth parameter.</param>
        /// <param name="param5">Fifth parameter.</param>
        public Task RunAsync(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5)
        {
            return _func(param1, param2, param3, param4, param5);
        }
    }

    /// <summary>
    ///     Func&lt;P1, Task&lt;T&gt;&gt; wrapper
    /// </summary>
    /// <typeparam name="T">The type of the result produced by this Task</typeparam>
    public class TaskReference<T>
    {
        private readonly Func<Task<T>> _func;

        /// <summary>
        ///     Create wrapper over specified function
        /// </summary>
        /// <param name="func">Wrapped function.</param>
        public TaskReference(Func<Task<T>> func)
        {
            _func = func;
        }

        /// <summary>
        ///     Execute wrapped function.
        /// </summary>
        /// <returns>Task returned from wrapped function.</returns>
        public Task<T> RunAsync()
        {
            return _func();
        }
    }

    /// <summary>
    ///     Func&lt;P1, Task&lt;T&gt;&gt; wrapper.
    /// </summary>
    /// <typeparam name="T1">First parameter type.</typeparam>
    /// <typeparam name="TResult">The type of the result produced by this Task.</typeparam>
    public class TaskReference<T1, TResult>
    {
        private readonly Func<T1, Task<TResult>> _func;

        /// <summary>
        ///     Create wrapper over specified function.
        /// </summary>
        /// <param name="func">Wrapped function.</param>
        public TaskReference(Func<T1, Task<TResult>> func)
        {
            _func = func;
        }

        /// <summary>
        ///     Execute wrapped function
        /// </summary>
        /// <returns>Task returned from wrapped function</returns>
        /// <param name="param1">First parameter</param>
        public Task<TResult> RunAsync(T1 param1)
        {
            return _func(param1);
        }
    }

    /// <summary>
    ///     Func&lt;P1, P2, Task&lt;T&gt;&gt; wrapper.
    /// </summary>
    /// <typeparam name="T1">First parameter type.</typeparam>
    /// <typeparam name="T2">Second parameter type.</typeparam>
    /// <typeparam name="TResult">The type of the result produced by this Task.</typeparam>
    public class TaskReference<T1, T2, TResult>
    {
        private readonly Func<T1, T2, Task<TResult>> _func;

        /// <summary>
        ///     Create wrapper over specified function
        /// </summary>
        /// <param name="func">Wrapped function.</param>
        public TaskReference(Func<T1, T2, Task<TResult>> func)
        {
            _func = func;
        }

        /// <summary>
        ///     Execute wrapped function.
        /// </summary>
        /// <returns>Task returned from wrapped function</returns>
        /// <param name="param1">First parameter</param>
        /// <param name="param2">Second parameter</param>
        public Task<TResult> RunAsync(T1 param1, T2 param2)
        {
            return _func(param1, param2);
        }
    }

    /// <summary>
    ///     Func&lt;P1, P2, P3, Task&lt;T&gt;&gt; wrapper
    /// </summary>
    /// <typeparam name="T1">First parameter type</typeparam>
    /// <typeparam name="T2">Second parameter type</typeparam>
    /// <typeparam name="T3">Third parameter type</typeparam>
    /// <typeparam name="TResult">The type of the result produced by this Task</typeparam>
    public class TaskReference<T1, T2, T3, TResult>
    {
        private readonly Func<T1, T2, T3, Task<TResult>> _func;

        /// <summary>
        ///     Create wrapper over specified function
        /// </summary>
        /// <param name="func">Wrapped function.</param>
        public TaskReference(Func<T1, T2, T3, Task<TResult>> func)
        {
            _func = func;
        }

        /// <summary>
        ///     Execute wrapped function
        /// </summary>
        /// <returns>Task returned from wrapped function</returns>
        /// <param name="param1">First parameter</param>
        /// <param name="param2">Second parameter</param>
        /// <param name="param3">Third parameter</param>
        public Task<TResult> RunAsync(T1 param1, T2 param2, T3 param3)
        {
            return _func(param1, param2, param3);
        }
    }

    /// <summary>
    ///     Func&lt;P1, P2, P3, P4, Task&lt;T&gt;&gt; wrapper
    /// </summary>
    /// <typeparam name="T1">First parameter type</typeparam>
    /// <typeparam name="T2">Second parameter type</typeparam>
    /// <typeparam name="T3">Third parameter type</typeparam>
    /// <typeparam name="T4">Fourth parameter type</typeparam>
    /// <typeparam name="TResult">The type of the result produced by this Task</typeparam>
    public class TaskReference<T1, T2, T3, T4, TResult>
    {
        private readonly Func<T1, T2, T3, T4, Task<TResult>> _func;

        /// <summary>
        ///     Create wrapper over specified function
        /// </summary>
        /// <param name="func">Wrapped function.</param>
        public TaskReference(Func<T1, T2, T3, T4, Task<TResult>> func)
        {
            _func = func;
        }

        /// <summary>
        ///     Execute wrapped function
        /// </summary>
        /// <returns>Task returned from wrapped function</returns>
        /// <param name="param1">First parameter</param>
        /// <param name="param2">Second parameter</param>
        /// <param name="param3">Third parameter</param>
        /// <param name="param4">Fourth parameter</param>
        public Task<TResult> RunAsync(T1 param1, T2 param2, T3 param3, T4 param4)
        {
            return _func(param1, param2, param3, param4);
        }
    }

    /// <summary>
    ///     Func&lt;P1, P2, P3, P4, P5, Task&lt;T&gt;&gt; wrapper.
    /// </summary>
    /// <typeparam name="T1">First parameter type</typeparam>
    /// <typeparam name="T2">Second parameter type</typeparam>
    /// <typeparam name="T3">Third parameter type</typeparam>
    /// <typeparam name="T4">Fourth parameter type</typeparam>
    /// <typeparam name="T5">Fifth parameter type</typeparam>
    /// <typeparam name="TResult">The type of the result produced by this Task</typeparam>
    public class TaskReference<T1, T2, T3, T4, T5, TResult>
    {
        private readonly Func<T1, T2, T3, T4, T5, Task<TResult>> _func;

        /// <summary>
        ///     Create wrapper over specified function.
        /// </summary>
        /// <param name="func">Wrapped function.</param>
        public TaskReference(Func<T1, T2, T3, T4, T5, Task<TResult>> func)
        {
            _func = func;
        }

        /// <summary>
        ///     Execute wrapped function.
        /// </summary>
        /// <returns>Task returned from wrapped function.</returns>
        /// <param name="param1">First parameter.</param>
        /// <param name="param2">Second parameter.</param>
        /// <param name="param3">Third parameter.</param>
        /// <param name="param4">Fourth parameter.</param>
        /// <param name="param5">Fifth parameter.</param>
        public Task<TResult> RunAsync(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5)
        {
            return _func(param1, param2, param3, param4, param5);
        }
    }
}
