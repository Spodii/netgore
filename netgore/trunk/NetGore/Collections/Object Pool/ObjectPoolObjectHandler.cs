using System.Linq;

namespace NetGore.Collections
{
    /// <summary>
    /// Delegate for handling events on the objects in the pool.
    /// </summary>
    /// <param name="poolObject">The object the event is related to.</param>
    public delegate void ObjectPoolObjectHandler<in T>(T poolObject) where T : class, IPoolable;
}