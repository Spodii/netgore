using System.Linq;
using NetGore;
using NetGore.World;

namespace DemoGame.Server
{
    /// <summary>
    /// Contains the different categories of messages that the server sends to the client.
    /// If you are not sure of which one to use, use <see cref="ServerMessageType.General"/>.
    /// </summary>
    /// <seealso cref="ServerMessageTypeExtensions"/>
    public enum ServerMessageType : byte
    {
        // NOTE: Whenever adding to this, make sure you also add to ServerMessageTypeExtensions.GetDeliveryMethod()!

        /// <summary>
        /// A general-purpose message.
        /// For organizational purposes, it is best to avoid this type when possible. Only (and always) use this
        /// when you are not sure of which type to use so you can easily find and change it later.
        /// </summary>
        General,

        /// <summary>
        /// Messages related to updating the position and velocity of a <see cref="DynamicEntity"/>. These messages require the lowest
        /// possible latency to keep things smooth.
        /// </summary>
        MapDynamicEntitySpatialUpdate,

        /// <summary>
        /// Messages related to the non-spatial properties of a <see cref="DynamicEntity"/>. This includes any property using a
        /// <see cref="SyncValueAttribute"/>.
        /// </summary>
        MapDynamicEntityProperty,

        /// <summary>
        /// Messages related to a character's health, mana, stamina, and other status points, that are seen by everyone.
        /// </summary>
        MapCharacterSP,

        /// <summary>
        /// Messages related to effects on map that are independent of other states and other effects. This includes
        /// sounds and visuals for attacks, spells, emoticons, etc. For effects that are dependent on the order that
        /// events take place, use <see cref="MapEffectDependent"/> instead.
        /// </summary>
        /// <seealso cref="MapEffectDependent"/>
        MapEffect,

        /// <summary>
        /// Same as <see cref="MapEffect"/>, but for effects that require sequencing since they depend on
        /// previous or future states. Since such effects are uncommon, <see cref="MapEffect"/> is usually used instead.
        /// </summary>
        /// <seealso cref="MapEffect"/>
        MapEffectDependent,

        /// <summary>
        /// General messages related to the map.
        /// Specialized Map messages are available, and should be used instead where applicable.
        /// </summary>
        Map,

        /// <summary>
        /// Messages related to chatting.
        /// </summary>
        GUIChat,

        /// <summary>
        /// General messages related to the GUI for a client. These are almost always focused purely at a single client. Includes
        /// anything from messages about acquiring items/money, returning the results of a console command, updating the
        /// listing of guild members, etc.
        /// Specialized GUI messages are available, and should be used instead where applicable.
        /// </summary>
        GUI,

        /// <summary>
        /// Messages related to a user's items, such as updating the inventory or equipped items.
        /// </summary>
        GUIItems,

        /// <summary>
        /// Messages related to requests for details on the information of items.
        /// </summary>
        GUIItemInfo,

        /// <summary>
        /// Messages related to updating a user's stats.
        /// </summary>
        GUIUserStats,

        /// <summary>
        /// Messages related to updating a user's status (such as status effects). This only concerns with updates for the user's status
        /// in respect to the GUI, and does not include status updates for the character entity itself that are visible to others.
        /// </summary>
        GUIUserStatus,

        /// <summary>
        /// Messages used to manage the game on a lower, more general level. This includes the login process, account creation,
        /// changing maps, errors, connection/disconnection, etc.
        /// </summary>
        System,
    }
}