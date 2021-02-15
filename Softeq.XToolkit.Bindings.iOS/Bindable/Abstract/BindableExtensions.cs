// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Windows.Input;
using Softeq.XToolkit.Bindings.iOS.Gestures;
using Softeq.XToolkit.Common.Commands;
using UIKit;

namespace Softeq.XToolkit.Bindings.iOS.Bindable.Abstract
{
    public static class BindableExtensions
    {
        public static Binding Bind(this GestureRecognizerBehaviour recognizer, Action action)
        {
            recognizer.Command = new RelayCommand(action);
            return new GestureRecognizerBinding(recognizer);
        }

        public static Binding SetCommand(this GestureRecognizerBehaviour recognizer, ICommand action)
        {
            recognizer.Command = action;
            return new GestureRecognizerBinding(recognizer);
        }

        public static Binding Bind<T>(this GestureRecognizerBehaviour recognizer, Action<T> action)
            where T : UIGestureRecognizer
        {
            recognizer.Command = new RelayCommand<T>(action);
            return new GestureRecognizerBinding(recognizer);
        }

        public static Binding SetCommand<T>(this GestureRecognizerBehaviour recognizer, ICommand<T> action)
            where T : UIGestureRecognizer
        {
            recognizer.Command = action;
            return new GestureRecognizerBinding(recognizer);
        }
    }
}
