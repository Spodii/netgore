using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Lidgren.Network;
using log4net;
using NetGore.IO;
using NetGore.Network;

namespace DemoGame.Client
{
    /// <summary>
    /// Extension methods for the <see cref="INetworkSender"/>.
    /// </summary>
    public static class INetworkSenderExtensions
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Sends data to the <see cref="INetworkSender"/>. This method is thread-safe.
        /// </summary>
        /// <param name="sender">The <see cref="INetworkSender"/> to use to send the data.</param>
        /// <param name="data">BitStream containing the data to send to the User.</param>
        /// <param name="messageType">The <see cref="ClientMessageType"/> to use for sending the <paramref name="data"/>.</param>
        public static void Send(this INetworkSender sender, BitStream data, ClientMessageType messageType)
        {
            if (!sender.IsConnected)
            {
                const string errmsg = "Send to `{0}` failed - not connected.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, sender);
                Debug.Fail(string.Format(errmsg, sender));
                return;
            }

            NetDeliveryMethod method;
            int seqChannel;
            messageType.GetDeliveryMethod(out method, out seqChannel);

            sender.Send(data, method, seqChannel);
        }
    }
}