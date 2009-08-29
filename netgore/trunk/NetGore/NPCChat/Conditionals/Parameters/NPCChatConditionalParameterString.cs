using System;
using System.Linq;
using NetGore.IO;

namespace NetGore.NPCChat
{
    /// <summary>
    /// A INPCChatConditionalParameter with a value of type String.
    /// </summary>
    class NPCChatConditionalParameterString : NPCChatConditionalParameter
    {
        string _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="NPCChatConditionalParameterString"/> struct.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        public NPCChatConditionalParameterString(string value)
        {
            _value = value;
        }

        /// <summary>
        /// When overridden in the derived class, reads the underlying value from the <paramref name="reader"/>.
        /// </summary>
        /// <param name="reader">The IValueReader to read from.</param>
        /// <param name="valueName">The name to of the value in the <paramref name="reader"/>.</param>
        protected override void ReadValue(IValueReader reader, string valueName)
        {
            _value = reader.ReadString(valueName);
        }

        /// <summary>
        /// When overridden in the derived class, writes the underlying value to the <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The IValueWriter to write to.</param>
        /// <param name="valueName">The name to of the value to use in the <paramref name="writer"/>.</param>
        protected override void WriteValue(IValueWriter writer, string valueName)
        {
            writer.Write(valueName, _value);
        }

        #region INPCChatConditionalParameter Members

        /// <summary>
        /// The NPCChatConditionalParameterType that describes the native value type of this parameter's Value.
        /// </summary>
        public override NPCChatConditionalParameterType ValueType
        {
            get { return NPCChatConditionalParameterType.String; }
        }

        /// <summary>
        /// Gets this parameter's Value as an object.
        /// </summary>
        public override object Value
        {
            get { return _value; }
        }

        /// <summary>
        /// Gets this parameter's Value as a String.
        /// </summary>
        /// <exception cref="MethodAccessException">The ValueType is not equal to
        /// NPCChatConditionalParameterType.String.</exception>
        public override string ValueAsString
        {
            get { return _value; }
        }

        #endregion
    }
}