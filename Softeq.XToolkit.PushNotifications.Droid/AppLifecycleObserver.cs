// Developed by Softeq Development Corporation
// http://www.softeq.com

using AndroidX.Lifecycle;
using Java.Interop;
using Java.Lang;

namespace Softeq.XToolkit.PushNotifications.Droid
{
    public class AppLifecycleObserver : Object, ILifecycleObserver
    {
        public bool IsForegrounded { get; private set; }

        [Lifecycle.Event.OnStop]
        [Export]
        public void Stopped()
        {
            IsForegrounded = false;
        }

        [Lifecycle.Event.OnStart]
        [Export]
        public void Started()
        {
            IsForegrounded = true;
        }
    }
}
