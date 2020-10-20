// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Playground.ViewModels.Controls;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.WhiteLabel.Droid;

namespace Playground.Droid.Views.Controls
{
    [Activity(Label = "Photo Viewer")]
    public class PhotoViewerActivity : ActivityBase<PhotoViewerViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Creating a new RelativeLayout
            var relativeLayout = new RelativeLayout(this);

            // Defining the RelativeLayout layout parameters.
            var relativeLayoutParams = new RelativeLayout.LayoutParams(
                ViewGroup.LayoutParams.MatchParent,
                ViewGroup.LayoutParams.MatchParent);

            // Creating a new Button
            var button = new Button(this);
            button.Text = "Open FullScreen Image";
            button.SetCommand(ViewModel.OpenCommand);

            // Defining the layout parameters of the Button
            var buttonParams = new RelativeLayout.LayoutParams(
                ViewGroup.LayoutParams.WrapContent,
                ViewGroup.LayoutParams.WrapContent);
            buttonParams.AddRule(LayoutRules.CenterInParent);
            button.LayoutParameters = buttonParams;

            // Adding the Button to the RelativeLayout as a child
            relativeLayout.AddView(button);

            // Setting the RelativeLayout as our content view
            SetContentView(relativeLayout, relativeLayoutParams);
        }
    }
}
