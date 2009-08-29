using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.NPCChat
{
    /// <summary>
    /// Enum of the different operators that can be used for evaluating multiple conditionals.
    /// </summary>
    public enum NPCChatConditionalEvaluationType : byte
    {
        /// <summary>
        /// Requires all conditionals to be true.
        /// </summary>
        AND,

        /// <summary>
        /// Requires only one conditional to be true.
        /// </summary>
        OR
    }
}
