// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.OS;
using Android.Views;
using Android.Widget;
using FFImageLoading;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Droid.Dialogs;
using Softeq.XToolkit.WhiteLabel.Essentials.FullScreenImage;

namespace Softeq.XToolkit.WhiteLabel.Essentials.Droid.FullScreenImage
{
    public class FullScreenImageDialogFragment : DialogFragmentBase<FullScreenImageViewModel>
    {
        private ImageButton? _closeButton;

        protected override int ThemeId => Resource.Style.CoreFullScreenImageTheme;

        public override View? OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return LayoutInflater.Inflate(Resource.Layout.dialog_full_screen_image, container, true);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _closeButton = View.FindViewById<ImageButton>(Resource.Id.dialog_full_screen_image_close_button);
            _closeButton.SetCommand(
                nameof(_closeButton.Click),
                new RelayCommand(() =>
                {
                    ViewModel.DialogComponent.CloseCommand.Execute(true);
                }));
            _closeButton!.SetImageResource(Resource.Drawable.core_ic_close);

            var imageView = View.FindViewById<ImageView>(Resource.Id.dialog_full_screen_image_image);

            LoadImageInto(imageView!);
        }

        private void LoadImageInto(ImageView imageView)
        {
            var imageService = ImageService.Instance;

            var task = string.IsNullOrEmpty(ViewModel.ImagePath) == false
                ? imageService.LoadFile(ViewModel.ImagePath)
                : imageService.LoadUrl(ViewModel.ImageUrl);

            task.IntoAsync(imageView);
        }
    }
}
