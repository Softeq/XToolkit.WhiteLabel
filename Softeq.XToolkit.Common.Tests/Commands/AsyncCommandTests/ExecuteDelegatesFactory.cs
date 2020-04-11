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

        public static Func<string, Task> CreateFuncWithArg()
        {
            return Substitute.For<Func<string, Task>>();
        }

        public static Func<string, Task> CreateFuncWithArgAndDelay()
        {
            var func = CreateFuncWithArg();
            func.Invoke(Arg.Any<string>()).Returns(_ => Task.Delay(10));
            return func;
        }

        public static Func<string, Task> CreateFuncWithArgAndException()
        {
            var func = CreateFuncWithArg();
            func.Invoke(Arg.Any<string>()).Throws<InvalidOperationException>();
            return func;
        }
    }
}
