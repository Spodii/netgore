using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.NPCChat
{
    public interface INPCChatDialogItem
    {
        ushort GetNextPage(object user, object npc, byte responseIndex);
    }
}
