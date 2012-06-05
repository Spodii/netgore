using System.Linq;

namespace NetGore.Features.NPCChat.Conditionals
{
    /// <summary>
    /// Contains the allowed value types for a NPCChatConditionalParameter.
    /// </summary>
    public enum NPCChatConditionalParameterType : byte
    {
        /// <summary>
        /// A 32-bit signed floating-point value.
        /// </summary>
        Float,

        /// <summary>
        /// A 32-bit signed integer.
        /// </summary>
        Integer,

        /// <summary>
        /// A string of up to 65535 characters in length.
        /// </summary>
        String
    }
}