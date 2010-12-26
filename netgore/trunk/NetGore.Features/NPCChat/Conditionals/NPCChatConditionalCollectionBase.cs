using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NetGore.IO;

namespace NetGore.Features.NPCChat.Conditionals
{
    /// <summary>
    /// Base class for a collection of conditionals.
    /// </summary>
    public abstract class NPCChatConditionalCollectionBase : IEnumerable<NPCChatConditionalCollectionItemBase>
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
        protected abstract NPCChatConditionalCollectionItemBase CreateItem(IValueReader reader);

        /// <summary>
        /// Evaluates every conditional in this collection using the provided EvaluationType.
        /// </summary>
        /// <param name="user">The User.</param>
        /// <param name="npc">The NPC.</param>
        /// <returns>True if the conditionals passed; otherwise false.</returns>
        /// <exception cref="InvalidOperationException"><see cref="EvaluationType"/> is not a defined
        /// <see cref="NPCChatConditionalEvaluationType"/> enum value.</exception>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "EvaluateType")]
        public bool Evaluate(object user, object npc)
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
                    const string errmsg = "Invalid EvaluateType value `{0}`.";
                    throw new InvalidOperationException(string.Format(errmsg, EvaluationType));
            }
        }

        /// <summary>
        /// Reads the values for this <see cref="NPCChatConditionalCollectionBase"/> from an <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="reader"><see cref="IValueReader"/> to read the values from.</param>
        /// <exception cref="InvalidEnumArgumentException">The read <see cref="NPCChatConditionalEvaluationType"/>
        /// was not a defined value of the <see cref="NPCChatConditionalEvaluationType"/> enum.</exception>
        public void Read(IValueReader reader)
        {
            var evaluationTypeValue = reader.ReadByte("EvaluationType");
            var items = reader.ReadManyNodes("Items", CreateItem);

            var evaluationType = (NPCChatConditionalEvaluationType)evaluationTypeValue;
            if (!EnumHelper<NPCChatConditionalEvaluationType>.IsDefined(evaluationType))
            {
                const string errmsg = "Invalid NPCChatConditionalEvaluationType `{0}`.";
                throw new InvalidEnumArgumentException(string.Format(errmsg, evaluationTypeValue));
            }

            SetReadValues(evaluationType, items);
        }

        /// <summary>
        /// When overridden in the derived class, sets the values read from the Read method.
        /// </summary>
        /// <param name="evaluationType">The <see cref="NPCChatConditionalEvaluationType"/>.</param>
        /// <param name="items">The <see cref="NPCChatConditionalCollectionItemBase"/>s.</param>
        protected abstract void SetReadValues(NPCChatConditionalEvaluationType evaluationType,
                                              NPCChatConditionalCollectionItemBase[] items);

        /// <summary>
        /// Writes the NPCChatConditionalCollectionBase's values to an IValueWriter.
        /// </summary>
        /// <param name="writer">IValueWriter to write the values to.</param>
        public void Write(IValueWriter writer)
        {
            writer.Write("EvaluationType", (byte)EvaluationType);
            writer.WriteManyNodes("Items", this, ((w, item) => item.Write(w)));
        }

        #region IEnumerable<NPCChatConditionalCollectionItemBase> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public abstract IEnumerator<NPCChatConditionalCollectionItemBase> GetEnumerator();

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}