using System.ComponentModel;
using System.Linq;

namespace NetGore.World
{
    /// <summary>
    /// Interface for a <see cref="DynamicEntity"/> that can be used by another <see cref="DynamicEntity"/> (usually by a user or NPC
    /// character). When this interface is attached to a <see cref="DynamicEntity"/>, then it will be able to be used on the map
    /// just like any other entity that can be used.
    /// </summary>
    public interface IUsableEntity
    {
        /// <summary>
        /// Notifies the listeners when the IUsableEntity was used, and the DynamicEntity that used it. On the Client, this
        /// event will only be triggered if NotifyClientsOfUsage is true. The DynamicEntity argument
        /// that used this IUsableEntity may be null.
        /// </summary>
        event TypedEventHandler<Entity, EventArgs<DynamicEntity>> Used;

        /// <summary>
        /// Gets if the Client should be notified when this IUsableEntity is used. If true, when this IUsableEntity is
        /// used on the Server, every Client in the Map will be notified of the usage. As a result, Use() will be called
        /// on each Client. If false, this message will never be sent. Only set to true if any code is placed in
        /// Use() on the Client implementation of the IUsableEntity, or there are expected to be listeners to OnUse.
        /// </summary>
        [Browsable(false)]
        bool NotifyClientsOfUsage { get; }

        /// <summary>
        /// Client: 
        ///     Checks if the Client's character can attempt to use the IUsableEntity. If false, the Client
        ///     wont even attempt to use the IUsableEntity. If true, the Client will attempt to use it, but
        ///     it is not guarenteed the Server will also allow it to be used.
        /// Server:
        ///     Checks if the specified Entity may use the IUsableEntity.
        /// </summary>
        /// <param name="dynamicEntity">The DynamicEntity that is trying to use this IUsableEntity. For the Client,
        /// this will always be the User's Character. Can be null.</param>
        /// <returns>True if this IUsableEntity can be used by the <paramref name="dynamicEntity"/>, else false.</returns>
        bool CanUse(DynamicEntity dynamicEntity);

        /// <summary>
        /// Client:
        ///     Handles any additional usage stuff. When this is called, it is to be assumed that the Server has recognized
        ///     the IUsableEntity as having been successfully used.
        /// Server:
        ///     Attempts to use this IUsableEntity on the <paramref name="dynamicEntity"/>.
        /// </summary>
        /// <param name="dynamicEntity">DynamicEntity that is trying to use this IUsableEntity. Can be null.</param>
        /// <returns>True if this IUsableEntity was successfully used, else false. On the Client, this is generally
        /// unused.</returns>
        bool Use(DynamicEntity dynamicEntity);
    }
}