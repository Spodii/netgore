using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using NetGore;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using NetGore.IO;

namespace DemoGame.Client
{
    /// <summary>
    /// Handles an event on the Toolbar.
    /// </summary>
    /// <param name="toolbar">Toolbar that the event took place on.</param>
    /// <param name="itemType">ToolbarItemType for the Toolbar item that raised the event.</param>
    /// <param name="control">Control that raised the event.</param>
    delegate void ToolbarEventHandler(Toolbar toolbar, ToolbarItemType itemType, Control control);

    class Toolbar : Form
    {
        /// <summary>
        /// Size of each toolbar item in pixels.
        /// </summary>
        const int _itemSize = 32;

        /// <summary>
        /// Number of pixels between the border and each item in the Toolbar.
        /// </summary>
        const int _padding = 4;

        readonly ToolbarItem[] _items;

        /// <summary>
        /// Notifies listeners when an individual tool item on the Toolbar has been clicked.
        /// </summary>
        public event ToolbarEventHandler OnClickItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="Toolbar"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="pos">The pos.</param>
        public Toolbar(Control parent, Vector2 pos) : base(parent, pos, new Vector2(800, 800))
        {
            _items = CreateToolbarItems();

            UpdateSize();
            Position = pos;
        }

        /// <summary>
        /// Handles when the <see cref="Control.Border"/> has changed.
        /// This is called immediately before <see cref="Control.OnChangeBorder"/>.
        /// Override this method instead of using an event hook on <see cref="Control.OnChangeBorder"/> when possible.
        /// </summary>
        protected override void ChangeBorder()
        {
            base.ChangeBorder();

            UpdateSize();
        }

        /// <summary>
        /// Creates all of the ToolbarItems.
        /// </summary>
        /// <returns>Array of the created ToolbarItems.</returns>
        ToolbarItem[] CreateToolbarItems()
        {
            // Get the values
            var values = EnumHelper<ToolbarItemType>.Values.Cast<int>();

            // Find the largest value, and create the array
            int max = values.Max();
            var items = new ToolbarItem[max + 1];

            // Create the items
            foreach (int index in values)
            {
                Vector2 pos = GetItemPosition(index);
                ISprite sprite = GetItemSprite(index);

                ToolbarItem item = new ToolbarItem(this, (ToolbarItemType)index, pos, sprite);
                items[index] = item;
                item.OnClick += ToolbarItem_OnClick;
            }

            return items;
        }

        /// <summary>
        /// Finds the ClientSize needed for the Toolbar.
        /// </summary>
        /// <returns>ClientSize needed for the Toolbar</returns>
        Vector2 FindNeededClientSize()
        {
            var allItems = _items.Where(x => x != null);
            float maxWidth = allItems.Max(x => x.Position.X + x.Size.X);
            float maxHeight = allItems.Max(x => x.Position.Y + x.Size.Y);
            return new Vector2(maxWidth, maxHeight) + new Vector2(_padding);
        }

        /// <summary>
        /// Gets the position for the item of the given index.
        /// </summary>
        /// <param name="index">Index of the item.</param>
        /// <returns>Position of the item.</returns>
        static Vector2 GetItemPosition(int index)
        {
            float offset = (_itemSize + (_padding * 2)) * index;
            return new Vector2(_padding, _padding + offset);
        }

        /// <summary>
        /// Gets the ISprite for the item of the given index.
        /// </summary>
        /// <param name="index">Index of the item.</param>
        /// <returns>ISprite for the given index.</returns>
        ISprite GetItemSprite(int index)
        {
            var title = EnumIOHelper<ToolbarItemType>.ToName((ToolbarItemType)index);
            return GUIManager.SkinManager.GetSprite("Toolbar", title);
        }

        /// <summary>
        /// When overridden in the derived class, loads the skinning information for the <see cref="Control"/>
        /// from the given <paramref name="skinManager"/>.
        /// </summary>
        /// <param name="skinManager">The <see cref="ISkinManager"/> to load the skinning information from.</param>
        public override void LoadSkin(ISkinManager skinManager)
        {
            base.LoadSkin(skinManager);

            // Re-load the toolbar icons
            if (_items != null)
            {
                for (int i = 0; i < _items.Length; i++)
                {
                    if (_items[i] == null)
                        continue;

                    _items[i].Sprite = GetItemSprite(i);
                }

                UpdateSize();
            }
        }

        /// <summary>
        /// Sets the default values for the <see cref="Control"/>. This should always begin with a call to the
        /// base class's method to ensure that changes to settings are hierchical.
        /// </summary>
        protected override void SetDefaultValues()
        {
            base.SetDefaultValues();

            Text = "Menu";
        }

        void ToolbarItem_OnClick(object sender, MouseClickEventArgs e)
        {
            if (OnClickItem == null)
                return;

            ToolbarItem item = (ToolbarItem)sender;
            OnClickItem(this, item.ToolbarItemType, item);
        }

        /// <summary>
        /// Updates the size to ensure the Toolbar has the needed ClientSize.
        /// </summary>
        void UpdateSize()
        {
            if (_items != null)
                ClientSize = FindNeededClientSize();
        }

        class ToolbarItem : PictureBox
        {
            readonly ToolbarItemType _type;

            public ToolbarItem(Control parent, ToolbarItemType type, Vector2 pos, ISprite sprite)
                : base(parent, pos, new Vector2(_itemSize))
            {
                Sprite = sprite;
                _type = type;
            }

            public ToolbarItemType ToolbarItemType
            {
                get { return _type; }
            }
        }
    }
}