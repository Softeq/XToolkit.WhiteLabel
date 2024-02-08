// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Diagnostics.CodeAnalysis;
using Android.App;
using Android.OS;
using Android.Views;
using Playground.ViewModels;
using Softeq.XToolkit.WhiteLabel.Droid;

namespace Playground.Droid.Views
{
    [Activity(Label = "")]
    [SuppressMessage("ReSharper", "RedundantOverriddenMember", Justification = "Just for play.")]
    public class EmptyPageActivity : ActivityBase<EmptyPageViewModel>
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_empty);

            SupportActionBar!.SetDisplayHomeAsUpEnabled(true);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                OnBackPressed();
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        protected override void DoAttachBindings()
        {
            base.DoAttachBindings();

            // Put your code HERE.
        }

        protected override void DoDetachBindings()
        {
            base.DoDetachBindings();

            // Put your code HERE.
        }
    }
}
