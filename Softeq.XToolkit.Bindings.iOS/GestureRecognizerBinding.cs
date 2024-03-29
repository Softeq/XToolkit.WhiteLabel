// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Bindings.iOS.Gestures;

#nullable disable

namespace Softeq.XToolkit.Bindings.iOS
{
    public class GestureRecognizerBinding : Binding
    {
        private GestureRecognizerBehaviour _behaviour;

        public GestureRecognizerBinding(GestureRecognizerBehaviour behaviour)
        {
            _behaviour = behaviour;
        }

#pragma warning disable 67, CS0067
        public override event EventHandler ValueChanged;
#pragma warning restore

        /// <inheritdoc />
        public override void Detach()
        {
            _behaviour.Command = null;
            _behaviour = null;
        }

        /// <inheritdoc />
        public override void ForceUpdateValueFromSourceToTarget()
        {
        }

        /// <inheritdoc />
        public override void ForceUpdateValueFromTargetToSource()
        {
        }
    }
}
