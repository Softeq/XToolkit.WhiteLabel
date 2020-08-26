// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.App;
using Android.Content;

namespace Softeq.XToolkit.WhiteLabel.Droid.Providers
{
    public interface IContextProvider
    {
        Activity CurrentActivity { get; }

        Context AppContext { get; }
    }

    public class EssentialsContextProvider : IContextProvider
    {
        public Activity CurrentActivity => Xamarin.Essentials.Platform.CurrentActivity;

        public Context AppContext => Xamarin.Essentials.Platform.AppContext;
    }
}
