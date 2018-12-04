// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.OS;
using Android.Views;
using Android.Widget;
using FFImageLoading;
using FFImageLoading.Views;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Common.Command;
using Softeq.XToolkit.WhiteLabel.Droid.Dialogs;
using Softeq.XToolkit.WhiteLabel.ViewModels;

namespace Softeq.XToolkit.WhiteLabel.Droid.Views
{
    public class FullScreenImageDialogFragment : DialogFragmentBase<FullScreenImageViewModel>
    {
        private ImageButton _closeButton;

        protected override int ThemeId => Resource.Style.CoreFullScreenImageTheme;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return LayoutInflater.Inflate(Resource.Layout.dialog_full_screen_image, container, true);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _closeButton = View.FindViewById<ImageButton>(Resource.Id.dialog_full_screen_image_close_button);
            _closeButton.SetCommand(nameof(_closeButton.Click),
                new RelayCommand(() => { ViewModel.DialogComponent.CloseCommand.Execute(true); }));
            _closeButton.SetImageResource(Resource.Drawable.core_ic_close);

            var imageView = View.FindViewById<ImageViewAsync>(Resource.Id.dialog_full_screen_image_image);
            ImageService.Instance.LoadUrl(ViewModel.ImageUrl).IntoAsync(imageView);
        }
    }
}