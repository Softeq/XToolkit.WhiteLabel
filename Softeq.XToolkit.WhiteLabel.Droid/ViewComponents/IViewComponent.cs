namespace Softeq.XToolkit.WhiteLabel.Droid.ViewComponents
{
    public interface IViewComponent<T>
    {
        string Key { get; }

        void Attach(T item);
        void Detach(T item = default(T));
    }
}
