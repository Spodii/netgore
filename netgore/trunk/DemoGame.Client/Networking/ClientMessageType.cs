using System.Linq;

namespace DemoGame.Client
{
    /// <summary>
    /// Contains the different categories of messages that the client sends to the server.
    /// If you are not sure of which one to use, use <see cref="ClientMessageType.General"/>.
    /// </summary>
    /// <seealso cref="ClientMessageTypeExtensions"/>
    public enum ClientMessageType : byte
    {
        // NOTE: Whenever adding to this, make sure you also add to ClientMessageTypeExtensions.GetDeliveryMethod()!

        /// <summary>
        /// A general-purpose message.
        /// For organizational purposes, it is best to avoid this type when possible. Only (and always) use this
        /// when you are not sure of which type to use so you can easily find and change it later.
        /// </summary>
        General,

        /// <summary>
        /// Messages related to moving the user's character.
        /// </summary>
        CharacterMove,

        /// <summary>
        /// Messages related to interacting with the map (using entities, picking entities up, etc).
        /// </summary>
        CharacterInteract,

        /// <summary>
        /// Messages related to actions performed by the user's character such as attacking and using skills/spells.
        /// </summary>
        CharacterAction,

        /// <summary>
        /// Messages related to the usage of emoticons.
        /// </summary>
        CharacterEmote,

        /// <summary>
        /// Messages related to chat that was entered into the chat box.
        /// </summary>
        Chat,

        /// <summary>
        /// Messages related to the GUI in general.
        /// </summary>
        GUI,

        /// <summary>
        /// Messages related to the GUI aspect of items (using items, moving them, dropping them, etc).
        /// </summary>
        GUIItems,

        /// <summary>
        /// Messages related to requesting information about items from the server (usually so detailed descriptions can be displayed).
        /// </summary>
        GUIItemInfoRequest,

        /// <summary>
        /// Messages related to requesting information about the user's quest status from the server.
        /// </summary>
        GUIQuestStatusRequest,

        /// <summary>
        /// Messages related to the game on a lower level, such as login, account creation, character selection/creation, etc.
        /// </summary>
        System,
    }
}