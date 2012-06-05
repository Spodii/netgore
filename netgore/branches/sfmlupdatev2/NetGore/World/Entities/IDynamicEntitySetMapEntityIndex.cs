using System.Linq;

namespace NetGore.World
{
    /// <summary>
    /// Interface for a <see cref="DynamicEntity"/> to hide the ability to set the <see cref="MapEntityIndex"/>.
    /// </summary>
    public interface IDynamicEntitySetMapEntityIndex
    {
        /// <summary>
        /// Sets the <see cref="MapEntityIndex"/> for this <see cref="DynamicEntity"/>. This should only ever be done by
        /// the <see cref="IMap"/> that contains this <see cref="DynamicEntity"/>.
        /// </summary>
        /// <param name="value">The new value.</param>
        void SetMapEntityIndex(MapEntityIndex value);
    }
}