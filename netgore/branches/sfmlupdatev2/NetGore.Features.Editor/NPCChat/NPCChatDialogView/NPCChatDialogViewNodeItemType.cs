using System.Linq;

namespace NetGore.Features.NPCChat
{
    /// <summary>
    /// Describes the type of NPC chat item that a <see cref="NPCChatDialogViewNode"/> handles.
    /// </summary>
    public enum NPCChatDialogViewNodeItemType : byte
    {
        /// <summary>
        /// Handles an object that derives from <see cref="NPCChatDialogItemBase"/> and displays the
        /// actual contents of the dialog item.
        /// </summary>
        DialogItem,

        /// <summary>
        /// An object that derives from <see cref="NPCChatResponseBase"/>. 
        /// </summary>
        Response,

        /// <summary>
        /// Handles an object that derives from <see cref="NPCChatDialogItemBase"/> but does not display
        /// the actual contents of the dialog item. Instead, a node of this type will redirect to the
        /// corresponding <see cref="DialogItem"/>.
        /// </summary>
        Redirect,
    }
}