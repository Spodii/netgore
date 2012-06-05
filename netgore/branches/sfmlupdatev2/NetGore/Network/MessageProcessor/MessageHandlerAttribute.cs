using System;
using System.Linq;

namespace NetGore.Network
{
    /// <summary>
    /// Marks a method as being a message processor. The method this attribute is attached to must have the same
    /// signature as <see cref="MessageProcessorHandler"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class MessageHandlerAttribute : Attribute
    {
        readonly MessageProcessorID _msgID;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageHandlerAttribute"/> class.
        /// </summary>
        /// <param name="msgID">The ID of the message that the method this attribute is attached to will handle.</param>
        public MessageHandlerAttribute(MessageProcessorID msgID)
        {
            _msgID = msgID;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageHandlerAttribute"/> class.
        /// </summary>
        /// <param name="msgID">The ID of the message that the method this attribute is attached to will handle.</param>
        public MessageHandlerAttribute(uint msgID) : this(new MessageProcessorID(msgID))
        {
        }

        /// <summary>
        /// Gets the ID of the message that the method this attribute is attached to will handle.
        /// </summary>
        public MessageProcessorID MsgID
        {
            get { return _msgID; }
        }
    }
}