using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// A control that displays a paginated list of items.
    /// </summary>
    /// <typeparam name="T">The type of items being displayed.</typeparam>
    public class PagedList<T> : TextControl
    {
        /// <summary>
        /// The name of this <see cref="Control"/> for when looking up the skin information.
        /// </summary>
        const string _controlSkinName = "PagedList";

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

        readonly SpriteControl _btnFirst;
        readonly SpriteControl _btnLast;
        readonly SpriteControl _btnNext;
        readonly SpriteControl _btnPrev;

        int _currentPage = 1;
        Action<SpriteBatch, Vector2, int> _itemDrawer;
        int _itemHeight = 12;
        IEnumerable<T> _items;
        StyledText _pageText = new StyledText("1/1");
        int _toolbarHeight = 8;

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedList{T}"/> class.
        /// </summary>
        /// <param name="parent">Parent <see cref="Control"/> of this <see cref="Control"/>.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <param name="clientSize">The size of the <see cref="Control"/>'s client area.</param>
        /// <exception cref="NullReferenceException"><paramref name="parent"/> is null.</exception>
        public PagedList(Control parent, Vector2 position, Vector2 clientSize) : base(parent, position, clientSize)
        {
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            if (Font != null)
                ItemHeight = Font.LineSpacing;
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
        /// Initializes a new instance of the <see cref="PagedList{T}"/> class.
        /// </summary>
        /// <param name="guiManager">The GUI manager this <see cref="Control"/> will be managed by.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <param name="clientSize">The size of the <see cref="Control"/>'s client area.</param>
        /// <exception cref="ArgumentNullException"><paramref name="guiManager"/> is null.</exception>
        public PagedList(IGUIManager guiManager, Vector2 position, Vector2 clientSize) : base(guiManager, position, clientSize)
        {
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            if (Font != null)
                ItemHeight = Font.LineSpacing;
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
        /// Gets or sets the current page. If the value is set to greater than the total number of pages, the last page
        /// will be used instead. If the value is less than 1, the first page will be used instead.
        /// </summary>
        public int CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = Math.Max(1, Math.Min(value, NumPages));

                string s = _currentPage + "/" + NumPages;
                if (!StringComparer.OrdinalIgnoreCase.Equals(_pageText.Text, s))
                    _pageText = new StyledText(s);
            }
        }

        /// <summary>
        /// Gets or sets how to draw items in the list.
        /// </summary>
        public Action<SpriteBatch, Vector2, int> ItemDrawer
        {
            get { return _itemDrawer; }
            set { _itemDrawer = value ?? GetDefaultItemDrawer(); }
        }

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
        public IEnumerable<T> Items
        {
            get { return _items; }
            set
            {
                if (_items == value)
                    return;

                _items = value;
                CurrentPage = CurrentPage;
            }
        }

        /// <summary>
        /// Gets the number of items that can be displayed per page.
        /// </summary>
        public int ItemsPerPage
        {
            get
            {
                var listHeight = ClientSize.Y - _toolbarHeight;
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

                return (int)Math.Ceiling((float)items.Count() / ItemsPerPage);
            }
        }

        /// <summary>
        /// Handles when the <see cref="TextControl.Font"/> has changed.
        /// This is called immediately before <see cref="TextControl.OnChangeFont"/>.
        /// Override this method instead of using an event hook on <see cref="TextControl.OnChangeFont"/> when possible.
        /// </summary>
        protected override void OnFontChanged()
        {
            base.OnFontChanged();

            if (Font != null)
                _itemHeight = Font.LineSpacing;
        }

        /// <summary>
        /// Draws the <see cref="Control"/>.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> to draw to.</param>
        protected override void DrawControl(SpriteBatch spriteBatch)
        {
            // Border
            var sp = ScreenPosition;
            var size = Size;
            var rect = new Rectangle((int)sp.X, (int)sp.Y, (int)size.X, (int)size.Y - _toolbarHeight);
            Border.Draw(spriteBatch, rect);

            // Draw the items
            DrawItems(spriteBatch);

            // Draw the page number
            var cs = ClientSize;
            var pagePos = new Vector2((cs.X / 2) - (_pageText.GetWidth(Font) / 2), cs.Y - _toolbarHeight + (_toolbarPadding * 2));
            _pageText.Draw(spriteBatch, Font, sp + pagePos, ForeColor);
        }

        void DrawItems(SpriteBatch spriteBatch)
        {
            if (Items == null)
                return;

            var pos = ScreenPosition + new Vector2(Border.LeftWidth, Border.TopHeight);
            int ipp = ItemsPerPage;
            int offset = (CurrentPage - 1) * ipp;
            int ih = ItemHeight;
            int count = Items.Count();

            for (int i = offset; i < offset + ipp && i < count; i++)
            {
                ItemDrawer(spriteBatch, pos, i);
                pos += new Vector2(0, ih);
            }
        }

        /// <summary>
        /// Gets the default item drawer.
        /// </summary>
        /// <returns>The default item drawer.</returns>
        protected virtual Action<SpriteBatch, Vector2, int> GetDefaultItemDrawer()
        {
            return (sb, p, v) => sb.DrawString(Font, Items.ElementAtOrDefault(v).ToString(), p, ForeColor);
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
        /// Handles when the <see cref="Control.Size"/> of this <see cref="Control"/> has changed.
        /// This is called immediately before <see cref="Control.Resized"/>.
        /// Override this method instead of using an event hook on <see cref="Control.Resized"/> when possible.
        /// </summary>
        protected override void OnResized()
        {
            base.OnResized();

            UpdateButtonPositions();
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
        /// A button that is on the <see cref="PagedList{T}"/>'s toolbar.
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
            /// <exception cref="NullReferenceException"><paramref name="parent"/> is null.</exception>
            /// <exception cref="NullReferenceException"><paramref name="spriteName"/> is null.</exception>
            /// <exception cref="NullReferenceException"><paramref name="clickAction"/> is null.</exception>
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
            /// Handles when this <see cref="Control"/> was clicked.
            /// This is called immediately before <see cref="Control.OnClick"/>.
            /// Override this method instead of using an event hook on <see cref="Control.OnClick"/> when possible.
            /// </summary>
            /// <param name="e">The event args.</param>
            protected override void OnClick(MouseClickEventArgs e)
            {
                base.OnClick(e);

                _clickAction();
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
            /// Handles when the mouse has entered the area of the <see cref="Control"/>.
            /// This is called immediately before <see cref="Control.OnMouseEnter"/>.
            /// Override this method instead of using an event hook on <see cref="Control.MouseEnter"/> when possible.
            /// </summary>
            /// <param name="e">The event args.</param>
            protected override void OnMouseEnter(MouseEventArgs e)
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
            protected override void OnMouseLeave(MouseEventArgs e)
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