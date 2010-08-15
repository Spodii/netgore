﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.IO;
using NetGore.Network;
using NetGore.World;

namespace NetGore.Features.PeerTrading
{
    /// <summary>
    /// Handles the networking for messages for PeerTrading on the client side.
    /// </summary>
    /// <typeparam name="TChar">The type of character.</typeparam>
    /// <typeparam name="TItem">The type of item.</typeparam>
    /// <typeparam name="TItemInfo">The type describing item information.</typeparam>
    public abstract class ClientPeerTradeInfoHandlerBase<TChar, TItem, TItemInfo> where TChar : Entity where TItem : Entity
                                                                                  where TItemInfo : class
    {
        /// <summary>
        /// Delegate for handling events on the <see cref="ClientPeerTradeInfoHandlerBase{TChar,TItem,TItemInfo}"/>.
        /// </summary>
        /// <param name="sender">The <see cref="ClientPeerTradeInfoHandlerBase{TChar,TItem,TItemInfo}"/> that the event came from.</param>
        /// <param name="isSourceSide">If true, the changed was on the source character's side.
        /// Otherwise, it was on the target character's side.</param>
        /// <param name="hasAccepted">If true, the status changed to accepted. If false, the status changed to not accepted.</param>
        public delegate void AcceptStatusChangedEventHandler(
            ClientPeerTradeInfoHandlerBase<TChar, TItem, TItemInfo> sender, bool isSourceSide, bool hasAccepted);

        /// <summary>
        /// Delegate for handling events on the <see cref="ClientPeerTradeInfoHandlerBase{TChar,TItem,TItemInfo}"/>.
        /// </summary>
        /// <param name="sender">The <see cref="ClientPeerTradeInfoHandlerBase{TChar,TItem,TItemInfo}"/> that the event came from.</param>
        public delegate void EventHandler(ClientPeerTradeInfoHandlerBase<TChar, TItem, TItemInfo> sender);

        /// <summary>
        /// Delegate for handling events on the <see cref="ClientPeerTradeInfoHandlerBase{TChar,TItem,TItemInfo}"/>.
        /// </summary>
        /// <param name="sender">The <see cref="ClientPeerTradeInfoHandlerBase{TChar,TItem,TItemInfo}"/> that the event came from.</param>
        /// <param name="slot">The slot that changed.</param>
        /// <param name="isSourceSide">If true, the changed slot was on the source character's side.
        /// Otherwise, it was on the target character's side.</param>
        public delegate void SlotUpdatedEventHandler(
            ClientPeerTradeInfoHandlerBase<TChar, TItem, TItemInfo> sender, InventorySlot slot, bool isSourceSide);

        /// <summary>
        /// Delegate for handling events on the <see cref="ClientPeerTradeInfoHandlerBase{TChar,TItem,TItemInfo}"/>.
        /// </summary>
        /// <param name="sender">The <see cref="ClientPeerTradeInfoHandlerBase{TChar,TItem,TItemInfo}"/> that the event came from.</param>
        /// <param name="sourceCanceled">If it was the source character who canceled the trade.</param>
        public delegate void TradeCanceledEventHandler(
            ClientPeerTradeInfoHandlerBase<TChar, TItem, TItemInfo> sender, bool sourceCanceled);

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static readonly int _numSlots = PeerTradingSettings.Instance.MaxTradeSlots;

        readonly TItemInfo[] _sourceSlots;
        readonly TItemInfo[] _targetSlots;

        bool _hasSourceAccepted;
        bool _hasTargetAccepted;
        bool _isTradeOpen;
        bool _userIsSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientPeerTradeInfoHandlerBase{TChar,TItem,TItemInfo}"/> class.
        /// </summary>
        protected ClientPeerTradeInfoHandlerBase()
        {
            _sourceSlots = new TItemInfo[_numSlots];
            _targetSlots = new TItemInfo[_numSlots];

            Debug.Assert(_sourceSlots.Length == _targetSlots.Length);
        }

        /// <summary>
        /// Notifies listeners when the trade accepting status has changed for one of the characters.
        /// </summary>
        public event AcceptStatusChangedEventHandler AcceptStatusChanged;

        /// <summary>
        /// Notifies listeners when a slot on the trade table has been updated.
        /// </summary>
        public event SlotUpdatedEventHandler SlotUpdated;

        /// <summary>
        /// Notifies listeners when a trade session has been canceled.
        /// </summary>
        public event TradeCanceledEventHandler TradeCanceled;

        /// <summary>
        /// Notifies listeners when a trade session has been closed (either completed or canceled).
        /// </summary>
        public event EventHandler TradeClosed;

        /// <summary>
        /// Notifies listeners when a trade session has been completed successfully (the trade was not canceled).
        /// </summary>
        public event EventHandler TradeCompleted;

        /// <summary>
        /// Notifies listeners when a new trade session has opened up.
        /// </summary>
        public event EventHandler TradeOpened;

        /// <summary>
        /// Gets if the source side has accepted the trade.
        /// </summary>
        public bool HasSourceAccepted
        {
            get { return _hasSourceAccepted; }
            private set
            {
                if (_hasSourceAccepted == value)
                    return;

                _hasSourceAccepted = value;

                OnAcceptStatusChanged(true, _hasSourceAccepted);
                if (AcceptStatusChanged != null)
                    AcceptStatusChanged(this, true, _hasTargetAccepted);
            }
        }

        /// <summary>
        /// Gets if the target side has accepted the trade.
        /// </summary>
        public bool HasTargetAccepted
        {
            get { return _hasTargetAccepted; }
            private set
            {
                if (_hasTargetAccepted == value)
                    return;

                _hasTargetAccepted = value;

                OnAcceptStatusChanged(false, _hasTargetAccepted);
                if (AcceptStatusChanged != null)
                    AcceptStatusChanged(this, false, _hasTargetAccepted);
            }
        }

        /// <summary>
        /// Gets if the client user has accepted the trade (returns the value of the corresponding
        /// <see cref="HasSourceAccepted"/> or <see cref="HasTargetAccepted"/> property).
        /// </summary>
        public bool HasUserAccepted
        {
            get { return (UserIsSource ? HasSourceAccepted : HasTargetAccepted); }
        }

        /// <summary>
        /// Gets if a trade is currently open and in progress.
        /// </summary>
        public bool IsTradeOpen
        {
            get { return _isTradeOpen; }
        }

        /// <summary>
        /// Gets if the client user is the source of this trade. If false, they are the target. Only valid when a trade session is open.
        /// </summary>
        public bool UserIsSource
        {
            get { return _userIsSource; }
        }

        /// <summary>
        /// When overridden in the derived class, gets a <see cref="PacketWriter"/> to use to write data to.
        /// The created <see cref="PacketWriter"/> should also contain a header ID so you can recognize when messages
        /// are to/from peer trading handler.
        /// </summary>
        /// <returns>A <see cref="PacketWriter"/> to use to write data to.</returns>
        protected abstract PacketWriter CreateWriter();

        /// <summary>
        /// When overridden in the derived class, gets a <see cref="PacketWriter"/> to use to write data to.
        /// The created <see cref="PacketWriter"/> should also contain a header ID so you can recognize when messages
        /// are to/from peer trading handler.
        /// </summary>
        /// <param name="id">The message ID.</param>
        /// <returns>A <see cref="PacketWriter"/> to use to write data to.</returns>
        PacketWriter CreateWriter(PeerTradeInfoClientMessage id)
        {
            var pw = CreateWriter();
            pw.WriteEnum(id);
            return pw;
        }

        /// <summary>
        /// Gets the item information slots for a side.
        /// </summary>
        /// <param name="sourceSide">If true, get the source side. Otherwise, get the target side.</param>
        /// <returns>The item information slots for the given side.</returns>
        TItemInfo[] GetSlots(bool sourceSide)
        {
            if (sourceSide)
                return _sourceSlots;
            return _targetSlots;
        }

        /// <summary>
        /// Gets the item information from the source character side of the trade.
        /// </summary>
        /// <param name="slot">The slot to get the item information for.</param>
        /// <returns>The item information for the item in the <paramref name="slot"/>, or null if the
        /// slot is empty or invalid.</returns>
        public TItemInfo GetSourceItemInfo(InventorySlot slot)
        {
            if (slot < 0 || slot >= _sourceSlots.Length)
                return null;

            return _sourceSlots[(int)slot];
        }

        /// <summary>
        /// Gets the item information from the target character side of the trade.
        /// </summary>
        /// <param name="slot">The slot to get the item information for.</param>
        /// <returns>The item information for the item in the <paramref name="slot"/>, or null if the
        /// slot is empty or invalid.</returns>
        public TItemInfo GetTargetItemInfo(InventorySlot slot)
        {
            if (slot < 0 || slot >= _targetSlots.Length)
                return null;

            return _targetSlots[(int)slot];
        }

        /// <summary>
        /// Gets the item information from the client user's side of the trade.
        /// </summary>
        /// <param name="slot">The slot to get the item information for.</param>
        /// <returns>The item information for the item in the <paramref name="slot"/>, or null if the
        /// slot is empty or invalid.</returns>
        public TItemInfo GetUserItemInfo(InventorySlot slot)
        {
            return UserIsSource ? GetSourceItemInfo(slot) : GetTargetItemInfo(slot);
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling the <see cref="AcceptStatusChanged"/> event.
        /// </summary>
        /// <param name="isSourceSide">If true, the change was on the source character's side.
        /// Otherwise, it was on the target character's side.</param>
        /// <param name="hasAccepted">If true, the status changed to accepted. If false, the status changed to not accepted.</param>
        protected virtual void OnAcceptStatusChanged(bool isSourceSide, bool hasAccepted)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling the <see cref="SlotUpdated"/> event.
        /// </summary>
        /// <param name="slot">The slot that changed.</param>
        /// <param name="isSourceSide">If true, the changed slot was on the source character's side.
        /// Otherwise, it was on the target character's side.</param>
        protected virtual void OnSlotUpdated(InventorySlot slot, bool isSourceSide)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling the <see cref="TradeCanceled"/> event.
        /// </summary>
        /// <param name="sourceCanceled">If it was the source character who canceled the trade.</param>
        protected virtual void OnTradeCanceled(bool sourceCanceled)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling the <see cref="TradeClosed"/> event.
        /// </summary>
        protected virtual void OnTradeClosed()
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling the <see cref="TradeCompleted"/> event.
        /// </summary>
        protected virtual void OnTradeCompleted()
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling the <see cref="TradeOpened"/> event.
        /// </summary>
        protected virtual void OnTradeOpened()
        {
        }

        /// <summary>
        /// Reads a stream of data sent from the server.
        /// </summary>
        /// <param name="reader">The <see cref="BitStream"/> containing the data to read.</param>
        public void Read(BitStream reader)
        {
            var id = reader.ReadEnum<PeerTradeInfoServerMessage>();

            switch (id)
            {
                case PeerTradeInfoServerMessage.Canceled:
                    ReadCanceled(reader);
                    break;

                case PeerTradeInfoServerMessage.Closed:
                    ReadClosed();
                    break;

                case PeerTradeInfoServerMessage.Completed:
                    ReadCompleted();
                    break;

                case PeerTradeInfoServerMessage.Open:
                    ReadOpen(reader);
                    break;

                case PeerTradeInfoServerMessage.UpdateAccepted:
                    ReadUpdateAccepted(reader);
                    break;

                case PeerTradeInfoServerMessage.UpdateSlot:
                    ReadUpdateSlot(reader);
                    break;

                default:
                    const string errmsg = "Invalid PeerTradeInfoServerMessage value `{0}`.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, id);
                    Debug.Fail(string.Format(errmsg, id));
                    break;
            }
        }

        /// <summary>
        /// Reads a <see cref="PeerTradeInfoServerMessage.Canceled"/>.
        /// </summary>
        /// <param name="reader">The <see cref="BitStream"/> containing the data to read.</param>
        void ReadCanceled(BitStream reader)
        {
            Debug.Assert(IsTradeOpen, "Why is the trade table not open...?");

            var sourceCanceled = reader.ReadBool();

            // Raise events
            OnTradeCanceled(sourceCanceled);
            if (TradeCanceled != null)
                TradeCanceled(this, sourceCanceled);
        }

        /// <summary>
        /// Reads a <see cref="PeerTradeInfoServerMessage.Closed"/>.
        /// </summary>
        void ReadClosed()
        {
            Debug.Assert(IsTradeOpen, "Why is the trade table not open...?");

            // Clean out old values
            ResetValues();

            // Raise events
            OnTradeClosed();
            if (TradeClosed != null)
                TradeClosed(this);
        }

        /// <summary>
        /// Reads a <see cref="PeerTradeInfoServerMessage.Completed"/>.
        /// </summary>
        void ReadCompleted()
        {
            Debug.Assert(IsTradeOpen, "Why is the trade table not open...?");

            // Raise events
            OnTradeCompleted();
            if (TradeCompleted != null)
                TradeCompleted(this);
        }

        /// <summary>
        /// When overridden in the derived class, reads the information for an item.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read from.</param>
        /// <returns>The read item information.</returns>
        protected abstract TItemInfo ReadItemInfo(IValueReader reader);

        string _otherCharName = string.Empty;

        /// <summary>
        /// Gets the name of the character that the client is trading with. This is always the name of the other character in the
        /// trade, never the name of the client's character.
        /// </summary>
        public string OtherCharName { get { return _otherCharName; } }

        /// <summary>
        /// Reads a <see cref="PeerTradeInfoServerMessage.Open"/>.
        /// </summary>
        /// <param name="reader">The <see cref="BitStream"/> containing the data to read.</param>
        void ReadOpen(BitStream reader)
        {
            Debug.Assert(!IsTradeOpen, "Why is the trade table already open...?");

            // Clean out old values
            ResetValues();

            // Read in the new values and start the trade
            _userIsSource = reader.ReadBool();
            _otherCharName = reader.ReadString();

            _isTradeOpen = true;

            // Raise events
            OnTradeOpened();
            if (TradeOpened != null)
                TradeOpened(this);
        }

        /// <summary>
        /// Reads a <see cref="PeerTradeInfoServerMessage.UpdateAccepted"/>.
        /// </summary>
        /// <param name="reader">The <see cref="BitStream"/> containing the data to read.</param>
        void ReadUpdateAccepted(BitStream reader)
        {
            var isSourceSide = reader.ReadBool();
            var acceptStatus = reader.ReadBool();

            if (isSourceSide)
                HasSourceAccepted = acceptStatus;
            else
                HasTargetAccepted = acceptStatus;
        }

        /// <summary>
        /// Reads a <see cref="PeerTradeInfoServerMessage.UpdateSlot"/>.
        /// </summary>
        /// <param name="reader">The <see cref="BitStream"/> containing the data to read.</param>
        void ReadUpdateSlot(BitStream reader)
        {
            var isSourceSide = reader.ReadBool();
            var slot = reader.ReadInventorySlot();
            var isSlotEmpty = reader.ReadBool();
            var itemInfo = isSlotEmpty ? null : ReadItemInfo(reader);

            var slots = GetSlots(isSourceSide);

            // Validate the slot value
            if (slot < 0 || slot >= slots.Length)
            {
                const string errmsg = "Received slot value `{0}` is out of range.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, slot);
                Debug.Fail(string.Format(errmsg, slot));
                return;
            }

            // Set the item info
            slots[(int)slot] = itemInfo;

            OnSlotUpdated(slot, isSourceSide);
            if (SlotUpdated != null)
                SlotUpdated(this, slot, isSourceSide);
        }

        /// <summary>
        /// Resets the state of the trade table.
        /// </summary>
        void ResetValues()
        {
            _isTradeOpen = false;
            _userIsSource = false;
            _otherCharName = string.Empty;

            Array.Clear(_sourceSlots, 0, _sourceSlots.Length);
            Array.Clear(_targetSlots, 0, _targetSlots.Length);

            _hasSourceAccepted = false;
            _hasTargetAccepted = false;
        }

        /// <summary>
        /// When overridden in the derived class, handles sending data to the server.
        /// </summary>
        /// <param name="bs">The data to send.</param>
        protected abstract void SendData(BitStream bs);

        /// <summary>
        /// Sends a notification to the server that the client wants to accept the trade.
        /// </summary>
        public void WriteAccept()
        {
            if (!IsTradeOpen)
                return;

            using (var pw = CreateWriter(PeerTradeInfoClientMessage.Accept))
            {
                SendData(pw);
            }
        }

        /// <summary>
        /// Sends a notification to the server that the client wants to add an inventory item to the trade.
        /// </summary>
        /// <param name="slot">The slot containing the item to add.</param>
        public void WriteAddInventoryItem(InventorySlot slot)
        {
            if (!IsTradeOpen)
                return;

            using (var pw = CreateWriter(PeerTradeInfoClientMessage.AddInventoryItem))
            {
                pw.Write(slot);
                SendData(pw);
            }
        }

        /// <summary>
        /// Sends a notification to the server that the client wants to cancel the trade.
        /// </summary>
        public void WriteCancel()
        {
            if (!IsTradeOpen)
                return;

            using (var pw = CreateWriter(PeerTradeInfoClientMessage.Cancel))
            {
                SendData(pw);
            }
        }

        /// <summary>
        /// Sends a notification to the server that the client wants to remove an item from the trade.
        /// </summary>
        /// <param name="slot">The slot containing the item to remove.</param>
        public void WriteRemoveItem(InventorySlot slot)
        {
            if (!IsTradeOpen)
                return;

            using (var pw = CreateWriter(PeerTradeInfoClientMessage.RemoveItem))
            {
                pw.Write(slot);
                SendData(pw);
            }
        }
    }
}