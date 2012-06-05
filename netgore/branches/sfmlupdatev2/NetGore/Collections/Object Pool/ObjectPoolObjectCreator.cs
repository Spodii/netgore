using System.Linq;

namespace NetGore.Collections
{
    /// <summary>
    /// Delegate describing how to create an object for the object pool.
    /// </summary>
    /// <param name="objectPool">The object pool that created the object.</param>
    /// <returns>The object instance.</returns>
    public delegate T ObjectPoolObjectCreator<T>(ObjectPool<T> objectPool) where T : class, IPoolable;
}