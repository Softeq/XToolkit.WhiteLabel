// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Windows.Input;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.WhiteLabel.Droid.Extensions;

namespace Softeq.XToolkit.WhiteLabel.Droid.Controls
{
    [Register("com.softeq.xtoolkit.whitelabel.droid.NavigationBarView")]
    public class NavigationBarView : RelativeLayout
    {
        private ImageView _centerImageView;
        private ImageButton _leftButton;
        private ImageButton _rightButton;
        private TextView _titleTextView;

        public NavigationBarView(Context context) : base(context)
        {
            Init(context);
        }

        public NavigationBarView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init(context);
        }

        public NavigationBarView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            Init(context);
        }

        public NavigationBarView(IntPtr handle, JniHandleOwnership owner) : base(handle, owner)
        {
        }

        public Button RightTextButton { get; private set; }

        public ImageButton RightImageButton => _rightButton;

        public void SetLeftButton(int resourceId, ICommand command, int? color = null)
        {
            _leftButton.SetImageResource(resourceId);
            _leftButton.Visibility = ViewStates.Visible;

            if (color.HasValue)
            {
                _leftButton.SetColorFilter(_leftButton.GetColor(color.Value));
            }

            if (command != null)
            {
                _leftButton.SetCommand(nameof(_leftButton.Click), command);
            }
        }

        public void SetRightButton(int resourceId, ICommand command)
        {
            _rightButton.SetImageResource(resourceId);
            _rightButton.Visibility = ViewStates.Visible;

            if (command != null)
            {
                _rightButton.SetCommand(nameof(_rightButton.Click), command);
            }
        }

        public void SetRightButton(Drawable drawable, ICommand command)
        {
            _rightButton.SetImageDrawable(drawable);
            _rightButton.Visibility = ViewStates.Visible;

            if (command != null)
            {
                _rightButton.SetCommand(nameof(_rightButton.Click), command);
            }
        }

        public void SetRightButton(string label, ICommand command)
        {
            RightTextButton.Text = label;
            RightTextButton.Visibility = ViewStates.Visible;

            if (command != null)
            {
                RightTextButton.SetCommand(nameof(_rightButton.Click), command);
            }
        }

        public void SetCenterImage(int resourceId, ICommand command)
        {
            _centerImageView.SetImageResource(resourceId);
            _centerImageView.Visibility = ViewStates.Visible;

            if (command != null)
            {
                _centerImageView.SetCommand(nameof(_centerImageView.Click), command);
            }
        }

        public void SetTitle(string text)
        {
            _titleTextView.Text = text;
            _titleTextView.Visibility = ViewStates.Visible;
        }

        public void SetBackground(int resourceId)
        {
            var view = FindViewById<View>(Resource.Id.control_navigation_bar_container);
            view.SetBackgroundResource(resourceId);
        }

        private void Init(Context context)
        {
            Inflate(context, Resource.Layout.control_navigation_bar, this);

            _leftButton = FindViewById<ImageButton>(Resource.Id.control_navigation_bar_left_button);
            _leftButton.Visibility = ViewStates.Gone;

            _rightButton = FindViewById<ImageButton>(Resource.Id.control_navigation_bar_right_button);
            _rightButton.Visibility = ViewStates.Gone;

            RightTextButton = FindViewById<Button>(Resource.Id.control_navigation_bar_right_text_button);
            RightTextButton.Visibility = ViewStates.Gone;

            _centerImageView = FindViewById<ImageView>(Resource.Id.control_navigation_bar_center_image);
            _centerImageView.Visibility = ViewStates.Gone;

            _titleTextView = FindViewById<TextView>(Resource.Id.control_navigation_bar_title);
            _titleTextView.Visibility = ViewStates.Gone;
        }
    }
}
