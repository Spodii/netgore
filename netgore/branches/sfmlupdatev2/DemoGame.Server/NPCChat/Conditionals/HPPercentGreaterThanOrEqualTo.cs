using System.Linq;
using NetGore.Features.NPCChat.Conditionals;

namespace DemoGame.Server.NPCChat.Conditionals
{
    public class HPPercentGreaterThanOrEqualTo : ServerNPCChatConditional<User, NPC>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HPPercentGreaterThanOrEqualTo"/> class.
        /// </summary>
        HPPercentGreaterThanOrEqualTo() : base("HP% >=", NPCChatConditionalParameterType.Float)
        {
        }

        /// <summary>
        /// When overridden in the derived class, performs the actual conditional evaluation.
        /// </summary>
        /// <param name="user">The User.</param>
        /// <param name="npc">The NPC.</param>
        /// <param name="parameters">The parameters to use. </param>
        /// <returns>True if the conditional returns true for the given <paramref name="user"/>,
        /// <paramref name="npc"/>, and <paramref name="parameters"/>; otherwise false.</returns>
        protected override bool DoEvaluate(User user, NPC npc, NPCChatConditionalParameter[] parameters)
        {
            var percent = GetPercent(user.HP, user.ModStats[StatType.MaxHP]);

            return percent >= parameters[0].ValueAsFloat;
        }
    }
}