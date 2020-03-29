// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.Common.Helpers.Hashing
{
    internal sealed class HashCalculator : IHashCalculator
    {
        private const int PrimeOne = 17;
        private const int PrimeTwo = 23;

        private int _hashCode;

        public HashCalculator()
        {
            _hashCode = PrimeOne;
        }

        public IHashCalculator Using<T>(T value)
        {
            unchecked
            {
                _hashCode = _hashCode * PrimeTwo + (value?.GetHashCode() ?? 0);
            }
            return this;
        }

        public int Calculate()
        {
            return _hashCode;
        }
    }
}
