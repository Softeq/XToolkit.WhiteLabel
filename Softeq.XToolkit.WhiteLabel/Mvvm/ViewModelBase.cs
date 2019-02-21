// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.Mvvm
{
    public interface IViewModelBase
    {
        string ScreensGroupName { get; set; }
        void OnInitialize();
        void OnAppearing();
        void OnDisappearing();
    }


    /// <summary>
    ///     A base class for the ViewModel classes in the MVVM pattern.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Design",
        "CA1012",
        Justification = "Constructors should remain public to allow serialization.")]
    public abstract class ViewModelBase : ObservableObject, IViewModelBase
    {
        private bool _isBusy;

        public bool IsBusy
        {
            get => _isBusy;
            set { Set(() => IsBusy, ref _isBusy, value); }
        }

        public bool IsInitialized { get; private set; }

        public IFrameNavigationService FrameNavigationService { get; set; }

        public string ScreensGroupName { get; set; }

        public virtual void OnInitialize()
        {
            IsInitialized = true;
        }

        public virtual void OnAppearing()
        {
        }

        public virtual void OnDisappearing()
        {
        }

        #region Property Change

        // ReSharper disable PublicConstructorInAbstractClass

        /// <summary>
        ///     Raises the PropertyChanged event if needed, and broadcasts a
        ///     PropertyChangedMessage using the Messenger instance (or the
        ///     static default instance if no Messenger instance is available).
        /// </summary>
        /// <typeparam name="T">
        ///     The type of the property that
        ///     changed.
        /// </typeparam>
        /// <param name="propertyName">
        ///     The name of the property that
        ///     changed.
        /// </param>
        /// <param name="oldValue">
        ///     The property's value before the change
        ///     occurred.
        /// </param>
        /// <param name="newValue">
        ///     The property's value after the change
        ///     occurred.
        /// </param>
        [SuppressMessage(
            "Microsoft.Design",
            "CA1026:DefaultParametersShouldNotBeUsed")]
        [SuppressMessage(
            "Microsoft.Design", "CA1030:UseEventsWhereAppropriate",
            Justification = "This cannot be an event")]
        public virtual void RaisePropertyChanged<T>(
            [CallerMemberName] string propertyName = null,
            T oldValue = default(T),
            T newValue = default(T))
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("This method cannot be called with an empty string", nameof(propertyName));
            }

            // ReSharper disable ExplicitCallerInfoArgument
            RaisePropertyChanged(propertyName);
            // ReSharper restore ExplicitCallerInfoArgument
        }

        /// <summary>
        ///     Raises the PropertyChanged event if needed, and broadcasts a
        ///     PropertyChangedMessage using the Messenger instance (or the
        ///     static default instance if no Messenger instance is available).
        /// </summary>
        /// <typeparam name="T">
        ///     The type of the property that
        ///     changed.
        /// </typeparam>
        /// <param name="propertyExpression">
        ///     An expression identifying the property
        ///     that changed.
        /// </param>
        /// <param name="oldValue">
        ///     The property's value before the change
        ///     occurred.
        /// </param>
        /// <param name="newValue">
        ///     The property's value after the change
        ///     occurred.
        /// </param>
        [SuppressMessage(
            "Microsoft.Design",
            "CA1030:UseEventsWhereAppropriate",
            Justification = "This cannot be an event")]
        [SuppressMessage(
            "Microsoft.Design",
            "CA1006:GenericMethodsShouldProvideTypeParameter",
            Justification = "This syntax is more convenient than the alternatives.")]
        public virtual void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression, T oldValue, T newValue)
        {
            RaisePropertyChanged(propertyExpression);
        }

        /// <summary>
        ///     Assigns a new value to the property. Then, raises the
        ///     PropertyChanged event if needed, and broadcasts a
        ///     PropertyChangedMessage using the Messenger instance (or the
        ///     static default instance if no Messenger instance is available).
        /// </summary>
        /// <typeparam name="T">
        ///     The type of the property that
        ///     changed.
        /// </typeparam>
        /// <param name="field">The field storing the property's value.</param>
        /// <param name="newValue">
        ///     The property's value after the change
        ///     occurred.
        /// </param>
        /// <param name="broadcast">
        ///     If true, a PropertyChangedMessage will
        ///     be broadcasted. If false, only the event will be raised.
        /// </param>
        /// <param name="propertyName">
        ///     (optional) The name of the property that
        ///     changed.
        /// </param>
        /// <returns>True if the PropertyChanged event was raised, false otherwise.</returns>
        protected bool Set<T>(
            ref T field,
            T newValue = default(T),
            bool broadcast = false,
            [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return false;
            }

            var oldValue = field;
            field = newValue;

            // ReSharper disable ExplicitCallerInfoArgument
            RaisePropertyChanged(propertyName, oldValue, field);
            // ReSharper restore ExplicitCallerInfoArgument

            return true;
        }

        #endregion Property Change
    }
}