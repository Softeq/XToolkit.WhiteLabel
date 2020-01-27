// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.Common.Helpers
{
    public static class HashHelper
    {
        private const int PrimeOne = 17;
        private const int PrimeTwo = 23;

        /// <summary>
        ///     Get hashcode from 10 objects.
        /// </summary>
        /// <returns>The hash code.</returns>
        /// <param name="arg1">Arg1.</param>
        /// <param name="arg2">Arg2.</param>
        /// <param name="arg3">Arg3.</param>
        /// <param name="arg4">Arg4.</param>
        /// <param name="arg5">Arg5.</param>
        /// <param name="arg6">Arg6.</param>
        /// <param name="arg7">Arg7.</param>
        /// <param name="arg8">Arg8.</param>
        /// <param name="arg9">Arg9.</param>
        /// <param name="arg10">Arg10.</param>
        /// <typeparam name="T1">The 1st type parameter.</typeparam>
        /// <typeparam name="T2">The 2nd type parameter.</typeparam>
        /// <typeparam name="T3">The 3rd type parameter.</typeparam>
        /// <typeparam name="T4">The 4th type parameter.</typeparam>
        /// <typeparam name="T5">The 5th type parameter.</typeparam>
        /// <typeparam name="T6">The 6th type parameter.</typeparam>
        /// <typeparam name="T7">The 7th type parameter.</typeparam>
        /// <typeparam name="T8">The 8th type parameter.</typeparam>
        /// <typeparam name="T9">The 9th type parameter.</typeparam>
        /// <typeparam name="T10">The 10th type parameter.</typeparam>
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

        /// <summary>
        ///     Get hashcode from 9 objects.
        /// </summary>
        /// <returns>The hash code.</returns>
        /// <param name="arg1">Arg1.</param>
        /// <param name="arg2">Arg2.</param>
        /// <param name="arg3">Arg3.</param>
        /// <param name="arg4">Arg4.</param>
        /// <param name="arg5">Arg5.</param>
        /// <param name="arg6">Arg6.</param>
        /// <param name="arg7">Arg7.</param>
        /// <param name="arg8">Arg8.</param>
        /// <param name="arg9">Arg9.</param>
        /// <typeparam name="T1">The 1st type parameter.</typeparam>
        /// <typeparam name="T2">The 2nd type parameter.</typeparam>
        /// <typeparam name="T3">The 3rd type parameter.</typeparam>
        /// <typeparam name="T4">The 4th type parameter.</typeparam>
        /// <typeparam name="T5">The 5th type parameter.</typeparam>
        /// <typeparam name="T6">The 6th type parameter.</typeparam>
        /// <typeparam name="T7">The 7th type parameter.</typeparam>
        /// <typeparam name="T8">The 8th type parameter.</typeparam>
        /// <typeparam name="T9">The 9th type parameter.</typeparam>
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

        /// <summary>
        ///     Get hashcode from 8 objects.
        /// </summary>
        /// <returns>The hash code.</returns>
        /// <param name="arg1">Arg1.</param>
        /// <param name="arg2">Arg2.</param>
        /// <param name="arg3">Arg3.</param>
        /// <param name="arg4">Arg4.</param>
        /// <param name="arg5">Arg5.</param>
        /// <param name="arg6">Arg6.</param>
        /// <param name="arg7">Arg7.</param>
        /// <param name="arg8">Arg8.</param>
        /// <typeparam name="T1">The 1st type parameter.</typeparam>
        /// <typeparam name="T2">The 2nd type parameter.</typeparam>
        /// <typeparam name="T3">The 3rd type parameter.</typeparam>
        /// <typeparam name="T4">The 4th type parameter.</typeparam>
        /// <typeparam name="T5">The 5th type parameter.</typeparam>
        /// <typeparam name="T6">The 6th type parameter.</typeparam>
        /// <typeparam name="T7">The 7th type parameter.</typeparam>
        /// <typeparam name="T8">The 8th type parameter.</typeparam>
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

        /// <summary>
        ///     Get hashcode from 7 objects.
        /// </summary>
        /// <returns>The hash code.</returns>
        /// <param name="arg1">Arg1.</param>
        /// <param name="arg2">Arg2.</param>
        /// <param name="arg3">Arg3.</param>
        /// <param name="arg4">Arg4.</param>
        /// <param name="arg5">Arg5.</param>
        /// <param name="arg6">Arg6.</param>
        /// <param name="arg7">Arg7.</param>
        /// <typeparam name="T1">The 1st type parameter.</typeparam>
        /// <typeparam name="T2">The 2nd type parameter.</typeparam>
        /// <typeparam name="T3">The 3rd type parameter.</typeparam>
        /// <typeparam name="T4">The 4th type parameter.</typeparam>
        /// <typeparam name="T5">The 5th type parameter.</typeparam>
        /// <typeparam name="T6">The 6th type parameter.</typeparam>
        /// <typeparam name="T7">The 7th type parameter.</typeparam>
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

        /// <summary>
        ///     Get hashcode from 6 objects.
        /// </summary>
        /// <returns>The hash code.</returns>
        /// <param name="arg1">Arg1.</param>
        /// <param name="arg2">Arg2.</param>
        /// <param name="arg3">Arg3.</param>
        /// <param name="arg4">Arg4.</param>
        /// <param name="arg5">Arg5.</param>
        /// <param name="arg6">Arg6.</param>
        /// <typeparam name="T1">The 1st type parameter.</typeparam>
        /// <typeparam name="T2">The 2nd type parameter.</typeparam>
        /// <typeparam name="T3">The 3rd type parameter.</typeparam>
        /// <typeparam name="T4">The 4th type parameter.</typeparam>
        /// <typeparam name="T5">The 5th type parameter.</typeparam>
        /// <typeparam name="T6">The 6th type parameter.</typeparam>
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

        /// <summary>
        ///     Get hashcode from 5 objects.
        /// </summary>
        /// <returns>The hash code.</returns>
        /// <param name="arg1">Arg1.</param>
        /// <param name="arg2">Arg2.</param>
        /// <param name="arg3">Arg3.</param>
        /// <param name="arg4">Arg4.</param>
        /// <param name="arg5">Arg5.</param>
        /// <typeparam name="T1">The 1st type parameter.</typeparam>
        /// <typeparam name="T2">The 2nd type parameter.</typeparam>
        /// <typeparam name="T3">The 3rd type parameter.</typeparam>
        /// <typeparam name="T4">The 4th type parameter.</typeparam>
        /// <typeparam name="T5">The 5th type parameter.</typeparam>
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

        /// <summary>
        ///     Get hashcode from 4 objects.
        /// </summary>
        /// <returns>The hash code.</returns>
        /// <param name="arg1">Arg1.</param>
        /// <param name="arg2">Arg2.</param>
        /// <param name="arg3">Arg3.</param>
        /// <param name="arg4">Arg4.</param>
        /// <typeparam name="T1">The 1st type parameter.</typeparam>
        /// <typeparam name="T2">The 2nd type parameter.</typeparam>
        /// <typeparam name="T3">The 3rd type parameter.</typeparam>
        /// <typeparam name="T4">The 4th type parameter.</typeparam>
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

        /// <summary>
        ///     Get hashcode from 3 objects.
        /// </summary>
        /// <returns>The hash code.</returns>
        /// <param name="arg1">Arg1.</param>
        /// <param name="arg2">Arg2.</param>
        /// <param name="arg3">Arg3.</param>
        /// <typeparam name="T1">The 1st type parameter.</typeparam>
        /// <typeparam name="T2">The 2nd type parameter.</typeparam>
        /// <typeparam name="T3">The 3rd type parameter.</typeparam>
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

        /// <summary>
        ///     Get hashcode from 2 objects.
        /// </summary>
        /// <returns>The hash code.</returns>
        /// <param name="arg1">Arg1.</param>
        /// <param name="arg2">Arg2.</param>
        /// <typeparam name="T1">The 1st type parameter.</typeparam>
        /// <typeparam name="T2">The 2nd type parameter.</typeparam>
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
