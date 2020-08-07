// Developed by Softeq Development Corporation
// http://www.softeq.com

using AndroidX.AppCompat.App;
using AndroidX.Fragment.App;
using Softeq.XToolkit.WhiteLabel.Droid.Navigation;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Droid.Extensions
{
    public static class FragmentExtensions
    {
        public static void AddViewForViewModel(this Fragment fragment, ViewModelBase viewModel, int containerId)
        {
            AddViewForViewModel(fragment.ChildFragmentManager, viewModel, containerId);
        }

        public static void AddViewForViewModel(this AppCompatActivity activity, ViewModelBase viewModel, int containerId)
        {
            AddViewForViewModel(activity.SupportFragmentManager, viewModel, containerId);
        }

        public static void AddViewForViewModel(this FragmentManager fragmentManager, ViewModelBase viewModel, int containerId)
        {
            var viewLocator = Dependencies.Container.Resolve<IViewLocator>();
            var fragment = (Fragment) viewLocator.GetView(viewModel, ViewType.Fragment);
            fragmentManager
                .BeginTransaction()
                .Add(containerId, fragment)
                .Commit();
        }
    }
}
