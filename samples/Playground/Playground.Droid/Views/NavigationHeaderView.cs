// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Content.Resources;
using Softeq.XToolkit.Bindings;

namespace Playground.Droid.Views
{
    [Register("com.views.navigation.NavigationHeaderView")]
    public sealed class NavigationHeaderView : FrameLayout
    {
        private Button _leftButton;
        private TextView _titleLabel;
        private Button _rightButton;

        public NavigationHeaderView(Context context) : base(context)
        {
            Initialize(context);
        }

        public NavigationHeaderView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize(context);
        }

        public NavigationHeaderView(Context context, IAttributeSet attrs, int defStyleAttr) :
            base(context, attrs, defStyleAttr)
        {
            Initialize(context);
        }

        public bool IsLeftButtonShown
        {
            get => _leftButton.Visibility == ViewStates.Visible;
            set => _leftButton.Visibility = value ? ViewStates.Visible : ViewStates.Invisible;
        }

        public bool IsRightButtonShown
        {
            get => _rightButton.Visibility == ViewStates.Visible;
            set => _rightButton.Visibility = value ? ViewStates.Visible : ViewStates.Gone;
        }

        public string Title
        {
            get => _titleLabel.Text;
            set => _titleLabel.Text = value;
        }

        private void Initialize(Context context)
        {
            var view = Inflate(context, Resource.Layout.view_navigation_header, this);

            _leftButton = view.FindViewById<Button>(Resource.Id.view_navigation_header_left_button);
            _leftButton.Visibility = ViewStates.Invisible;

            _titleLabel = view.FindViewById<TextView>(Resource.Id.view_navigation_header_title_label);
            _titleLabel.Visibility = ViewStates.Visible;

            _rightButton = view.FindViewById<Button>(Resource.Id.view_navigation_header_right_button);
            _rightButton.Visibility = ViewStates.Gone;
        }

        public void SetLeftButton(NavigationBarButtonSettings settings)
        {
            _leftButton.Text = settings.Title;
            _leftButton.SetTypeface(ResourcesCompat.GetFont(Context, settings.IsBold ?
                Resource.Font.open_sans_semibold :
                Resource.Font.open_sans_regular), TypefaceStyle.Normal);
            _leftButton.SetCompoundDrawablesWithIntrinsicBounds(settings.Image, null, null, null);
            _leftButton.SetCommand(settings.Command);
            _leftButton.Visibility = ViewStates.Visible;
        }

        public void SetRightButton(NavigationBarButtonSettings settings)
        {
            _rightButton.Text = settings.Title;
            _rightButton.SetTypeface(ResourcesCompat.GetFont(Context, settings.IsBold ?
                Resource.Font.open_sans_semibold :
                Resource.Font.open_sans_regular), TypefaceStyle.Normal);
            _rightButton.SetCompoundDrawablesWithIntrinsicBounds(settings.Image, null, null, null);
            _rightButton.SetCommand(settings.Command);
            _rightButton.Visibility = ViewStates.Visible;
        }
    }
}
