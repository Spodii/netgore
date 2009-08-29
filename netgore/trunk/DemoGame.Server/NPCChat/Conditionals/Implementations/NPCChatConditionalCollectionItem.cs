using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore.NPCChat;

namespace DemoGame.Server.NPCChat.Conditionals
{
    public class NPCChatConditionalCollectionItem<TUser, TNPC> : NPCChatConditionalCollectionItemBase<TUser, TNPC>
        where TUser : class
        where TNPC : class
    {
        bool _not;
        NPCChatConditionalBase<TUser, TNPC> _conditional;
        NPCChatConditionalParameter[] _parameters;

        /// <summary>
        /// When overridden in the derived class, sets the values read from the Read method.
        /// </summary>
        /// <param name="conditional">The conditional.</param>
        /// <param name="not">The Not value.</param>
        /// <param name="parameters">The parameters.</param>
        protected override void SetReadValues(NPCChatConditionalBase<TUser, TNPC> conditional, bool not, NPCChatConditionalParameter[] parameters)
        {
            _conditional = conditional;
            _not = not;
            _parameters = parameters;
        }

        /// <summary>
        /// When overridden in the derived class, gets a boolean that, if true, the result of this conditional
        /// when evaluating will be flipped. That is, True becomes False and vise versa. If false, the
        /// evaluated value is unchanged.
        /// </summary>
        public override bool Not
        {
            get { return _not; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the NPCChatConditionalBase.
        /// </summary>
        public override NPCChatConditionalBase<TUser, TNPC> Conditional
        {
            get { return _conditional; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the collection of parameters to use when evaluating
        /// the conditional.
        /// </summary>
        public override NPCChatConditionalParameter[] Parameters
        {
            get { return _parameters; }
        }
    }
}
