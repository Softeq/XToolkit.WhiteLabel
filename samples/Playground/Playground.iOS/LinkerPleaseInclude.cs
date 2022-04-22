// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using CoreAnimation;
using Foundation;
using UIKit;

namespace Playground.iOS
{
    [SuppressMessage("ReSharper", "UnusedVariable", Justification = "Disabled.")]
    public class LinkerPleaseInclude
    {
        public void Include(UIButton uiButton)
        {
            uiButton.TouchUpInside += (s, e) =>
                uiButton.SetTitle(uiButton.Title(UIControlState.Normal), UIControlState.Normal);
            uiButton.Enabled = true;
        }

        public void Include(UIBarButtonItem barButton)
        {
            barButton.Clicked += (s, e) =>
                barButton.Title = barButton.Title + string.Empty;
        }

        public void Include(UITextField textField)
        {
            textField.Text = textField.Text + string.Empty;
            textField.EditingChanged += (sender, args) => { textField.Text = string.Empty; };
            textField.Started += (sender, args) => { textField.Text = string.Empty; };
            textField.Ended += (sender, args) => { textField.Text = string.Empty; };
            textField.EditingDidBegin += (sender, args) => { textField.Text = string.Empty; };
        }

        public void Include(UITextView textView)
        {
            textView.Text = textView.Text + string.Empty;
            textView.Changed += (sender, args) => { textView.Text = string.Empty; };
            textView.Started += (sender, args) => { textView.Text = string.Empty; };
            textView.Ended += (sender, args) => { textView.Text = string.Empty; };
        }

        public void Include(UIScrollView scrollView)
        {
            scrollView.DraggingStarted += (sender, e) => { };
        }

        public void Include(UILabel label)
        {
            label.Text = label.Text + string.Empty;
        }

        public void Include(UIImageView imageView)
        {
            imageView.Image = new UIImage(imageView.Image!.CGImage!);
        }

        public void Include(UIDatePicker date)
        {
            date.Date = date.Date.AddSeconds(1);
            date.ValueChanged += (sender, args) => { date.Date = new NSDate(); };
        }

        public void Include(UISlider slider)
        {
            slider.Value = slider.Value + 1;
            slider.ValueChanged += (sender, args) => { slider.Value = 1; };
        }

        public void Include(UISwitch sw)
        {
            sw.On = !sw.On;
            sw.ValueChanged += (sender, args) => { sw.On = false; };
        }

        public void Include(CATransition transition)
        {
            transition.AnimationStopped += (sender, args) => { transition.Duration = 0.5; };
        }

        public void Include(INotifyCollectionChanged changed)
        {
            changed.CollectionChanged += (s, e) =>
            {
                var test = $"{e.Action}{e.NewItems}{e.NewStartingIndex}{e.OldItems}{e.OldStartingIndex}";
            };
        }

        public void Include(ICommand command)
        {
            command.CanExecuteChanged += (s, e) =>
            {
                if (command.CanExecute(null))
                {
                    command.Execute(null);
                }
            };
        }

        public void Include(UITabBarController controller)
        {
            controller.ViewControllerSelected += (sender, e) =>
            {
                var test = $"{sender}, {e.ViewController}, {e}";
            };
            controller.SelectedIndex = 1;
        }
    }
}
