// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Common.Weak;
using Softeq.XToolkit.WhiteLabel.Forms.Navigation;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Xamarin.Forms;

namespace Softeq.XToolkit.WhiteLabel.Forms.Behaviors
{
    public class MasterPageBehavior : Behavior<MasterDetailPage>
    {
        private WeakReferenceEx<MasterDetailPage>? _parentRef;
        private IFormsViewLocator? _viewLocator;
        private ILogger? _logger;

        public static readonly BindableProperty SelectedDetailProperty = BindableProperty.Create(
            nameof(SelectedDetail),
            typeof(IViewModelBase),
            typeof(MasterPageBehavior));

        public IViewModelBase SelectedDetail
        {
            get => (ViewModelBase) GetValue(SelectedDetailProperty);
            set => SetValue(SelectedDetailProperty, value);
        }

        protected override void OnAttachedTo(MasterDetailPage bindable)
        {
            base.OnAttachedTo(bindable);

            _parentRef = WeakReferenceEx.Create(bindable);
            _viewLocator = Dependencies.Container.Resolve<IFormsViewLocator>();
            _logger = Dependencies.Container.Resolve<ILogManager>().GetLogger<MasterPageBehavior>();
        }

        protected override void OnPropertyChanged(string? propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(SelectedDetail) && _logger != null)
            {
                SetDetailPageAsync().FireAndForget(_logger);
            }
        }

        private async Task SetDetailPageAsync()
        {
            if (_viewLocator != null && _parentRef != null)
            {
                var view = await _viewLocator.GetPageAsync(SelectedDetail);

                _parentRef.Target.Detail = view;
                _parentRef.Target.IsPresented = false;
            }
        }
    }
}
