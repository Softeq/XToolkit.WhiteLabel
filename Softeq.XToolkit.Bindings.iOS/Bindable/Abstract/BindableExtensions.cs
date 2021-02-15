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
        public static Binding Bind(this GestureRecognizerBehaviour recognizer, Action command)
        {
            recognizer.Command = new RelayCommand(command);
            return new GestureRecognizerBinding(recognizer);
        }

        public static Binding SetCommand(this GestureRecognizerBehaviour recognizer, ICommand command)
        {
            recognizer.Command = command;
            return new GestureRecognizerBinding(recognizer);
        }

        public static Binding Bind<T>(this GestureRecognizerBehaviour recognizer, Action<T> command)
            where T : UIGestureRecognizer
        {
            recognizer.Command = new RelayCommand<T>(command);
            return new GestureRecognizerBinding(recognizer);
        }

        public static Binding SetCommand<T>(this GestureRecognizerBehaviour recognizer, ICommand<T> command)
            where T : UIGestureRecognizer
        {
            recognizer.Command = command;
            return new GestureRecognizerBinding(recognizer);
        }
    }
}
