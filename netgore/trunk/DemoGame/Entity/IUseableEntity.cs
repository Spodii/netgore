using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NetGore;

namespace DemoGame
{
    /// <summary>
    /// Interface for a DynamicEntity that can be used by a DynamicEntity.
    /// </summary>
    public interface IUseableEntity
    {
        /// <summary>
        /// Gets if the Client should be notified when this IUseableEntity is used. If true, when this IUsableEntity is
        /// used on the Server, every Client in the Map will be notified of the usage. As a result, Use() will be called
        /// on each Client. If false, this message will never be sent. Only set to true if any code is placed in
        /// Use() on the Client implementation of the IUseableEntity, or there are expected to be listeners to OnUse.
        /// </summary>
        bool NotifyClientsOfUsage { get; }

        /// <summary>
        /// Notifies the listeners when the IUseableEntity was used, and the DynamicEntity that used it. On the Client, this
        /// event will only be triggered if NotifyClientsOfUsage is true. The DynamicEntity argument
        /// that used this IUsableEntity may be null.
        /// </summary>
        event EntityEventHandler<DynamicEntity> OnUse;

        /// <summary>
        /// Client: 
        ///     Checks if the Client's character can attempt to use the IUseableEntity. If false, the Client
        ///     wont even attempt to use the IUseableEntity. If true, the Client will attempt to use it, but
        ///     it is not guarenteed the Server will also allow it to be used.
        /// Server:
        ///     Checks if the specified Entity may use the IUseableEntity.
        /// </summary>
        /// <param name="charEntity">The CharacterEntity that is trying to use this IUsableEntity. For the Client,
        /// this will always be the User's Character. Can be null.</param>
        /// <returns>True if this IUsableEntity can be used by the <paramref name="charEntity"/>, else false.</returns>
        bool CanUse(DynamicEntity charEntity);

        /// <summary>
        /// Client:
        ///     Handles any additional usage stuff. When this is called, it is to be assumed that the Server has recognized
        ///     the IUseableEntity as having been successfully used.
        /// Server:
        ///     Attempts to use this IUsableEntity on the <paramref name="charEntity"/>.
        /// </summary>
        /// <param name="charEntity">CharacterEntity that is trying to use this IUseableEntity. Can be null.</param>
        /// <returns>True if this IUseableEntity was successfully used, else false. On the Client, this is generally
        /// unused.</returns>
        bool Use(DynamicEntity charEntity);
    }
}