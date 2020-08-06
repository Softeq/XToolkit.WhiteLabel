// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Widget;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Common.Droid.Converters;
using Softeq.XToolkit.Common.Weak;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;

namespace Softeq.XToolkit.WhiteLabel.Droid.Controls
{
    internal class BadgeView<TKey> : FrameLayout, IBindingsOwner
    {
        private TextView _textView = default!;
        private WeakReferenceEx<TabViewModel<TKey>>? _viewModelRef;

        public BadgeView(Context context)
            : base(context)
        {
            Initialize(context);
        }

        public BadgeView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            Initialize(context);
        }

        public BadgeView(Context context, IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
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

        public List<Binding> Bindings { get; } = new List<Binding>();

        internal void SetViewModel(TabViewModel<TKey> viewModel)
        {
            _viewModelRef = new WeakReferenceEx<TabViewModel<TKey>>(viewModel);

            this.DetachBindings();
            this.Bind(() => _viewModelRef.Target.BadgeText, () => _textView.Text);
            this.Bind(() => _viewModelRef.Target.IsBadgeVisible, () => Visibility, new GoneConverter());
        }

        private void Initialize(Context context)
        {
            Inflate(context, Resource.Layout.control_notification_badge, this);
            _textView = FindViewById<TextView>(Resource.Id.notification_badge_text_view);
        }
    }
}
