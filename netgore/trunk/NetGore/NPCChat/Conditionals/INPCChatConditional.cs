using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.NPCChat
{
    public interface INPCChatConditional
    {
        /// <summary>
        /// Gets the unique name for this INPCChatConditional. This string is case-sensitive.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the number of parameters required by this INPCChatConditional.
        /// </summary>
        int ParameterCount { get; }

        /// <summary>
        /// Gets an IEnumerable of the NPCChatConditionalParameterTypes used in this INPCChatConditional.
        /// </summary>
        IEnumerable<NPCChatConditionalParameterType> ParameterTypes { get; }

        /// <summary>
        /// Gets the NPCChatConditionalParameterType for the parameter at the given <paramref name="index"/>.
        /// </summary>
        /// <param name="index">Index of the parameter to get the NPCChatConditionalParameterType for.</param>
        /// <returns>The NPCChatConditionalParameterType for the parameter at the given <paramref name="index"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="index"/> is less than 0 or greater
        /// than ParameterCount.</exception>
        NPCChatConditionalParameterType GetParameter(int index);
    }
}