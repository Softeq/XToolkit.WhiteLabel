// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.OS;
using Softeq.XToolkit.WhiteLabel.Droid.ViewComponents;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;

namespace Softeq.XToolkit.WhiteLabel.Droid.Views
{
    public abstract class ToolbarActivityBase<TViewModel, TKey> : ActivityBase<TViewModel>
        where TViewModel : ToolbarViewModelBase<TKey>
    {
        protected abstract ToolbarComponent<TViewModel, TKey> ToolbarComponent { get; }

        public override void OnBackPressed()
        {
            if (!ToolbarComponent.HandleBackPress(ViewModel))
            {
                base.OnBackPressed();
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ToolbarComponent.Initialize(ViewModel);
        }
    }
}
