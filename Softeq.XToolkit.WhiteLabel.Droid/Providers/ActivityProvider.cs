// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.App;

namespace Softeq.XToolkit.WhiteLabel.Droid.Providers
{
    public interface IActivityProvider
    {
        Activity Current { get; }
    }

    public class EssentialsActivityProvider : IActivityProvider
    {
        public Activity Current => Xamarin.Essentials.Platform.CurrentActivity;
    }
}
