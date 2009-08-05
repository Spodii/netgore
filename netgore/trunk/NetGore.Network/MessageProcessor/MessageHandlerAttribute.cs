using System;
using System.Linq;

namespace NetGore.Network
{
    /// <summary>
    /// Marks a method as being a message processor. The method is required
    /// to be public and have a void return.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class MessageHandlerAttribute : Attribute
    {
        /// <summary>
        /// ID of the message this method will handle
        /// </summary>
        public readonly byte MsgID;

        /// <summary>
        /// MessageAttribute constructor
        /// </summary>
        /// <param name="msgID">ID of the message this method will handle</param>
        public MessageHandlerAttribute(byte msgID)
        {
            MsgID = msgID;
        }
    }
}