using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore.NPCChat;

namespace DemoGame.Server.NPCChat.Conditionals
{
    public class HPPercentLessThanOrEqualTo : NPCChatConditional
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NPCChatConditionalBase&lt;TUser, TNPC&gt;"/> class.
        /// </summary>
        public HPPercentLessThanOrEqualTo()
            : base("HP% <=", NPCChatConditionalParameterType.Float)
        {
        }

        /// <summary>
        /// When overridden in the derived class, performs the actual conditional checking.
        /// </summary>
        /// <param name="user">The User.</param>
        /// <param name="npc">The NPC.</param>
        /// <param name="parameters">The parameters to use. </param>
        /// <returns>True if the conditional returns true for the given <paramref name="user"/>,
        /// <paramref name="npc"/>, and <paramref name="parameters"/>; otherwise false.</returns>
        protected override bool DoCheck(User user, NPC npc, INPCChatConditionalParameter[] parameters)
        {
            float percent = GetPercent(user.HP, user.ModStats[StatType.MaxHP]);

            return percent <= parameters[0].ValueAsFloat;
        }
    }
}
