// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.Whitelabel.Tests.Bootstrapper.DefaultViewModelFinderTests
{
    interface IDroidViewStub {}

    // simulate ios types
    class ViewControllerStub {}
    class ViewControllerStub<T> : ViewControllerStub where T : ViewModelBase {}

    // simulate android types
    class ActivityStub : IDroidViewStub  {}
    class ActivityStub<T> : ActivityStub where T : ViewModelBase {}

    // simulate VM types
    class ViewModelStub1 : ViewModelBase {}
    class ViewModelStub2 : ViewModelBase {}

    // simulate simple custom views types
    class CustomViewControllerStub : ViewControllerStub {}
    class CustomActivityStub : ActivityStub {}
    class CustomViewStub :  IDroidViewStub {}

    // simulate simple custom views types with VM
    class VmActivityStub : ActivityStub<ViewModelStub1> {}
    class VmViewControllerStub : ViewControllerStub<ViewModelStub2> {}
}
