using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.Whitelabel.Tests
{
    public class ViewModelStub: ViewModelBase
    {
        public int IntParameter { get; set; }
        public string StringParameter { get; set; }
        public object ObjectParameter { get; set; }
    }
}