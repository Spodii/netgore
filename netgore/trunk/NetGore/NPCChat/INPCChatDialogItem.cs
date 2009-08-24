using System.Linq;

namespace NetGore.NPCChat
{
    public interface INPCChatDialogItem
    {
        ushort GetNextPage(object user, object npc, byte responseIndex);
    }
}