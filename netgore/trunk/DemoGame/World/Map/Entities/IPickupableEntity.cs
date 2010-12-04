using System.Linq;
using NetGore;
using NetGore.World;

namespace DemoGame
{
    /// <summary>
    /// Interface for an <see cref="Entity"/> that can be picked up by a <see cref="CharacterEntity"/>
    /// </summary>
    public interface IPickupableEntity
    {
        /// <summary>
        /// Notifies listeners that this <see cref="Entity"/> was picked up
        /// </summary>
        event TypedEventHandler<Entity, EventArgs<CharacterEntity>> PickedUp;

        /// <summary>
        /// Checks if this <see cref="Entity"/> can be picked up by the specified <paramref name="charEntity"/>, but does
        /// not actually pick up this <see cref="Entity"/>
        /// </summary>
        /// <param name="charEntity"><see cref="CharacterEntity"/> that is trying to use this <see cref="Entity"/></param>
        /// <returns>True if this <see cref="Entity"/> can be picked up, else false</returns>
        bool CanPickup(CharacterEntity charEntity);

        /// <summary>
        /// Picks up this <see cref="Entity"/>
        /// </summary>
        /// <param name="charEntity"><see cref="CharacterEntity"/> that is trying to pick up this <see cref="Entity"/></param>
        /// <returns>True if this <see cref="Entity"/> was successfully picked up, else false</returns>
        bool Pickup(CharacterEntity charEntity);
    }
}