// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Windows.Input;
using Android.OS;
using Android.Views;
using Android.Widget;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.WhiteLabel.Droid.Dialogs;
using Softeq.XToolkit.WhiteLabel.Droid.Interfaces;
using Softeq.XToolkit.WhiteLabel.Essentials.FullScreenImage;

namespace Softeq.XToolkit.WhiteLabel.Essentials.Droid.FullScreenImage
{
    public class FullScreenImageDialogFragment : DialogFragmentBase<FullScreenImageViewModel>, View.IOnTouchListener
    {
        private GestureDetector? _gestureDetector;

        protected override int ThemeId => Resource.Style.FullScreenImageDialogTheme;

        protected override int? DialogAnimationId => Resource.Style.FullScreenImageDialogAnimation;

        public override View? OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
        {
            return LayoutInflater.Inflate(Resource.Layout.dialog_full_screen_image, container, true);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            // Setup Swipe handling
            view.SetOnTouchListener(this);

            var listener = new SwipeListener();
            listener.SetCommand(nameof(listener.SwipedBottom), (ICommand) ViewModel.DialogComponent.CloseCommand);
            _gestureDetector = new GestureDetector(Context, listener);

            // Load Image
            var imageView = View!.FindViewById<ImageView>(Resource.Id.dialog_full_screen_image_image);

            LoadImageInto(imageView!);
        }

        public bool OnTouch(View? v, MotionEvent? e)
        {
            return _gestureDetector!.OnTouchEvent(e);
        }

        private void LoadImageInto(ImageView imageView)
        {
            var imageService = Dependencies.Container.Resolve<IDroidImageService>();

            var url = string.IsNullOrEmpty(ViewModel.ImagePath)
                ? ViewModel.ImageUrl
                : ViewModel.ImagePath;

            imageService.LoadImage(Context!, url!, imageView);
        }
    }
}
