// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Reflection;

namespace Softeq.XToolkit.Bindings.Tests
{
    internal class StubEvent
    {
        public StubEvent(object element, string eventName)
        {
            ElementType = element.GetType();
            EventInfo = ElementType.GetRuntimeEvent(eventName);
            EventName = eventName;
        }

        public string EventName { get; }
        public EventInfo EventInfo { get; }
        public Type ElementType { get; }
    }
}
