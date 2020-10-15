// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Tests.Stubs
{
    internal interface IDroidViewStub { }

    // simulate ios types
    internal class ViewControllerStub { }
    internal class ViewControllerStub<T> : ViewControllerStub where T : ViewModelBase { }

    // simulate android types
    internal class ActivityStub : IDroidViewStub { }
    internal class ActivityStub<T> : ActivityStub where T : ViewModelBase { }

    // simulate VM types
    internal class ViewModelStub1 : ViewModelBase { }
    internal class ViewModelStub2 : ViewModelBase { }

    // simulate simple custom views types
    internal class CustomViewControllerStub : ViewControllerStub { }
    internal class CustomActivityStub : ActivityStub { }
    internal class CustomViewStub : IDroidViewStub { }

    // simulate simple custom views types with VM
    internal class VmActivityStub : ActivityStub<ViewModelStub1> { }
    internal class VmViewControllerStub : ViewControllerStub<ViewModelStub2> { }
}
