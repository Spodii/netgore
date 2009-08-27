using System;
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

    class Toolbar : Form, IRestorableSettings
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
        /// Needed size of the client area of the form.
        /// </summary>
        readonly Vector2 _neededClientSize;

        /// <summary>
        /// Notifies listeners when an individual tool item on the Toolbar has been clicked.
        /// </summary>
        public event ToolbarEventHandler OnClickItem;

        public Toolbar(Control parent, Vector2 pos) : base(parent.GUIManager, "Menu", pos, new Vector2(800, 800), parent)
        {
            // Create the ToolbarItems
            _items = CreateToolbarItems();

            // Set the size
            _neededClientSize = FindNeededClientSize();
            UpdateSize(this);
            OnChangeBorder += UpdateSize;

            // Re-adjust the position
            Position = pos;
        }

        /// <summary>
        /// Creates all of the ToolbarItems.
        /// </summary>
        /// <returns>Array of the created ToolbarItems.</returns>
        ToolbarItem[] CreateToolbarItems()
        {
            // Get the values
            var values = EnumHelper.GetValues<ToolbarItemType>().Cast<int>();

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
        static ISprite GetItemSprite(int index)
        {
            string title = Enum.GetName(typeof(ToolbarItemType), index);
            GrhData gd = Skin.GetToolbarItem(title);
            return new Grh(gd, AnimType.Loop, 0); // Start time doesn't matter
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
        static void UpdateSize(Control sender)
        {
            Toolbar toolbar = (Toolbar)sender;
            toolbar.ClientSize = toolbar._neededClientSize;
        }

        #region IRestorableSettings Members

        public void Load(IDictionary<string, string> items)
        {
            Position = new Vector2(float.Parse(items["X"]), float.Parse(items["Y"]));
            IsVisible = bool.Parse(items["IsVisible"]);
        }

        public IEnumerable<NodeItem> Save()
        {
            return new NodeItem[]
            { new NodeItem("X", Position.X), new NodeItem("Y", Position.Y), new NodeItem("IsVisible", IsVisible) };
        }

        #endregion

        class ToolbarItem : PictureBox
        {
            readonly ToolbarItemType _type;

            public ToolbarItemType ToolbarItemType
            {
                get { return _type; }
            }

            public ToolbarItem(Control parent, ToolbarItemType type, Vector2 pos, ISprite sprite)
                : base(pos, sprite, new Vector2(_itemSize), parent)
            {
                _type = type;
            }
        }
    }
}