using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics.GUI
{
    public class PagedList<T> : TextControl
    {
        /// <summary>
        /// The name of this <see cref="Control"/> for when looking up the skin information.
        /// </summary>
        const string _controlSkinName = "PagedList";

        const string _toolbarCategory = "Toolbar";
        int _toolbarHeight = 8;

        SpriteControl _btnFirst;
        SpriteControl _btnLast;
        SpriteControl _btnNext;
        SpriteControl _btnPrev;
        int _currentPage = 1;
        Action<SpriteBatch, Vector2, T> _itemDrawer;
        int _itemHeight = 12;
        ISkinManager _skinManager;

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
            // ReSharper restore DoNotCallOverridableMethodsInConstructor

            _itemDrawer = GetDefaultItemDrawer();

            CreateButtons();
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
            // ReSharper restore DoNotCallOverridableMethodsInConstructor

            _itemDrawer = GetDefaultItemDrawer();

            CreateButtons();
        }

        public int CurrentPage
        {
            get { return _currentPage; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value");

                var newValue = Math.Min(value, NumPages);
                if (_currentPage == newValue)
                    return;

                _currentPage = newValue;
                _pageText = new StyledText(_currentPage + "/" + NumPages);
            }
        }

        /// <summary>
        /// Gets or sets how to draw items in the list.
        /// </summary>
        public Action<SpriteBatch, Vector2, T> ItemDrawer
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
        public IList<T> Items { get; set; }

        public int ItemsPerPage
        {
            get
            {
                var listHeight = ClientSize.Y - _toolbarHeight;
                var value = listHeight / ItemHeight;
                return (int)Math.Max(1, value);
            }
        }

        public int NumPages
        {
            get
            {
                var items = Items;
                if (items == null)
                    return 1;

                return items.Count() / ItemsPerPage;
            }
        }

        /// <summary>
        /// Handles when the <see cref="TextControl.Font"/> has changed.
        /// This is called immediately before <see cref="TextControl.OnChangeFont"/>.
        /// Override this method instead of using an event hook on <see cref="TextControl.OnChangeFont"/> when possible.
        /// </summary>
        protected override void ChangeFont()
        {
            base.ChangeFont();

            if (Font != null)
                _itemHeight = Font.LineSpacing;
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

        const int _toolbarPadding = 2;

        void UpdateButtonPositions()
        {
            CreateButtons();

            var cs = ClientSize;

            var firstSize = GetSpriteSize(_btnFirst);
            _btnFirst.Position = new Vector2(_toolbarPadding, cs.Y - _toolbarPadding - firstSize.Y);
            _btnPrev.Position = _btnFirst.Position + new Vector2(_toolbarPadding + firstSize.X, 0);

            var lastSize = GetSpriteSize(_btnLast);
            _btnLast.Position = cs - new Vector2(_toolbarPadding) - lastSize;
            _btnNext.Position = _btnLast.Position - new Vector2(_toolbarPadding + lastSize.X, 0);
        }

        void CreateButtons()
        {
            if (_btnFirst != null)
                return;

            Vector2 defaultPos = new Vector2(0);
            Vector2 defaultSize = new Vector2(8);

            _btnFirst = new PictureBox(this, defaultPos, defaultSize);
            _btnPrev = new PictureBox(this, defaultPos, defaultSize);
            _btnNext = new PictureBox(this, defaultPos, defaultSize);
            _btnLast = new PictureBox(this, defaultPos, defaultSize);

            UpdateButtonSprites(_skinManager);
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

        StyledText _pageText = new StyledText("1/1");

        void DrawItems(SpriteBatch spriteBatch)
        {
            var pos = ScreenPosition + new Vector2(Border.LeftWidth, Border.TopHeight);
            int ipp = ItemsPerPage;
            int offset = (CurrentPage - 1) * ipp;
            int ih = ItemHeight;

            for (int i = offset; i < offset + ipp && i < Items.Count; i++)
            {
                var item = Items[i];
                ItemDrawer(spriteBatch, pos, item);
                pos += new Vector2(0, ih);
            }
        }

        /// <summary>
        /// Gets the default item drawer.
        /// </summary>
        /// <returns>The default item drawer.</returns>
        Action<SpriteBatch, Vector2, T> GetDefaultItemDrawer()
        {
            return (sb, p, v) => sb.DrawString(Font, v.ToString(), p, ForeColor);
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

            UpdateButtonSprites(skinManager);
        }

        void UpdateButtonSprites(ISkinManager skinManager)
        {
            CreateButtons();

            _btnFirst.Sprite = skinManager.GetControlSprite(_controlSkinName, _toolbarCategory, "First");
            _btnPrev.Sprite = skinManager.GetControlSprite(_controlSkinName, _toolbarCategory, "Previous");
            _btnNext.Sprite = skinManager.GetControlSprite(_controlSkinName, _toolbarCategory, "Next");
            _btnLast.Sprite = skinManager.GetControlSprite(_controlSkinName, _toolbarCategory, "Last");

            _toolbarHeight = 6;
            if (_btnFirst.Size.Y > _toolbarHeight)
                _toolbarHeight = (int)_btnFirst.Size.Y;
            if (_btnPrev.Size.Y > _toolbarHeight)
                _toolbarHeight = (int)_btnPrev.Size.Y;
            if (_btnNext.Size.Y > _toolbarHeight)
                _toolbarHeight = (int)_btnNext.Size.Y;
            if (_btnLast.Size.Y > _toolbarHeight)
                _toolbarHeight = (int)_btnLast.Size.Y;

            _toolbarHeight += _toolbarPadding * 3;

            UpdateButtonPositions();
        }

        /// <summary>
        /// Handles when the <see cref="Control.Size"/> of this <see cref="Control"/> has changed.
        /// This is called immediately before <see cref="Control.OnResize"/>.
        /// Override this method instead of using an event hook on <see cref="Control.OnResize"/> when possible.
        /// </summary>
        protected override void Resize()
        {
            base.Resize();

            UpdateButtonPositions();
        }
    }
}