// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Navigation
{
    public class RootFrameNavigationControllerBase<TViewModel>
        : UINavigationController, IBindable
        where TViewModel : RootFrameNavigationViewModelBase
    {
        protected RootFrameNavigationControllerBase()
        {
            Bindings = new List<Binding>();
        }

        public List<Binding> Bindings { get; }

        public object DataContext { get; set; }

        protected TViewModel ViewModel => (TViewModel) DataContext;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            if (ViewModel.IsInitialized)
            {
                return;
            }

            ViewModel.InitializeNavigation(this);
            ViewModel.NavigateToFirstPage();
            ViewModel.OnInitialize();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            ViewModel.OnAppearing();
            DoAttachBindings();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            DoDetachBindings();
            ViewModel.OnDisappearing();
        }

        /// <inheritdoc />
        public virtual void DoAttachBindings()
        {
        }

        /// <inheritdoc />
        public virtual void DoDetachBindings()
        {
            this.DetachBindings();
        }
    }
}
