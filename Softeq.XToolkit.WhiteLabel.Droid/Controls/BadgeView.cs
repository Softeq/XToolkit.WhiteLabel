// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Widget;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Common;
using Softeq.XToolkit.Common.Droid.Converters;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;

namespace Softeq.XToolkit.WhiteLabel.Droid.Controls
{
    internal class BadgeView : FrameLayout
    {
        private WeakReferenceEx<TabViewModel> _viewModelRef;
        private TextView _textView;
        private Binding _textBinding;
        private Binding _visibilityBinding;

        public BadgeView(Context context) :
            base(context)
        {
            Initialize(context);
        }

        public BadgeView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize(context);
        }

        public BadgeView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize(context);
        }

        public ColorStateList BackgroundColor
        {
            get => ((GradientDrawable) _textView.Background).Color;
            set => ((GradientDrawable) _textView.Background).SetColor(value);
        }

        public ColorStateList TextColor
        {
            get => _textView.TextColors;
            set => _textView.SetTextColor(value);
        }

        internal void SetViewModel(TabViewModel viewModel)
        {
            _viewModelRef = new WeakReferenceEx<TabViewModel>(viewModel);

            _textBinding?.Detach();
            _textBinding = this.SetBinding(() => _viewModelRef.Target.BadgeText, () => _textView.Text);

            _visibilityBinding?.Detach();
            _visibilityBinding = this.SetBinding(() => _viewModelRef.Target.IsBadgeVisible, () => Visibility)
                .ConvertSourceToTarget(BoolToViewStateConverter.ConvertGone);
        }

        private void Initialize(Context context)
        {
            Inflate(context, Resource.Layout.control_notification_badge, this);
            _textView = FindViewById<TextView>(Resource.Id.notification_badge_text_view);
        }
    }
}
