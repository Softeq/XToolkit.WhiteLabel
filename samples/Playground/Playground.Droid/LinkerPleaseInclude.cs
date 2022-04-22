// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using Android.App;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;

namespace Playground.Droid
{
    [SuppressMessage("ReSharper", "UnusedVariable", Justification = "Disabled.")]
    public class LinkerPleaseInclude
    {
        public void Include(Button button)
        {
            button.Click += (s, e) => button.Text = button.Text + string.Empty;
        }

        public void Include(AppCompatImageButton button)
        {
            button.Click += (s, e) => { button.TextAlignment = TextAlignment.Center; };
        }

        public void Include(CheckBox checkBox)
        {
            checkBox.CheckedChange += (sender, args) => checkBox.Checked = !checkBox.Checked;
        }

        public void Include(Switch @switch)
        {
            @switch.CheckedChange += (sender, args) => @switch.Checked = !@switch.Checked;
        }

        public void Include(View view)
        {
            view.Click += (s, e) => view.ContentDescription = view.ContentDescription + string.Empty;
        }

        public void Include(TextView text)
        {
            text.TextChanged += (sender, args) => text.Text = string.Empty + text.Text;
            text.Hint = string.Empty + text.Hint;
        }

        public void Include(CheckedTextView text)
        {
            text.TextChanged += (sender, args) => text.Text = string.Empty + text.Text;
            text.Hint = string.Empty + text.Hint;
        }

        public void Include(CompoundButton cb)
        {
            cb.CheckedChange += (sender, args) => cb.Checked = !cb.Checked;
        }

        public void Include(SeekBar sb)
        {
            sb.ProgressChanged += (sender, args) => sb.Progress = sb.Progress + 1;
        }

        public void Include(Activity act)
        {
            act.Title = act.Title + string.Empty;
        }

        public void Include(Dialog dialog)
        {
            dialog.DismissEvent += (sender, e) => { dialog.SetTitle("title"); };
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

        public void Include(INotifyPropertyChanged changed)
        {
            changed.PropertyChanged += (sender, e) =>
            {
                var test = e.PropertyName;
            };
        }
    }
}
