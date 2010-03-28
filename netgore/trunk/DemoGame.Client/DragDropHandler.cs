using System.Diagnostics;
using System.Linq;
using NetGore.Graphics;
using NetGore.Graphics.GUI;

namespace DemoGame.Client
{
    /// <summary>
    /// Handles the drag-and-drop support for all of the <see cref="IDragDropProvider"/>s on
    /// <see cref="Control"/>s. While this could done through the controls directly, it is done
    /// here to make it easier to keep track of all the drop-and-drop connections.
    /// </summary>
    class DragDropHandler
    {
        readonly GameplayScreen _gps;

        /// <summary>
        /// Initializes a new instance of the <see cref="DragDropHandler"/> class.
        /// </summary>
        /// <param name="gps">The <see cref="GameplayScreen"/>.</param>
        public DragDropHandler(GameplayScreen gps)
        {
            _gps = gps;
        }

        /// <summary>
        /// Gets if the specified <see cref="IDragDropProvider"/> can be dropped on this <see cref="IDragDropProvider"/>.
        /// </summary>
        /// <param name="src">The <see cref="IDragDropProvider"/> that was dragged.</param>
        /// <param name="dest">The <see cref="IDragDropProvider"/> that that <paramref name="src"/> was dropped onto.</param>
        /// <returns>True if the <paramref name="src"/> can be dropped on this <see cref="IDragDropProvider"/>;
        /// otherwise false.</returns>
        public bool CanDrop(IDragDropProvider src, IDragDropProvider dest)
        {
            // Check for valid parameters
            if (src == null || dest == null)
            {
                Debug.Fail("Shouldn't ever be passed a null argument.");
                return false;
            }

            // Check if any of our implementations are supported

            // TODO: Delegates + reflection
            if (CanDrop_EquippedItemToInventory(src, dest))
                return true;
            if (CanDrop_InventoryItemToShop(src, dest))
                return true;
            if (CanDrop_InventorySlotToInventorySlot(src, dest))
                return true;
            if (CanDrop_InventoryItemToEquipped(src, dest))
                return true;

            return false;
        }

        bool CanDrop_EquippedItemToInventory(IDragDropProvider srcDDP, IDragDropProvider destDDP)
        {
            var src = srcDDP as EquippedForm.EquippedItemPB;
            var dest = destDDP as InventoryForm;

            if (src == null || dest == null)
                return false;

            var eqForm = src.Parent as EquippedForm;
            if (eqForm == null)
                return false;

            return true;
        }

        bool CanDrop_InventoryItemToEquipped(IDragDropProvider srcDDP, IDragDropProvider destDDP)
        {
            var src = srcDDP as InventoryForm.InventoryItemPB;
            var dest = destDDP as EquippedForm;

            if (src == null || dest == null)
                return false;

            if (src.Item == null)
                return false;

            var inv = src.Parent as InventoryForm;
            if (inv == null)
                return false;

            if (inv.Inventory != _gps.UserInfo.Inventory)
                return false;

            return true;
        }

        bool CanDrop_InventoryItemToShop(IDragDropProvider srcDDP, IDragDropProvider destDDP)
        {
            var src = srcDDP as InventoryForm.InventoryItemPB;
            var dest = destDDP as ShopForm;

            if (src == null || dest == null)
                return false;

            return true;
        }

        bool CanDrop_InventorySlotToInventorySlot(IDragDropProvider srcDDP, IDragDropProvider destDDP)
        {
            var src = srcDDP as InventoryForm.InventoryItemPB;
            var dest = destDDP as InventoryForm.InventoryItemPB;

            if (src == null || dest == null)
                return false;

            if (src == dest || src.Slot == dest.Slot)
                return false;

            if (src.Parent != src.Parent)
                return false;

            var invForm = src.Parent as InventoryForm;
            if (invForm == null)
                return false;

            if (invForm.Inventory != _gps.UserInfo.Inventory)
                return false;

            return true;
        }

        /// <summary>
        /// Handles the drag-and-drop for the given <see cref="IDragDropProvider"/>s.
        /// </summary>
        /// <param name="src">The <see cref="IDragDropProvider"/> that was dragged.</param>
        /// <param name="dest">The <see cref="IDragDropProvider"/> that that <paramref name="src"/> was dropped onto.</param>
        public void Drop(IDragDropProvider src, IDragDropProvider dest)
        {
            if (Drop_EquippedItemToInventory(src, dest))
                return;
            if (Drop_InventoryItemToEquipped(src, dest))
                return;
            if (Drop_InventoryItemToShop(src, dest))
                return;
            if (Drop_InventorySlotToInventorySlot(src, dest))
                return;
        }

        bool Drop_EquippedItemToInventory(IDragDropProvider srcDDP, IDragDropProvider destDDP)
        {
            if (!CanDrop_EquippedItemToInventory(srcDDP, destDDP))
                return false;

            var src = (EquippedForm.EquippedItemPB)srcDDP;

            _gps.EquippedForm_RequestUnequip((EquippedForm)src.Parent, src.Slot);

            return true;
        }

        bool Drop_InventoryItemToEquipped(IDragDropProvider srcDDP, IDragDropProvider destDDP)
        {
            if (!CanDrop_InventoryItemToEquipped(srcDDP, destDDP))
                return false;

            var src = (InventoryForm.InventoryItemPB)srcDDP;

            ((InventoryForm)src.Parent).InvokeRequestUseItem(src.Slot);

            return true;
        }

        bool Drop_InventoryItemToShop(IDragDropProvider srcDDP, IDragDropProvider destDDP)
        {
            var src = srcDDP as InventoryForm.InventoryItemPB;
            var dest = destDDP as ShopForm;

            // TODO: Sell item
            return true;
        }

        bool Drop_InventorySlotToInventorySlot(IDragDropProvider srcDDP, IDragDropProvider destDDP)
        {
            if (!CanDrop_InventorySlotToInventorySlot(srcDDP, destDDP))
                return false;

            var src = (InventoryForm.InventoryItemPB)srcDDP;
            var dest = (InventoryForm.InventoryItemPB)destDDP;

            using (var pw = ClientPacket.SwapInventorySlots(src.Slot, dest.Slot))
            {
                _gps.Socket.Send(pw);
            }

            return true;
        }
    }
}