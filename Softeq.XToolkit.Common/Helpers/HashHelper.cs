// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.Common.Helpers
{
    public static class HashHelper
    {
        private const int PrimeOne = 17;
        private const int PrimeTwo = 23;

        public static int GetHashCode<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T1 arg1, T2 arg2, T3 arg3, T4 arg4,
            T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            unchecked
            {
                var hash = PrimeOne;
                hash = hash * PrimeTwo + arg1.GetHashCode();
                hash = hash * PrimeTwo + arg2.GetHashCode();
                hash = hash * PrimeTwo + arg3.GetHashCode();
                hash = hash * PrimeTwo + arg4.GetHashCode();
                hash = hash * PrimeTwo + arg5.GetHashCode();
                hash = hash * PrimeTwo + arg6.GetHashCode();
                hash = hash * PrimeTwo + arg7.GetHashCode();
                hash = hash * PrimeTwo + arg8.GetHashCode();
                hash = hash * PrimeTwo + arg9.GetHashCode();
                hash = hash * PrimeTwo + arg10.GetHashCode();

                return hash;
            }
        }

        public static int GetHashCode<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5,
            T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            unchecked
            {
                var hash = PrimeOne;
                hash = hash * PrimeTwo + arg1.GetHashCode();
                hash = hash * PrimeTwo + arg2.GetHashCode();
                hash = hash * PrimeTwo + arg3.GetHashCode();
                hash = hash * PrimeTwo + arg4.GetHashCode();
                hash = hash * PrimeTwo + arg5.GetHashCode();
                hash = hash * PrimeTwo + arg6.GetHashCode();
                hash = hash * PrimeTwo + arg7.GetHashCode();
                hash = hash * PrimeTwo + arg8.GetHashCode();
                hash = hash * PrimeTwo + arg9.GetHashCode();

                return hash;
            }
        }

        public static int GetHashCode<T1, T2, T3, T4, T5, T6, T7, T8>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5,
            T6 arg6, T7 arg7, T8 arg8)
        {
            unchecked
            {
                var hash = PrimeOne;
                hash = hash * PrimeTwo + arg1.GetHashCode();
                hash = hash * PrimeTwo + arg2.GetHashCode();
                hash = hash * PrimeTwo + arg3.GetHashCode();
                hash = hash * PrimeTwo + arg4.GetHashCode();
                hash = hash * PrimeTwo + arg5.GetHashCode();
                hash = hash * PrimeTwo + arg6.GetHashCode();
                hash = hash * PrimeTwo + arg7.GetHashCode();
                hash = hash * PrimeTwo + arg8.GetHashCode();

                return hash;
            }
        }

        public static int GetHashCode<T1, T2, T3, T4, T5, T6, T7>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6,
            T7 arg7)
        {
            unchecked
            {
                var hash = PrimeOne;
                hash = hash * PrimeTwo + arg1.GetHashCode();
                hash = hash * PrimeTwo + arg2.GetHashCode();
                hash = hash * PrimeTwo + arg3.GetHashCode();
                hash = hash * PrimeTwo + arg4.GetHashCode();
                hash = hash * PrimeTwo + arg5.GetHashCode();
                hash = hash * PrimeTwo + arg6.GetHashCode();
                hash = hash * PrimeTwo + arg7.GetHashCode();

                return hash;
            }
        }

        public static int GetHashCode<T1, T2, T3, T4, T5, T6>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            unchecked
            {
                var hash = PrimeOne;
                hash = hash * PrimeTwo + arg1.GetHashCode();
                hash = hash * PrimeTwo + arg2.GetHashCode();
                hash = hash * PrimeTwo + arg3.GetHashCode();
                hash = hash * PrimeTwo + arg4.GetHashCode();
                hash = hash * PrimeTwo + arg5.GetHashCode();
                hash = hash * PrimeTwo + arg6.GetHashCode();

                return hash;
            }
        }

        public static int GetHashCode<T1, T2, T3, T4, T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            unchecked
            {
                var hash = PrimeOne;
                hash = hash * PrimeTwo + arg1.GetHashCode();
                hash = hash * PrimeTwo + arg2.GetHashCode();
                hash = hash * PrimeTwo + arg3.GetHashCode();
                hash = hash * PrimeTwo + arg4.GetHashCode();
                hash = hash * PrimeTwo + arg5.GetHashCode();

                return hash;
            }
        }

        public static int GetHashCode<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            unchecked
            {
                var hash = PrimeOne;
                hash = hash * PrimeTwo + arg1.GetHashCode();
                hash = hash * PrimeTwo + arg2.GetHashCode();
                hash = hash * PrimeTwo + arg3.GetHashCode();
                hash = hash * PrimeTwo + arg4.GetHashCode();

                return hash;
            }
        }

        public static int GetHashCode<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3)
        {
            unchecked
            {
                var hash = PrimeOne;
                hash = hash * PrimeTwo + arg1.GetHashCode();
                hash = hash * PrimeTwo + arg2.GetHashCode();
                hash = hash * PrimeTwo + arg3.GetHashCode();

                return hash;
            }
        }

        public static int GetHashCode<T1, T2>(T1 arg1, T2 arg2)
        {
            unchecked
            {
                var hash = PrimeOne;
                hash = hash * PrimeTwo + arg1.GetHashCode();
                hash = hash * PrimeTwo + arg2.GetHashCode();

                return hash;
            }
        }
    }
}