// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.ComponentModel;
using Xunit;
using Xunit.Sdk;

namespace Softeq.XToolkit.WhiteLabel.Tests.Mvvm.ViewModelBaseTests
{
    public class ViewModelBaseTests
    {
        [Fact]
        public void ViewModelBase_IsObservable()
        {
            var vm = new ViewModelStub();

            Assert.IsAssignableFrom<INotifyPropertyChanged>(vm);
        }

        [Fact]
        public void OnInitialized_ChangesCorrespondingProperty()
        {
            var vm = new ViewModelStub();

            Assert.False(vm.IsInitialized);

            vm.OnInitialize();

            Assert.True(vm.IsInitialized);
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void ViewModelBase_NotifiesWhenIsBusyChanges(bool initialIsBusy, bool changedIsBusy)
        {
            var vm = new ViewModelStub()
            {
                IsBusy = initialIsBusy
            };

            Assert.PropertyChanged(vm, nameof(vm.IsBusy), () => vm.IsBusy = changedIsBusy);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ViewModelBase_DoesNotNotifyWhenIsBusyNotChanges(bool initialIsBusy)
        {
            var vm = new ViewModelStub()
            {
                IsBusy = initialIsBusy
            };

            Assert.Throws<PropertyChangedException>(() =>
                Assert.PropertyChanged(vm, nameof(vm.IsBusy), () => vm.IsBusy = initialIsBusy));
        }
    }
}
