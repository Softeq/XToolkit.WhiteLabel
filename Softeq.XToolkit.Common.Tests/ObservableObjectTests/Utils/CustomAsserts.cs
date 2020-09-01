// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.ComponentModel;
using Xunit;
using Xunit.Sdk;

namespace Softeq.XToolkit.Common.Tests.ObservableObjectTests.Utils
{
    internal static class CustomAsserts
    {
        public static void Assert_NotRaises<T>(Action<EventHandler<T>> attach, Action<EventHandler<T>> detach, Action testCode) where T : EventArgs
        {
            Assert.Throws<RaisesException>(() => Assert.Raises(attach, detach, testCode));
        }

        public static void Assert_PropertyChanged(
            INotifyPropertyChanged @object,
            string propertyName,
            Action testCode)
        {
            bool propertyChangeHappened = false;
            var changedEventHandler = (PropertyChangedEventHandler) ((sender, args) =>
            {
                // YP: removed string.IsNullOrEmpty check from original Xunit Assert.PropertyChanged
                propertyChangeHappened = ((propertyChangeHappened ? 1 : 0) | (args.PropertyName == null
                        ? 0
                        : propertyName.Equals(args.PropertyName, StringComparison.OrdinalIgnoreCase) ? 1 : 0)) != 0;
            });
            @object.PropertyChanged += changedEventHandler;
            try
            {
                testCode();
                if (!propertyChangeHappened)
                {
                    throw new PropertyChangedException(propertyName);
                }
            }
            finally
            {
                @object.PropertyChanged -= changedEventHandler;
            }
        }
    }
}
