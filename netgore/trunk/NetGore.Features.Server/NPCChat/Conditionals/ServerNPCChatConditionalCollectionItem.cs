using System.Collections.Generic;
using System.Linq;
using NetGore.IO;

namespace NetGore.Features.NPCChat.Conditionals
{
    public class ServerNPCChatConditionalCollectionItem : NPCChatConditionalCollectionItemBase
    {
        NPCChatConditionalBase _conditional;
        bool _not;
        NPCChatConditionalParameter[] _parameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerNPCChatConditionalCollectionItem"/> class.
        /// </summary>
        /// <param name="r">The IValueReader to read from.</param>
        public ServerNPCChatConditionalCollectionItem(IValueReader r)
        {
            Read(r);
        }

        /// <summary>
        /// When overridden in the derived class, gets the NPCChatConditionalBase.
        /// </summary>
        public override NPCChatConditionalBase Conditional
        {
            get { return _conditional; }
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
        /// When overridden in the derived class, gets the collection of parameters to use when evaluating
        /// the conditional.
        /// </summary>
        public override ICollection<NPCChatConditionalParameter> Parameters
        {
            get { return _parameters; }
        }

        /// <summary>
        /// When overridden in the derived class, sets the values read from the Read method.
        /// </summary>
        /// <param name="conditional">The conditional.</param>
        /// <param name="not">The Not value.</param>
        /// <param name="parameters">The parameters.</param>
        protected override void SetReadValues(NPCChatConditionalBase conditional, bool not,
                                              NPCChatConditionalParameter[] parameters)
        {
            _conditional = conditional;
            _not = not;
            _parameters = parameters;
        }
    }
}