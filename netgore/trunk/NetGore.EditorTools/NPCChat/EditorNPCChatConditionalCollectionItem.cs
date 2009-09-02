using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore.IO;
using NetGore.NPCChat;

namespace NetGore.EditorTools
{
    public class EditorNPCChatConditionalCollectionItem : NPCChatConditionalCollectionItemBase
    {
        NPCChatConditionalBase _conditional;
        bool _not;
        NPCChatConditionalParameter[] _parameters;

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
        /// Sets the Not property's value.
        /// </summary>
        /// <param name="value">The new value.</param>
        public void SetNot(bool value)
        {
            _not = value;
        }

        /// <summary>
        /// Tries the set one of the Parameters.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="value">The value.</param>
        /// <returns>True if set successfully; otherwise false.</returns>
        public bool TrySetParameter(int index, NPCChatConditionalParameter value)
        {
            _parameters[index] = value;
            return true;
        }

        /// <summary>
        /// When overridden in the derived class, gets the collection of parameters to use when evaluating
        /// the conditional.
        /// </summary>
        public override NPCChatConditionalParameter[] Parameters
        {
            get { return _parameters; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorNPCChatConditionalCollectionItem"/> class.
        /// </summary>
        /// <param name="r">The IValueReader to read from.</param>
        public EditorNPCChatConditionalCollectionItem(IValueReader r)
        {
            Read(r);
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
