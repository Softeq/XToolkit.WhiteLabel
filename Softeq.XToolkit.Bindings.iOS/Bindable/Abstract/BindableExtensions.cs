// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Bindings.iOS.Gestures;
using Softeq.XToolkit.Common.Commands;
using UIKit;

namespace Softeq.XToolkit.Bindings.iOS.Bindable.Abstract
{
    public static class BindableExtensions
    {
        public static Binding Bind<T>(this GestureRecognizerBehaviour<T> recognizer, Action<T> command)
        {
            recognizer.Command = new RelayCommand<T>(command);
            return new GestureRecognizerBinding<T>(recognizer);
        }

        public static Binding SetCommand<T>(this GestureRecognizerBehaviour<T> recognizer, ICommand<T> command)
        {
            recognizer.Command = command;
            return new GestureRecognizerBinding<T>(recognizer);
        }
    }
}
