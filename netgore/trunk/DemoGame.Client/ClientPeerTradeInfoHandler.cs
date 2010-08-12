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
        /// Delegate for handling the <see cref="GameMessageCallback"/> event.
        /// </summary>
        /// <param name="sender">The <see cref="ClientPeerTradeInfoHandler"/> this event came from.</param>
        /// <param name="gameMessage">The <see cref="GameMessage"/>.</param>
        /// <param name="args">The arguments for the message.</param>
        public delegate void GameMessageCallbackHandler(ClientPeerTradeInfoHandler sender, GameMessage gameMessage, string[] args);

        /// <summary>
        /// Notifies listeners when this object has generated a <see cref="GameMessage"/> that needs to be displayed.
        /// </summary>
        public event GameMessageCallbackHandler GameMessageCallback;

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
                    GameMessageCallback(this, GameMessage.PeerTradingTradeCanceledByYou, new string[] { OtherCharName });
            }
            else
            {
                // They canceled
                if (GameMessageCallback != null)
                    GameMessageCallback(this, GameMessage.PeerTradingTradeCanceledByOther, new string[] { OtherCharName });
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
                GameMessageCallback(this, GameMessage.PeerTradingTradeComplete, new string[] { OtherCharName });
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling the
        /// <see cref="ClientPeerTradeInfoHandlerBase{TChar,TItem,TItemInfo}.TradeOpened"/> event.
        /// </summary>
        protected override void OnTradeOpened()
        {
            base.OnTradeOpened();

            if (GameMessageCallback != null)
                GameMessageCallback(this, GameMessage.PeerTradingTradeOpened, new string[] { OtherCharName });
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