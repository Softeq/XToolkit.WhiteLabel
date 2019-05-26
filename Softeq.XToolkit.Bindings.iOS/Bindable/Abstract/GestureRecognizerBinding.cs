using System;
using System.Windows.Input;
using Softeq.XToolkit.Bindings.iOS.Gestures;

namespace Softeq.XToolkit.Bindings.iOS.Bindable.Abstract
{
    public class GestureRecognizerBinding : Binding
    {
        private GestureRecognizerBehavior _behaviour;

        public GestureRecognizerBinding(GestureRecognizerBehavior behaviour)
        {
            _behaviour = behaviour;
        }

        public void ChangeTarget(ICommand command)
        {
            _behaviour.Command = command;
        }

        public override void Detach()
        {
            _behaviour.Command = null;
            _behaviour = null;
        }

        public override void ForceUpdateValueFromSourceToTarget()
        {
        }

        public override void ForceUpdateValueFromTargetToSource()
        {
        }

        public override event EventHandler ValueChanged;
    }
}