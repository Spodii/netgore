using System.Linq;

namespace NetGore.Features.NPCChat
{
    /// <summary>
    /// Handles when the selected <see cref="EditorNPCChatConditionalCollectionItem"/> in the
    /// <see cref="NPCChatConditionalsListBox"/> changes.
    /// </summary>
    /// <param name="sender">The <see cref="NPCChatConditionalsListBox"/> that the event came from.</param>
    /// <param name="item">The new selected <see cref="EditorNPCChatConditionalCollectionItem"/>.</param>
    public delegate void SelectedConditionalItemChangeHandler(
        NPCChatConditionalsListBox sender, EditorNPCChatConditionalCollectionItem item);
}