using System.Linq;

namespace NetGore.NPCChat
{
    /// <summary>
    /// Contains the allowed value types for a NPCChatConditionalParameter.
    /// </summary>
    public enum NPCChatConditionalParameterType
    {
        /// <summary>
        /// A 32-bit signed integer.
        /// </summary>
        Integer,

        /// <summary>
        /// A 32-bit signed floating-point value.
        /// </summary>
        Float,

        /// <summary>
        /// A string of up to 65535 characters in length.
        /// </summary>
        String
    }
}