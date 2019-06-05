
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Playground.ViewModels.Pages.Temp;
using Softeq.XToolkit.WhiteLabel.Droid;
using Softeq.XToolkit.Bindings;


namespace Playground.Droid.Views.Pages.Temp
{
    public class RedFragment : FragmentBase<RedViewModel>
    {
        private Button _button;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_red, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _button = view.FindViewById<Button>(Resource.Id.button1);
            _button.SetCommand(ViewModel.NavigateCommand);
        }
    }
}
