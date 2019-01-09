namespace Softeq.XToolkit.WhiteLabel.Interfaces
{
    public interface IValueConverter<T1, T2>
    {
        T2 Convert(T1 from);
        T1 ConvertBack(T2 from);
    }
}