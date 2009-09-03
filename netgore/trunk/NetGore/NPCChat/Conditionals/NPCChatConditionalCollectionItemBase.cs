using System;
using System.Linq;
using NetGore.IO;

namespace NetGore.NPCChat.Conditionals
{
    /// <summary>
    /// Describes an item used in the NPCChatConditionalCollectionBase, which contains the conditional to use
    /// and the values to use with it.
    /// </summary>
    public abstract class NPCChatConditionalCollectionItemBase
    {
        /// <summary>
        /// When overridden in the derived class, gets the NPCChatConditionalBase.
        /// </summary>
        public abstract NPCChatConditionalBase Conditional { get; }

        /// <summary>
        /// When overridden in the derived class, gets a boolean that, if true, the result of this conditional
        /// when evaluating will be flipped. That is, True becomes False and vise versa. If false, the
        /// evaluated value is unchanged.
        /// </summary>
        public abstract bool Not { get; }

        /// <summary>
        /// When overridden in the derived class, gets the collection of parameters to use when evaluating
        /// the conditional.
        /// </summary>
        public abstract NPCChatConditionalParameter[] Parameters { get; }

        /// <summary>
        /// Evaluates the conditional using the supplied values.
        /// </summary>
        /// <param name="user">The User.</param>
        /// <param name="npc">The NPC.</param>
        /// <returns>The result of the conditional's evaluation.</returns>
        public bool Evaluate(object user, object npc)
        {
            bool ret = Conditional.Evaluate(user, npc, Parameters);

            if (Not)
                ret = !ret;

            return ret;
        }

        /// <summary>
        /// Reads the values for this NPCChatConditionalCollectionItemBase from an IValueReader.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        protected void Read(IValueReader reader)
        {
            bool not = reader.ReadBool("Not");
            string conditionalName = reader.ReadString("ConditionalName");
            var parameters = reader.ReadManyNodes<NPCChatConditionalParameter>("Parameters", NPCChatConditionalParameter.Read);

            NPCChatConditionalBase conditional = NPCChatConditionalBase.GetConditional(conditionalName);
            if (conditional == null)
                throw new Exception(string.Format("Failed to get conditional `{0}`.", conditionalName));

            SetReadValues(conditional, not, parameters);
        }

        /// <summary>
        /// When overridden in the derived class, sets the values read from the Read method.
        /// </summary>
        /// <param name="conditional">The conditional.</param>
        /// <param name="not">The Not value.</param>
        /// <param name="parameters">The parameters.</param>
        protected abstract void SetReadValues(NPCChatConditionalBase conditional, bool not,
                                              NPCChatConditionalParameter[] parameters);

        /// <summary>
        /// Writes the NPCChatConditionalCollectionItemBase's values to an IValueWriter.
        /// </summary>
        /// <param name="writer">IValueWriter to write the values to.</param>
        public void Write(IValueWriter writer)
        {
            writer.Write("Not", Not);
            writer.Write("ConditionalName", Conditional.Name);
            writer.WriteManyNodes("Parameters", Parameters, ((w, item) => item.Write(w)));
        }
    }
}