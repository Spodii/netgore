using System;
using System.Linq;

namespace NetGore.NPCChat
{
    /// <summary>
    /// A INPCChatConditionalParameter with a value of type String.
    /// </summary>
    internal class NPCChatConditionalParameterString : INPCChatConditionalParameter
    {
        readonly string _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="NPCChatConditionalParameterString"/> struct.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        public NPCChatConditionalParameterString(string value)
        {
            _value = value;
        }

        #region INPCChatConditionalParameter Members

        /// <summary>
        /// The NPCChatConditionalParameterType that describes the native value type of this parameter's Value.
        /// </summary>
        public NPCChatConditionalParameterType ValueType
        {
            get { return NPCChatConditionalParameterType.String; }
        }

        /// <summary>
        /// Gets this parameter's Value as an object.
        /// </summary>
        public object Value
        {
            get { return _value; }
        }

        /// <summary>
        /// Gets this parameter's Value as an Integer.
        /// </summary>
        /// <exception cref="MethodAccessException">The ValueType is not equal to
        /// NPCChatConditionalParameterType.Integer.</exception>
        public int ValueAsInteger
        {
            get { throw NPCChatConditionalParameter.GetInvalidValueAsException(); }
        }

        /// <summary>
        /// Gets this parameter's Value as a Float.
        /// </summary>
        /// <exception cref="MethodAccessException">The ValueType is not equal to
        /// NPCChatConditionalParameterType.Float.</exception>
        public float ValueAsFloat
        {
            get { throw NPCChatConditionalParameter.GetInvalidValueAsException(); }
        }

        /// <summary>
        /// Gets this parameter's Value as a String.
        /// </summary>
        /// <exception cref="MethodAccessException">The ValueType is not equal to
        /// NPCChatConditionalParameterType.String.</exception>
        public string ValueAsString
        {
            get { return _value; }
        }

        #endregion
    }
}