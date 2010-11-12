using System.Linq;
using DemoGame.DbObjs;
using NetGore;
using NetGore.Features.PeerTrading;
using NetGore.IO;
using NetGore.Network;

namespace DemoGame.Server.PeerTrading
{
    public class ServerPeerTradeInfoHandler : ServerPeerTradeInfoHandlerBase<User, ItemEntity, IItemTable>
    {
        static readonly ServerPeerTradeInfoHandler _instance;

        /// <summary>
        /// A cached <see cref="ItemTable"/> instanced for being able to use <see cref="IPersistable"/> with an
        /// <see cref="IItemTable"/>.
        /// </summary>
        readonly ItemTable _itemTableInfoCache = new ItemTable();

        /// <summary>
        /// The locking object for the <see cref="_itemTableInfoCache"/>.
        /// </summary>
        readonly object _itemTableInfoCacheSync = new object();

        /// <summary>
        /// Initializes the <see cref="ServerPeerTradeInfoHandler"/> class.
        /// </summary>
        static ServerPeerTradeInfoHandler()
        {
            _instance = new ServerPeerTradeInfoHandler();
        }

        /// <summary>
        /// Gets the <see cref="ServerPeerTradeInfoHandler"/> instance.
        /// </summary>
        public static ServerPeerTradeInfoHandler Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// When overridden in the derived class, gets if a character can have data sent to it.
        /// </summary>
        /// <param name="receiver">The character to check if can have data sent to it.</param>
        /// <returns>True if <paramref name="receiver"/> can have data sent to it; otherwise false.</returns>
        protected override bool CanSendDataTo(User receiver)
        {
#pragma warning disable 183

            // Anything that implements IClientCommunicator can be communicated with
            return (receiver is INetworkSender);

#pragma warning restore 183
        }

        /// <summary>
        /// When overridden in the derived class, gets a <see cref="PacketWriter"/> to use to write data to.
        /// The created <see cref="PacketWriter"/> should also contain a header ID so you can recognize when messages
        /// are to/from peer trading handler.
        /// </summary>
        /// <returns>A <see cref="PacketWriter"/> to use to write data to.</returns>
        protected override PacketWriter CreateWriter()
        {
            return ServerPacket.GetWriter(ServerPacketID.PeerTradeEvent);
        }

        /// <summary>
        /// When overridden in the derived class, gets the name for a character that will be displayed for the trade.
        /// </summary>
        /// <param name="character">The character to get the display name of.</param>
        /// <returns>The display name of the <paramref name="character"/>.</returns>
        protected override string GetCharDisplayName(User character)
        {
            return character.Name;
        }

        /// <summary>
        /// When overridden in the derived class, gets the information for an item.
        /// </summary>
        /// <param name="item">The item to get the information for.</param>
        protected override IItemTable GetItemInfo(ItemEntity item)
        {
            return item;
        }

        /// <summary>
        /// Gets the current <see cref="IPeerTradeSession{TChar,TItem}"/> for a character.
        /// </summary>
        /// <param name="c">The character to get the current <see cref="IPeerTradeSession{TChar,TItem}"/> for.</param>
        /// <returns>The current <see cref="IPeerTradeSession{TChar,TItem}"/> for the <paramref name="c"/>, or null
        /// if <paramref name="c"/> is null or invalid.</returns>
        protected override IPeerTradeSession<User, ItemEntity> GetTradeSession(User c)
        {
            return c.PeerTradeSession;
        }

        /// <summary>
        /// When overridden in the derived class, handles giving cash to a character.
        /// </summary>
        /// <param name="c">The character to give the cash to.</param>
        /// <param name="cash">The amount of cash to give the character.</param>
        protected override void GiveCashTo(User c, int cash)
        {
            c.Cash += cash;
        }

        /// <summary>
        /// When overridden in the derived class, handles giving an item to a character.
        /// </summary>
        /// <param name="c">The character to give the item to.</param>
        /// <param name="item">The item to give the character.</param>
        protected override void GiveItemTo(User c, ItemEntity item)
        {
            // Give the character the item
            var remainder = c.TryGiveItem(item);

            // Throw any remainder on the ground (not much else we can do with it)
            c.DropItem(remainder);
        }

        /// <summary>
        /// When overridden in the derived class, handles sending data to a character.
        /// </summary>
        /// <param name="receiver">The character to send the data to.</param>
        /// <param name="data">The data to send.</param>
        protected override void SendDataTo(User receiver, BitStream data)
        {
            // Anything that implements IClientCommunicator can be communicated with
            var asClientCommunicator = receiver as INetworkSender;
            if (asClientCommunicator != null)
                asClientCommunicator.Send(data, ServerMessageType.GUI);
        }

        /// <summary>
        /// When overridden in the derived class, takes cash from the character.
        /// </summary>
        /// <param name="c">The character to take the cash from.</param>
        /// <param name="cash">The amount of cash to take.</param>
        /// <returns>The amount of cash that was taken, or less than or equal to zero if no cash was taken.</returns>
        protected override int TakeCash(User c, int cash)
        {
            // Check if we have enough cash available
            if (c.Cash < cash)
                return 0;

            c.Cash -= cash;
            return cash;
        }

        /// <summary>
        /// When overridden in the derived class, takes an item out of a characters inventory.
        /// </summary>
        /// <param name="c">The character to take the inventory item from.</param>
        /// <param name="slot">The slot of the inventory item to take.</param>
        /// <param name="amount">The amount.</param>
        /// <returns>
        /// The character's inventory item at the given slot, or null if the slot is invalid or empty.
        /// </returns>
        protected override ItemEntity TakeInventoryItem(User c, InventorySlot slot, byte amount)
        {
            // Get the item reference
            var item = c.Inventory[slot];

            // Check exactly how much of the item we are taking
            if (amount >= item.Amount)
            {
                // If we take all of the item, remove it from the inventory
                c.Inventory.RemoveAt(slot, false);
            }
            else
            {
                // Otherwise, only take a portion of the item
                item = item.Split(amount);
            }

            return item;
        }

        /// <summary>
        /// When overridden in the derived class, writes the information for an item.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write to.</param>
        /// <param name="itemInfo">The item info to write.</param>
        protected override void WriteItemInfo(IValueWriter writer, IItemTable itemInfo)
        {
            var asPersistable = itemInfo as IPersistable;

            if (asPersistable != null)
            {
                // Implements IPersistable, so can easily write the state
                asPersistable.WriteState(writer);
            }
            else
            {
                // Doesn't implement IPersistable, so we have to create an ItemTable which is able to load the values from an IItemTable
                // and does implement IPersistable. To avoid creating garbage, we cache an ItemTable to use instead of creating
                // a new instance every time. Of course, this also means we need thread safety.
                // This operation is quite fast (just lots of copying values around) so it would probably be of greater cost to try
                // and allow parallization by using multiple ItemTable caches.
                lock (_itemTableInfoCacheSync)
                {
                    _itemTableInfoCache.CopyValuesFrom(itemInfo);
                    _itemTableInfoCache.WriteState(writer);
                }
            }
        }
    }
}