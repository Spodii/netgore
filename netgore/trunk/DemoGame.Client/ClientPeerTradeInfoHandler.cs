using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DemoGame.DbObjs;
using NetGore.Features.PeerTrading;
using NetGore.IO;
using NetGore.Network;

namespace DemoGame.Client
{
    public class ClientPeerTradeInfoHandler : ClientPeerTradeInfoHandlerBase<Character, ItemEntity, IItemTable>
    {
        ISocketSender _socketSender;

        /// <summary>
        /// Gets or sets the <see cref="ISocketSender"/> used to communicate with the server.
        /// </summary>
        public ISocketSender SocketSender
        {
            get { return _socketSender; }
            set
            {
                if (_socketSender == value)
                    return;

                if (value == null)
                    throw new ArgumentNullException("value");

                _socketSender = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientPeerTradeInfoHandler"/> class.
        /// </summary>
        /// <param name="socketSender">The <see cref="ISocketSender"/> used to communicate with the server.</param>
        /// <exception cref="ArgumentNullException"><paramref name="socketSender"/> is null.</exception>
        public ClientPeerTradeInfoHandler(ISocketSender socketSender)
        {
            if (socketSender == null)
                throw new ArgumentNullException("socketSender");

            SocketSender = socketSender;
        }

        /// <summary>
        /// When overridden in the derived class, gets a <see cref="PacketWriter"/> to use to write data to.
        /// The created <see cref="PacketWriter"/> should also contain a header ID so you can recognize when messages
        /// are to/from peer trading handler.
        /// </summary>
        /// <returns>A <see cref="PacketWriter"/> to use to write data to.</returns>
        protected override PacketWriter CreateWriter()
        {
            return ClientPacket.GetWriter(ClientPacketID.PeerTradeEvent);
        }

        /// <summary>
        /// When overridden in the derived class, reads the information for an item.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read from.</param>
        /// <returns>The read item information.</returns>
        protected override IItemTable ReadItemInfo(IValueReader reader)
        {
            var ret = new ItemTable();
            ret.ReadState(reader);
            return ret;
        }

        /// <summary>
        /// When overridden in the derived class, handles sending data to the server.
        /// </summary>
        /// <param name="bs">The data to send.</param>
        protected override void SendData(BitStream bs)
        {
            SocketSender.Send(bs);
        }
    }
}
