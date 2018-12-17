using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NetGore;
using NetGore.Features.Skills;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using NetGore.IO;
using SFML.Graphics;
using SFML.Window;

namespace DemoGame.Client
{
    /// <summary>
    /// A <see cref="Form"/> that contains multiple user-assignable slots for quickly accessing and using different items, spells,
    /// skills, etc.
    /// </summary>
    public class QuickBarForm : Form
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

        /// <summary>
        /// Creates a <see cref="QuickBarItemPB"/>.
        /// </summary>
        /// <param name="slot">The slot of the <see cref="QuickBarItemPB"/>.</param>
        /// <returns>The <see cref="QuickBarItemPB"/>.</returns>
        QuickBarItemPB CreateQuickBarSlot(int slot)
        {
            var c = new QuickBarItemPB(this, Vector2.Zero, (byte)slot);
            c.Position = new Vector2((c.Size.X + _slotPadding) * (slot - 1), _slotPadding);
            return c;
        }

        /// <summary>
        /// Ensures the <see cref="QuickBarForm"/>'s <see cref="QuickBarItemPB"/>s are created.
        /// </summary>
        void CreateSlots()
        {
            if (_slots != null)
                return;

            _slots = new QuickBarItemPB[_numSlots];

            for (var i = 0; i < _numSlots; i++)
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

        /// <summary>
        /// Reads the state of the object from an <see cref="IValueReader"/>. Values should be read in the exact
        /// same order as they were written.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        public override void ReadState(IValueReader reader)
        {
            base.ReadState(reader);

            CreateSlots();

            var slotValues = reader.ReadManyNodes("QuickBarItems", QuickBarSlotValues.Read);

            foreach (var sv in slotValues)
            {
                if (sv.Slot < _slots.Length)
                    _slots[sv.Slot].SetQuickBar(sv.Type, sv.Value);
            }
        }

        /// <summary>
        /// Respositions all of the <see cref="QuickBarItemPB"/>s on the form, and shrinks down the form
        /// to the appropriate size.
        /// </summary>
        void RepositionSlots()
        {
            CreateSlots();

            var offset = new Vector2(_slotPadding);
            for (var i = 0; i < _slots.Length; i++)
            {
                _slots[i].Position = offset;
                offset.X += _slots[i].Size.X + _slotPadding;
            }

            ClientSize = new Vector2(offset.X, _slots.First().Size.Y + (_slotPadding * 2));
        }

        /// <summary>
        /// Sets the default values for the <see cref="Control"/>. This should always begin with a call to the
        /// base class's method to ensure that changes to settings are hierchical.
        /// </summary>
        protected override void SetDefaultValues()
        {
            base.SetDefaultValues();

            Text = "Quickbar";
            IsCloseButtonVisible = false;
        }

        /// <summary>
        /// Uses a quick bar slot.
        /// </summary>
        /// <param name="slotIndex">The 0-based index of the slot to use.</param>
        public void UseSlot(byte slotIndex)
        {
            // Ensure a valid slot value
            if (slotIndex >= _slots.Length)
                return;

            // Make sure that the slot is loaded (would be very strange if it wasn't... but whatever)
            var slot = _slots[slotIndex];
            if (slot == null)
                return;

            slot.UseQuickBarItem();
        }

        /// <summary>
        /// Writes the state of the object to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
        public override void WriteState(IValueWriter writer)
        {
            base.WriteState(writer);

            CreateSlots();

            writer.WriteManyNodes("QuickBarItems", _slots.Select(x => new QuickBarSlotValues(x)).ToArray(), (w, x) => x.Write(w));
        }

        /// <summary>
        /// A <see cref="PictureBox"/> for a quick bar slot on a <see cref="QuickBarForm"/>.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public class QuickBarItemPB : PictureBox, IDragDropProvider, IQuickBarItemProvider
        {
            readonly Grh _grh = new Grh();
            readonly byte _slot;

            QuickBarItemType _quickBarItemType;
            int _quickBarItemValue;

            /// <summary>
            /// The <see cref="SkillInfo{T}"/> for when the quick bar item is a skill.
            /// </summary>
            SkillInfo<SkillType> _skillInfo;

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
            /// Gets the <see cref="ISkillCooldownManager"/>.
            /// </summary>
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

            /// <summary>
            /// Gets the <see cref="QuickBarItemType"/> of the item in this quick bar slot.
            /// </summary>
            public QuickBarItemType QuickBarItemType
            {
                get { return _quickBarItemType; }
            }

            /// <summary>
            /// Gets the value of the item in this quick bar slot.
            /// </summary>
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

            /// <summary>
            /// Gets the screen position to use to place an item centered in this control.
            /// </summary>
            /// <param name="grh">The <see cref="Grh"/> being drawn.</param>
            /// <returns>The screen position to use to place an item centered in this control.</returns>
            Vector2 CenterOnSlot(ISprite grh)
            {
                return CenterOnSlot(grh != null ? grh.Size : Vector2.Zero);
            }

            /// <summary>
            /// Gets the screen position to use to place an item centered in this control.
            /// </summary>
            /// <param name="itemSize">The size of the item.</param>
            /// <returns>The screen position to use to place an item centered in this control.</returns>
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
                var isOnBar = (position == null);

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

                        if (isOnBar && CooldownManager.IsCoolingDown(_skillInfo.CooldownGroup, TickCount.Now))
                            RenderRectangle.Draw(spriteBatch, GetScreenArea(), new Color(0, 0, 0, 150));

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
            protected override void OnClick(MouseButtonEventArgs e)
            {
                base.OnClick(e);

                if (e.Button == Mouse.Button.Right)
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
                        _skillInfo = SkillInfoManager.Instance[(SkillType)QuickBarItemValue];
                        if (_skillInfo == null)
                        {
                            SetQuickBar(QuickBarItemType.None, 0);
                            return;
                        }

                        _grh.SetGrh(_skillInfo.Icon, AnimType.Loop, TickCount.Now);
                        if (_grh.GrhData == null)
                        {
                            SetQuickBar(QuickBarItemType.None, 0);
                            return;
                        }

                        break;
                }
            }

            /// <summary>
            /// Updates the <see cref="Control"/>. This is called for every <see cref="Control"/>, even if it is disabled or
            /// not visible.
            /// </summary>
            /// <param name="currentTime">The current time in milliseconds.</param>
            protected override void UpdateControl(TickCount currentTime)
            {
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
                        QuickBarForm._gps.SkillsForm_RequestUseSkill(this, EventArgsHelper.Create((SkillType)QuickBarItemValue));
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

        /// <summary>
        /// A struct used to read and write the quick bar values, namely for a <see cref="QuickBarItemPB"/>.
        /// </summary>
        struct QuickBarSlotValues
        {
            /// <summary>
            /// The quick bar slot.
            /// </summary>
            public readonly byte Slot;

            /// <summary>
            /// The <see cref="QuickBarItemType"/>.
            /// </summary>
            public readonly QuickBarItemType Type;

            /// <summary>
            /// The item value.
            /// </summary>
            public readonly int Value;

            /// <summary>
            /// Initializes a new instance of the <see cref="QuickBarSlotValues"/> struct.
            /// </summary>
            /// <param name="itemPB">The <see cref="QuickBarItemPB"/> to get the values from..</param>
            public QuickBarSlotValues(QuickBarItemPB itemPB)
                : this(itemPB.Slot, itemPB.QuickBarItemType, itemPB.QuickBarItemValue)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="QuickBarSlotValues"/> struct.
            /// </summary>
            /// <param name="slot">The slot.</param>
            /// <param name="type">The type.</param>
            /// <param name="value">The value.</param>
            QuickBarSlotValues(byte slot, QuickBarItemType type, int value)
            {
                Slot = slot;
                Type = type;
                Value = value;
            }

            /// <summary>
            /// Reads a <see cref="QuickBarSlotValues"/> from an <see cref="IValueReader"/>.
            /// </summary>
            /// <param name="r">The <see cref="IValueReader"/> to read the values from.</param>
            /// <returns>The <see cref="QuickBarSlotValues"/> read from the <paramref name="r"/>.</returns>
            public static QuickBarSlotValues Read(IValueReader r)
            {
                var slot = r.ReadByte("Slot");
                var type = r.ReadEnum<QuickBarItemType>("Type");
                var value = r.ReadInt("Value");

                return new QuickBarSlotValues(slot, type, value);
            }

            /// <summary>
            /// Writes the <see cref="QuickBarSlotValues"/> to an <see cref="IValueWriter"/>.
            /// </summary>
            /// <param name="w">The <see cref="IValueWriter"/> to write the values to.</param>
            public void Write(IValueWriter w)
            {
                w.Write("Slot", Slot);
                w.WriteEnum("Type", Type);
                w.Write("Value", Value);
            }
        }
    }
}