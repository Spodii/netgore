using System;
using System.Linq;
using Microsoft.Xna.Framework;
using NetGore;
using NetGore.Graphics.GUI;

namespace DemoGame.Client
{
    class QuickBarForm : Form
    {
        const int _slotSize = 32;
        const int _slotPadding = 2;
        const int _numSlots = 10;

        QuickBarItemPB[] _slots;

        readonly GameplayScreen _gps;

        /// <summary>
        /// Initializes a new instance of the <see cref="Form"/> class.
        /// </summary>
        /// <param name="gps">The <see cref="GameplayScreen"/>.</param>
        /// <param name="parent">Parent <see cref="Control"/> of this <see cref="Control"/>.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <exception cref="NullReferenceException"><paramref name="parent"/> is null.</exception>
        public QuickBarForm(GameplayScreen gps, Control parent, Vector2 position) : base(parent, position, Vector2.One)
        {
            ResizeToChildren = false;

            _gps = gps;

            RepositionSlots();
        }

        QuickBarItemPB CreateQuickBarSlot(int slot)
        {
            QuickBarItemPB c = new QuickBarItemPB(this, Vector2.Zero, (byte)slot);
            c.Position = new Vector2((c.Size.X + _slotPadding) * (slot - 1), _slotPadding);
            return c;
        }

        void CreateSlots()
        {
            if (_slots != null)
                return;

            _slots = new QuickBarItemPB[_numSlots];

            for (int i = 0; i < _numSlots; i++)
                _slots[i] = CreateQuickBarSlot(i);
        }

        /// <summary>
        /// When overridden in the derived class, loads the skinning information for the <see cref="Control"/>
        /// from the given <paramref name="skinManager"/>.
        /// </summary>
        /// <param name="skinManager">The <see cref="ISkinManager"/> to load the skinning information from.</param>
        public override void LoadSkin(ISkinManager skinManager)
        {
            base.LoadSkin(skinManager);

            RepositionSlots();
        }

        void RepositionSlots()
        {
            CreateSlots();

            Vector2 offset = new Vector2(_slotPadding);
            for (int i = 0; i < _slots.Length; i++)
            {
                _slots[i].Position = offset;
                offset.X += _slots[i].Size.X + _slotPadding;
            }

            ClientSize = new Vector2(offset.X, _slots.First().Size.Y + (_slotPadding * 2));
        }

        public class QuickBarItemPB : PictureBox
        {
            readonly byte _slot;

            /// <summary>
            /// Initializes a new instance of the <see cref="Control"/> class.
            /// </summary>
            /// <param name="parent">Parent <see cref="Control"/> of this <see cref="Control"/>.</param>
            /// <param name="position">Position of the Control reletive to its parent.</param>
            /// <param name="slot">The 0-based slot number of this <see cref="QuickBarItemPB"/>.</param>
            /// <exception cref="NullReferenceException"><paramref name="parent"/> is null.</exception>
            // ReSharper disable SuggestBaseTypeForParameter
            public QuickBarItemPB(QuickBarForm parent, Vector2 position, byte slot)
                : base(parent, position, new Vector2(_slotSize)) // ReSharper restore SuggestBaseTypeForParameter
            {
                IsBoundToParentArea = false;
                
                _slot = slot;
            }

            /// <summary>
            /// Gets the <see cref="QuickBarForm"/> that this <see cref="QuickBarItemPB"/> is on.
            /// </summary>
            public QuickBarForm QuickBarForm
            {
                get { return (QuickBarForm)Parent; }
            }

            public QuickBarItemType QuickBarItemType { get; set; }

            public int QuickBarItemValue { get; set; }

            /// <summary>
            /// Gets the 0-based slot number of this <see cref="QuickBarItemPB"/>.
            /// </summary>
            public byte Slot
            {
                get { return _slot; }
            }

            /// <summary>
            /// When overridden in the derived class, loads the skinning information for the <see cref="Control"/>
            /// from the given <paramref name="skinManager"/>.
            /// </summary>
            /// <param name="skinManager">The <see cref="ISkinManager"/> to load the skinning information from.</param>
            public override void LoadSkin(ISkinManager skinManager)
            {
                base.LoadSkin(skinManager);

                Sprite = GUIManager.SkinManager.GetSprite("item_slot");
            }

            /// <summary>
            /// Handles when this <see cref="Control"/> was clicked.
            /// This is called immediately before <see cref="Control.OnClick"/>.
            /// Override this method instead of using an event hook on <see cref="Control.OnClick"/> when possible.
            /// </summary>
            /// <param name="e">The event args.</param>
            protected override void OnClick(MouseClickEventArgs e)
            {
                base.OnClick(e);

                UseQuickBarItem();
            }

            /// <summary>
            /// Updates the <see cref="Control"/> for anything other than the mouse or keyboard.
            /// </summary>
            /// <param name="currentTime">The current time in milliseconds.</param>
            protected override void UpdateControl(int currentTime)
            {
                // Make sure the quick bar item is valid
                switch (QuickBarItemType)
                {
                    case QuickBarItemType.Inventory:
                        if (QuickBarForm._gps.UserInfo.Inventory[(InventorySlot)QuickBarItemValue] == null)
                            QuickBarItemType = QuickBarItemType.None;
                        break;

                    case QuickBarItemType.Skill:
                        // TODO: Ensure skill is valid
                        break;
                }

                base.UpdateControl(currentTime);
            }

            /// <summary>
            /// Uses the item on the quick bar slot, if possible.
            /// </summary>
            public void UseQuickBarItem()
            {
                switch (QuickBarItemType)
                {
                    case QuickBarItemType.Inventory:
                        QuickBarForm._gps.UserInfo.Inventory.Use((InventorySlot)QuickBarItemValue);
                        break;

                    case QuickBarItemType.Skill:
                        // TODO: Use skill on quickbar
                        break;
                }
            }
        }
    }
}