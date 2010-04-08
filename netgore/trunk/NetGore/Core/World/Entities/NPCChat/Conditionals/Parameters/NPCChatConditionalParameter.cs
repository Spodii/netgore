using System;
using System.Linq;
using NetGore.IO;

namespace NetGore.NPCChat.Conditionals
{
    /// <summary>
    /// Base class for a parameter used on a conditional in the NPC chat.
    /// </summary>
    public abstract class NPCChatConditionalParameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NPCChatConditionalParameter"/> class.
        /// </summary>
        protected internal NPCChatConditionalParameter()
        {
        }

        /// <summary>
        /// When overridden in the derived class, gets this parameter's Value as an object.
        /// </summary>
        public abstract object Value { get; set; }

        /// <summary>
        /// When overridden in the derived class, gets this parameter's Value as a Float.
        /// </summary>
        /// <exception cref="MethodAccessException">The ValueType is not equal to
        /// NPCChatConditionalParameterType.Float.</exception>
        public virtual float ValueAsFloat
        {
            get { throw GetInvalidValueAsException(); }
        }

        /// <summary>
        /// When overridden in the derived class, gets this parameter's Value as an Integer.
        /// </summary>
        /// <exception cref="MethodAccessException">The ValueType is not equal to
        /// NPCChatConditionalParameterType.Integer.</exception>
        public virtual int ValueAsInteger
        {
            get { throw GetInvalidValueAsException(); }
        }

        /// <summary>
        /// When overridden in the derived class, gets this parameter's Value as a String.
        /// </summary>
        /// <exception cref="MethodAccessException">The ValueType is not equal to
        /// NPCChatConditionalParameterType.String.</exception>
        public virtual string ValueAsString
        {
            get { throw GetInvalidValueAsException(); }
        }

        /// <summary>
        /// When overridden in the derived class, gets the NPCChatConditionalParameterType
        /// that describes the native value type of this parameter's Value.
        /// </summary>
        public abstract NPCChatConditionalParameterType ValueType { get; }

        /// <summary>
        /// Creates a INPCChatConditionalParameter for the given <paramref name="valueType"/> that contains
        /// the given <paramref name="value"/>.
        /// </summary>
        /// <param name="valueType">Type NPCChatConditionalParameterType that describes the value type to use
        /// for the INPCChatConditionalParameter.</param>
        /// <param name="value">The value for the INPCChatConditionalParameter. Must be a valid type for the
        /// given <paramref name="valueType"/>.</param>
        /// <returns>A INPCChatConditionalParameter created using the given <paramref name="valueType"/>
        /// and <paramref name="value"/>.</returns>
        public static NPCChatConditionalParameter CreateParameter(NPCChatConditionalParameterType valueType, object value)
        {
            switch (valueType)
            {
                case NPCChatConditionalParameterType.String:
                    if (value.GetType() != typeof(string))
                        throw GetInvalidTypeForValueException(valueType, value);
                    return CreateParameterAsString((string)value);

                case NPCChatConditionalParameterType.Float:
                    if (value.GetType() != typeof(float))
                        throw GetInvalidTypeForValueException(valueType, value);
                    return CreateParameterAsFloat((float)value);

                case NPCChatConditionalParameterType.Integer:
                    if (value.GetType() != typeof(int))
                        throw GetInvalidTypeForValueException(valueType, value);
                    return CreateParameterAsInteger((int)value);

                default:
                    throw new ArgumentOutOfRangeException("valueType", "Invalid NPCChatConditionalParameterType.");
            }
        }

        /// <summary>
        /// Creates a NPCChatConditionalParameter for the given NPCChatConditionalParameterType.
        /// </summary>
        /// <param name="valueType">The NPCChatConditionalParameterType to create the parameter for.</param>
        /// <returns>A NPCChatConditionalParameter for the given NPCChatConditionalParameterType.</returns>
        public static NPCChatConditionalParameter CreateParameter(NPCChatConditionalParameterType valueType)
        {
            switch (valueType)
            {
                case NPCChatConditionalParameterType.Float:
                    return CreateParameterAsFloat(default(float));

                case NPCChatConditionalParameterType.Integer:
                    return CreateParameterAsInteger(default(int));

                case NPCChatConditionalParameterType.String:
                    return CreateParameterAsString(default(string));

                default:
                    throw new ArgumentOutOfRangeException("valueType", "Invalid NPCChatConditionalParameterType.");
            }
        }

        /// <summary>
        /// Creates a NPCChatConditionalParameter with the underlying value type as a float.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A NPCChatConditionalParameter with the underlying value type as a float.</returns>
        public static NPCChatConditionalParameter CreateParameterAsFloat(float value)
        {
            return new NPCChatConditionalParameterFloat(value);
        }

        /// <summary>
        /// Creates a NPCChatConditionalParameter with the underlying value type as an int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A NPCChatConditionalParameter with the underlying value type as an int.</returns>
        public static NPCChatConditionalParameter CreateParameterAsInteger(int value)
        {
            return new NPCChatConditionalParameterInteger(value);
        }

        /// <summary>
        /// Creates a NPCChatConditionalParameter with the underlying value type as a string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A NPCChatConditionalParameter with the underlying value type as a string.</returns>
        public static NPCChatConditionalParameter CreateParameterAsString(string value)
        {
            return new NPCChatConditionalParameterString(value);
        }

        static ArgumentException GetInvalidTypeForValueException(NPCChatConditionalParameterType valueType, object value)
        {
            const string errmsg =
                "The given value [{0}] is of Type `{1}`, which is not a" +
                " valid type for NPCChatConditionalParameterType `{2}`.";
            const string paramName = "value";

            return new ArgumentException(string.Format(errmsg, value, value.GetType(), valueType), paramName);
        }

        static MethodAccessException GetInvalidValueAsException()
        {
            const string errmsg =
                "Cannot cast the value to the requested Type." +
                " The ValueAs... methods are only valid when the base Type equals the requested Type.";
            return new MethodAccessException(errmsg);
        }

        /// <summary>
        /// Reads a NPCChatConditionalParameter from the given IValueReader.
        /// </summary>
        /// <param name="reader">The IValueReader to read the NPCChatConditionalParameter from.</param>
        /// <returns>The NPCChatConditionalParameter read from the IValueReader.</returns>
        public static NPCChatConditionalParameter Read(IValueReader reader)
        {
            byte valueTypeValue = reader.ReadByte("ValueType");
            NPCChatConditionalParameterType valueType = (NPCChatConditionalParameterType)valueTypeValue;

            if (!EnumHelper<NPCChatConditionalParameterType>.IsDefined(valueType))
                throw new Exception(string.Format("Invalid NPCChatConditionalParameterType `{0}`.", valueType));

            // Create the parameter and read the value
            NPCChatConditionalParameter parameter = CreateParameter(valueType);
            parameter.ReadValue(reader, "Value");

            return parameter;
        }

        /// <summary>
        /// When overridden in the derived class, reads the underlying value from the <paramref name="reader"/>.
        /// </summary>
        /// <param name="reader">The IValueReader to read from.</param>
        /// <param name="valueName">The name to of the value in the <paramref name="reader"/>.</param>
        protected abstract void ReadValue(IValueReader reader, string valueName);

        /// <summary>
        /// When overridden in the derived class, tries to set the Value of this NPCChatConditionalParameter
        /// from a string.
        /// </summary>
        /// <param name="value">The string containing the new value.</param>
        /// <param name="parser">The parser to use.</param>
        /// <returns>True if the value was successfully set; otherwise false.</returns>
        public abstract bool TrySetValue(string value, Parser parser);

        /// <summary>
        /// Writes the NPCChatConditionalParameter to the given IValueWriter.
        /// </summary>
        /// <param name="p">The NPCChatConditionalParameter.</param>
        /// <param name="writer">The IValueWriter to write to.</param>
        public static void Write(NPCChatConditionalParameter p, IValueWriter writer)
        {
            writer.Write("ValueType", (byte)p.ValueType);
            p.WriteValue(writer, "Value");
        }

        /// <summary>
        /// Writes the NPCChatConditionalParameter's values to an IValueWriter.
        /// </summary>
        /// <param name="writer">IValueWriter to write the values to.</param>
        public void Write(IValueWriter writer)
        {
            Write(this, writer);
        }

        /// <summary>
        /// When overridden in the derived class, writes the underlying value to the <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The IValueWriter to write to.</param>
        /// <param name="valueName">The name to of the value to use in the <paramref name="writer"/>.</param>
        protected abstract void WriteValue(IValueWriter writer, string valueName);
    }
}