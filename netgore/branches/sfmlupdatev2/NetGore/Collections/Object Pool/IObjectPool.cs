using System;
using System.Linq;

namespace NetGore.Collections
{
    /// <summary>
    /// Interface for an object pool.
    /// </summary>
    /// <typeparam name="T">The type of object to be pooled.</typeparam>
    public interface IObjectPool<T> where T : class, IPoolable
    {
        /// <summary>
        /// Gets the number of live objects in the pool.
        /// </summary>
        int LiveObjects { get; }

        /// <summary>
        /// Gets a free object instance from the pool.
        /// </summary>
        /// <returns>A free object instance from the pool.</returns>
        T Acquire();

        /// <summary>
        /// Frees all live objects in the pool.
        /// </summary>
        void Clear();

        /// <summary>
        /// Frees the object so the pool can reuse it. After freeing an object, it should not be used
        /// in any way, and be treated like it has been disposed. No exceptions will be thrown for trying to free
        /// an object that does not belong to this pool.
        /// </summary>
        /// <param name="poolObject">The object to be freed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="poolObject"/> is null.</exception>
        void Free(T poolObject);

        /// <summary>
        /// Frees the object so the pool can reuse it. After freeing an object, it should not be used
        /// in any way, and be treated like it has been disposed.
        /// </summary>
        /// <param name="poolObject">The object to be freed.</param>
        /// <param name="throwArgumentException">Whether or not an <see cref="ArgumentException"/> will be thrown for
        /// objects that do not belong to this pool.</param>
        /// <exception cref="ArgumentException"><paramref name="throwArgumentException"/> is tru and the 
        /// <paramref name="poolObject"/> does not belong to this pool.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="poolObject"/> is null.</exception>
        void Free(T poolObject, bool throwArgumentException);

        /// <summary>
        /// Frees all live objects in the pool that match the given <paramref name="condition"/>.
        /// </summary>
        /// <param name="condition">The condition used to determine if an object should be freed.</param>
        /// <returns>The number of objects that were freed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="condition"/> is null.</exception>
        int FreeAll(Func<T, bool> condition);

        /// <summary>
        /// Performs the <paramref name="action"/> on all live objects in the object pool.
        /// </summary>
        /// <param name="action">The action to perform on all live objects in the object pool.</param>
        void Perform(Action<T> action);
    }
}