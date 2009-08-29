using System;
using System.Linq;

namespace NetGore.NPCChat
{
    public static class NPCChatConditionalParameter
    {
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
        public static INPCChatConditionalParameter CreateParameter(NPCChatConditionalParameterType valueType, object value)
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

        public static INPCChatConditionalParameter CreateParameterAsFloat(float value)
        {
            return new NPCChatConditionalParameterFloat(value);
        }

        public static INPCChatConditionalParameter CreateParameterAsInteger(int value)
        {
            return new NPCChatConditionalParameterInteger(value);
        }

        public static INPCChatConditionalParameter CreateParameterAsString(string value)
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

        internal static MethodAccessException GetInvalidValueAsException()
        {
            const string errmsg =
                "Cannot cast the value to the requested Type." +
                " The ValueAs... methods are only valid when the base Type equals the requested Type.";
            return new MethodAccessException(errmsg);
        }
    }
}