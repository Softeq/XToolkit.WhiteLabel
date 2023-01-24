// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Playground.ViewModels.RIB.Platform
{
    public interface IInteractorScope
    {
        bool IsActive { get; }

        IObservable<bool> IsActiveStream { get; }
    }

    public interface IInteractable : IInteractorScope
    {
        void Activate();

        void Deactivate();
    }

    public class Interactor : IInteractable
    {
        private readonly BehaviorSubject<bool> _isActiveSubject = new BehaviorSubject<bool>(false);
        private CompositeDisposable? _activenessDisposable;

        public bool IsActive
        {
            get
            {
                try
                {
                    return _isActiveSubject.Value;
                }
                catch
                {
                    return false;
                }
            }
        }

        public IObservable<bool> IsActiveStream => _isActiveSubject.AsObservable().DistinctUntilChanged();

        public void Activate()
        {
            if (IsActive)
            {
                return;
            }

            _activenessDisposable = new CompositeDisposable();

            _isActiveSubject.OnNext(true);

            DidBecomeActive();
        }

        public void Deactivate()
        {
            if (!IsActive)
            {
                return;
            }

            WillResignActive();

            _activenessDisposable?.Dispose();
            _activenessDisposable = null;

            _isActiveSubject.OnNext(false);
        }

        protected virtual void DidBecomeActive()
        {
        }

        protected virtual void WillResignActive()
        {
        }
    }
}
