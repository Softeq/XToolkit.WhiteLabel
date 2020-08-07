// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections;
using System.Collections.Generic;

namespace Softeq.XToolkit.Common.Tests.Logger
{
    public class ConsoleLoggerExceptionTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "LogCategory", new Exception() };
            yield return new object[] { "LogCategory", new Exception("log error with message") };
            yield return new object[] { "LogCategory", GenerateException() };
            yield return new object[] { "LogCategory", GenerateException(GenerateException("e1"), GenerateException("e2")) };
            yield return new object[] { "LogCategory", null };
            yield return new object[] { null, new Exception() };
            yield return new object[] { null, null };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private static Exception GenerateException(string message)
        {
            try
            {
                throw new InvalidOperationException(message);
            }
            catch (Exception e)
            {
                return e;
            }
        }

        private static Exception GenerateException(params Exception[] inners)
        {
            try
            {
                throw new AggregateException(inners);
            }
            catch (Exception e)
            {
                return e;
            }
        }
    }
}
