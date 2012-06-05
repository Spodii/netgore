using System.Linq;
using Lidgren.Network;
using NetGore.IO;

namespace NetGore.Network
{
    /// <summary>
    /// Interface for an object that can send data to one or more network connections.
    /// </summary>
    public interface INetworkSender
    {
        /// <summary>
        /// Gets if the connection is alive and functional.
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Sends data to the other end of the connection.
        /// </summary>
        /// <param name="data">Data to send.</param>
        /// <param name="deliveryMethod">The method to use to deliver the message. This determines how reliability, ordering,
        /// and sequencing will be handled.</param>
        /// <param name="sequenceChannel">The sequence channel to use to deliver the message. Only valid when
        /// <paramref name="deliveryMethod"/> is not equal to <see cref="NetDeliveryMethod.Unreliable"/> or
        /// <see cref="NetDeliveryMethod.ReliableUnordered"/>. Must also be a value greater than or equal to 0 and
        /// less than <see cref="NetConstants.NetChannelsPerDeliveryMethod"/>.</param>
        /// <returns>
        /// True if the <paramref name="data"/> was successfully enqueued for sending; otherwise false.
        /// </returns>
        /// <exception cref="NetException"><paramref name="deliveryMethod"/> equals <see cref="NetDeliveryMethod.Unreliable"/>
        /// or <see cref="NetDeliveryMethod.ReliableUnordered"/> and <paramref name="sequenceChannel"/> is non-zero.</exception>
        /// <exception cref="NetException"><paramref name="sequenceChannel"/> is less than 0 or greater than or equal to
        /// <see cref="NetConstants.NetChannelsPerDeliveryMethod"/>.</exception>
        /// <exception cref="NetException"><paramref name="deliveryMethod"/> equals <see cref="NetDeliveryMethod.Unknown"/>.</exception>
        bool Send(BitStream data, NetDeliveryMethod deliveryMethod, int sequenceChannel = 0);

        /// <summary>
        /// Sends data to the other end of the connection.
        /// </summary>
        /// <param name="data">Data to send.</param>
        /// <param name="deliveryMethod">The method to use to deliver the message. This determines how reliability, ordering,
        /// and sequencing will be handled.</param>
        /// <param name="sequenceChannel">The sequence channel to use to deliver the message. Only valid when
        /// <paramref name="deliveryMethod"/> is not equal to <see cref="NetDeliveryMethod.Unreliable"/> or
        /// <see cref="NetDeliveryMethod.ReliableUnordered"/>. Must also be a value greater than or equal to 0 and
        /// less than <see cref="NetConstants.NetChannelsPerDeliveryMethod"/>.</param>
        /// <returns>
        /// True if the <paramref name="data"/> was successfully enqueued for sending; otherwise false.
        /// </returns>
        /// <exception cref="NetException"><paramref name="deliveryMethod"/> equals <see cref="NetDeliveryMethod.Unreliable"/>
        /// or <see cref="NetDeliveryMethod.ReliableUnordered"/> and <paramref name="sequenceChannel"/> is non-zero.</exception>
        /// <exception cref="NetException"><paramref name="sequenceChannel"/> is less than 0 or greater than or equal to
        /// <see cref="NetConstants.NetChannelsPerDeliveryMethod"/>.</exception>
        /// <exception cref="NetException"><paramref name="deliveryMethod"/> equals <see cref="NetDeliveryMethod.Unknown"/>.</exception>
        bool Send(byte[] data, NetDeliveryMethod deliveryMethod, int sequenceChannel = 0);
    }
}