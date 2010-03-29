using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.Features.Skills;
using NetGore.Graphics;
using NetGore.Graphics.GUI;

namespace DemoGame.Client
{
    class QuickBarForm : Form
    {
        const int _numSlots = 10;
        const int _slotPadding = 2;
        const int _slotSize = 32;

        readonly GameplayScreen _gps;
        QuickBarItemPB[] _slots;

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
            {
                _slots[i] = CreateQuickBarSlot(i);
            }
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

        public class QuickBarItemPB : PictureBox, IDragDropProvider, IQuickBarItemProvider
        {
            readonly Grh _grh = new Grh();
            readonly byte _slot;
            int _currentTime;
            QuickBarItemType _quickBarItemType;
            int _quickBarItemValue;

            /// <summary>
            /// The <see cref="SkillInfoAttribute"/> for when the quick bar item is a skill.
            /// </summary>
            SkillInfoAttribute _skillInfo;

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

            ISkillCooldownManager CooldownManager
            {
                get { return QuickBarForm._gps.SkillCooldownManager; }
            }

            /// <summary>
            /// Gets the <see cref="QuickBarForm"/> that this <see cref="QuickBarItemPB"/> is on.
            /// </summary>
            public QuickBarForm QuickBarForm
            {
                get { return (QuickBarForm)Parent; }
            }

            public QuickBarItemType QuickBarItemType
            {
                get { return _quickBarItemType; }
            }

            public int QuickBarItemValue
            {
                get { return _quickBarItemValue; }
            }

            /// <summary>
            /// Gets the 0-based slot number of this <see cref="QuickBarItemPB"/>.
            /// </summary>
            public byte Slot
            {
                get { return _slot; }
            }

            Vector2 CenterOnSlot(Grh grh)
            {
                return CenterOnSlot(grh != null ? grh.Size : Vector2.Zero);
            }

            Vector2 CenterOnSlot(Vector2 itemSize)
            {
                return ScreenPosition + ((new Vector2(_slotSize) - itemSize) / 2f);
            }

            /// <summary>
            /// Draws the <see cref="Control"/>.
            /// </summary>
            /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
            protected override void DrawControl(ISpriteBatch spriteBatch)
            {
                base.DrawControl(spriteBatch);

                DrawQuickBarItem(spriteBatch, null, Color.White);
            }

            /// <summary>
            /// Draws the quick bar item.
            /// </summary>
            /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
            /// <param name="position">The position to draw the item at. If null, the item will be
            /// drawn in the center of the quick bar slot.</param>
            /// <param name="color">The color.</param>
            void DrawQuickBarItem(ISpriteBatch spriteBatch, Vector2? position, Color color)
            {
                bool isOnBar = (position == null);

                // Draw the item in the quick bar
                switch (QuickBarItemType)
                {
                    case QuickBarItemType.Inventory:
                        var item = QuickBarForm._gps.UserInfo.Inventory[(InventorySlot)QuickBarItemValue];
                        if (item == null)
                            break;

                        if (position == null)
                            position = CenterOnSlot(item.Grh);

                        item.Draw(spriteBatch, position.Value, color);
                        break;

                    case QuickBarItemType.Skill:
                        if (_skillInfo == null)
                            break;

                        if (position == null)
                            position = CenterOnSlot(_grh);

                        _grh.Draw(spriteBatch, position.Value);

                        if (isOnBar && CooldownManager.IsCoolingDown(_skillInfo.CooldownGroup, _currentTime))
                            XNARectangle.Draw(spriteBatch, GetScreenArea(), new Color(0, 0, 0, 150));

                        break;
                }
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

                if (e.Button == MouseButtons.Right)
                    UseQuickBarItem();
            }

            /// <summary>
            /// Sets the quick bar item's type and value.
            /// </summary>
            /// <param name="type">The <see cref="QuickBarItemType"/>.</param>
            /// <param name="value">The value.</param>
            public void SetQuickBar(QuickBarItemType type, int value)
            {
                // Check that at least one of the values are new
                if (type == QuickBarItemType && value == QuickBarItemValue)
                    return;

                // Set the new values
                _quickBarItemType = type;
                _quickBarItemValue = value;

                // Clear some values
                _grh.SetGrh(null);
                _skillInfo = null;

                // Additional type-based handling
                switch (type)
                {
                    case QuickBarItemType.Skill:
                        _skillInfo = SkillInfoManager.Instance.GetAttribute((SkillType)QuickBarItemValue);
                        if (_skillInfo == null)
                        {
                            SetQuickBar(QuickBarItemType.None, 0);
                            return;
                        }

                        _grh.SetGrh(_skillInfo.Icon, AnimType.Loop, _currentTime);
                        if (_grh.GrhData == null)
                        {
                            SetQuickBar(QuickBarItemType.None, 0);
                            return;
                        }

                        break;
                }
            }

            /// <summary>
            /// Updates the <see cref="Control"/> for anything other than the mouse or keyboard.
            /// </summary>
            /// <param name="currentTime">The current time in milliseconds.</param>
            protected override void UpdateControl(int currentTime)
            {
                _currentTime = currentTime;

                // Make sure the quick bar item is valid
                switch (QuickBarItemType)
                {
                    case QuickBarItemType.Inventory:
                        if (QuickBarForm._gps.UserInfo.Inventory[(InventorySlot)QuickBarItemValue] == null)
                            SetQuickBar(QuickBarItemType.None, 0);
                        break;

                    case QuickBarItemType.Skill:
                        break;
                }

                if (_grh.GrhData != null)
                    _grh.Update(currentTime);

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
                        QuickBarForm._gps.SkillsForm_RequestUseSkill((SkillType)QuickBarItemValue);
                        break;
                }
            }

            #region IDragDropProvider Members

            /// <summary>
            /// Gets if this <see cref="IDragDropProvider"/> can be dragged. In the case of something that only
            /// supports having items dropped on it but not dragging, this will always return false. For items that can be
            /// dragged, this will return false if there is currently nothing to drag (such as an empty inventory slot) or
            /// there is some other reason that this item cannot currently be dragged.
            /// </summary>
            bool IDragDropProvider.CanDragContents
            {
                get { return QuickBarItemType != QuickBarItemType.None; }
            }

            /// <summary>
            /// Gets if the specified <see cref="IDragDropProvider"/> can be dropped on this <see cref="IDragDropProvider"/>.
            /// </summary>
            /// <param name="source">The <see cref="IDragDropProvider"/> to check if can be dropped on this
            /// <see cref="IDragDropProvider"/>. This value will never be null.</param>
            /// <returns>True if the <paramref name="source"/> can be dropped on this <see cref="IDragDropProvider"/>;
            /// otherwise false.</returns>
            bool IDragDropProvider.CanDrop(IDragDropProvider source)
            {
                return QuickBarForm._gps.DragDropHandler.CanDrop(source, this);
            }

            /// <summary>
            /// Draws the item that this <see cref="IDragDropProvider"/> contains for when this item
            /// is being dragged.
            /// </summary>
            /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
            /// <param name="position">The position to draw the sprite at.</param>
            /// <param name="color">The color to use when drawing the item.</param>
            void IDragDropProvider.DrawDraggedItem(ISpriteBatch spriteBatch, Vector2 position, Color color)
            {
                DrawQuickBarItem(spriteBatch, position, color);
            }

            /// <summary>
            /// Draws a visual highlighting on this <see cref="IDragDropProvider"/> for when an item is being
            /// dragged onto it but not yet dropped.
            /// </summary>
            /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
            void IDragDropProvider.DrawDropHighlight(ISpriteBatch spriteBatch)
            {
                DragDropProviderHelper.DrawDropHighlight(spriteBatch, GetScreenArea());
            }

            /// <summary>
            /// Handles when the specified <see cref="IDragDropProvider"/> is dropped on this <see cref="IDragDropProvider"/>.
            /// </summary>
            /// <param name="source">The <see cref="IDragDropProvider"/> that is being dropped on this
            /// <see cref="IDragDropProvider"/>.</param>
            void IDragDropProvider.Drop(IDragDropProvider source)
            {
                QuickBarForm._gps.DragDropHandler.Drop(source, this);
            }

            #endregion

            #region IQuickBarItemProvider Members

            /// <summary>
            /// Gets the <see cref="QuickBarItemType"/> and value to add to the quick bar.
            /// </summary>
            /// <param name="type">When this method returns true, contains the <see cref="QuickBarItemType"/>
            /// to add.</param>
            /// <param name="value">When this method returns true, contains the value for for the quick bar item.</param>
            /// <returns>True if the item can be added to the quick bar; otherwise false.</returns>
            bool IQuickBarItemProvider.TryAddToQuickBar(out QuickBarItemType type, out int value)
            {
                type = QuickBarItemType;
                value = QuickBarItemValue;

                if (!((IDragDropProvider)this).CanDragContents)
                    return false;

                return true;
            }

            #endregion
        }
    }
}