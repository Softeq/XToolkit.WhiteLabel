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

            var raisedEvent = Assert.Raises<PropertyChangedEventArgs>(
                    handler => vm.PropertyChanged += (s, e) => handler.Invoke(s, e),
                    handler => { },
                    () => vm.IsBusy = changedIsBusy);

            Assert.NotNull(raisedEvent);
            Assert.Equal(nameof(vm.IsBusy), raisedEvent.Arguments.PropertyName);
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

            Assert.Throws<RaisesException>(() => Assert.Raises<PropertyChangedEventArgs>(
                handler => vm.PropertyChanged += (s, e) => handler.Invoke(s, e),
                handler => { },
                () => vm.IsBusy = initialIsBusy));
        }
    }
}
