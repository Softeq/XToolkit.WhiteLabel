// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Softeq.XToolkit.WhiteLabel.Forms.Controls
{
    /// <summary>
    ///     A button with On/Off states. State names for VisualStateManager are
    ///     <see cref="ToggledOnState"/>, <see cref="ToggledOffState"/>.
    /// </summary>
    public class ToggleButton : Button
    {
        /// <summary>
        ///     On state name for VisualStateManager.
        /// </summary>
        public const string ToggledOnState = "ToggledOn";

        /// <summary>
        ///     Off state name for VisualStateManager.
        /// </summary>
        public const string ToggledOffState = "ToggledOff";

        public static BindableProperty IsToggledProperty = BindableProperty.Create(
            nameof(IsToggled),
            typeof(bool),
            typeof(ToggleButton),
            false);

        public static BindableProperty IsToggleEnabledProperty = BindableProperty.Create(
            nameof(IsToggleEnabled),
            typeof(bool),
            typeof(ToggleButton),
            true);

        public ToggleButton()
        {
            Clicked += OnClick;
        }

        /// <summary>
        ///     Event for button state changes.
        /// </summary>
        public event EventHandler<ToggledEventArgs>? Toggled;

        /// <summary>
        ///     Gets or sets a value indicating whether button state is On.
        /// </summary>
        public bool IsToggled
        {
            get => (bool) GetValue(IsToggledProperty);
            set => SetValue(IsToggledProperty, value);
        }

        /// <summary>
        ///     Gets or sets a value indicating whether button can be toggled.
        /// </summary>
        public bool IsToggleEnabled
        {
            get => (bool) GetValue(IsToggleEnabledProperty);
            set => SetValue(IsToggleEnabledProperty, value);
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();
            VisualStateManager.GoToState(this, IsToggled ? ToggledOnState : ToggledOffState);
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == IsToggledProperty.PropertyName)
            {
                Toggled?.Invoke(this, new ToggledEventArgs(IsToggled));
                VisualStateManager.GoToState(this, IsToggled ? ToggledOnState : ToggledOffState);
            }
        }

        private void OnClick(object sender, EventArgs e)
        {
            if (IsToggleEnabled)
            {
                IsToggled = !IsToggled;
            }
        }
    }
}
