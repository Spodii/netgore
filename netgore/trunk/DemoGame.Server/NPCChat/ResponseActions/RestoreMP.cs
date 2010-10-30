using System.Linq;
using NetGore.Features.NPCChat;

namespace DemoGame.Server.NPCChat.ResponseActions
{
    public class RestoreMP : ServerNPCChatResponseAction<User, NPC>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NPCChatResponseActionBase"/> class.
        /// </summary>
        RestoreMP() : base("Restore MP")
        {
        }

        /// <summary>
        /// When overridden in the derived class, performs the actions implemented by this
        /// <see cref="NPCChatResponseActionBase"/>.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="npc">The npc.</param>
        protected override void DoExecute(User user, NPC npc)
        {
            user.MP = (int)user.ModStats[StatType.MaxMP];
        }
    }
}