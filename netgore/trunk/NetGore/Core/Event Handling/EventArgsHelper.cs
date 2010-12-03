using System.Linq;

namespace NetGore
{
    /// <summary>
    /// Helper methods for <see cref="EventArgs{T}"/>.
    /// </summary>
    public static class EventArgsHelper
    {
        /// <summary>
        /// Creates an <see cref="EventArgs{T1}"/> instance.
        /// </summary>
        /// <typeparam name="T1">The type of the first component.</typeparam>
        /// <param name="item1">The value of the first component.</param>
        /// <returns>An <see cref="EventArgs{T1}"/> instance.</returns>
        public static EventArgs<T1> Create<T1>(T1 item1)
        {
            return new EventArgs<T1>(item1);
        }

        /// <summary>
        /// Creates an <see cref="EventArgs{T1, T2}"/> instance.
        /// </summary>
        /// <typeparam name="T1">The type of the first component.</typeparam>
        /// <typeparam name="T2">The type of the second component.</typeparam>
        /// <param name="item1">The value of the first component.</param>
        /// <param name="item2">The value of the second component.</param>
        /// <returns>
        /// An <see cref="EventArgs{T1, T2}"/> instance.
        /// </returns>
        public static EventArgs<T1, T2> Create<T1, T2>(T1 item1, T2 item2)
        {
            return new EventArgs<T1, T2>(item1, item2);
        }

        /// <summary>
        /// Creates an <see cref="EventArgs{T1, T2, T3}"/> instance.
        /// </summary>
        /// <typeparam name="T1">The type of the first component.</typeparam>
        /// <typeparam name="T2">The type of the second component.</typeparam>
        /// <typeparam name="T3">The type of the third component.</typeparam>
        /// <param name="item1">The value of the first component.</param>
        /// <param name="item2">The value of the second component.</param>
        /// <param name="item3">The value of the third component.</param>
        /// <returns>
        /// An <see cref="EventArgs{T1, T2, T3}"/> instance.
        /// </returns>
        public static EventArgs<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3)
        {
            return new EventArgs<T1, T2, T3>(item1, item2, item3);
        }

        /// <summary>
        /// Creates an <see cref="EventArgs{T1, T2, T3, T4}"/> instance.
        /// </summary>
        /// <typeparam name="T1">The type of the first component.</typeparam>
        /// <typeparam name="T2">The type of the second component.</typeparam>
        /// <typeparam name="T3">The type of the third component.</typeparam>
        /// <typeparam name="T4">The type of the fourth component.</typeparam>
        /// <param name="item1">The value of the first component.</param>
        /// <param name="item2">The value of the second component.</param>
        /// <param name="item3">The value of the third component.</param>
        /// <param name="item4">The value of the fourth component.</param>
        /// <returns>
        /// An <see cref="EventArgs{T1, T2, T3, T4}"/> instance.
        /// </returns>
        public static EventArgs<T1, T2, T3, T4> Create<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            return new EventArgs<T1, T2, T3, T4>(item1, item2, item3, item4);
        }

        /// <summary>
        /// Creates an <see cref="EventArgs{T1, T2, T3, T4, T5}"/> instance.
        /// </summary>
        /// <typeparam name="T1">The type of the first component.</typeparam>
        /// <typeparam name="T2">The type of the second component.</typeparam>
        /// <typeparam name="T3">The type of the third component.</typeparam>
        /// <typeparam name="T4">The type of the fourth component.</typeparam>
        /// <typeparam name="T5">The type of the fifth component.</typeparam>
        /// <param name="item1">The value of the first component.</param>
        /// <param name="item2">The value of the second component.</param>
        /// <param name="item3">The value of the third component.</param>
        /// <param name="item4">The value of the fourth component.</param>
        /// <param name="item5">The value of the fifth component.</param>
        /// <returns>
        /// An <see cref="EventArgs{T1, T2, T3, T4, T5}"/> instance.
        /// </returns>
        public static EventArgs<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
        {
            return new EventArgs<T1, T2, T3, T4, T5>(item1, item2, item3, item4, item5);
        }
    }
}