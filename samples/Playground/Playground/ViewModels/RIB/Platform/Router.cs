// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Playground.ViewModels.RIB.Platform
{
    public enum RouterLifecycle
    {
        DidLoad,
    }

    public interface IRouterScope
    {
        IObservable<RouterLifecycle> Lifecycle { get; }
    }

    public interface IRouting : IRouterScope
    {
        IInteractable Interactable { get; }

        IEnumerable<IRouting> Children { get; }

        void Load();

        void AttachChild(IRouting child);

        void DetachChild(IRouting child);
    }

    public class Router<TInteractorType> : IRouting
        where TInteractorType : IInteractable
    {
        private bool _didLoadFlag = false;
        private readonly List<IRouting> _children = new List<IRouting>();
        private readonly Subject<RouterLifecycle> _lifecycleSubject = new Subject<RouterLifecycle>();

        private readonly CompositeDisposable _deinitDisposable = new CompositeDisposable();

        protected void Init(TInteractorType interactor)
        {
            Interactable = interactor;
        }

        public IInteractable Interactable { get; private set; }

        public IEnumerable<IRouting> Children => _children.ToArray();

        public IObservable<RouterLifecycle> Lifecycle => _lifecycleSubject.AsObservable();

        public void AttachChild(IRouting child)
        {
            if (_children.Contains(child))
            {
                throw new ArgumentException("Attempt to attach child, which already attached");
            }

            _children.Add(child);
            child.Interactable.Activate();
            child.Load();
        }

        public void DetachChild(IRouting child)
        {
            child.Interactable.Deactivate();

            _children.Remove(child);
        }

        public void Load()
        {
            if (_didLoadFlag)
            {
                return;
            }

            _didLoadFlag = true;
            InternalDidLoad();
            DidLoad();
        }

        protected virtual void DidLoad()
        {
        }

        private void InternalDidLoad()
        {
            BindSubtreeActiveState();
            _lifecycleSubject.OnNext(RouterLifecycle.DidLoad);
        }

        private void BindSubtreeActiveState()
        {
            var disposable = Interactable.IsActiveStream
                .Subscribe(onNext: (isActive) => this.SetSubtreeActive(isActive));

            _deinitDisposable.Append(disposable);
        }

        private void SetSubtreeActive(bool isActive)
        {
            if (isActive)
            {
                IterateSubtree(this, (IRouting router) =>
                {
                    if (!router.Interactable.IsActive)
                    {
                        router.Interactable.Activate();
                    }
                });
            }
            else
            {
                IterateSubtree(this, (IRouting router) =>
                {
                    if (!router.Interactable.IsActive)
                    {
                        router.Interactable.Deactivate();
                    }
                });
            }
        }

        private void IterateSubtree(IRouting root, Action<IRouting> closure)
        {
            closure(root);

            foreach (var child in root.Children)
            {
                IterateSubtree(child, closure);
            }
        }

        private void DetachAllChildren()
        {
            foreach (var child in Children)
            {
                DetachChild(child);
            }
        }

        private void Dispose()
        {
            Interactable.Deactivate();

            if (Children.Any())
            {
                DetachAllChildren();
            }

            _lifecycleSubject.OnCompleted();

            _deinitDisposable.Dispose();
        }
    }
}
