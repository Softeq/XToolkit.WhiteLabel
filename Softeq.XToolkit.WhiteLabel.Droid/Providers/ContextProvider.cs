// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Android.App;
using Android.Content;
using Microsoft.Maui.ApplicationModel;
using EssentialsActivityState = Microsoft.Maui.ApplicationModel.ActivityState;

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

        private static ActivityState ToLifecycleState(EssentialsActivityState activityState)
        {
            return activityState switch
            {
                EssentialsActivityState.Created => ActivityState.Created,
                EssentialsActivityState.Destroyed => ActivityState.Destroyed,
                EssentialsActivityState.Paused => ActivityState.Paused,
                EssentialsActivityState.Resumed => ActivityState.Resumed,
                EssentialsActivityState.SaveInstanceState => ActivityState.SaveInstanceState,
                EssentialsActivityState.Started => ActivityState.Started,
                EssentialsActivityState.Stopped => ActivityState.Stopped,
                _ => throw new NotImplementedException()
            };
        }
    }
}
