// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests
{
    public static class ExecuteDelegatesFactory
    {
        public static Func<Task> CreateFunc()
        {
            return Substitute.For<Func<Task>>();
        }

        public static Func<Task> CreateFuncWithDelay()
        {
            var func = CreateFunc();
            func.Invoke().Returns(_ => Task.Delay(10));
            return func;
        }

        public static Func<Task> CreateFuncWithException()
        {
            var func = CreateFunc();
            func.Invoke().Throws<InvalidOperationException>();
            return func;
        }

        public static Func<T, Task> CreateFunc<T>()
        {
            return Substitute.For<Func<T, Task>>();
        }

        public static Func<T, Task> CreateFuncWithDelay<T>()
        {
            var func = CreateFunc<T>();
            func.Invoke(Arg.Any<T>()).Returns(_ => Task.Delay(10));
            return func;
        }

        public static Func<T, Task> CreateFuncWithException<T>()
        {
            var func = CreateFunc<T>();
            func.Invoke(Arg.Any<T>()).Throws<InvalidOperationException>();
            return func;
        }
    }
}
