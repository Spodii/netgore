using System.Linq;
using NetGore;

namespace DemoGame.Client
{
    [MapFileEntity]
    public class TeleportEntity : TeleportEntityBase
    {
        /// <summary>
        /// Notifies the listeners when the IUsableEntity was used, and the DynamicEntity that used it. On the Client, this
        /// event will only be triggered if NotifyClientsOfUsage is true. The DynamicEntity argument
        /// that used this IUsableEntity may be null.
        /// </summary>
        public override event EntityEventHandler<DynamicEntity> OnUse;

        /// <summary>
        /// Client:
        ///     Handles any additional usage stuff. When this is called, it is to be assumed that the Server has recognized
        ///     the IUsableEntity as having been successfully used.
        /// Server:
        ///     Attempts to use this IUsableEntity on the <paramref name="charEntity"/>.
        /// </summary>
        /// <param name="charEntity">CharacterEntity that is trying to use this IUsableEntity. Can be null.</param>
        /// <returns>True if this IUsableEntity was successfully used, else false. On the Client, this is generally
        /// unused.</returns>
        public override bool Use(DynamicEntity charEntity)
        {
            if (OnUse != null)
                OnUse(this, charEntity);

            return true;
        }
    }
}