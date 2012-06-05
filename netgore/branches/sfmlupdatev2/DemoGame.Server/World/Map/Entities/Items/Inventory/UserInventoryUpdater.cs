using System;
using System.Collections;
using System.Linq;
using NetGore;
using NetGore.Network;

namespace DemoGame.Server
{
    /// <summary>
    /// Handles tracking which slots of the UserInventory have changed, along with notifying the User of the
    /// new information for their inventory items.
    /// </summary>
    class UserInventoryUpdater
    {
        readonly BitArray _slotChanged = new BitArray(GameData.MaxInventorySize);

        readonly UserInventory _userInventory;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInventoryUpdater"/> class.
        /// </summary>
        /// <param name="userInventory"><see cref="UserInventory"/> that this UserInventoryUpdater will manage.</param>
        /// <exception cref="ArgumentNullException"><paramref name="userInventory"/> is <c>null</c>.</exception>
        public UserInventoryUpdater(UserInventory userInventory)
        {
            if (userInventory == null)
                throw new ArgumentNullException("userInventory");

            _userInventory = userInventory;
        }

        /// <summary>
        /// Gets the User to send the updates to.
        /// </summary>
        User OwnerUser
        {
            get { return UserInventory.Character as User; }
        }

        /// <summary>
        /// Gets the UserInventory that this UserInventoryUpdater manages.
        /// </summary>
        public UserInventory UserInventory
        {
            get { return _userInventory; }
        }

        /// <summary>
        /// Lets the UserInventoryUpdater know that an Inventory slot has changed and needs to be updated.
        /// </summary>
        /// <param name="slot">Slot that changed.</param>
        public void SlotChanged(InventorySlot slot)
        {
            _slotChanged[(int)slot] = true;
        }

        /// <summary>
        /// Updates the client with the changes that have been made to the UserInventory.
        /// </summary>
        public void Update()
        {
            // Don't actually grab the PacketWriter from the pool until we know we will need it
            PacketWriter pw = null;

            try
            {
                // Loop through all slots
                for (var slot = 0; slot < GameData.MaxInventorySize; slot++)
                {
                    // Skip unchanged slots
                    if (!_slotChanged[slot])
                        continue;

                    // Get the item in the slot
                    var invSlot = new InventorySlot(slot);
                    var item = UserInventory[invSlot];

                    // Get the values to send, which depends on if the slot is empty (item == null) or not
                    GrhIndex sendItemGraphic;
                    byte sendItemAmount;

                    if (item == null)
                    {
                        sendItemGraphic = GrhIndex.Invalid;
                        sendItemAmount = 0;
                    }
                    else
                    {
                        sendItemGraphic = item.GraphicIndex;
                        sendItemAmount = item.Amount;
                    }

                    // Grab the PacketWriter if we have not already, or clear it if we have
                    if (pw == null)
                        pw = ServerPacket.GetWriter();
                    else
                        pw.Reset();

                    // Pack the data and send it
                    ServerPacket.SetInventorySlot(pw, invSlot, sendItemGraphic, sendItemAmount);
                    OwnerUser.Send(pw, ServerMessageType.GUIItems);
                }
            }
            finally
            {
                // If we grabbed a PacketWriter, make sure we dispose of it!
                if (pw != null)
                    pw.Dispose();
            }

            // Changes complete
            _slotChanged.SetAll(false);
        }
    }
}