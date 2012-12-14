using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.Window;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// A control that displays a list of items.
    /// </summary>
    /// <typeparam name="T">The type of items being displayed.</typeparam>
    public class ListBox<T> : TextControl
    {
        /// <summary>
        /// The name of this <see cref="Control"/> for when looking up the skin information.
        /// </summary>
        const string _controlSkinName = "ListBox";

        /// <summary>
        /// The string to append to a toolbar sprite to get the MouseOver sprite.
        /// </summary>
        const string _mouseOverSpriteSuffix = "_MouseOver";

        /// <summary>
        /// The sub-category containing the toolbar sprites.
        /// </summary>
        const string _toolbarCategory = "Toolbar";

        /// <summary>
        /// The amount of padding to add around the toolbar items.
        /// </summary>
        const int _toolbarPadding = 2;

        static readonly object _eventSelectedIndexChanged = new object();
        static readonly object _eventShowPagingChanged = new object();

        readonly SpriteControl _btnFirst;
        readonly SpriteControl _btnLast;
        readonly SpriteControl _btnNext;
        readonly SpriteControl _btnPrev;
        bool _canSelect = true;

        int _currentPage = 1;
        Action<ISpriteBatch, Vector2, T, int> _itemDrawer;
        int _itemHeight = 12;
        IList<T> _items;
        StyledText _pageText = new StyledText("1/1");
        int _selectedIndex;
        bool _showPaging = true;
        int _toolbarHeight = 8;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListBox{T}"/> class.
        /// </summary>
        /// <param name="parent">Parent <see cref="Control"/> of this <see cref="Control"/>.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <param name="clientSize">The size of the <see cref="Control"/>'s client area.</param>
        /// <exception cref="NullReferenceException"><paramref name="parent"/> is null.</exception>
        public ListBox(Control parent, Vector2 position, Vector2 clientSize) : base(parent, position, clientSize)
        {
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            if (Font != null)
                ItemHeight = Font.GetLineSpacing();
            else
                ItemHeight = 12;

            _itemDrawer = GetDefaultItemDrawer();
            // ReSharper restore DoNotCallOverridableMethodsInConstructor

            // Create the buttons
            _btnFirst = new PagedListButton(this, "First", () => CurrentPage = 1);
            _btnPrev = new PagedListButton(this, "Previous", () => CurrentPage--);
            _btnNext = new PagedListButton(this, "Next", () => CurrentPage++);
            _btnLast = new PagedListButton(this, "Last", () => CurrentPage = NumPages);

            UpdateButtonPositions();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListBox{T}"/> class.
        /// </summary>
        /// <param name="guiManager">The GUI manager this <see cref="Control"/> will be managed by.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <param name="clientSize">The size of the <see cref="Control"/>'s client area.</param>
        /// <exception cref="ArgumentNullException"><paramref name="guiManager"/> is null.</exception>
        public ListBox(IGUIManager guiManager, Vector2 position, Vector2 clientSize) : base(guiManager, position, clientSize)
        {
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            if (Font != null)
                ItemHeight = Font.GetLineSpacing();
            else
                ItemHeight = 12;

            _itemDrawer = GetDefaultItemDrawer();
            // ReSharper restore DoNotCallOverridableMethodsInConstructor

            // Create the buttons
            _btnFirst = new PagedListButton(this, "First", () => CurrentPage = 1);
            _btnPrev = new PagedListButton(this, "Previous", () => CurrentPage--);
            _btnNext = new PagedListButton(this, "Next", () => CurrentPage++);
            _btnLast = new PagedListButton(this, "Last", () => CurrentPage = NumPages);

            UpdateButtonPositions();
        }

        /// <summary>
        /// Notifies listeners when the <see cref="ListBox{T}.SelectedIndex"/> changes.
        /// </summary>
        public event TypedEventHandler<Control> SelectedIndexChanged
        {
            add { Events.AddHandler(_eventSelectedIndexChanged, value); }
            remove { Events.RemoveHandler(_eventSelectedIndexChanged, value); }
        }

        /// <summary>
        /// Notifies listeners when the <see cref="ListBox{T}.ShowPaging"/> changes.
        /// </summary>
        public event TypedEventHandler<Control> ShowPagingChanged
        {
            add { Events.AddHandler(_eventShowPagingChanged, value); }
            remove { Events.RemoveHandler(_eventShowPagingChanged, value); }
        }

        /// <summary>
        /// Gets or sets if this list allows items to be selected.
        /// </summary>
        public bool CanSelect
        {
            get { return _canSelect; }
            set
            {
                if (_canSelect == value)
                    return;

                _selectedIndex = -1;
                _canSelect = value;
            }
        }

        /// <summary>
        /// Gets or sets the current page. If the value is set to greater than the total number of pages, the last page
        /// will be used instead. If the value is less than 1, the first page will be used instead.
        /// </summary>
        public int CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = Math.Max(1, Math.Min(value, NumPages));

                var s = _currentPage + "/" + NumPages;
                if (!StringComparer.OrdinalIgnoreCase.Equals(_pageText.Text, s))
                    _pageText = new StyledText(s);
            }
        }

        /// <summary>
        /// Gets or sets how to draw items in the list.
        /// </summary>
        public Action<ISpriteBatch, Vector2, T, int> ItemDrawer
        {
            get { return _itemDrawer; }
            set { _itemDrawer = value ?? GetDefaultItemDrawer(); }
        }

        /// <summary>
        /// Gets or sets the height of each item in the list. By default, this is equal to the height of the
        /// <see cref="TextControl.Font"/>.
        /// </summary>
        public int ItemHeight
        {
            get { return _itemHeight; }
            set
            {
                if (value < 2)
                    _itemHeight = 2;
                else
                    _itemHeight = value;
            }
        }

        /// <summary>
        /// Gets or sets the items to display.
        /// </summary>
        public virtual IList<T> Items
        {
            get { return _items; }
            set
            {
                if (_items == value)
                    return;

                _items = value;
                SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Gets the number of items that can be displayed per page.
        /// </summary>
        public int ItemsPerPage
        {
            get
            {
                var listHeight = ClientSize.Y - ToolbarDisplayHeight;
                var value = listHeight / ItemHeight;
                return (int)Math.Max(1, value);
            }
        }

        /// <summary>
        /// Gets the total number of pages.
        /// </summary>
        public int NumPages
        {
            get
            {
                var items = Items;
                if (items == null)
                    return 1;

                return Math.Max(1, (int)Math.Ceiling((float)items.Count() / ItemsPerPage));
            }
        }

        /// <summary>
        /// Gets or sets the selected item index. If the value is set to greater than the number of items in the list,
        /// then it will be set to the last item in the list. If <see cref="ListBox{T}.CanSelect"/> is not set,
        /// this value will always return -1. If no items are selected, this will return -1.
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                if (!CanSelect)
                    return -1;

                var c = Items.Count();
                if (_selectedIndex >= c)
                    SelectedIndex = c - 1;

                return _selectedIndex;
            }

            set
            {
                if (!CanSelect)
                    return;

                if (value < 0)
                    value = -1;

                var c = Items.Count();
                if (value >= c)
                    value = c - 1;

                if (_selectedIndex == value)
                    return;

                _selectedIndex = value;

                InvokeSelectedIndexChanged();
            }
        }

        /// <summary>
        /// Gets the selected item.
        /// </summary>
        public T SelectedItem
        {
            get
            {
                if (SelectedIndex < 0)
                    return default(T);

                return Items.ElementAtOrDefault(SelectedIndex);
            }
        }

        /// <summary>
        /// Gets or sets if the paging buttons are shown.
        /// </summary>
        public bool ShowPaging
        {
            get { return _showPaging; }
            set
            {
                if (_showPaging == value)
                    return;

                _showPaging = value;

                _btnFirst.IsVisible = ShowPaging;
                _btnLast.IsVisible = ShowPaging;
                _btnNext.IsVisible = ShowPaging;
                _btnPrev.IsVisible = ShowPaging;

                UpdateButtonPositions();

                InvokeShowPagingChanged();
            }
        }

        /// <summary>
        /// Gets the height the paging toolbar display will be. If paging is not being shown, this will be 0.
        /// </summary>
        int ToolbarDisplayHeight
        {
            get { return (ShowPaging ? _toolbarHeight : 0); }
        }

        /// <summary>
        /// Draws the <see cref="Control"/>.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
        protected override void DrawControl(ISpriteBatch spriteBatch)
        {
            // Border
            Vector2 sp = ScreenPosition;
            Vector2 size = Size;
            Rectangle rect = new Rectangle(sp.X, sp.Y, size.X, size.Y - ToolbarDisplayHeight);
            Border.Draw(spriteBatch, rect);

            // Draw the items
            DrawItems(spriteBatch);

            // Draw the page number
            if (ShowPaging)
            {
                Vector2 cs = ClientSize;
                Vector2 pagePos = new Vector2((cs.X / 2) - (_pageText.GetWidth(Font) / 2),
                    cs.Y - ToolbarDisplayHeight + (_toolbarPadding * 2));
                _pageText.Draw(spriteBatch, Font, sp + pagePos, ForeColor);
            }
        }

        void DrawItems(ISpriteBatch spriteBatch)
        {
            var itemsToDraw = _items;
            if (itemsToDraw == null)
                return;

            var pos = ScreenPosition + new Vector2(Border.LeftWidth, Border.TopHeight);
            var ipp = ItemsPerPage;
            var offset = (CurrentPage - 1) * ipp;
            var ih = ItemHeight;
            var count = Items.Count();

            var selIndex = SelectedIndex;

            for (int i = offset; i < offset + ipp && i < count; i++)
            {
                if (i >= itemsToDraw.Count)
                    return;

                if (selIndex == i)
                    DrawSelectionRegion(spriteBatch, new Rectangle(pos.X, pos.Y, ClientSize.X, ItemHeight));

                T itemToDraw = itemsToDraw[i];
                ItemDrawer(spriteBatch, pos, itemToDraw, i);

                pos += new Vector2(0, ih);
            }
        }

        /// <summary>
        /// Draws the selection region a selected item.
        /// </summary>
        /// <param name="sb">The <see cref="ISpriteBatch"/>.</param>
        /// <param name="area">The area to draw the selection.</param>
        protected virtual void DrawSelectionRegion(ISpriteBatch sb, Rectangle area)
        {
            RenderRectangle.Draw(sb, area, new Color(100, 100, 255, 150));
        }

        /// <summary>
        /// Gets the default item drawer.
        /// </summary>
        /// <returns>The default item drawer.</returns>
        protected virtual Action<ISpriteBatch, Vector2, T, int> GetDefaultItemDrawer()
        {
            return (sb, p, item, index) =>
            {
                if (item == null)
                    return;

                sb.DrawString(Font, item.ToString(), p, ForeColor);
            };
        }

        static Vector2 GetSpriteSize(SpriteControl spriteControl)
        {
            if (spriteControl == null)
                return Vector2.Zero;

            if (spriteControl.Sprite == null)
                return Vector2.Zero;

            var src = spriteControl.Sprite.Source;
            return new Vector2(src.Width, src.Height);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeSelectedIndexChanged()
        {
            OnSelectedIndexChanged();
            var handler = Events[_eventSelectedIndexChanged] as TypedEventHandler<Control>;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeShowPagingChanged()
        {
            OnShowPagingChanged();
            var handler = Events[_eventShowPagingChanged] as TypedEventHandler<Control>;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// When overridden in the derived class, loads the skinning information for the <see cref="Control"/>
        /// from the given <paramref name="skinManager"/>.
        /// </summary>
        /// <param name="skinManager">The <see cref="ISkinManager"/> to load the skinning information from.</param>
        public override void LoadSkin(ISkinManager skinManager)
        {
            base.LoadSkin(skinManager);

            UpdateButtonPositions();
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

            if (!CanSelect)
                return;

            var ipp = ItemsPerPage;
            var offset = (CurrentPage - 1) * ipp;

            int minIndex = offset;
            int maxIndex = offset + Math.Min(minIndex + ipp - 1, _items.Count() - 1);

            float yOffsetFromFirstListItem = e.Y - ScreenPosition.Y - Border.TopHeight;
            var itemIndex = (int)Math.Floor(yOffsetFromFirstListItem / ItemHeight) + ((CurrentPage - 1) * ItemsPerPage);

            SelectedIndex = itemIndex.Clamp(minIndex, maxIndex);
        }

        /// <summary>
        /// Handles when the <see cref="TextControl.Font"/> has changed.
        /// This is called immediately before <see cref="TextControl.FontChanged"/>.
        /// Override this method instead of using an event hook on <see cref="TextControl.FontChanged"/> when possible.
        /// </summary>
        protected override void OnFontChanged()
        {
            base.OnFontChanged();

            if (Font != null)
                _itemHeight = Font.GetLineSpacing();
        }

        /// <summary>
        /// Handles when the <see cref="Control.Size"/> of this <see cref="Control"/> has changed.
        /// This is called immediately before <see cref="Control.Resized"/>.
        /// Override this method instead of using an event hook on <see cref="Control.Resized"/> when possible.
        /// </summary>
        protected override void OnResized()
        {
            base.OnResized();

            UpdateButtonPositions();
        }

        /// <summary>
        /// Handles when the <see cref="ListBox{T}.SelectedIndex"/> changes.
        /// This is called immediately before <see cref="ListBox{T}.SelectedIndexChanged"/>.
        /// Override this method instead of using an event hook on <see cref="ListBox{T}.SelectedIndexChanged"/> when possible.
        /// </summary>
        protected virtual void OnSelectedIndexChanged()
        {
        }

        /// <summary>
        /// Handles when the <see cref="ListBox{T}.ShowPaging"/> changes.
        /// This is called immediately before <see cref="ListBox{T}.ShowPagingChanged"/>.
        /// Override this method instead of using an event hook on <see cref="ListBox{T}.ShowPagingChanged"/> when possible.
        /// </summary>
        protected virtual void OnShowPagingChanged()
        {
        }

        void UpdateButtonPositions()
        {
            _toolbarHeight = 6;
            if (_btnFirst == null)
                return;

            // Update the toolbar size
            if (_btnFirst.Size.Y > _toolbarHeight)
                _toolbarHeight = (int)_btnFirst.Size.Y;
            if (_btnPrev.Size.Y > _toolbarHeight)
                _toolbarHeight = (int)_btnPrev.Size.Y;
            if (_btnNext.Size.Y > _toolbarHeight)
                _toolbarHeight = (int)_btnNext.Size.Y;
            if (_btnLast.Size.Y > _toolbarHeight)
                _toolbarHeight = (int)_btnLast.Size.Y;

            _toolbarHeight += _toolbarPadding * 3;

            // Update the positions
            var cs = ClientSize;

            var firstSize = GetSpriteSize(_btnFirst);
            _btnFirst.Position = new Vector2(_toolbarPadding, cs.Y - _toolbarPadding - firstSize.Y);
            _btnPrev.Position = _btnFirst.Position + new Vector2(_toolbarPadding + firstSize.X, 0);

            var lastSize = GetSpriteSize(_btnLast);
            _btnLast.Position = cs - new Vector2(_toolbarPadding) - lastSize;
            _btnNext.Position = _btnLast.Position - new Vector2(_toolbarPadding + lastSize.X, 0);
        }

        /// <summary>
        /// A button that is on the <see cref="ListBox{T}"/>'s toolbar.
        /// </summary>
        class PagedListButton : PictureBox
        {
            readonly Action _clickAction;
            readonly string _spriteName;
            ISkinManager _skinManager;

            ISprite _sprite;
            ISprite _spriteMouseOver;

            /// <summary>
            /// Initializes a new instance of the <see cref="Control"/> class.
            /// </summary>
            /// <param name="parent">Parent <see cref="Control"/> of this <see cref="Control"/>.</param>
            /// <param name="spriteName">Name of the toolbar sprite.</param>
            /// <param name="clickAction">The action to perform when clicked.</param>
            /// <exception cref="ArgumentNullException"><paramref name="parent"/> is null.</exception>
            /// <exception cref="ArgumentNullException"><paramref name="spriteName"/> is null or empty.</exception>
            /// <exception cref="ArgumentNullException"><paramref name="clickAction"/> is null.</exception>
            public PagedListButton(Control parent, string spriteName, Action clickAction)
                : base(parent, Vector2.Zero, new Vector2(8))
            {
                if (clickAction == null)
                    throw new ArgumentNullException("clickAction");
                if (string.IsNullOrEmpty(spriteName))
                    throw new ArgumentNullException("spriteName");

                _spriteName = spriteName;
                _clickAction = clickAction;

                UpdateSprites(_skinManager);
            }

            /// <summary>
            /// When overridden in the derived class, loads the skinning information for the <see cref="Control"/>
            /// from the given <paramref name="skinManager"/>.
            /// </summary>
            /// <param name="skinManager">The <see cref="ISkinManager"/> to load the skinning information from.</param>
            public override void LoadSkin(ISkinManager skinManager)
            {
                _skinManager = skinManager;
                base.LoadSkin(skinManager);
                UpdateSprites(skinManager);
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

                _clickAction();
            }

            /// <summary>
            /// Handles when the mouse has entered the area of the <see cref="Control"/>.
            /// This is called immediately before <see cref="Control.OnMouseEnter"/>.
            /// Override this method instead of using an event hook on <see cref="Control.MouseEnter"/> when possible.
            /// </summary>
            /// <param name="e">The event args.</param>
            protected override void OnMouseEnter(MouseMoveEventArgs e)
            {
                base.OnMouseEnter(e);

                Sprite = _spriteMouseOver;
            }

            /// <summary>
            /// Handles when the mouse has left the area of the <see cref="Control"/>.
            /// This is called immediately before <see cref="Control.OnMouseLeave"/>.
            /// Override this method instead of using an event hook on <see cref="Control.MouseLeave"/> when possible.
            /// </summary>
            /// <param name="e">The event args.</param>
            protected override void OnMouseLeave(MouseMoveEventArgs e)
            {
                base.OnMouseLeave(e);

                Sprite = _sprite;
            }

            /// <summary>
            /// Updates the sprites to use for the button.
            /// </summary>
            /// <param name="skinManager">The skin manager.</param>
            void UpdateSprites(ISkinManager skinManager)
            {
                // Make sure the sprite name has been set
                if (_spriteName == null || skinManager == null)
                    return;

                _sprite = skinManager.GetControlSprite(_controlSkinName, _toolbarCategory, _spriteName);
                _spriteMouseOver = skinManager.GetControlSprite(_controlSkinName, _toolbarCategory,
                    _spriteName + _mouseOverSpriteSuffix);

                if (IsMouseEntered)
                    Sprite = _spriteMouseOver;
                else
                    Sprite = _sprite;
            }
        }
    }
}