using System.Linq;
using NetGore;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using SFML.Graphics;
using SFML.Window;

namespace DemoGame.Client
{
    /// <summary>
    /// A <see cref="Form"/> containing buttons to toggle the visibility of the various forms.
    /// </summary>
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
        /// Initializes a new instance of the <see cref="Toolbar"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="pos">The pos.</param>
        public Toolbar(Control parent, Vector2 pos) : base(parent, pos, Vector2.One)
        {
            ResizeToChildren = true;
            ResizeToChildrenPadding = _padding;

            _items = CreateToolbarItems();

            Position = pos;
        }

        /// <summary>
        /// Notifies listeners when an individual tool item on the Toolbar has been clicked.
        /// </summary>
        public event TypedEventHandler<Toolbar, ToolbarEventArgs> ItemClicked;

        /// <summary>
        /// Creates all of the ToolbarItems.
        /// </summary>
        /// <returns>Array of the created ToolbarItems.</returns>
        ToolbarItem[] CreateToolbarItems()
        {
            // Get the values
            var values = EnumHelper<ToolbarItemType>.Values.Select(EnumHelper<ToolbarItemType>.ToInt);

            // Find the largest value, and create the array
            var max = values.Max();
            var items = new ToolbarItem[max + 1];

            // Create the items
            foreach (var index in values)
            {
                var pos = GetItemPosition(index);
                var sprite = GetItemSprite(index);

                var item = new ToolbarItem(this, (ToolbarItemType)index, pos, sprite);
                item.Clicked += ToolbarItem_Clicked;

                items[index] = item;
            }

            return items;
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
            var title = EnumHelper<ToolbarItemType>.ToName((ToolbarItemType)index);
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

            // Don't reload the toolbar icons if the toolbar items have not been made yet
            if (_items == null)
                return;

            // Re-load the toolbar icons
            for (var i = 0; i < _items.Length; i++)
            {
                if (_items[i] == null)
                    continue;

                _items[i].Sprite = GetItemSprite(i);
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
            IsCloseButtonVisible = false;
        }

        /// <summary>
        /// Handles when a <see cref="ToolbarItem"/> in this <see cref="Toolbar"/> is clicked.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SFML.Window.MouseButtonEventArgs"/> instance containing the event data.</param>
        void ToolbarItem_Clicked(object sender, MouseButtonEventArgs e)
        {
            if (ItemClicked == null)
                return;

            var item = (ToolbarItem)sender;

            if (ItemClicked != null)
                ItemClicked.Raise(this, new ToolbarEventArgs(item.ToolbarItemType, item));
        }

        /// <summary>
        /// A <see cref="Control"/> for a single item in a <see cref="Toolbar"/>.
        /// </summary>
        class ToolbarItem : PictureBox
        {
            readonly ToolbarItemType _type;

            /// <summary>
            /// Initializes a new instance of the <see cref="ToolbarItem"/> class.
            /// </summary>
            /// <param name="parent">The parent.</param>
            /// <param name="type">The type.</param>
            /// <param name="pos">The position.</param>
            /// <param name="sprite">The sprite.</param>
            public ToolbarItem(Control parent, ToolbarItemType type, Vector2 pos, ISprite sprite)
                : base(parent, pos, new Vector2(_itemSize))
            {
                Sprite = sprite;
                _type = type;

                Tooltip = TooltipHandler;
            }

            /// <summary>
            /// Gets the <see cref="ToolbarItemType"/> for this item.
            /// </summary>
            public ToolbarItemType ToolbarItemType
            {
                get { return _type; }
            }

            /// <summary>
            /// Gets the default tooltip handler for a <see cref="ToolbarItem"/>.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="args">The args.</param>
            /// <returns>The tooltip text.</returns>
            StyledText[] TooltipHandler(Control sender, TooltipArgs args)
            {
                return new StyledText[] { new StyledText(ToolbarItemType.ToString()) };
            }
        }
    }
}