using System.Linq;
using System.Reflection;
using Lidgren.Network;
using log4net;
using NetGore.IO;
using NetGore.Network;

namespace DemoGame.Server
{
    /// <summary>
    /// Extension methods for the <see cref="INetworkSender"/> interface.
    /// </summary>
    public static class INetworkSenderExtensions
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Sends data to the <see cref="INetworkSender"/>. This method is thread-safe.
        /// </summary>
        /// <param name="sender">The <see cref="INetworkSender"/> to use to send the data.</param>
        /// <param name="data">BitStream containing the data to send to the User.</param>
        /// <param name="messageType">The <see cref="ServerMessageType"/> to use for sending the <paramref name="data"/>.</param>
        public static void Send(this INetworkSender sender, BitStream data, ServerMessageType messageType)
        {
            if (!sender.IsConnected)
            {
                const string errmsg = "Send to `{0}` failed - not connected.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, sender);
                return;
            }

            NetDeliveryMethod method;
            int seqChannel;
            messageType.GetDeliveryMethod(out method, out seqChannel);

            sender.Send(data, method, seqChannel);
        }

        /// <summary>
        /// Sends data to the <see cref="INetworkSender"/>. This method is thread-safe.
        /// </summary>
        /// <param name="sender">The <see cref="INetworkSender"/> to use to send the data.</param>
        /// <param name="message">GameMessage to send to the User.</param>
        /// <param name="messageType">The <see cref="ServerMessageType"/> to use for sending the <paramref name="message"/>.</param>
        public static void Send(this INetworkSender sender, GameMessage message, ServerMessageType messageType)
        {
            sender.Send(message, messageType, null);
        }

        /// <summary>
        /// Sends data to the <see cref="INetworkSender"/>. This method is thread-safe.
        /// </summary>
        /// <param name="sender">The <see cref="INetworkSender"/> to use to send the data.</param>
        /// <param name="message">GameMessage to send to the User.</param>
        /// <param name="messageType">The <see cref="ServerMessageType"/> to use for sending the <paramref name="message"/>.</param>
        /// <param name="parameters">Message parameters.</param>
        public static void Send(this INetworkSender sender, GameMessage message, ServerMessageType messageType,
                                params object[] parameters)
        {
            using (var pw = ServerPacket.SendMessage(message, parameters))
            {
                sender.Send(pw, messageType);
            }
        }
    }
}