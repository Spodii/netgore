using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.NPCChat
{
    /// <summary>
    /// Describes an item used in the NPCChatConditionalCollectionBase, which contains the conditional to use
    /// and the values to use with it.
    /// </summary>
    /// <typeparam name="TUser">The Type of User.</typeparam>
    /// <typeparam name="TNPC">The Type of NPC.</typeparam>
    public abstract class NPCChatConditionalCollectionItemBase<TUser, TNPC> where TUser : class where TNPC : class
    {
        /// <summary>
        /// When overridden in the derived class, gets a boolean that, if true, the result of this conditional
        /// when evaluating will be flipped. That is, True becomes False and vise versa. If false, the
        /// evaluated value is unchanged.
        /// </summary>
        public abstract bool Not { get; }

        /// <summary>
        /// When overridden in the derived class, gets the NPCChatConditionalBase.
        /// </summary>
        public abstract NPCChatConditionalBase<TUser, TNPC> Conditional { get; }

        /// <summary>
        /// When overridden in the derived class, gets the collection of parameters to use when evaluating
        /// the conditional.
        /// </summary>
        public abstract INPCChatConditionalParameter[] Parameters { get; }

        /// <summary>
        /// Evaluates the conditional using the supplied values.
        /// </summary>
        /// <param name="user">The User.</param>
        /// <param name="npc">The NPC.</param>
        /// <returns>The result of the conditional's evaluation.</returns>
        public bool Evaluate(TUser user, TNPC npc)
        {
            bool ret = Conditional.Evaluate(user, npc, Parameters);

            if (Not)
                ret = !ret;

            return ret;
        }
    }
}
