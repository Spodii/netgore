using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        /// <summary>
        /// Draws the ControlBorder to a given control
        /// </summary>
        /// <param name="sb">SpriteBatch to draw with</param>
        /// <param name="c">Control to draw to</param>
        public void Draw(SpriteBatch sb, Control c)
        {
            // Don't draw if any of the sprites are null
            if (_t == null || _tl == null || _tr == null || _l == null || _r == null || _b == null || _bl == null || _br == null)
                return;

            // Set some values we will be using extensively
            Vector2 sp = c.ScreenPosition;
            int cX = (int)sp.X;
            int cY = (int)sp.Y;
            int cW = (int)c.Size.X;
            int cH = (int)c.Size.Y;

            Rectangle r;

            // Background (draw first to ensure it stays behind the border in case something goes wrong)
            if (_bg != null)
            {
                r = new Rectangle(cX + _l.Source.Width, cY + _t.Source.Height, cW - _l.Source.Width - _r.Source.Width,
                                  cH - _t.Source.Height - _b.Source.Height);
                _bg.Draw(sb, r, _color);
            }

            // Top side
            r = new Rectangle(cX + _tl.Source.Width, cY, cW - _tl.Source.Width - _tr.Source.Width, _t.Source.Height);
            _t.Draw(sb, r, _color);

            // Left side
            r = new Rectangle(cX, cY + _tl.Source.Height, _l.Source.Width, cH - _tl.Source.Height - _bl.Source.Height);
            _l.Draw(sb, r, _color);

            // Right side
            r = new Rectangle(cX + cW - _r.Source.Width, cY + _tr.Source.Height, _r.Source.Width,
                              cH - _tr.Source.Height - _br.Source.Height);
            _r.Draw(sb, r, _color);

            // Bottom side
            r = new Rectangle(cX + _bl.Source.Width, cY + cH - _b.Source.Height, cW - _bl.Source.Width - _br.Source.Width,
                              _b.Source.Height);
            _b.Draw(sb, r, _color);

            // Top-left corner
            r = new Rectangle(cX, cY, _tl.Source.Width, _tl.Source.Height);
            _tl.Draw(sb, r, _color);

            // Top-right corner
            r = new Rectangle(cX + cW - _tr.Source.Width, cY, _tr.Source.Width, _tr.Source.Height);
            _tr.Draw(sb, r, _color);

            // Bottom-left corner
            r = new Rectangle(cX, cY + cH - _bl.Source.Height, _bl.Source.Width, _bl.Source.Height);
            _bl.Draw(sb, r, _color);

            // Bottom-right corner
            r = new Rectangle(cX + cW - _br.Source.Width, cY + cH - _br.Source.Height, _br.Source.Width, _br.Source.Height);
            _br.Draw(sb, r, _color);
        }
    }
}