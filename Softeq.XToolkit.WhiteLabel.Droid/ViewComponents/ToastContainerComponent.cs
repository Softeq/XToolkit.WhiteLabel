namespace Softeq.XToolkit.WhiteLabel.Droid.ViewComponents
{
    public class ToastContainerComponent : IViewComponent<ActivityBase>
    {
        public ToastContainerComponent(int containerId)
        {
            ContainerId = containerId;
        }

        public string Key => nameof(ToastContainerComponent);

        public int ContainerId { get; }

        public void Attach(ActivityBase item)
        {
        }

        public void Detach(ActivityBase item = null)
        {
        }
    }
}
