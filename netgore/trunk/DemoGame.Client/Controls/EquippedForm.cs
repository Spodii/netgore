using System;
using System.Linq;
using DemoGame.DbObjs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Graphics;
using NetGore.Graphics.GUI;

namespace DemoGame.Client
{
    delegate void RequestUnequipHandler(EquippedForm equippedForm, EquipmentSlot slot);

    class EquippedForm : Form
    {
        static readonly Vector2 _itemSize = new Vector2(32, 32);

        readonly ItemInfoRequesterBase<EquipmentSlot> _infoRequester;
        UserEquipped _userEquipped;

        /// <summary>
        /// Initializes a new instance of the <see cref="EquippedForm"/> class.
        /// </summary>
        /// <param name="infoRequester">The info requester.</param>
        /// <param name="position">The position.</param>
        /// <param name="parent">The parent.</param>
        public EquippedForm(ItemInfoRequesterBase<EquipmentSlot> infoRequester, Vector2 position, Control parent)
            : base(parent, position, new Vector2(200, 200))
        {
            if (infoRequester == null)
                throw new ArgumentNullException("infoRequester");

            _infoRequester = infoRequester;

            CreateItemSlots();
        }

        /// <summary>
        /// Notifies listeners when a request has been made to unequip an item.
        /// </summary>
        public event RequestUnequipHandler RequestUnequip;

        public UserEquipped UserEquipped
        {
            get { return _userEquipped; }
            set { _userEquipped = value; }
        }

        void CreateItemSlots()
        {
            CreateItemSlots(0, 0, EquipmentSlot.Head);
            CreateItemSlots(1, 0, EquipmentSlot.Body);

            CreateItemSlots(0, 1, EquipmentSlot.LeftHand);
            CreateItemSlots(1, 1, EquipmentSlot.RightHand);
        }

        void CreateItemSlots(int row, int column, EquipmentSlot slot)
        {
            Vector2 loc = new Vector2(row, column);
            Vector2 pos = _itemSize * loc;

            new EquippedItemPB(this, pos, slot);
        }

        void EquippedItemPB_OnMouseUp(object sender, MouseClickEventArgs e)
        {
            if (UserEquipped == null)
                return;

            EquipmentSlot slot = ((EquippedItemPB)sender).Slot;
            if (UserEquipped[slot] == null)
                return;

            switch (e.Button)
            {
                case MouseButtons.Right:
                    if (RequestUnequip != null)
                        RequestUnequip(this, slot);
                    break;
            }
        }

        protected override void SetDefaultValues()
        {
            base.SetDefaultValues();

            Text = "Equipment";
        }

        class EquippedItemPB : PictureBox
        {
            static readonly TooltipHandler _tooltipHandler = TooltipCallback;
            readonly EquippedForm _equippedForm;

            readonly EquipmentSlot _slot;

            public EquippedItemPB(EquippedForm parent, Vector2 pos, EquipmentSlot slot) : base(parent, pos, _itemSize)
            {
                if (parent == null)
                    throw new ArgumentNullException("parent");

                _equippedForm = parent;
                _slot = slot;
                Tooltip = _tooltipHandler;

                MouseUp += _equippedForm.EquippedItemPB_OnMouseUp;
            }

            public EquipmentSlot Slot
            {
                get { return _slot; }
            }

            /// <summary>
            /// Draws the <see cref="Control"/>.
            /// </summary>
            /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
            protected override void DrawControl(ISpriteBatch spriteBatch)
            {
                base.DrawControl(spriteBatch);

                UserEquipped equipped = _equippedForm.UserEquipped;
                if (equipped == null)
                    return;

                ItemEntity item = equipped[_slot];
                if (item == null)
                    return;

                // Draw the item in the center of the slot
                Vector2 offset = (_itemSize - item.Grh.Size) / 2f;
                item.Draw(spriteBatch, ScreenPosition + offset);
            }

            static StyledText[] TooltipCallback(Control sender, TooltipArgs args)
            {
                EquippedItemPB src = (EquippedItemPB)sender;
                EquipmentSlot slot = src.Slot;
                IItemTable itemInfo;

                if (!src._equippedForm._infoRequester.TryGetInfo(slot, out itemInfo))
                {
                    // The data has not been received yet - returning null will make the tooltip retry later
                    return null;
                }

                // Data was received, so format it and return it
                return ItemInfoHelper.GetStyledText(itemInfo);
            }
        }
    }
}