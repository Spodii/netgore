using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using NetGore;
using NetGore.Graphics;
using NetGore.Graphics.GUI;

// ReSharper disable UnusedMember.Local

namespace DemoGame.Client
{
    /// <summary>
    /// Handles the drag-and-drop support for all of the <see cref="IDragDropProvider"/>s on
    /// <see cref="Control"/>s. While this could done through the controls directly, it is done
    /// here to make it easier to keep track of all the drop-and-drop connections.
    /// </summary>
    public class DragDropHandler
    {
        /// <summary>
        /// A delegate the <see cref="IDragDropProvider.CanDrop"/> and <see cref="IDragDropProvider.Drop"/>
        /// implementation methods.
        /// </summary>
        /// <param name="src">The <see cref="IDragDropProvider"/> that is being dragged.</param>
        /// <param name="dest">The <see cref="IDragDropProvider"/> that the <paramref name="src"/> was dropped on.</param>
        /// <returns>True if the <paramref name="src"/> and <paramref name="dest"/> can be or were successfully
        /// handled by this delegate; otherwise false.</returns>
        delegate bool DropCallback(IDragDropProvider src, IDragDropProvider dest);

        /// <summary>
        /// The <see cref="BindingFlags"/> used to find the methods to bind to.
        /// </summary>
        const BindingFlags _bindingFlags =
            BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.InvokeMethod;

        /// <summary>
        /// The name prefix for the methods to be handled by CanDrop(). Only methods who's name start with this prefix
        /// will be used in CanDrop().
        /// </summary>
        const string _canDropMethodPrefix = "CanDrop_";

        /// <summary>
        /// The name prefix for the methods to be handled by Drop(). Only methods who's name start with this prefix
        /// will be used in Drop().
        /// </summary>
        const string _dropMethodPrefix = "Drop_";

        /// <summary>
        /// The <see cref="DropCallback"/>s for the CanDrop_ methods.
        /// </summary>
        readonly DropCallback[] _canDropCallbacks;

        /// <summary>
        /// The <see cref="DropCallback"/>s for the Drop_ methods.
        /// </summary>
        readonly DropCallback[] _dropCallbacks;

        readonly GameplayScreen _gps;

        /// <summary>
        /// Initializes a new instance of the <see cref="DragDropHandler"/> class.
        /// </summary>
        /// <param name="gps">The <see cref="GameplayScreen"/>.</param>
        public DragDropHandler(GameplayScreen gps)
        {
            // NOTE: Please read this before implementing new methods!
            // All methods are hooked automatically using reflection. The methods that begin with CanDrop_
            // implement the logic for the CanDrop() method for a specific source and destination. Methods that
            // begin with Drop_ do the same for Drop(). It is highly recommended that you follow the pattern
            // provided. That is, use the naming conventions used already. In the Drop_ method, be sure to
            // call the corresponding CanDrop_ method before handling.

            _gps = gps;

            // Find all the methods
            var methods = typeof(DragDropHandler).GetMethods(_bindingFlags);

            // Get the delegates for the methods
            _canDropCallbacks = CreateDropCallbacks(methods, _canDropMethodPrefix);
            _dropCallbacks = CreateDropCallbacks(methods, _dropMethodPrefix);
        }

        Inventory UserInventory
        {
            get
            {
                if (_gps == null)
                    return null;

                if (_gps.UserInfo == null)
                    return null;

                return _gps.UserInfo.Inventory;
            }
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
            for (var i = 0; i < _canDropCallbacks.Length; i++)
            {
                if (_canDropCallbacks[i](src, dest))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Creates the <see cref="DropCallback"/>s for a set of <see cref="MethodInfo"/>s.
        /// </summary>
        /// <param name="methods">The <see cref="MethodInfo"/>s to create the <see cref="DropCallback"/>s for.</param>
        /// <param name="namePrefix">The name prefix the <paramref name="methods"/> must have to be included
        /// in the returned collection.</param>
        /// <returns>The <see cref="DropCallback"/>s for the <paramref name="methods"/> that include the given
        /// <paramref name="namePrefix"/>.</returns>
        DropCallback[] CreateDropCallbacks(IEnumerable<MethodInfo> methods, string namePrefix)
        {
            var ret = new List<DropCallback>();

            foreach (var method in methods)
            {
                // Check for the needed method name prefix
                if (!method.Name.StartsWith(namePrefix, StringComparison.Ordinal))
                    continue;

                // For instance methods, the instance of this class is the first argument. For static methods, the
                // first argument will need to be null.
                var firstArg = this;
                if (method.IsStatic)
                    firstArg = null;

                // Create the delegate for the method
                var del = Delegate.CreateDelegate(typeof(DropCallback), firstArg, method, true);
                ret.Add((DropCallback)del);
            }

            return ret.ToArray();
        }

        /// <summary>
        /// Handles the drag-and-drop for the given <see cref="IDragDropProvider"/>s.
        /// </summary>
        /// <param name="src">The <see cref="IDragDropProvider"/> that was dragged.</param>
        /// <param name="dest">The <see cref="IDragDropProvider"/> that that <paramref name="src"/> was dropped onto.</param>
        public void Drop(IDragDropProvider src, IDragDropProvider dest)
        {
            // Check for valid parameters
            if (src == null || dest == null)
            {
                Debug.Fail("Shouldn't ever be passed a null argument.");
                return;
            }

            // Use the first method that successfully handles the given source and destination
            for (var i = 0; i < _dropCallbacks.Length; i++)
            {
                if (_dropCallbacks[i](src, dest))
                    return;
            }
        }

        #region IQuickBarItemProvider -> Quick Bar item

        /// <summary>
        /// Adds support for dragging a
        /// <see cref="IQuickBarItemProvider"/> onto an <see cref="QuickBarForm.QuickBarItemPB"/>.
        /// </summary>
        /// <param name="srcDDP">The <see cref="IDragDropProvider"/> that was dragged.</param>
        /// <param name="destDDP">The <see cref="IDragDropProvider"/> that that <paramref name="srcDDP"/>
        /// was dropped onto.</param>
        /// <returns>True if the drag-and-drop can be or was successful; otherwise false.</returns>
        static bool CanDrop_IQuickBarItemProviderToQuickBar(IDragDropProvider srcDDP, IDragDropProvider destDDP)
        {
            var src = srcDDP as IQuickBarItemProvider;
            var dest = destDDP as QuickBarForm.QuickBarItemPB;

            if (src == null || dest == null)
                return false;

            // Check if can be added to quick bar
            QuickBarItemType type;
            int value;
            if (!src.TryAddToQuickBar(out type, out value))
                return false;

            return true;
        }

        /// <summary>
        /// Adds support for dragging a
        /// <see cref="IQuickBarItemProvider"/> onto an <see cref="QuickBarForm.QuickBarItemPB"/>.
        /// </summary>
        /// <param name="srcDDP">The <see cref="IDragDropProvider"/> that was dragged.</param>
        /// <param name="destDDP">The <see cref="IDragDropProvider"/> that that <paramref name="srcDDP"/>
        /// was dropped onto.</param>
        /// <returns>True if the drag-and-drop can be or was successful; otherwise false.</returns>
        static bool Drop_IQuickBarItemProviderToQuickBar(IDragDropProvider srcDDP, IDragDropProvider destDDP)
        {
            if (!CanDrop_IQuickBarItemProviderToQuickBar(srcDDP, destDDP))
                return false;

            var src = (IQuickBarItemProvider)srcDDP;
            var dest = (QuickBarForm.QuickBarItemPB)destDDP;

            // Get the quick bar values
            QuickBarItemType type;
            int value;
            if (!src.TryAddToQuickBar(out type, out value))
                return false;

            var oldType = dest.QuickBarItemType;
            var oldValue = dest.QuickBarItemValue;

            // Set the quick bar values
            dest.SetQuickBar(type, value);

            // If the source was a quick bar item, too, then set the source's item to the dest's item to give a "swap"
            // instead of just "copying" the value.
            var srcQBI = src as QuickBarForm.QuickBarItemPB;
            if (srcQBI != null)
                srcQBI.SetQuickBar(oldType, oldValue);

            return true;
        }

        #endregion

        #region Equipped Item -> Inventory

        /// <summary>
        /// Adds support for dragging a
        /// <see cref="EquippedForm.EquippedItemPB"/> onto an <see cref="InventoryForm"/>.
        /// </summary>
        /// <param name="srcDDP">The <see cref="IDragDropProvider"/> that was dragged.</param>
        /// <param name="destDDP">The <see cref="IDragDropProvider"/> that that <paramref name="srcDDP"/>
        /// was dropped onto.</param>
        /// <returns>True if the drag-and-drop can be or was successful; otherwise false.</returns>
        static bool CanDrop_EquippedItemToInventory(IDragDropProvider srcDDP, IDragDropProvider destDDP)
        {
            var src = srcDDP as EquippedForm.EquippedItemPB;
            var dest = destDDP as InventoryForm;

            if (src == null || dest == null)
                return false;

            if (src.EquippedForm == null)
                return false;

            return true;
        }

        /// <summary>
        /// Adds support for dragging a
        /// <see cref="EquippedForm.EquippedItemPB"/> onto an <see cref="InventoryForm"/>.
        /// </summary>
        /// <param name="srcDDP">The <see cref="IDragDropProvider"/> that was dragged.</param>
        /// <param name="destDDP">The <see cref="IDragDropProvider"/> that that <paramref name="srcDDP"/>
        /// was dropped onto.</param>
        /// <returns>True if the drag-and-drop can be or was successful; otherwise false.</returns>
        bool Drop_EquippedItemToInventory(IDragDropProvider srcDDP, IDragDropProvider destDDP)
        {
            if (!CanDrop_EquippedItemToInventory(srcDDP, destDDP))
                return false;

            var src = (EquippedForm.EquippedItemPB)srcDDP;

            _gps.EquippedForm_RequestUnequip(src.EquippedForm, EventArgsHelper.Create(src.Slot));

            return true;
        }

        #endregion

        #region Inventory Item -> Equipped

        /// <summary>
        /// Adds support for dragging a
        /// <see cref="InventoryForm.InventoryItemPB"/> onto an <see cref="EquippedForm"/>.
        /// </summary>
        /// <param name="srcDDP">The <see cref="IDragDropProvider"/> that was dragged.</param>
        /// <param name="destDDP">The <see cref="IDragDropProvider"/> that that <paramref name="srcDDP"/>
        /// was dropped onto.</param>
        /// <returns>True if the drag-and-drop can be or was successful; otherwise false.</returns>
        bool CanDrop_InventoryItemToEquipped(IDragDropProvider srcDDP, IDragDropProvider destDDP)
        {
            var src = srcDDP as InventoryForm.InventoryItemPB;
            var dest = destDDP as EquippedForm;

            if (src == null || dest == null)
                return false;

            if (src.Item == null)
                return false;

            if (src.InventoryForm.Inventory != _gps.UserInfo.Inventory)
                return false;

            return true;
        }

        /// <summary>
        /// Adds support for dragging a
        /// <see cref="InventoryForm.InventoryItemPB"/> onto an <see cref="EquippedForm"/>.
        /// </summary>
        /// <param name="srcDDP">The <see cref="IDragDropProvider"/> that was dragged.</param>
        /// <param name="destDDP">The <see cref="IDragDropProvider"/> that that <paramref name="srcDDP"/>
        /// was dropped onto.</param>
        /// <returns>True if the drag-and-drop can be or was successful; otherwise false.</returns>
        bool Drop_InventoryItemToEquipped(IDragDropProvider srcDDP, IDragDropProvider destDDP)
        {
            if (!CanDrop_InventoryItemToEquipped(srcDDP, destDDP))
                return false;

            var src = (InventoryForm.InventoryItemPB)srcDDP;

            src.InventoryForm.InvokeRequestUseItem(src.Slot);

            return true;
        }

        #endregion

        #region Shop Item -> Inventory

        /// <summary>
        /// Adds support for dragging a
        /// <see cref="ShopForm.ShopItemPB"/> onto an <see cref="InventoryForm"/>.
        /// </summary>
        /// <param name="srcDDP">The <see cref="IDragDropProvider"/> that was dragged.</param>
        /// <param name="destDDP">The <see cref="IDragDropProvider"/> that that <paramref name="srcDDP"/>
        /// was dropped onto.</param>
        /// <returns>True if the drag-and-drop can be or was successful; otherwise false.</returns>
        static bool CanDrop_ShopItemToInventory(IDragDropProvider srcDDP, IDragDropProvider destDDP)
        {
            var src = srcDDP as ShopForm.ShopItemPB;
            var dest = destDDP as InventoryForm;

            if (src == null || dest == null)
                return false;

            if (src.ShopForm.ShopInfo == null)
                return false;

            return true;
        }

        /// <summary>
        /// Adds support for dragging a
        /// <see cref="ShopForm.ShopItemPB"/> onto an <see cref="InventoryForm"/>.
        /// </summary>
        /// <param name="srcDDP">The <see cref="IDragDropProvider"/> that was dragged.</param>
        /// <param name="destDDP">The <see cref="IDragDropProvider"/> that that <paramref name="srcDDP"/>
        /// was dropped onto.</param>
        /// <returns>True if the drag-and-drop can be or was successful; otherwise false.</returns>
        bool Drop_ShopItemToInventory(IDragDropProvider srcDDP, IDragDropProvider destDDP)
        {
            if (!CanDrop_ShopItemToInventory(srcDDP, destDDP))
                return false;

            var src = (ShopForm.ShopItemPB)srcDDP;

            using (var pw = ClientPacket.BuyFromShop(src.Slot, 1))
            {
                _gps.Socket.Send(pw, ClientMessageType.GUIItems);
            }

            return true;
        }

        #endregion

        #region Inventory Item -> Shop

        /// <summary>
        /// Adds support for dragging a
        /// <see cref="InventoryForm.InventoryItemPB"/> onto an <see cref="ShopForm"/>.
        /// </summary>
        /// <param name="srcDDP">The <see cref="IDragDropProvider"/> that was dragged.</param>
        /// <param name="destDDP">The <see cref="IDragDropProvider"/> that that <paramref name="srcDDP"/>
        /// was dropped onto.</param>
        /// <returns>True if the drag-and-drop can be or was successful; otherwise false.</returns>
        static bool CanDrop_InventoryItemToShop(IDragDropProvider srcDDP, IDragDropProvider destDDP)
        {
            var src = srcDDP as InventoryForm.InventoryItemPB;
            var dest = destDDP as ShopForm;

            if (src == null || dest == null)
                return false;

            if (dest.ShopInfo == null)
                return false;

            if (!dest.ShopInfo.CanBuy)
                return false;

            return true;
        }

        /// <summary>
        /// Adds support for dragging a
        /// <see cref="InventoryForm.InventoryItemPB"/> onto an <see cref="ShopForm"/>.
        /// </summary>
        /// <param name="srcDDP">The <see cref="IDragDropProvider"/> that was dragged.</param>
        /// <param name="destDDP">The <see cref="IDragDropProvider"/> that that <paramref name="srcDDP"/>
        /// was dropped onto.</param>
        /// <returns>True if the drag-and-drop can be or was successful; otherwise false.</returns>
        bool Drop_InventoryItemToShop(IDragDropProvider srcDDP, IDragDropProvider destDDP)
        {
            if (!CanDrop_InventoryItemToShop(srcDDP, destDDP))
                return false;

            if (UserInventory == null)
                return false;

            var src = (InventoryForm.InventoryItemPB)srcDDP;

            UserInventory.SellToShop(src.Slot, _gps.GUIManager);

            return true;
        }

        #endregion

        #region Inventory Item -> Inventory Item

        /// <summary>
        /// Adds support for dragging a
        /// <see cref="InventoryForm.InventoryItemPB"/> onto an <see cref="InventoryForm.InventoryItemPB"/>.
        /// </summary>
        /// <param name="srcDDP">The <see cref="IDragDropProvider"/> that was dragged.</param>
        /// <param name="destDDP">The <see cref="IDragDropProvider"/> that that <paramref name="srcDDP"/>
        /// was dropped onto.</param>
        /// <returns>True if the drag-and-drop can be or was successful; otherwise false.</returns>
        bool CanDrop_InventoryItemToInventoryItem(IDragDropProvider srcDDP, IDragDropProvider destDDP)
        {
            var src = srcDDP as InventoryForm.InventoryItemPB;
            var dest = destDDP as InventoryForm.InventoryItemPB;

            if (src == null || dest == null)
                return false;

            if (src == dest || src.Slot == dest.Slot)
                return false;

            if (src.Parent != dest.Parent)
                return false;

            if (src.InventoryForm.Inventory != _gps.UserInfo.Inventory)
                return false;

            return true;
        }

        /// <summary>
        /// Adds support for dragging a
        /// <see cref="InventoryForm.InventoryItemPB"/> onto an <see cref="InventoryForm.InventoryItemPB"/>.
        /// </summary>
        /// <param name="srcDDP">The <see cref="IDragDropProvider"/> that was dragged.</param>
        /// <param name="destDDP">The <see cref="IDragDropProvider"/> that that <paramref name="srcDDP"/>
        /// was dropped onto.</param>
        /// <returns>True if the drag-and-drop can be or was successful; otherwise false.</returns>
        bool Drop_InventoryItemToInventoryItem(IDragDropProvider srcDDP, IDragDropProvider destDDP)
        {
            if (!CanDrop_InventoryItemToInventoryItem(srcDDP, destDDP))
                return false;

            var src = (InventoryForm.InventoryItemPB)srcDDP;
            var dest = (InventoryForm.InventoryItemPB)destDDP;

            using (var pw = ClientPacket.SwapInventorySlots(src.Slot, dest.Slot))
            {
                _gps.Socket.Send(pw, ClientMessageType.GUIItems);
            }

            return true;
        }

        #endregion
    }
}