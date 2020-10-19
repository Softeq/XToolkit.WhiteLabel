// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Windows.Input;
using Android.OS;
using Android.Views;
using Android.Widget;
using FFImageLoading;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.WhiteLabel.Droid.Dialogs;
using Softeq.XToolkit.WhiteLabel.Essentials.FullScreenImage;

namespace Softeq.XToolkit.WhiteLabel.Essentials.Droid.FullScreenImage
{
    public class FullScreenImageDialogFragment : DialogFragmentBase<FullScreenImageViewModel>, View.IOnTouchListener
    {
        private GestureDetector? _detector;

        protected override int ThemeId => Resource.Style.CoreFullScreenImageTheme;

        protected override int? DialogAnimationId => Resource.Style.CoreFullScreenImageDialogAnimation;

        public override View? OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return LayoutInflater.Inflate(Resource.Layout.dialog_full_screen_image, container, true);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            // Setup Swipe handling
            view.SetOnTouchListener(this);

            var listener = new GestureListener();
            listener.SetCommand(nameof(listener.SwipedBottom), (ICommand) ViewModel.DialogComponent.CloseCommand);

            _detector = new GestureDetector(Context, listener);

            // Load Image
            var imageView = View.FindViewById<ImageView>(Resource.Id.dialog_full_screen_image_image);

            LoadImageInto(imageView!);
        }

        public bool OnTouch(View? v, MotionEvent? e)
        {
            return _detector!.OnTouchEvent(e);
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
