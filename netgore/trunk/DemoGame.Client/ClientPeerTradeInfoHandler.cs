using System;
using System.Linq;
using DemoGame.DbObjs;
using NetGore;
using NetGore.Features.PeerTrading;
using NetGore.IO;
using NetGore.Network;

namespace DemoGame.Client
{
    public class ClientPeerTradeInfoHandler : ClientPeerTradeInfoHandlerBase<Character, ItemEntity, IItemTable>
    {
        INetworkSender _networkSender;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientPeerTradeInfoHandler"/> class.
        /// </summary>
        /// <param name="networkSender">The <see cref="INetworkSender"/> used to communicate with the server.</param>
        /// <exception cref="ArgumentNullException"><paramref name="networkSender"/> is null.</exception>
        public ClientPeerTradeInfoHandler(INetworkSender networkSender)
        {
            if (networkSender == null)
                throw new ArgumentNullException("networkSender");

            NetworkSender = networkSender;
        }

        /// <summary>
        /// Notifies listeners when this object has generated a <see cref="GameMessage"/> that needs to be displayed.
        /// </summary>
        public event TypedEventHandler<ClientPeerTradeInfoHandler, ClientPeerTradeInfoHandlerEventArgs> GameMessageCallback;

        /// <summary>
        /// Gets or sets the <see cref="INetworkSender"/> used to communicate with the server.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
        public INetworkSender NetworkSender
        {
            get { return _networkSender; }
            set
            {
                if (_networkSender == value)
                    return;

                if (value == null)
                    throw new ArgumentNullException("value");

                _networkSender = value;
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
        /// When overridden in the derived class, allows for handling the <see cref="ClientPeerTradeInfoHandlerBase{TChar,TItem,TItemInfo}.TradeCanceled"/> event.
        /// </summary>
        /// <param name="sourceCanceled">If it was the source character who canceled the trade.</param>
        protected override void OnTradeCanceled(bool sourceCanceled)
        {
            base.OnTradeCanceled(sourceCanceled);

            // Display the cancel message, using the appropriate one for if we were the one to cancel the trade
            if ((sourceCanceled && UserIsSource) || (!sourceCanceled && !UserIsSource))
            {
                // We canceled
                if (GameMessageCallback != null)
                {
                    GameMessageCallback.Raise(this,
                        new ClientPeerTradeInfoHandlerEventArgs(GameMessage.PeerTradingTradeCanceledByYou,
                            new string[] { OtherCharName }));
                }
            }
            else
            {
                // They canceled
                if (GameMessageCallback != null)
                {
                    GameMessageCallback.Raise(this,
                        new ClientPeerTradeInfoHandlerEventArgs(GameMessage.PeerTradingTradeCanceledByOther,
                            new string[] { OtherCharName }));
                }
            }
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling the
        /// <see cref="ClientPeerTradeInfoHandlerBase{TChar,TItem,TItemInfo}.TradeCompleted"/> event.
        /// </summary>
        protected override void OnTradeCompleted()
        {
            base.OnTradeCompleted();

            if (GameMessageCallback != null)
            {
                GameMessageCallback.Raise(this,
                    new ClientPeerTradeInfoHandlerEventArgs(GameMessage.PeerTradingTradeComplete, new string[] { OtherCharName }));
            }
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling the
        /// <see cref="ClientPeerTradeInfoHandlerBase{TChar,TItem,TItemInfo}.TradeOpened"/> event.
        /// </summary>
        protected override void OnTradeOpened()
        {
            base.OnTradeOpened();

            if (GameMessageCallback != null)
            {
                GameMessageCallback.Raise(this,
                    new ClientPeerTradeInfoHandlerEventArgs(GameMessage.PeerTradingTradeOpened, new string[] { OtherCharName }));
            }
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
            NetworkSender.Send(bs, ClientMessageType.GUI);
        }
    }
}