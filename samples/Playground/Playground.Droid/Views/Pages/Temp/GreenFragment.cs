// Developed by Softeq Development Corporation
// http://www.softeq.com

﻿using Android.OS;
using Android.Views;
using Playground.ViewModels.Pages.Temp;
using Softeq.XToolkit.WhiteLabel.Droid;

namespace Playground.Droid.Views.Pages.Temp
{
    public class GreenFragment : FragmentBase<GreenViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_green, container, false);
        }
    }
}
