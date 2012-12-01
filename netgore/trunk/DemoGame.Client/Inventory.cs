using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore;
using NetGore.Graphics.GUI;
using NetGore.Network;

namespace DemoGame.Client
{
    /// <summary>
    /// A very simple client-side Inventory, used only by the client's character.
    /// </summary>
    public class Inventory
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        readonly ItemEntity[] _buffer = new ItemEntity[GameData.MaxInventorySize];

        readonly INetworkSender _socket;

        /// <summary>
        /// Initializes a new instance of the <see cref="Inventory"/> class.
        /// </summary>
        /// <param name="socket">The <see cref="INetworkSender"/> to use to communicate with the server.</param>
        /// <exception cref="ArgumentNullException"><paramref name="socket" /> is <c>null</c>.</exception>
        public Inventory(INetworkSender socket)
        {
            if (socket == null)
                throw new ArgumentNullException("socket");

            _socket = socket;
        }

        /// <summary>
        /// Clears out all the inventory slots.
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < GameData.MaxInventorySize; i++)
            {
                UpdateEmpty((InventorySlot)i);
            }
        }

        /// <summary>
        /// Gets an inventory item.
        /// </summary>
        /// <param name="slot">Index of the inventory item slot.</param>
        /// <returns>Item in the specified inventory slot, or null if the slot is empty or invalid</returns>
        public ItemEntity this[InventorySlot slot]
        {
            get
            {
                // Check for a valid index
                if (!slot.IsLegalValue())
                {
                    const string errmsg = "Tried to get invalid inventory slot `{0}`.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, slot);
                    Debug.Fail(string.Format(errmsg, slot));
                    return null;
                }

                // If the item has an amount of 0, return null
                var item = _buffer[(int)slot];
                if (item != null && item.Amount == 0)
                    return null;

                // Item is either null or valid
                return _buffer[(int)slot];
            }

            private set
            {
                // Check for a valid index
                if (!slot.IsLegalValue())
                {
                    const string errmsg = "Tried to set invalid inventory slot `{0}`.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, slot);
                    Debug.Fail(string.Format(errmsg, slot));
                    return;
                }

                _buffer[(int)slot] = value;
            }
        }

        /// <summary>
        /// Drops an item from the inventory onto the ground. If the user has just one item in the given <paramref name="slot"/>,
        /// then it is dropped. If they have multiple items in the <paramref name="slot"/>, then they are presented with an
        /// <see cref="InputBox"/> so they can enter in how much to drop.
        /// </summary>
        /// <param name="slot">The slot of the item to drop.</param>
        /// <param name="guiManager">The <see cref="IGUIManager"/> to use to create the <see cref="InputBox"/> if it is needed.</param>
        public void Drop(InventorySlot slot, IGUIManager guiManager)
        {
            // Check for a valid item
            var item = this[slot];
            if (item == null)
                return;

            // Check the amount
            if (item.Amount > 1)
            {
                // Create an InputBox to ask how much to drop
                const string text = "Drop item";
                const string message = "How much of the item do you wish to drop?\n(Enter a value from 1 to {0})";

                var inBox = InputBox.CreateNumericInputBox(guiManager, text, string.Format(message, item.Amount));
                inBox.Tag = slot;
                inBox.OptionSelected += DropInputBox_OptionSelected;
            }
            else
            {
                // Auto-drop if there is just one of the item
                Drop(slot, 1);
            }
        }

        /// <summary>
        /// Drops an item from the inventory onto the ground.
        /// </summary>
        /// <param name="slot">The slot of the item to drop.</param>
        /// <param name="amount">The amount of the item in the slot to drop.</param>
        public void Drop(InventorySlot slot, byte amount)
        {
            // Check for a valid item
            var item = this[slot];
            if (item == null)
                return;

            // Drop
            using (var pw = ClientPacket.DropInventoryItem(slot, amount))
            {
                _socket.Send(pw, ClientMessageType.GUIItems);
            }
        }

        /// <summary>
        /// Handles the OptionSelected event of the DropInputBox, which is the <see cref="InputBox"/> created to
        /// let the user specify how much of the item they want to drop.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="NetGore.EventArgs{MessageBoxButton}"/> instance containing the event data.</param>
        void DropInputBox_OptionSelected(Control sender, EventArgs<MessageBoxButton> args)
        {
            var slot = (InventorySlot)sender.Tag;
            var inBox = (InputBox)sender;

            byte amount;
            if (!byte.TryParse(inBox.InputText, out amount))
                return;

            Drop(slot, amount);
        }

        /// <summary>
        /// Sells an inventory item to the currently open shop.
        /// </summary>
        /// <param name="slot">The slot of the item to sell.</param>
        /// <param name="guiManager">The <see cref="IGUIManager"/> to use to create the <see cref="InputBox"/> if it is needed.</param>
        public void SellToShop(InventorySlot slot, IGUIManager guiManager)
        {
            // Check for a valid item
            var item = this[slot];
            if (item == null)
                return;

            // Check the amount
            if (item.Amount > 1)
            {
                // Create an InputBox to ask how much to drop
                const string text = "Sell item";
                const string message = "How much of the item do you wish to sell?\n(Enter a value from 1 to {0})";

                var inBox = InputBox.CreateNumericInputBox(guiManager, text, string.Format(message, item.Amount));
                inBox.Tag = slot;
                inBox.OptionSelected += SellToShopInputBox_OptionSelected;
            }
            else
            {
                // Auto-drop if there is just one of the item
                SellToShop(slot, 1);
            }
        }

        /// <summary>
        /// Sells an inventory item to the currently open shop.
        /// </summary>
        /// <param name="slot">The slot of the item to sell.</param>
        /// <param name="amount">The amount of the item in the slot to sell.</param>
        public void SellToShop(InventorySlot slot, byte amount)
        {
            // Check for a valid item
            var item = this[slot];
            if (item == null)
                return;

            using (var pw = ClientPacket.SellInventoryToShop(slot, amount))
            {
                _socket.Send(pw, ClientMessageType.GUIItems);
            }
        }

        /// <summary>
        /// Handles the OptionSelected event of the SellToShopInputBox, which is the <see cref="InputBox"/> created to
        /// let the user specify how much of the item they want to sell to a shop.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{MessageBoxButton}"/> instance containing the event data.</param>
        void SellToShopInputBox_OptionSelected(Control sender, EventArgs<MessageBoxButton> e)
        {
            var slot = (InventorySlot)sender.Tag;
            var inBox = (InputBox)sender;

            byte amount;
            if (!byte.TryParse(inBox.InputText, out amount))
                return;

            SellToShop(slot, amount);
        }

        /// <summary>
        /// Updates a slot in the Inventory
        /// </summary>
        /// <param name="slot">Slot to update</param>
        /// <param name="graphic">New graphic index</param>
        /// <param name="amount">New item amount</param>
        /// <param name="time">Current time</param>
        public void Update(InventorySlot slot, GrhIndex graphic, byte amount, TickCount time)
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
                var item = this[slot];
                item.GraphicIndex = graphic;
                item.Amount = amount;
            }
        }

        /// <summary>
        /// Updates a slot in the Invetory by setting it to an empty item
        /// </summary>
        public void UpdateEmpty(InventorySlot slot)
        {
            var item = this[slot];
            if (item == null)
                return;

            // Only need to clear the item if one already exists in the slot
            item.GraphicIndex = GrhIndex.Invalid;
            item.Amount = 0;
        }

        /// <summary>
        /// Uses an item in the inventory.
        /// </summary>
        /// <param name="slot">Slot of the item to use.</param>
        public void Use(InventorySlot slot)
        {
            var item = this[slot];
            if (item == null)
                return;

            using (var pw = ClientPacket.UseInventoryItem(slot))
            {
                _socket.Send(pw, ClientMessageType.GUIItems);
            }
        }
    }
}