using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Extensions;
using log4net;
using Microsoft.Xna.Framework.Net;
using NetGore.Network;

namespace DemoGame.Client
{
    /// <summary>
    /// A very simple client-side Inventory, used only by the User's character
    /// </summary>
    public class Inventory : InventoryBase
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        readonly ItemEntity[] _buffer = new ItemEntity[MaxInventorySize];

        readonly ISocketSender _socket;

        /// <summary>
        /// Gets an inventory item
        /// </summary>
        /// <param name="index">Index of the inventory item slot</param>
        /// <returns>Item in the specified inventory slot, or null if the slot is empty or invalid</returns>
        public ItemEntity this[int index]
        {
            get
            {
                // Check for a valid index
                if (index >= MaxInventorySize || index < 0)
                {
                    const string errmsg = "Tried to get invalid inventory slot `{0}`.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, index);
                    Debug.Fail(string.Format(errmsg, index));
                    return null;
                }

                // If the item has an amount of 0, return null
                ItemEntity item = _buffer[index];
                if (item != null && item.Amount == 0)
                    return null;

                // Item is either null or valid
                return _buffer[index];
            }

            private set
            {
                // Check for a valid index
                if (index >= MaxInventorySize || index < 0)
                {
                    const string errmsg = "Tried to set invalid inventory slot `{0}`.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, index);
                    Debug.Fail(string.Format(errmsg, index));
                    return;
                }

                _buffer[index] = value;
            }
        }

        public Inventory(ISocketSender socket)
        {
            if (socket == null)
                throw new ArgumentNullException("socket");

            _socket = socket;
        }

        public void Drop(int slot)
        {
            ItemEntity item = this[slot];
            if (item == null)
                return;

            using (PacketWriter pw = ClientPacket.DropInventoryItem((byte)slot))
            {
                _socket.Send(pw);
            }
        }

        /// <summary>
        /// Updates a slot in the Inventory
        /// </summary>
        /// <param name="slot">Slot to update</param>
        /// <param name="graphic">New graphic index</param>
        /// <param name="amount">New item amount</param>
        /// <param name="time">Current time</param>
        public void Update(byte slot, ushort graphic, byte amount, int time)
        {
            // If we get an amount of 0, just use UpdateEmpty()
            if (amount == 0)
            {
                UpdateEmpty(slot);
                return;
            }

            if (this[slot] == null)
            {
                // Add a new item
                this[slot] = new ItemEntity(graphic, amount, time);
            }
            else
            {
                // Update an existing item
                ItemEntity item = this[slot];
                item.GraphicIndex = graphic;
                item.Amount = amount;
            }
        }

        /// <summary>
        /// Updates a slot in the Invetory by setting it to an empty item
        /// </summary>
        public void UpdateEmpty(byte slot)
        {
            ItemEntity item = this[slot];
            if (item == null)
                return;

            // Only need to clear the item if one already exists in the slot
            item.GraphicIndex = 0;
            item.Amount = 0;
        }

        /// <summary>
        /// Uses an item in the inventory.
        /// </summary>
        /// <param name="slot">Slot of the item to use.</param>
        public void Use(int slot)
        {
            ItemEntity item = this[slot];
            if (item == null)
                return;

            using (PacketWriter pw = ClientPacket.UseInventoryItem((byte)slot))
            {
                _socket.Send(pw);
            }
        }
    }
}