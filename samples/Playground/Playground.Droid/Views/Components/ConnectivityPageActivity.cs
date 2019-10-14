﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Playground.ViewModels.Components;
using Softeq.XToolkit.WhiteLabel.Droid;
using Softeq.XToolkit.Bindings.Extensions;

namespace Playground.Droid.Views.Components
{
    [Activity(Label = "ConnectivityPageActivity")]
    public class ConnectivityPageActivity : ActivityBase<ConnectivityPageViewModel>
    {
        private TextView _connectionTextView;
        private TextView _typesTextView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_connectivity);

            _connectionTextView = FindViewById<TextView>(Resource.Id.textViewConnection);
            _typesTextView = FindViewById<TextView>(Resource.Id.textViewTypes);
        }

        protected override void DoAttachBindings()
        {
            base.DoAttachBindings();

            this.Bind(() => ViewModel.ConnectionStatus, () => _connectionTextView.Text);
            this.Bind(() => ViewModel.ConnectionTypes, () => _typesTextView.Text);
        }
    }
}
