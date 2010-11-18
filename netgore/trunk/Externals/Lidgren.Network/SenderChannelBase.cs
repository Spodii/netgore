using System.Linq;

namespace Lidgren.Network
{
    abstract class SenderChannelBase
    {
        internal abstract void Reset();

        internal abstract NetSendResult Send(float now, NetOutgoingMessage message);

        internal abstract void SendQueuedMessages(float now);
    }
}