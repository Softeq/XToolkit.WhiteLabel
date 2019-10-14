using System;
using Foundation;
using RemoteApp.ViewModels;
using UIKit;

namespace RemoteApp.iOS
{
    public partial class ViewController : UIViewController
    {
        private MainPageViewModel _viewModel;

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _viewModel = new MainPageViewModel();
            _viewModel.PropertyChanged += _viewModel_PropertyChanged;

            RequestBtn.TouchUpInside += RequestBtn_TouchUpInside;
            ClearLogBtn.TouchUpInside += ClearLogBtn_TouchUpInside;
        }

        private void _viewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            BeginInvokeOnMainThread(() =>
            {
                if (e.PropertyName == nameof(_viewModel.ResultData))
                {
                    ResultLabel.Text = _viewModel.ResultData;
                }
                else if (e.PropertyName == nameof(_viewModel.IsBusy))
                {
                    IndicatorView.Hidden = !_viewModel.IsBusy;
                }
                else if (e.PropertyName == nameof(_viewModel.LogData))
                {
                    LogView.Text = _viewModel.LogData;

                    LogView.ScrollRangeToVisible(new NSRange(0, -1)); // scroll text to bottom
                }
                else
                {
                    throw new InvalidOperationException($"Invalid property: {e.PropertyName}");
                }
            });
        }

        private void RequestBtn_TouchUpInside(object sender, EventArgs e)
        {
            _viewModel.RequestCommand.Execute(null);
        }

        private void ClearLogBtn_TouchUpInside(object sender, EventArgs e)
        {
            _viewModel.ClearLogCommand.Execute(null);
        }
    }
}