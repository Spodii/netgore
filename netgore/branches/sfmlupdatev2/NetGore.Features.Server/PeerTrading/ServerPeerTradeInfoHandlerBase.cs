using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.IO;
using NetGore.Network;
using NetGore.World;

namespace NetGore.Features.PeerTrading
{
    /// <summary>
    /// Handles the networking for messages for PeerTrading on the server side.
    /// </summary>
    /// <typeparam name="TChar">The type of character.</typeparam>
    /// <typeparam name="TItem">The type of item.</typeparam>
    /// <typeparam name="TItemInfo">The type describing item information.</typeparam>
    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")]
    public abstract class ServerPeerTradeInfoHandlerBase<TChar, TItem, TItemInfo>
        where TChar : Entity where TItem : Entity where TItemInfo : class
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// When overridden in the derived class, gets if a character can have data sent to it.
        /// </summary>
        /// <param name="receiver">The character to check if can have data sent to it.</param>
        /// <returns>True if <paramref name="receiver"/> can have data sent to it; otherwise false.</returns>
        protected abstract bool CanSendDataTo(TChar receiver);

        /// <summary>
        /// When overridden in the derived class, gets a <see cref="PacketWriter"/> to use to write data to.
        /// The created <see cref="PacketWriter"/> should also contain a header ID so you can recognize when messages
        /// are to/from peer trading handler.
        /// </summary>
        /// <returns>A <see cref="PacketWriter"/> to use to write data to.</returns>
        protected abstract PacketWriter CreateWriter();

        /// <summary>
        /// Gets a <see cref="PacketWriter"/> to use to write data to.
        /// </summary>
        /// <param name="id">The ID of the message to write.</param>
        /// <returns>
        /// A <see cref="PacketWriter"/> to use to write data to.
        /// </returns>
        PacketWriter CreateWriter(PeerTradeInfoServerMessage id)
        {
            var pw = CreateWriter();
            pw.WriteEnum(id);
            return pw;
        }

        /// <summary>
        /// When overridden in the derived class, gets the name for a character that will be displayed for the trade.
        /// </summary>
        /// <param name="character">The character to get the display name of.</param>
        /// <returns>The display name of the <paramref name="character"/>.</returns>
        protected abstract string GetCharDisplayName(TChar character);

        /// <summary>
        /// When overridden in the derived class, gets the information for an item.
        /// </summary>
        /// <param name="item">The item to get the information for.</param>
        protected abstract TItemInfo GetItemInfo(TItem item);

        /// <summary>
        /// Gets the current <see cref="IPeerTradeSession{TChar,TItem}"/> for a character.
        /// </summary>
        /// <param name="c">The character to get the current <see cref="IPeerTradeSession{TChar,TItem}"/> for.</param>
        /// <returns>The current <see cref="IPeerTradeSession{TChar,TItem}"/> for the <paramref name="c"/>, or null
        /// if <paramref name="c"/> is null or invalid.</returns>
        protected abstract IPeerTradeSession<TChar, TItem> GetTradeSession(TChar c);

        /// <summary>
        /// When overridden in the derived class, handles giving cash to a character.
        /// </summary>
        /// <param name="c">The character to give the cash to.</param>
        /// <param name="cash">The amount of cash to give the character.</param>
        protected abstract void GiveCashTo(TChar c, int cash);

        /// <summary>
        /// When overridden in the derived class, handles giving an item to a character.
        /// </summary>
        /// <param name="c">The character to give the item to.</param>
        /// <param name="item">The item to give the character.</param>
        protected abstract void GiveItemTo(TChar c, TItem item);

        /// <summary>
        /// Reads a stream of data sent from the client.
        /// </summary>
        /// <param name="source">The character the received data came from.</param>
        /// <param name="reader">The <see cref="BitStream"/> containing the data to read.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public void Read(TChar source, BitStream reader)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (reader == null)
                throw new ArgumentNullException("reader");

            var id = reader.ReadEnum<PeerTradeInfoClientMessage>();

            switch (id)
            {
                case PeerTradeInfoClientMessage.Accept:
                    ReadAccept(source);
                    break;

                case PeerTradeInfoClientMessage.AddInventoryItem:
                    ReadAddInventoryItem(source, reader);
                    break;

                case PeerTradeInfoClientMessage.Cancel:
                    ReadCancel(source);
                    break;

                case PeerTradeInfoClientMessage.RemoveInventoryItem:
                    ReadRemoveInventoryItem(source, reader);
                    break;

                case PeerTradeInfoClientMessage.AddCash:
                    ReadAddCash(source, reader);
                    break;

                case PeerTradeInfoClientMessage.RemoveCash:
                    ReadRemoveCash(source, reader);
                    break;

                default:
                    const string errmsg = "Invalid PeerTradeInfoClientMessage value `{0}`.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, id);
                    Debug.Fail(string.Format(errmsg, id));
                    break;
            }
        }

        /// <summary>
        /// Reads a <see cref="PeerTradeInfoClientMessage.Accept"/>.
        /// </summary>
        /// <param name="source">The character the received data came from.</param>
        void ReadAccept(TChar source)
        {
            var session = GetTradeSession(source);
            if (session == null)
                return;

            session.AcceptTrade(source);
        }

        /// <summary>
        /// Reads a <see cref="PeerTradeInfoClientMessage.AddCash"/>.
        /// </summary>
        /// <param name="source">The character the received data came from.</param>
        /// <param name="reader">The <see cref="BitStream"/> to read the data from.</param>
        void ReadAddCash(TChar source, BitStream reader)
        {
            var cash = reader.ReadInt();

            if (cash <= 0)
                return;

            var session = GetTradeSession(source);
            if (session == null)
                return;

            // Get the cash from the character
            var cashToAdd = TakeCash(source, cash);
            if (cashToAdd <= 0)
                return;

            // Add the cash
            if (!session.AddCash(source, cashToAdd))
            {
                // Couldn't add the cash, so give it back to the character
                GiveCashTo(source, cashToAdd);
            }
        }

        /// <summary>
        /// Reads a <see cref="PeerTradeInfoClientMessage.AddInventoryItem"/>.
        /// </summary>
        /// <param name="source">The character the received data came from.</param>
        /// <param name="reader">The <see cref="BitStream"/> to read the data from.</param>
        void ReadAddInventoryItem(TChar source, BitStream reader)
        {
            var slot = reader.ReadInventorySlot();
            var amount = reader.ReadByte();

            var session = GetTradeSession(source);
            if (session == null)
                return;

            // Get the inventory item
            var item = TakeInventoryItem(source, slot, amount);
            if (item == null)
                return;

            // Add the item to the trade
            var remainder = session.TryAddItem(source, item);

            // Handle any remainder by giving it back to the character
            if (remainder != null)
                GiveItemTo(source, remainder);
        }

        /// <summary>
        /// Reads a <see cref="PeerTradeInfoClientMessage.Cancel"/>.
        /// </summary>
        /// <param name="source">The character the received data came from.</param>
        void ReadCancel(TChar source)
        {
            var session = GetTradeSession(source);
            if (session == null)
                return;

            session.Cancel(source);
        }

        /// <summary>
        /// Reads a <see cref="PeerTradeInfoClientMessage.AddCash"/>.
        /// </summary>
        /// <param name="source">The character the received data came from.</param>
        /// <param name="reader">The <see cref="BitStream"/> to read the data from.</param>
        void ReadRemoveCash(TChar source, BitStream reader)
        {
            var cash = reader.ReadInt();

            if (cash <= 0)
                return;

            var session = GetTradeSession(source);
            if (session == null)
                return;

            // Remove the cash
            session.TryRemoveCash(source, cash);
        }

        /// <summary>
        /// Reads a <see cref="PeerTradeInfoClientMessage.RemoveInventoryItem"/>.
        /// </summary>
        /// <param name="source">The character the received data came from.</param>
        /// <param name="reader">The <see cref="BitStream"/> to read the data from.</param>
        void ReadRemoveInventoryItem(TChar source, BitStream reader)
        {
            var slot = reader.ReadInventorySlot();

            var session = GetTradeSession(source);
            if (session == null)
                return;

            session.TryRemoveItem(source, slot);
        }

        /// <summary>
        /// When overridden in the derived class, handles sending data to a character.
        /// </summary>
        /// <param name="receiver">The character to send the data to.</param>
        /// <param name="data">The data to send.</param>
        protected abstract void SendDataTo(TChar receiver, BitStream data);

        /// <summary>
        /// When overridden in the derived class, takes cash from the character.
        /// </summary>
        /// <param name="c">The character to take the cash from.</param>
        /// <param name="cash">The amount of cash to take.</param>
        /// <returns>The amount of cash that was taken, or less than or equal to zero if no cash was taken.</returns>
        protected abstract int TakeCash(TChar c, int cash);

        /// <summary>
        /// When overridden in the derived class, takes an item out of a characters inventory.
        /// </summary>
        /// <param name="c">The character to take the inventory item from.</param>
        /// <param name="slot">The slot of the inventory item to take.</param>
        /// <param name="amount">The amount of the item to take.</param>
        /// <returns>The character's inventory item at the given slot, or null if the slot is invalid or empty.</returns>
        protected abstract TItem TakeInventoryItem(TChar c, InventorySlot slot, byte amount);

        /// <summary>
        /// Handles writing the information about the acceptance of the trade status changing.
        /// </summary>
        /// <param name="peerTradeSession">The <see cref="IPeerTradeSession{TChar,TItem}"/>.</param>
        /// <param name="c">The character who's accept status has changed.</param>
        /// <param name="hasAccepted"><paramref name="c"/>'s accept status.</param>
        public void WriteAcceptStatusChanged(IPeerTradeSession<TChar, TItem> peerTradeSession, TChar c, bool hasAccepted)
        {
            if (peerTradeSession == null)
            {
                const string errmsg = "Parameter `peerTradeSession` cannot be null.";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                Debug.Fail(errmsg);
                return;
            }

            if (c == null)
            {
                const string errmsg = "Parameter `c` cannot be null.";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                Debug.Fail(errmsg);
                return;
            }

            if (c != peerTradeSession.CharSource && c != peerTradeSession.CharTarget)
            {
                const string errmsg = "Character `{0}` is not part of the trade session `{1}`!";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, c, peerTradeSession);
                Debug.Fail(string.Format(errmsg, c, peerTradeSession));
                return;
            }

            // Check who can receive the data
            var sendToSource = CanSendDataTo(peerTradeSession.CharSource);
            var sendToTarget = CanSendDataTo(peerTradeSession.CharTarget);

            if (!sendToSource && !sendToTarget)
                return;

            // Find out what side the change took place on
            var isSourceSide = (peerTradeSession.CharSource == c);

            // Construct and send the data
            using (var pw = CreateWriter(PeerTradeInfoServerMessage.UpdateAccepted))
            {
                // Build
                pw.Write(isSourceSide);
                pw.Write(hasAccepted);

                // Send
                if (sendToSource)
                    SendDataTo(peerTradeSession.CharSource, pw);

                if (sendToTarget)
                    SendDataTo(peerTradeSession.CharTarget, pw);
            }
        }

        /// <summary>
        /// Handles writing the information about the amount of cash on the table changing.
        /// </summary>
        /// <param name="peerTradeSession">The <see cref="IPeerTradeSession{TChar,TItem}"/>.</param>
        /// <param name="c">The character that owns the side of the trade table that changed.</param>
        /// <param name="cash">The amount of cash on <paramref name="c"/>'s side of the table.</param>
        public void WriteCashChanged(IPeerTradeSession<TChar, TItem> peerTradeSession, TChar c, int cash)
        {
            if (peerTradeSession == null)
            {
                const string errmsg = "Parameter `peerTradeSession` cannot be null.";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                Debug.Fail(errmsg);
                return;
            }

            if (c == null)
            {
                const string errmsg = "Parameter `c` cannot be null.";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                Debug.Fail(errmsg);
                return;
            }

            if (c != peerTradeSession.CharSource && c != peerTradeSession.CharTarget)
            {
                const string errmsg = "Character `{0}` is not part of the trade session `{1}`!";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, c, peerTradeSession);
                Debug.Fail(string.Format(errmsg, c, peerTradeSession));
                return;
            }

            // Check who can receive the data
            var sendToSource = CanSendDataTo(peerTradeSession.CharSource);
            var sendToTarget = CanSendDataTo(peerTradeSession.CharTarget);

            if (!sendToSource && !sendToTarget)
                return;

            // Find out what side the change took place on
            var isSourceSide = (peerTradeSession.CharSource == c);

            // Construct and send the data
            using (var pw = CreateWriter(PeerTradeInfoServerMessage.UpdateCash))
            {
                // Build
                pw.Write(isSourceSide);
                pw.Write(cash);

                // Send
                if (sendToSource)
                    SendDataTo(peerTradeSession.CharSource, pw);

                if (sendToTarget)
                    SendDataTo(peerTradeSession.CharTarget, pw);
            }
        }

        /// <summary>
        /// When overridden in the derived class, writes the information for an item.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write to.</param>
        /// <param name="itemInfo">The item info to write.</param>
        protected abstract void WriteItemInfo(IValueWriter writer, TItemInfo itemInfo);

        /// <summary>
        /// Handles writing the information about a trade being closed.
        /// </summary>
        /// <param name="peerTradeSession">The <see cref="IPeerTradeSession{TChar,TItem}"/>.</param>
        /// <param name="c">The character that canceled the trade.</param>
        public void WriteTradeCanceled(IPeerTradeSession<TChar, TItem> peerTradeSession, TChar c)
        {
            if (peerTradeSession == null)
            {
                const string errmsg = "Parameter `peerTradeSession` cannot be null.";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                Debug.Fail(errmsg);
                return;
            }

            if (c == null)
            {
                const string errmsg = "Parameter `c` cannot be null.";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                Debug.Fail(errmsg);
                return;
            }

            // Check who can receive the data
            var sendToSource = CanSendDataTo(peerTradeSession.CharSource);
            var sendToTarget = CanSendDataTo(peerTradeSession.CharTarget);

            if (!sendToSource && !sendToTarget)
                return;

            // Construct and send the data
            using (var pw = CreateWriter(PeerTradeInfoServerMessage.Canceled))
            {
                var sourceClosed = (c == peerTradeSession.CharSource);
                pw.Write(sourceClosed);

                if (sendToSource)
                    SendDataTo(peerTradeSession.CharSource, pw);

                if (sendToTarget)
                    SendDataTo(peerTradeSession.CharTarget, pw);
            }
        }

        /// <summary>
        /// Handles writing the information about a trade being closed.
        /// </summary>
        /// <param name="peerTradeSession">The <see cref="IPeerTradeSession{TChar,TItem}"/>.</param>
        public void WriteTradeClosed(IPeerTradeSession<TChar, TItem> peerTradeSession)
        {
            if (peerTradeSession == null)
            {
                const string errmsg = "Parameter `peerTradeSession` cannot be null.";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                Debug.Fail(errmsg);
                return;
            }

            // Check who can receive the data
            var sendToSource = CanSendDataTo(peerTradeSession.CharSource);
            var sendToTarget = CanSendDataTo(peerTradeSession.CharTarget);

            if (!sendToSource && !sendToTarget)
                return;

            // Construct and send the data
            using (var pw = CreateWriter(PeerTradeInfoServerMessage.Closed))
            {
                if (sendToSource)
                    SendDataTo(peerTradeSession.CharSource, pw);

                if (sendToTarget)
                    SendDataTo(peerTradeSession.CharTarget, pw);
            }
        }

        /// <summary>
        /// Handles writing the information about a trade being completed.
        /// </summary>
        /// <param name="peerTradeSession">The <see cref="IPeerTradeSession{TChar,TItem}"/>.</param>
        public void WriteTradeCompleted(IPeerTradeSession<TChar, TItem> peerTradeSession)
        {
            if (peerTradeSession == null)
            {
                const string errmsg = "Parameter `peerTradeSession` cannot be null.";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                Debug.Fail(errmsg);
                return;
            }

            // Check who can receive the data
            var sendToSource = CanSendDataTo(peerTradeSession.CharSource);
            var sendToTarget = CanSendDataTo(peerTradeSession.CharTarget);

            if (!sendToSource && !sendToTarget)
                return;

            // Construct and send the data
            using (var pw = CreateWriter(PeerTradeInfoServerMessage.Completed))
            {
                if (sendToSource)
                    SendDataTo(peerTradeSession.CharSource, pw);

                if (sendToTarget)
                    SendDataTo(peerTradeSession.CharTarget, pw);
            }
        }

        /// <summary>
        /// Handles writing the information for starting a trade session.
        /// </summary>
        /// <param name="peerTradeSession">The <see cref="IPeerTradeSession{TChar,TItem}"/>.</param>
        public void WriteTradeOpened(IPeerTradeSession<TChar, TItem> peerTradeSession)
        {
            if (peerTradeSession == null)
            {
                const string errmsg = "Parameter `peerTradeSession` cannot be null.";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                Debug.Fail(errmsg);
                return;
            }

            // Check who can receive the data
            var sendToSource = CanSendDataTo(peerTradeSession.CharSource);
            var sendToTarget = CanSendDataTo(peerTradeSession.CharTarget);

            // Construct and send the data
            if (sendToSource)
            {
                using (var pw = CreateWriter(PeerTradeInfoServerMessage.Open))
                {
                    const bool isClientSource = true;
                    var otherCharDisplayName = GetCharDisplayName(peerTradeSession.CharTarget);

                    pw.Write(isClientSource);
                    pw.Write(otherCharDisplayName);

                    SendDataTo(peerTradeSession.CharSource, pw);
                }
            }

            if (sendToTarget)
            {
                using (var pw = CreateWriter(PeerTradeInfoServerMessage.Open))
                {
                    const bool isClientSource = false;
                    var otherCharDisplayName = GetCharDisplayName(peerTradeSession.CharSource);

                    pw.Write(isClientSource);
                    pw.Write(otherCharDisplayName);

                    SendDataTo(peerTradeSession.CharTarget, pw);
                }
            }
        }

        /// <summary>
        /// Handles writing the information about a table slot changing.
        /// </summary>
        /// <param name="peerTradeSession">The <see cref="IPeerTradeSession{TChar,TItem}"/>.</param>
        /// <param name="c">The character that owns the side of the trade table that changed.</param>
        /// <param name="slot">The slot that changed.</param>
        /// <param name="item">The item that currently occupies the slot.</param>
        public void WriteTradeTableSlotChanged(IPeerTradeSession<TChar, TItem> peerTradeSession, TChar c, InventorySlot slot,
                                               TItem item)
        {
            if (peerTradeSession == null)
            {
                const string errmsg = "Parameter `peerTradeSession` cannot be null.";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                Debug.Fail(errmsg);
                return;
            }

            if (c == null)
            {
                const string errmsg = "Parameter `c` cannot be null.";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                Debug.Fail(errmsg);
                return;
            }

            if (c != peerTradeSession.CharSource && c != peerTradeSession.CharTarget)
            {
                const string errmsg = "Character `{0}` is not part of the trade session `{1}`!";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, c, peerTradeSession);
                Debug.Fail(string.Format(errmsg, c, peerTradeSession));
                return;
            }

            // Check who can receive the data
            var sendToSource = CanSendDataTo(peerTradeSession.CharSource);
            var sendToTarget = CanSendDataTo(peerTradeSession.CharTarget);

            if (!sendToSource && !sendToTarget)
                return;

            // Find out what side the change took place on
            var isSourceSide = (peerTradeSession.CharSource == c);

            // Get the item information
            var itemInfo = item == null ? null : GetItemInfo(item);

            // Construct and send the data
            using (var pw = CreateWriter(PeerTradeInfoServerMessage.UpdateSlot))
            {
                // Build
                pw.Write(isSourceSide);
                pw.Write(slot);
                pw.Write(itemInfo == null);

                if (itemInfo != null)
                    WriteItemInfo(pw, itemInfo);

                // Send
                if (sendToSource)
                    SendDataTo(peerTradeSession.CharSource, pw);

                if (sendToTarget)
                    SendDataTo(peerTradeSession.CharTarget, pw);
            }
        }
    }
}