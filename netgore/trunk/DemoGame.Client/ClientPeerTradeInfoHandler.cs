using System;
using System.Linq;
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

        protected override void OnTradeCanceled(bool sourceCanceled)
        {
            // TODO: !!
            base.OnTradeCanceled(sourceCanceled);
        }

        protected override void OnAcceptStatusChanged(bool isSourceSide, bool hasAccepted)
        {
            // TODO: !!
            base.OnAcceptStatusChanged(isSourceSide, hasAccepted);
        }

        protected override void OnSlotUpdated(NetGore.InventorySlot slot, bool isSourceSide)
        {
            // TODO: !!
            base.OnSlotUpdated(slot, isSourceSide);
        }

        protected override void OnTradeClosed()
        {
            // TODO: !!
            base.OnTradeClosed();
        }

        protected override void OnTradeCompleted()
        {
            // TODO: !!
            base.OnTradeCompleted();
        }

        protected override void OnTradeOpened()
        {
            // TODO: !!
            base.OnTradeOpened();
        }

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