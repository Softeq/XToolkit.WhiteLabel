// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
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

        // TODO WL: Export to WL.Droid
        private class GestureListener : GestureDetector.SimpleOnGestureListener
        {
            private const int SwipeThreshold = 100;
            private const int SwipeVelocityThreshold = 100;

            public event EventHandler? SwipedRight;
            public event EventHandler? SwipedLeft;
            public event EventHandler? SwipedBottom;
            public event EventHandler? SwipedTop;

            public override bool OnDown(MotionEvent? e)
            {
                return true;
            }

            public override bool OnFling(MotionEvent? e1, MotionEvent? e2, float velocityX, float velocityY)
            {
                var result = false;

                if (e1 == null || e2 == null)
                {
                    return result;
                }

                try
                {
                    float diffY = e2.GetY() - e1.GetY();
                    float diffX = e2.GetX() - e1.GetX();
                    if (Math.Abs(diffX) > Math.Abs(diffY))
                    {
                        if (Math.Abs(diffX) > SwipeThreshold && Math.Abs(velocityX) > SwipeVelocityThreshold)
                        {
                            if (diffX > 0)
                            {
                                SwipedRight?.Invoke(this, EventArgs.Empty);
                            }
                            else
                            {
                                SwipedLeft?.Invoke(this, EventArgs.Empty);
                            }

                            result = true;
                        }
                    }
                    else if (Math.Abs(diffY) > SwipeThreshold && Math.Abs(velocityY) > SwipeVelocityThreshold)
                    {
                        if (diffY > 0)
                        {
                            SwipedBottom?.Invoke(this, EventArgs.Empty);
                        }
                        else
                        {
                            SwipedTop?.Invoke(this, EventArgs.Empty);
                        }

                        result = true;
                    }
                }
                catch (Exception exception)
                {
                    System.Diagnostics.Debug.WriteLine(exception);
                }

                return result;
            }
        }
    }
}
