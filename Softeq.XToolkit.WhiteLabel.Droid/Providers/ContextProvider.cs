// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Android.App;
using Android.Content;
using Softeq.XToolkit.Common.Weak;

#nullable disable

namespace Softeq.XToolkit.WhiteLabel.Droid.Providers
{
    public interface IContextProvider
    {
        event EventHandler CurrentActivityChanged;

        Activity CurrentActivity { get; }

        Context AppContext { get; }
    }

    public class EssentialsContextProvider : IContextProvider
    {
        private WeakReferenceEx<Activity> _activityRef;

        public EssentialsContextProvider()
        {
            _activityRef = WeakReferenceEx.Create(CurrentActivity);
            Xamarin.Essentials.Platform.ActivityStateChanged += OnPlatformActivityStateChanged;
        }

        public event EventHandler CurrentActivityChanged;

        public Activity CurrentActivity => Xamarin.Essentials.Platform.CurrentActivity;

        public Context AppContext => Xamarin.Essentials.Platform.AppContext;

        private void OnPlatformActivityStateChanged(object sender, Xamarin.Essentials.ActivityStateChangedEventArgs e)
        {
            if (_activityRef.Target != CurrentActivity)
            {
                CurrentActivityChanged?.Invoke(this, EventArgs.Empty);
            }

            _activityRef = WeakReferenceEx.Create(CurrentActivity);
        }
    }
}
