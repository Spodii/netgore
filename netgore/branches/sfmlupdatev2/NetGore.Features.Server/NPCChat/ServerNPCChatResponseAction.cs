using System.Linq;
using System.Reflection;
using log4net;

namespace NetGore.Features.NPCChat
{
    /// <summary>
    /// Class for an action that takes place after choosing a <see cref="ServerNPCChatResponse"/> in
    /// a <see cref="ServerNPCChatDialog"/>.
    /// </summary>
    /// <typeparam name="TUser">The type of User.</typeparam>
    /// <typeparam name="TNPC">The type of NPC.</typeparam>
    public abstract class ServerNPCChatResponseAction<TUser, TNPC> : NPCChatResponseActionBase
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the <see cref="NPCChatResponseActionBase"/> class.
        /// </summary>
        /// <param name="name">The unique name for this <see cref="NPCChatResponseActionBase"/>.
        /// This string is case-sensitive.</param>
        protected ServerNPCChatResponseAction(string name) : base(name)
        {
        }

        /// <summary>
        /// When overridden in the derived class, performs the actions implemented by this
        /// <see cref="NPCChatResponseActionBase"/>.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="npc">The npc.</param>
        protected abstract void DoExecute(TUser user, TNPC npc);

        /// <summary>
        /// When overridden in the derived class, performs the actions implemented by this
        /// <see cref="NPCChatResponseActionBase"/>.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="npc">The npc.</param>
        public override void Execute(object user, object npc)
        {
            if (log.IsInfoEnabled)
                log.InfoFormat("User `{0}` invoked response action `{1}` with NPC `{2}`.", user, Name, npc);

            DoExecute((TUser)user, (TNPC)npc);
        }
    }
}