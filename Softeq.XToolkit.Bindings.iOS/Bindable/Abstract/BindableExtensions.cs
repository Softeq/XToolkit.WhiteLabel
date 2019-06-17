// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Bindings.iOS.Gestures;
using Softeq.XToolkit.Common.Command;

namespace Softeq.XToolkit.Bindings.iOS.Bindable.Abstract
{
    public static class BindableExtensions
    {
        public static Binding Bind(this GestureRecognizerBehavior recognizer, Action command)
        {
            recognizer.Command = new RelayCommand(command);
            return new GestureRecognizerBinding(recognizer);
        }
    }
}
