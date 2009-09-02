using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore.IO;

namespace NetGore.NPCChat
{
    public interface INPCChatConditionalCollection
    {
        /// <summary>
        /// Gets the NPCChatConditionalEvaluationType
        /// used when evaluating this conditional collection.
        /// </summary>
        NPCChatConditionalEvaluationType EvaluationType { get; }

        /// <summary>
        /// Reads the values for this NPCChatConditionalCollectionBase from an IValueReader.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        void Read(IValueReader reader);

        /// <summary>
        /// Writes the NPCChatConditionalCollectionBase's values to an IValueWriter.
        /// </summary>
        /// <param name="writer">IValueWriter to write the values to.</param>
        void Write(IValueWriter writer);
    }
}
