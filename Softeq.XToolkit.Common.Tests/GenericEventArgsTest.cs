// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using NSubstitute;
using Softeq.XToolkit.Common.EventArguments;
using Xunit;

namespace Softeq.XToolkit.Tests.Core.Common
{
    public class GenericEventArgsTest
    {
        private const string TestString = "test string";
        private const int TestInt = 1;

        public class TestClass
        {
            public event EventHandler<GenericEventArgs<Tuple<string, int>>> TestEvent;

            public void RaiseEvent()
            {
                TestEvent?.Invoke(this,
                    new GenericEventArgs<Tuple<string, int>>(new Tuple<string, int>(TestString, TestInt)));
            }
        }

        [Fact]
        public void Test()
        {
            var mockedItem = Substitute.For<TestClass>();

            mockedItem.Received().TestEvent += (sender, e) =>
            {
                Assert.True(e.Value.Item1 == TestString && e.Value.Item2 == TestInt);
            };

            mockedItem.RaiseEvent();
        }
    }
}