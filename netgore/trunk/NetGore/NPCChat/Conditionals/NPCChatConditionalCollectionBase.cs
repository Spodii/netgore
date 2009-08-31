using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NetGore.IO;

namespace NetGore.NPCChat
{
    /// <summary>
    /// Base class for a collection of conditionals.
    /// </summary>
    /// <typeparam name="TUser">The Type of User.</typeparam>
    /// <typeparam name="TNPC">The Type of NPC.</typeparam>
    public abstract class NPCChatConditionalCollectionBase<TUser, TNPC> :
        IEnumerable<NPCChatConditionalCollectionItemBase<TUser, TNPC>> where TUser : class where TNPC : class
    {
        /// <summary>
        /// When overridden in the derived class, gets the NPCChatConditionalEvaluationType
        /// used when evaluating this conditional collection.
        /// </summary>
        public abstract NPCChatConditionalEvaluationType EvaluationType { get; }

        /// <summary>
        /// Creates a NPCChatConditionalCollectionItemBase instance then reades the values for it from the
        /// given IValueReader.
        /// </summary>
        /// <param name="reader">The IValueReader to read from.</param>
        /// <returns>A NPCChatConditionalCollectionItemBase instance with values read from
        /// the <paramref name="reader"/>.</returns>
        protected abstract NPCChatConditionalCollectionItemBase<TUser, TNPC> CreateItem(IValueReader reader);

        /// <summary>
        /// Evaluates every conditional in this collection using the provided EvaluationType.
        /// </summary>
        /// <param name="user">The User.</param>
        /// <param name="npc">The NPC.</param>
        /// <returns>True if the conditionals passed; otherwise false.</returns>
        public bool Evaluate(TUser user, TNPC npc)
        {
            switch (EvaluationType)
            {
                case NPCChatConditionalEvaluationType.OR:
                    foreach (var conditional in this)
                    {
                        if (conditional.Evaluate(user, npc))
                            return true;
                    }
                    return false;

                case NPCChatConditionalEvaluationType.AND:
                    foreach (var conditional in this)
                    {
                        if (!conditional.Evaluate(user, npc))
                            return false;
                    }
                    return true;

                default:
                    throw new Exception(string.Format("Invalid EvaluateType value `{0}`.", EvaluationType));
            }
        }

        /// <summary>
        /// Reads the values for this NPCChatConditionalCollectionBase from an IValueReader.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        protected void Read(IValueReader reader)
        {
            byte evaluationTypeValue = reader.ReadByte("EvaluationType");
            var items = reader.ReadManyNodes("Items", x => CreateItem(x));

            NPCChatConditionalEvaluationType evaluationType = (NPCChatConditionalEvaluationType)evaluationTypeValue;
            if (!EnumHelper.IsDefined(evaluationType))
                throw new Exception(string.Format("Invalid NPCChatConditionalEvaluationType `{0}`.", evaluationTypeValue));

            SetReadValues(evaluationType, items);
        }

        /// <summary>
        /// When overridden in the derived class, sets the values read from the Read method.
        /// </summary>
        protected abstract void SetReadValues(NPCChatConditionalEvaluationType evaluationType,
                                              NPCChatConditionalCollectionItemBase<TUser, TNPC>[] items);

        /// <summary>
        /// Writes the NPCChatConditionalCollectionBase's values to an IValueWriter.
        /// </summary>
        /// <param name="writer">IValueWriter to write the values to.</param>
        public void Write(IValueWriter writer)
        {
            writer.Write("EvaluationType", (byte)EvaluationType);
            writer.WriteManyNodes("Items", this, ((w, item) => item.Write(w)));
        }

        #region IEnumerable<NPCChatConditionalCollectionItemBase<TUser,TNPC>> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public abstract IEnumerator<NPCChatConditionalCollectionItemBase<TUser, TNPC>> GetEnumerator();

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}