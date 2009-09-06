using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;
using NetGore.NPCChat;

namespace DemoGame.Server.NPCChat
{    
    /// <summary>
    /// Class for an action that takes place after choosing a <see cref="NPCChatResponse"/> in
    /// a <see cref="NPCChatDialog"/>.
    /// </summary>
    public abstract class NPCChatResponseAction : NPCChatResponseActionBase
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the <see cref="NPCChatResponseActionBase"/> class.
        /// </summary>
        /// <param name="name">The unique name for this <see cref="NPCChatResponseActionBase"/>.
        /// This string is case-sensitive.</param>
        protected NPCChatResponseAction(string name) : base(name)
        {
        }

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

            DoExecute((User)user, (NPC)npc);
        }

        /// <summary>
        /// When overridden in the derived class, performs the actions implemented by this
        /// <see cref="NPCChatResponseActionBase"/>.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="npc">The npc.</param>
        protected abstract void DoExecute(User user, NPC npc);
    }
}
