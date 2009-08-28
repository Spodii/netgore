using System;
using System.Linq;

namespace NetGore.NPCChat
{
    /// <summary>
    /// Interface for a NPCChatConditionalParameter.
    /// </summary>
    public interface INPCChatConditionalParameter
    {
        /// <summary>
        /// Gets this parameter's Value as an object.
        /// </summary>
        object Value { get; }

        /// <summary>
        /// Gets this parameter's Value as a Float.
        /// </summary>
        /// <exception cref="MethodAccessException">The ValueType is not equal to
        /// NPCChatConditionalParameterType.Float.</exception>
        float ValueAsFloat { get; }

        /// <summary>
        /// Gets this parameter's Value as an Integer.
        /// </summary>
        /// <exception cref="MethodAccessException">The ValueType is not equal to
        /// NPCChatConditionalParameterType.Integer.</exception>
        int ValueAsInteger { get; }

        /// <summary>
        /// Gets this parameter's Value as a String.
        /// </summary>
        /// <exception cref="MethodAccessException">The ValueType is not equal to
        /// NPCChatConditionalParameterType.String.</exception>
        string ValueAsString { get; }

        /// <summary>
        /// The NPCChatConditionalParameterType that describes the native value type of this parameter's Value.
        /// </summary>
        NPCChatConditionalParameterType ValueType { get; }
    }
}