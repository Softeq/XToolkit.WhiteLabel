// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.Common.Helpers.Hashing
{
    /// <summary>
    /// Class helps to get a hash code for a number of objects combined
    /// </summary>
    public static class Hash
    {
        public static IHashCalculator Get() => new HashCalculator();
    }
}
