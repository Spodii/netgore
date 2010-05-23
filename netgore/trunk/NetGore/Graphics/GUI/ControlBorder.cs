using System.Linq;
using SFML.Graphics;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Defines and draws the border of a control
    /// </summary>
    public class ControlBorder
    {
        static readonly ControlBorder _empty;

        /// <summary>
        /// Bottom border
        /// </summary>
        ISprite _b = null;

        /// <summary>
        /// Background
        /// </summary>
        ISprite _bg = null;

        /// <summary>
        /// Bottom-left border
        /// </summary>
        ISprite _bl = null;

        /// <summary>
        /// Bottom-right border
        /// </summary>
        ISprite _br = null;

        /// <summary>
        /// Contains a cached value of whether or not the border can be drawn so we only have to do a quick boolean
        /// check each time we draw instead of a more extensive and expensive check.
        /// </summary>
        bool _canDraw = false;

        /// <summary>
        /// Left border
        /// </summary>
        ISprite _l = null;

        /// <summary>
        /// Right border
        /// </summary>
        ISprite _r = null;

        /// <summary>
        /// Top border
        /// </summary>
        ISprite _t = null;

        /// <summary>
        /// Top-left border
        /// </summary>
        ISprite _tl = null;

        /// <summary>
        /// Top-right border
        /// </summary>
        ISprite _tr = null;

        /// <summary>
        /// Initializes the <see cref="ControlBorder"/> class.
        /// </summary>
        static ControlBorder()
        {
            _empty = new ControlBorder(null, null, null, null, null, null, null, null, null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlBorder"/> class.
        /// </summary>
        /// <param name="source">ControlBorder to copy from</param>
        public ControlBorder(ControlBorder source)
            : this(source._tl, source._t, source._tr, source._r, source._br, source._b, source._bl, source._l, source._bg)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlBorder"/> class.
        /// </summary>
        /// <param name="topLeft">Source of the top-left corner</param>
        /// <param name="top">Source of the top side</param>
        /// <param name="topRight">Source of the top-right corner</param>
        /// <param name="right">Source of the right side</param>
        /// <param name="bottomRight">Source of the bottom-right corner</param>
        /// <param name="bottom">Source of the bottom side</param>
        /// <param name="bottomLeft">Source of the bottom-left corner</param>
        /// <param name="left">Source of the left side</param>
        /// <param name="background">Source of the background</param>
        public ControlBorder(ISprite topLeft, ISprite top, ISprite topRight, ISprite right, ISprite bottomRight, ISprite bottom,
                             ISprite bottomLeft, ISprite left, ISprite background)
        {
            _tl = topLeft;
            _t = top;
            _tr = topRight;
            _r = right;
            _br = bottomRight;
            _b = bottom;
            _bl = bottomLeft;
            _l = left;
            _bg = background;

            UpdateCanDraw();
        }

        /// <summary>
        /// Gets the height of the bottom side of the border
        /// </summary>
        public int BottomHeight
        {
            get
            {
                // If the ISprite is null, return a 0
                if (_b == null)
                    return 0;

                return _b.Source.Height;
            }
        }

        /// <summary>
        /// Gets the default empty <see cref="ControlBorder"/>.
        /// </summary>
        public static ControlBorder Empty
        {
            get { return _empty; }
        }

        /// <summary>
        /// Gets the height of both the top and bottom border sides
        /// </summary>
        public int Height
        {
            get { return TopHeight + BottomHeight; }
        }

        /// <summary>
        /// Gets the width of the left side of the border
        /// </summary>
        public int LeftWidth
        {
            get
            {
                // If the ISprite is null, return a 0
                if (_l == null)
                    return 0;

                return _l.Source.Width;
            }
        }

        /// <summary>
        /// Gets the width of the right side of the border
        /// </summary>
        public int RightWidth
        {
            get
            {
                // If the ISprite is null, return a 0
                if (_r == null)
                    return 0;

                return _r.Source.Width;
            }
        }

        /// <summary>
        /// Gets a Vector2 that represents the Width and Height of all sides of the border.
        /// </summary>
        public Vector2 Size
        {
            get { return new Vector2(Width, Height); }
        }

        /// <summary>
        /// Gets or sets the source for the background
        /// </summary>
        public ISprite SourceBackground
        {
            get { return _bg; }
            set { _bg = value; }
        }

        /// <summary>
        /// Gets or sets the source for the bottom side of the bottom
        /// </summary>
        public ISprite SourceBottom
        {
            get { return _b; }
            set
            {
                _b = value;
                UpdateCanDraw();
            }
        }

        /// <summary>
        /// Gets or sets the source for the bottom-left corner of the border
        /// </summary>
        public ISprite SourceBottomLeft
        {
            get { return _bl; }
            set
            {
                _bl = value;
                UpdateCanDraw();
            }
        }

        /// <summary>
        /// Gets or sets the source for the bottom-right corner of the border
        /// </summary>
        public ISprite SourceBottomRight
        {
            get { return _br; }
            set
            {
                _br = value;
                UpdateCanDraw();
            }
        }

        /// <summary>
        /// Gets or sets the source for the left side of the border
        /// </summary>
        public ISprite SourceLeft
        {
            get { return _l; }
            set
            {
                _l = value;
                UpdateCanDraw();
            }
        }

        /// <summary>
        /// Gets or sets the source for the right side of the border
        /// </summary>
        public ISprite SourceRight
        {
            get { return _r; }
            set
            {
                _r = value;
                UpdateCanDraw();
            }
        }

        /// <summary>
        /// Gets or sets the source for the top side of the border
        /// </summary>
        public ISprite SourceTop
        {
            get { return _t; }
            set
            {
                _t = value;
                UpdateCanDraw();
            }
        }

        /// <summary>
        /// Gets or sets the source for the top-left corner of the border
        /// </summary>
        public ISprite SourceTopLeft
        {
            get { return _tl; }
            set
            {
                _tl = value;
                UpdateCanDraw();
            }
        }

        /// <summary>
        /// Gets or sets the source for the top-right corner of the border
        /// </summary>
        public ISprite SourceTopRight
        {
            get { return _tr; }
            set
            {
                _tr = value;
                UpdateCanDraw();
            }
        }

        /// <summary>
        /// Gets the height of the top side of the border
        /// </summary>
        public int TopHeight
        {
            get
            {
                // If the ISprite is null, return a 0
                if (_t == null)
                    return 0;

                return _t.Source.Height;
            }
        }

        /// <summary>
        /// Gets the width of both the left and right border sides
        /// </summary>
        public int Width
        {
            get { return LeftWidth + RightWidth; }
        }

        /// <summary>
        /// Draws the <see cref="ControlBorder"/> to the specified region.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to draw with.</param>
        /// <param name="region">Region to draw the <see cref="ControlBorder"/> to. These values represent
        /// the absolute screen position.</param>
        public void Draw(ISpriteBatch sb, Rectangle region)
        {
            Draw(sb, region, Color.White);
        }

        /// <summary>
        /// Draws the <see cref="ControlBorder"/> to the specified region.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to draw with.</param>
        /// <param name="region">Region to draw the <see cref="ControlBorder"/> to. These values represent
        /// the absolute screen position.</param>
        /// <param name="color">The <see cref="Color"/> to use to draw the <see cref="ControlBorder"/>.</param>
        public void Draw(ISpriteBatch sb, Rectangle region, Color color)
        {
            if (!_canDraw)
                return;

            var tSrc = _t.Source;
            var tlSrc = _tl.Source;
            var trSrc = _tr.Source;
            var lSrc = _l.Source;
            var rSrc = _r.Source;
            var bSrc = _b.Source;
            var blSrc = _bl.Source;
            var brSrc = _br.Source;

            // Set some values we will be using extensively
            var cX = region.X;
            var cY = region.Y;
            var cW = region.Width;
            var cH = region.Height;

            Rectangle r;

            // Background (draw first to ensure it stays behind the border in case something goes wrong)
            if (_bg != null)
            {
                r = new Rectangle(cX + lSrc.Width, cY + tSrc.Height, cW - lSrc.Width - rSrc.Width, cH - tSrc.Height - bSrc.Height);
                _bg.Draw(sb, r, color);
            }

            // Top side
            r = new Rectangle(cX + tlSrc.Width, cY, cW - tlSrc.Width - trSrc.Width, tSrc.Height);
            _t.Draw(sb, r, color);

            // Left side
            r = new Rectangle(cX, cY + tlSrc.Height, lSrc.Width, cH - tlSrc.Height - blSrc.Height);
            _l.Draw(sb, r, color);

            // Right side
            r = new Rectangle(cX + cW - rSrc.Width, cY + trSrc.Height, rSrc.Width, cH - trSrc.Height - brSrc.Height);
            _r.Draw(sb, r, color);

            // Bottom side
            r = new Rectangle(cX + _bl.Source.Width, cY + cH - bSrc.Height, cW - blSrc.Width - brSrc.Width, bSrc.Height);
            _b.Draw(sb, r, color);

            // Top-left corner
            r = new Rectangle(cX, cY, tlSrc.Width, tlSrc.Height);
            _tl.Draw(sb, r, color);

            // Top-right corner
            r = new Rectangle(cX + cW - trSrc.Width, cY, trSrc.Width, trSrc.Height);
            _tr.Draw(sb, r, color);

            // Bottom-left corner
            r = new Rectangle(cX, cY + cH - blSrc.Height, blSrc.Width, blSrc.Height);
            _bl.Draw(sb, r, color);

            // Bottom-right corner
            r = new Rectangle(cX + cW - brSrc.Width, cY + cH - brSrc.Height, brSrc.Width, brSrc.Height);
            _br.Draw(sb, r, color);
        }

        /// <summary>
        /// Draws the <see cref="ControlBorder"/> to a given <see cref="Control"/>.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to draw with.</param>
        /// <param name="c">Control to draw to.</param>
        public void Draw(ISpriteBatch sb, Control c)
        {
            if (!_canDraw)
                return;

            var sp = c.ScreenPosition;
            var region = new Rectangle((int)sp.X, (int)sp.Y, (int)c.Size.X, (int)c.Size.Y);
            Draw(sb, region, c.BorderColor);
        }

        /// <summary>
        /// Updates the <see cref="_canDraw"/> value.
        /// </summary>
        void UpdateCanDraw()
        {
            _canDraw = (_t != null && _tl != null && _tr != null && _l != null && _r != null && _b != null && _bl != null &&
                        _br != null);
        }
    }
}