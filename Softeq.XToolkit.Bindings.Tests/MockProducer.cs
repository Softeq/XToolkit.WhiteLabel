// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.Bindings.Tests
{
    public class MockProducer
    {
        public event EventHandler SimpleEvent;
        public event EventHandler<string> GenericStringEvent;

        public virtual void RaiseSimpleEvent() => SimpleEvent?.Invoke(this, EventArgs.Empty);
        public virtual void RaiseGenericStringEvent(string parameter) => GenericStringEvent?.Invoke(this, parameter);
    }
}
