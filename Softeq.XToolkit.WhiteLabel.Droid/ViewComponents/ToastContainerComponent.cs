// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.WhiteLabel.Droid.ViewComponents
{
    public class ToastContainerComponent : IViewComponent<ActivityBase>
    {
        public ToastContainerComponent(int containerId)
        {
            ContainerId = containerId;
        }

        public int ContainerId { get; }

        public string Key => nameof(ToastContainerComponent);

        public void Attach(ActivityBase item)
        {
        }

        public void Detach(ActivityBase? item = null)
        {
        }
    }
}
