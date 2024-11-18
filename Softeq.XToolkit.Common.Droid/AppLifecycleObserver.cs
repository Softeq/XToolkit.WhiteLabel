// Developed by Softeq Development Corporation
// http://www.softeq.com

using AndroidX.Lifecycle;
using Java.Interop;
using Java.Lang;
using Softeq.XToolkit.Common.Weak;

namespace Softeq.XToolkit.Common.Droid
{
    public class AppLifecycleObserver : Object, ILifecycleObserver
    {
        private readonly WeakAction? _startAction;
        private readonly WeakAction? _stopAction;

        public AppLifecycleObserver(WeakAction? startAction = default, WeakAction? stopAction = default)
        {
            _startAction = startAction;
            _stopAction = stopAction;
        }

        public bool IsForegrounded { get; private set; }

        [Lifecycle.Event.OnStart]
        [Export]
        public void Started()
        {
            IsForegrounded = true;
            _startAction?.Execute();
        }

        [Lifecycle.Event.OnStop]
        [Export]
        public void Stopped()
        {
            IsForegrounded = false;
            _stopAction?.Execute();
        }
    }
}
