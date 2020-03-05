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
        public List<Binding> Bindings { get; } = new List<Binding>();

        public object DataContext { get; private set; } = default!;

        protected TViewModel ViewModel => (TViewModel) DataContext;

        void IBindable.SetDataContext(object dataContext)
        {
            DataContext = dataContext;
        }

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

        protected virtual void DoAttachBindings()
        {
        }

        protected virtual void DoDetachBindings()
        {
            this.DetachBindings();
        }
    }
}
