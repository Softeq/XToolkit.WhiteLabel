// Developed by Softeq Development Corporation
// http://www.softeq.com

﻿using Android.OS;
using Android.Views;
using Android.Widget;
using Playground.ViewModels.Pages.Temp;
using Softeq.XToolkit.WhiteLabel.Droid;
using Softeq.XToolkit.Bindings;

namespace Playground.Droid.Views.Pages.Temp
{
    public class RedFragment : FragmentBase<RedViewModel>
    {
        private Button _navigateButton;
        private Button _incrementButton;
        private TextView _incrementLabel;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_red, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _navigateButton = view.FindViewById<Button>(Resource.Id.button1);
            _navigateButton.SetCommand(ViewModel.NavigateCommand);

            _incrementButton = view.FindViewById<Button>(Resource.Id.button2);
            _incrementButton.SetCommand(ViewModel.IncrementCommand);
            _incrementButton.Click += (sender, e) => _incrementLabel.Text = "changed";

            _incrementLabel = view.FindViewById<TextView>(Resource.Id.textView1);
        }

        protected override void DoAttachBindings()
        {
            base.DoAttachBindings();
            Bindings.Add(this.SetBinding(() => ViewModel.Count).WhenSourceChanges((() =>
            {
                _incrementButton.Text = ViewModel.Count.ToString();
            })));
        }
    }
}
