using System.Linq;

namespace NetGore
{
    public static class IMapExtensions
    {
        /// <summary>
        /// Gets the <see cref="IEntitySpatial"/> for the given type of <see cref="Entity"/>.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="Entity"/>.</typeparam>
        /// <returns>The <see cref="IEntitySpatial"/> that contains type <typeparamref name="T"/>.</returns>
        public static IEntitySpatial GetSpatial<T>(this IMap map) where T : Entity
        {
            return map.GetSpatial(typeof(T));
        }
    }
}