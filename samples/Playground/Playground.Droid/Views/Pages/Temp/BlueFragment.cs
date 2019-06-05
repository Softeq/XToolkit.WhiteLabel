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
    public class BlueFragment : FragmentBase<BlueViewModel>
    {
        private Button _button;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_blue, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _button = view.FindViewById<Button>(Resource.Id.button1);
            _button.SetCommand(ViewModel.NavigateCommand);
        }
    }
}
