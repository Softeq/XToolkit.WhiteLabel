// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common.Weak;
using Softeq.XToolkit.WhiteLabel.Forms.Navigation;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Xamarin.Forms;

namespace Softeq.XToolkit.WhiteLabel.Forms.Behaviors
{
    public class MasterPageBehavior : Behavior<MasterDetailPage>
    {
        public static readonly BindableProperty SelectedDetailProperty = BindableProperty.Create(
            nameof(SelectedDetail),
            typeof(ViewModelBase),
            typeof(MasterPageBehavior));

        private WeakReferenceEx<MasterDetailPage>? _parentRef;
        private IFormsViewLocator? _viewLocator;

        public ViewModelBase SelectedDetail
        {
            get => (ViewModelBase) GetValue(SelectedDetailProperty);
            set => SetValue(SelectedDetailProperty, value);
        }

        protected override void OnAttachedTo(MasterDetailPage bindable)
        {
            base.OnAttachedTo(bindable);

            _parentRef = WeakReferenceEx.Create(bindable);
            _viewLocator = Dependencies.Container.Resolve<IFormsViewLocator>();
        }

        protected override void OnPropertyChanged(string? propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(SelectedDetail) && _viewLocator != null && _parentRef != null)
            {
                var view = _viewLocator.GetPage(SelectedDetail);
                _parentRef.Target.Detail = view;
                _parentRef.Target.IsPresented = false;
            }
        }
    }
}
