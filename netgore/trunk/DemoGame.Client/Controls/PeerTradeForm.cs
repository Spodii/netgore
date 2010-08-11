using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DemoGame.DbObjs;
using NetGore.Features.PeerTrading;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using SFML.Graphics;

namespace DemoGame.Client
{
    public class PeerTradeForm : PeerTradeFormBase<Character, ItemEntity, IItemTable>
    {
        public PeerTradeForm(Control parent, Vector2 position)
            : base(parent, position)
        {
        }

        public PeerTradeForm(IGUIManager guiManager, Vector2 position) : base(guiManager, position)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of setting up the control that holds all the
        /// controls for one side of a peer trade.
        /// </summary>
        /// <param name="tradePanel">The <see cref="PeerTradeFormBase{TChar,TItem,TItemInfo}.PeerTradeSidePanel"/> to set up.</param>
        protected override void SetupTradePanelControl(PeerTradeSidePanel tradePanel)
        {
            base.SetupTradePanelControl(tradePanel);

            tradePanel.Border = GUIManager.SkinManager.GetBorder("TextBox");
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of setting up the control for an item slot.
        /// </summary>
        /// <param name="slot">The <see cref="PeerTradeFormBase{TChar,TItem,TItemInfo}.PeerTradeSidePanel.PeerTradeItemsCollectionSlot"/> to set up.</param>
        protected override void SetupItemSlotControl(PeerTradeSidePanel.PeerTradeItemsCollectionSlot slot)
        {
            base.SetupItemSlotControl(slot);

            // TODO: !! Set up the tooltip
            slot.Sprite = GUIManager.SkinManager.GetSprite("item_slot");
        }

        /// <summary>
        /// Updates the <see cref="Control"/>. This is called for every <see cref="Control"/>, even if it is disabled or
        /// not visible.
        /// </summary>
        /// <param name="currentTime">The current time in milliseconds.</param>
        protected override void UpdateControl(NetGore.TickCount currentTime)
        {
            base.UpdateControl(currentTime);

            var ptih = PeerTradeInfoHandler;
            if (ptih == null)
                return;

            // Always show when a trade is active
            IsVisible = ptih.IsTradeOpen;

            if (!IsVisible)
                return;

            // TODO: !! Display the character names instead of "Source" and "Target"
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling of the <see cref="PeerTradeFormBase{TChar,TItem,TItemInfo}.ItemSlotClicked"/> event.
        /// </summary>
        /// <param name="e">The <see cref="SFML.Window.MouseButtonEventArgs"/> instance containing the event data.</param>
        /// <param name="isSourceSide">If the item slot clicked is on the source side.</param>
        /// <param name="slot">The slot that was clicked.</param>
        protected override void OnItemSlotClicked(SFML.Window.MouseButtonEventArgs e, bool isSourceSide, NetGore.InventorySlot slot)
        {
            base.OnItemSlotClicked(e, isSourceSide, slot);

            // TODO: !!
        }

        /// <summary>
        /// When overridden in the derived class, initializes a <see cref="Grh"/> to display the information for an item.
        /// </summary>
        /// <param name="grh">The <see cref="Grh"/> to initialize (cannot be null).</param>
        /// <param name="itemInfo">The item information to be displayed. Can be null.</param>
        protected override void InitializeItemInfoSprite(Grh grh, IItemTable itemInfo)
        {
            if (itemInfo == null)
            {
                // Clear the sprite
                grh.SetGrh(null);
            }
            else
            {
                // Set the new sprite
                var newGD = GrhInfo.GetData(itemInfo.Graphic);
                grh.SetGrh(newGD);
            }
        }
    }
}
