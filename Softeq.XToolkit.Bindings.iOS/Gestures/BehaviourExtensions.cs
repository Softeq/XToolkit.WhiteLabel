// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Windows.Input;
using Softeq.XToolkit.Common.Commands;
using UIKit;

namespace Softeq.XToolkit.Bindings.iOS.Gestures
{
    public static class BehaviourExtensions
    {
        public static TapGestureRecognizerBehaviour Tap(
            this UIView view,
            uint numberOfTapsRequired = 1,
            uint numberOfTouchesRequired = 1,
            bool cancelsTouchesInView = true)
        {
            return new TapGestureRecognizerBehaviour(
                view,
                numberOfTapsRequired,
                numberOfTouchesRequired,
                cancelsTouchesInView);
        }

        public static SwipeGestureRecognizerBehaviour Swipe(
            this UIView view,
            UISwipeGestureRecognizerDirection direction,
            uint numberOfTouchesRequired = 1)
        {
            return new SwipeGestureRecognizerBehaviour(view, direction, numberOfTouchesRequired);
        }

        public static PanGestureRecognizerBehaviour Pan(this UIView view)
        {
            return new PanGestureRecognizerBehaviour(view);
        }

        public static Binding Bind(this GestureRecognizerBehaviour recognizer, Action action)
        {
            recognizer.Command = new RelayCommand(action);
            return new GestureRecognizerBinding(recognizer);
        }

        public static Binding Bind<T>(this GestureRecognizerBehaviour<T> recognizer, Action<T> action)
            where T : UIGestureRecognizer
        {
            recognizer.Command = new RelayCommand<T>(action);
            return new GestureRecognizerBinding(recognizer);
        }

        public static void SetCommand(this GestureRecognizerBehaviour recognizer, ICommand command)
        {
            recognizer.Command = command;
        }

        public static void SetCommand<T>(this GestureRecognizerBehaviour<T> recognizer, ICommand<T> command)
            where T : UIGestureRecognizer
        {
            recognizer.Command = command;
        }
    }
}
