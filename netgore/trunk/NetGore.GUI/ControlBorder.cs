using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Defines and draws the border of a control
    /// </summary>
    public class ControlBorder
    {
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
        /// Color to draw the ISprites with
        /// </summary>
        Color _color = Color.White;

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
        /// Gets the color to draw with
        /// </summary>
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
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
            set { _b = value; }
        }

        /// <summary>
        /// Gets or sets the source for the bottom-left corner of the border
        /// </summary>
        public ISprite SourceBottomLeft
        {
            get { return _bl; }
            set { _bl = value; }
        }

        /// <summary>
        /// Gets or sets the source for the bottom-right corner of the border
        /// </summary>
        public ISprite SourceBottomRight
        {
            get { return _br; }
            set { _br = value; }
        }

        /// <summary>
        /// Gets or sets the source for the left side of the border
        /// </summary>
        public ISprite SourceLeft
        {
            get { return _l; }
            set { _l = value; }
        }

        /// <summary>
        /// Gets or sets the source for the right side of the border
        /// </summary>
        public ISprite SourceRight
        {
            get { return _r; }
            set { _r = value; }
        }

        /// <summary>
        /// Gets or sets the source for the top side of the border
        /// </summary>
        public ISprite SourceTop
        {
            get { return _t; }
            set { _t = value; }
        }

        /// <summary>
        /// Gets or sets the source for the top-left corner of the border
        /// </summary>
        public ISprite SourceTopLeft
        {
            get { return _tl; }
            set { _tl = value; }
        }

        /// <summary>
        /// Gets or sets the source for the top-right corner of the border
        /// </summary>
        public ISprite SourceTopRight
        {
            get { return _tr; }
            set { _tr = value; }
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
        /// ControlBorder constructor
        /// </summary>
        /// <param name="source">ControlBorder to copy from</param>
        public ControlBorder(ControlBorder source)
            : this(source._tl, source._t, source._tr, source._r, source._br, source._b, source._bl, source._l, source._bg)
        {
        }

        /// <summary>
        /// ControlBorder constructor
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
        }

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

         /// <summary>
        /// Draws the <see cref="ControlBorder"/> to the specified region.
        /// </summary>
        /// <param name="sb"><see cref="SpriteBatch"/> to draw with.</param>
        /// <param name="region">Region to draw the <see cref="ControlBorder"/> to. These values represent
        /// the absolute screen position.</param>
        public void Draw(SpriteBatch sb, Rectangle region)
        {
            // Don't draw if any of the sprites are null
            if (_t == null || _tl == null || _tr == null || _l == null || _r == null || _b == null || _bl == null || _br == null)
            {
                const string errmsg = "Failed to draw ControlBorder `{0}` - one or more border sprites missing.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this);
                return;
            }

            Rectangle tSrc = _t.Source;
            Rectangle tlSrc = _tl.Source;
            Rectangle trSrc = _tr.Source;
            Rectangle lSrc = _l.Source;
            Rectangle rSrc = _r.Source;
            Rectangle bSrc = _b.Source;
            Rectangle blSrc = _bl.Source;
            Rectangle brSrc = _br.Source;

            // Set some values we will be using extensively
            int cX = region.X;
            int cY = region.Y;
            int cW = region.Width;
            int cH = region.Height;

            Rectangle r;

            // Background (draw first to ensure it stays behind the border in case something goes wrong)
            if (_bg != null)
            {
                r = new Rectangle(cX + lSrc.Width, cY + tSrc.Height, cW - lSrc.Width - rSrc.Width, cH - tSrc.Height - bSrc.Height);
                _bg.Draw(sb, r, _color);
            }

            // Top side
            r = new Rectangle(cX + tlSrc.Width, cY, cW - tlSrc.Width - trSrc.Width, tSrc.Height);
            _t.Draw(sb, r, _color);

            // Left side
            r = new Rectangle(cX, cY + tlSrc.Height, lSrc.Width, cH - tlSrc.Height - blSrc.Height);
            _l.Draw(sb, r, _color);

            // Right side
            r = new Rectangle(cX + cW - rSrc.Width, cY + trSrc.Height, rSrc.Width, cH - trSrc.Height - brSrc.Height);
            _r.Draw(sb, r, _color);

            // Bottom side
            r = new Rectangle(cX + _bl.Source.Width, cY + cH - bSrc.Height, cW - blSrc.Width - brSrc.Width, bSrc.Height);
            _b.Draw(sb, r, _color);

            // Top-left corner
            r = new Rectangle(cX, cY, tlSrc.Width, tlSrc.Height);
            _tl.Draw(sb, r, _color);

            // Top-right corner
            r = new Rectangle(cX + cW - trSrc.Width, cY, trSrc.Width, trSrc.Height);
            _tr.Draw(sb, r, _color);

            // Bottom-left corner
            r = new Rectangle(cX, cY + cH - blSrc.Height, blSrc.Width, blSrc.Height);
            _bl.Draw(sb, r, _color);

            // Bottom-right corner
            r = new Rectangle(cX + cW - brSrc.Width, cY + cH - brSrc.Height, brSrc.Width, brSrc.Height);
            _br.Draw(sb, r, _color);
        }

        /// <summary>
        /// Draws the <see cref="ControlBorder"/> to a given <see cref="Control"/>.
        /// </summary>
        /// <param name="sb"><see cref="SpriteBatch"/> to draw with.</param>
        /// <param name="c">Control to draw to.</param>
        public void Draw(SpriteBatch sb, Control c)
        {
            Vector2 sp = c.ScreenPosition;
            Rectangle region = new Rectangle((int)sp.X, (int)sp.Y, (int)c.Size.X, (int)c.Size.Y);
            Draw(sb, region);
        }
    }
}