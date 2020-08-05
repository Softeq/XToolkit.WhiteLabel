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
        public static Func<Task> Create()
        {
            return Substitute.For<Func<Task>>();
        }

        public static Func<Task> FromSource(TaskCompletionSource<bool> source)
        {
            var func = Create();
            func.Invoke().Returns(source.Task);
            return func;
        }

        public static Func<Task> WithException()
        {
            var func = Create();
            func.Invoke().Throws<InvalidOperationException>();
            return func;
        }

        public static Func<T, Task> Create<T>()
        {
            return Substitute.For<Func<T, Task>>();
        }

        public static Func<T, Task> FromSource<T>(TaskCompletionSource<bool> source)
        {
            var func = Create<T>();
            func.Invoke(Arg.Any<T>()).Returns(source.Task);
            return func;
        }

        public static Func<T, Task> WithException<T>()
        {
            var func = Create<T>();
            func.Invoke(Arg.Any<T>()).Throws<InvalidOperationException>();
            return func;
        }
    }
}
