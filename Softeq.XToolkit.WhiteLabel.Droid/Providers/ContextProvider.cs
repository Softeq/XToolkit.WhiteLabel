// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Android.App;
using Android.Content;
using Xamarin.Essentials;

#nullable disable

namespace Softeq.XToolkit.WhiteLabel.Droid.Providers
{
    public enum ActivityState
    {
        Created,
        Resumed,
        Paused,
        Destroyed,
        SaveInstanceState,
        Started,
        Stopped
    }

    public interface IContextProvider
    {
        event EventHandler<ActivityStateChangedEventArgs> ActivityStateChanged;

        Activity CurrentActivity { get; }

        Context AppContext { get; }
    }

    public class ActivityStateChangedEventArgs : EventArgs
    {
        internal ActivityStateChangedEventArgs(Activity activity, ActivityState state)
        {
            State = state;
            Activity = activity;
        }

        public ActivityState State { get; }

        public Activity Activity { get; }
    }

    public class EssentialsContextProvider : IContextProvider
    {
        public EssentialsContextProvider()
        {
            Platform.ActivityStateChanged += (sender, args) =>
                ActivityStateChanged?.Invoke(sender, new ActivityStateChangedEventArgs(args.Activity, ToLifecycleState( args.State)));
        }

        public event EventHandler<ActivityStateChangedEventArgs> ActivityStateChanged;

        public Activity CurrentActivity => Platform.CurrentActivity;

        public Context AppContext => Platform.AppContext;

        private static ActivityState ToLifecycleState(Xamarin.Essentials.ActivityState activityState)
        {
            return activityState switch
            {
                Xamarin.Essentials.ActivityState.Created => ActivityState.Created,
                Xamarin.Essentials.ActivityState.Destroyed => ActivityState.Destroyed,
                Xamarin.Essentials.ActivityState.Paused => ActivityState.Paused,
                Xamarin.Essentials.ActivityState.Resumed => ActivityState.Resumed,
                Xamarin.Essentials.ActivityState.SaveInstanceState => ActivityState.SaveInstanceState,
                Xamarin.Essentials.ActivityState.Started => ActivityState.Started,
                Xamarin.Essentials.ActivityState.Stopped => ActivityState.Stopped,
                _ => throw new NotImplementedException()
            };
        }
    }
}
