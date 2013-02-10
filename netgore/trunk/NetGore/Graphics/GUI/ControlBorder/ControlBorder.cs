using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;
using SFML.Graphics;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Defines and draws the border of a control
    /// </summary>
    public class ControlBorder
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The maximum draw calls that are to be made before a Debug.Error() is produced. This is an indicated that
        /// the <see cref="ControlBorder"/> is simply drawing way too much and that you either are using 
        /// <see cref="ControlBorderDrawStyle.Tile"/> when you meant to use <see cref="ControlBorderDrawStyle.Stretch"/>,
        /// your <see cref="ControlBorder"/> is larger than anticipated, or that the <see cref="ISprite"/> used to draw
        /// the border is too small. This is not an error, but rather more of a warning, and only shows in debug.
        /// </summary>
        const int _maxDrawCallsBeforeDebugWarning = 30;

        static readonly object _drawSync = new object();
        static readonly ControlBorder _empty;
        static readonly List<Func<Control, Color, Color>> _globalColorTransformations = new List<Func<Control, Color, Color>>();
        static readonly object _globalColorTransformationsSync = new object();
        static readonly int _numSprites;

        readonly ControlBorderDrawStyle[] _drawStyles;
        readonly ISprite[] _sprites;

        /// <summary>
        /// Contains a cached value of whether or not the border can be drawn so we only have to do a quick boolean
        /// check each time we draw instead of a more extensive and expensive check.
        /// </summary>
        bool _canDraw = false;

        /// <summary>
        /// Initializes the <see cref="ControlBorder"/> class.
        /// </summary>
        static ControlBorder()
        {
            _numSprites = EnumHelper<ControlBorderSpriteType>.MaxValue + 1;
            _empty = new ControlBorder(null, null, null, null, null, null, null, null, null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlBorder"/> class.
        /// </summary>
        /// <param name="source">ControlBorder to copy from</param>
        public ControlBorder(ControlBorder source)
        {
            _sprites = new ISprite[_numSprites];
            _drawStyles = new ControlBorderDrawStyle[_numSprites];

            Array.Copy(source._sprites, 0, _sprites, 0, _sprites.Length);
            Array.Copy(source._drawStyles, 0, _drawStyles, 0, _drawStyles.Length);
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
            _sprites = new ISprite[_numSprites];
            _drawStyles = new ControlBorderDrawStyle[_numSprites];

            SetSprite(ControlBorderSpriteType.Background, background);
            SetSprite(ControlBorderSpriteType.Bottom, bottom);
            SetSprite(ControlBorderSpriteType.BottomLeft, bottomLeft);
            SetSprite(ControlBorderSpriteType.BottomRight, bottomRight);
            SetSprite(ControlBorderSpriteType.Left, left);
            SetSprite(ControlBorderSpriteType.Right, right);
            SetSprite(ControlBorderSpriteType.Top, top);
            SetSprite(ControlBorderSpriteType.TopLeft, topLeft);
            SetSprite(ControlBorderSpriteType.TopRight, topRight);

            UpdateCanDraw();
        }

        /// <summary>
        /// Gets the height of the bottom side of the border
        /// </summary>
        public int BottomHeight
        {
            get
            {
                var s = GetSprite(ControlBorderSpriteType.Bottom);

                // If the ISprite is null, return a 0
                if (s == null)
                    return 0;

                return s.Source.Height;
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
                var s = GetSprite(ControlBorderSpriteType.Left);

                // If the ISprite is null, return a 0
                if (s == null)
                    return 0;

                return s.Source.Width;
            }
        }

        /// <summary>
        /// Gets the width of the right side of the border
        /// </summary>
        public int RightWidth
        {
            get
            {
                var s = GetSprite(ControlBorderSpriteType.Right);

                // If the ISprite is null, return a 0
                if (s == null)
                    return 0;

                return s.Source.Width;
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
        /// Gets the height of the top side of the border
        /// </summary>
        public int TopHeight
        {
            get
            {
                var s = GetSprite(ControlBorderSpriteType.Top);

                // If the ISprite is null, return a 0
                if (s == null)
                    return 0;

                return s.Source.Height;
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
        /// Adds a <see cref="Func{T,U}"/> that can be used to apply a transformation on the <see cref="Color"/> of a
        /// <see cref="ControlBorder"/> for a <see cref="Control"/> for every <see cref="ControlBorder"/> being drawn.
        /// </summary>
        /// <param name="transformer">The transformation.</param>
        public static void AddGlobalColorTransformation(Func<Control, Color, Color> transformer)
        {
            lock (_globalColorTransformationsSync)
            {
                if (!_globalColorTransformations.Contains(transformer))
                    _globalColorTransformations.Add(transformer);
            }
        }

        /// <summary>
        /// Checks that the amount being drawn for a tiled draw call is a sane amount.
        /// </summary>
        /// <param name="min">The starting draw coordinate.</param>
        /// <param name="max">The ending draw coordinate.</param>
        /// <param name="spriteSize">The size of the sprite being drawn.</param>
        /// <param name="side">The <see cref="ControlBorderSpriteType"/>.</param>
        [Conditional("DEBUG")]
        void AssertSaneDrawTilingAmount(int min, int max, float spriteSize, ControlBorderSpriteType side)
        {
            const string errmsg =
                "Too many draw calls being made to draw the ControlBorder `{0}` on side `{1}` - performance will suffer." +
                " Either use a larger sprite, or use ControlBorderDrawStyle.Stretch instead of ControlBorderDrawStyle.Tile." +
                " This may also be an indication that you are using ControlBorderDrawStyle.Tile unintentionally.";

            var drawCalls = (int)((max - min) / spriteSize);
            if (drawCalls <= _maxDrawCallsBeforeDebugWarning)
                return;

            if (log.IsWarnEnabled)
                log.WarnFormat(errmsg, this, side);

            Debug.Fail(string.Format(errmsg, this, side));
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

            var _bg = GetSprite(ControlBorderSpriteType.Background);
            var _t = GetSprite(ControlBorderSpriteType.Top);
            var _tl = GetSprite(ControlBorderSpriteType.TopLeft);
            var _tr = GetSprite(ControlBorderSpriteType.TopRight);
            var _l = GetSprite(ControlBorderSpriteType.Left);
            var _r = GetSprite(ControlBorderSpriteType.Right);
            var _b = GetSprite(ControlBorderSpriteType.Bottom);
            var _bl = GetSprite(ControlBorderSpriteType.BottomLeft);
            var _br = GetSprite(ControlBorderSpriteType.BottomRight);

            var bSrc = _b.Source;
            var tSrc = _t.Source;
            var tlSrc = _tl.Source;
            var trSrc = _tr.Source;
            var lSrc = _l.Source;
            var rSrc = _r.Source;
            var blSrc = _bl.Source;
            var brSrc = _br.Source;

            // Set some values we will be using extensively
            var cX = region.X;
            var cY = region.Y;
            var cW = region.Width;
            var cH = region.Height;

            lock (_drawSync)
            {
                // Background (draw first to ensure it stays behind the border in case something goes wrong)
                Rectangle r;
                if (_bg != null)
                {
                    r = new Rectangle(cX + lSrc.Width, cY + tSrc.Height, cW - lSrc.Width - rSrc.Width, cH - tSrc.Height - bSrc.Height);

                    switch (GetDrawStyle(ControlBorderSpriteType.Background))
                    {
                        case ControlBorderDrawStyle.Stretch:
                            _bg.Draw(sb, r, Color.White);
                            break;

                        case ControlBorderDrawStyle.Tile:
                            AssertSaneDrawTilingAmount(r.X, r.Right, _bg.Size.X, ControlBorderSpriteType.Background);
                            AssertSaneDrawTilingAmount(r.Y, r.Bottom, _bg.Size.Y, ControlBorderSpriteType.Background);
                            sb.DrawTiledXY(r.X, r.Right, r.Y, r.Bottom, _bg, Color.White);
                            break;
                    }
                }

                // Top side
                r = new Rectangle(cX + tlSrc.Width, cY, cW - tlSrc.Width - trSrc.Width, tSrc.Height);
                switch (GetDrawStyle(ControlBorderSpriteType.Top))
                {
                    case ControlBorderDrawStyle.Stretch:
                        _t.Draw(sb, r, Color.White);
                        break;

                    case ControlBorderDrawStyle.Tile:
                        AssertSaneDrawTilingAmount(r.X, r.Right, _t.Size.X, ControlBorderSpriteType.Top);
                        sb.DrawTiledX(r.X, r.Right, r.Y, _t, Color.White);
                        break;
                }

                // Left side
                r = new Rectangle(cX, cY + tlSrc.Height, lSrc.Width, cH - tlSrc.Height - blSrc.Height);
                switch (GetDrawStyle(ControlBorderSpriteType.Left))
                {
                    case ControlBorderDrawStyle.Stretch:
                        _l.Draw(sb, r, Color.White);
                        break;

                    case ControlBorderDrawStyle.Tile:
                        AssertSaneDrawTilingAmount(r.Y, r.Bottom, _l.Size.Y, ControlBorderSpriteType.Left);
                        sb.DrawTiledY(r.Y, r.Bottom, r.X, _l, Color.White);
                        break;
                }

                // Right side
                r = new Rectangle(cX + cW - rSrc.Width, cY + trSrc.Height, rSrc.Width, cH - trSrc.Height - brSrc.Height);
                switch (GetDrawStyle(ControlBorderSpriteType.Right))
                {
                    case ControlBorderDrawStyle.Stretch:
                        _r.Draw(sb, r, Color.White);
                        break;

                    case ControlBorderDrawStyle.Tile:
                        AssertSaneDrawTilingAmount(r.Y, r.Bottom, _r.Size.Y, ControlBorderSpriteType.Right);
                        sb.DrawTiledY(r.Y, r.Bottom, r.X, _r, Color.White);
                        break;
                }

                // Bottom side
                r = new Rectangle(cX + _bl.Source.Width, cY + cH - bSrc.Height, cW - blSrc.Width - brSrc.Width, bSrc.Height);
                switch (GetDrawStyle(ControlBorderSpriteType.Bottom))
                {
                    case ControlBorderDrawStyle.Stretch:
                        _b.Draw(sb, r, Color.White);
                        break;

                    case ControlBorderDrawStyle.Tile:
                        AssertSaneDrawTilingAmount(r.X, r.Right, _b.Size.X, ControlBorderSpriteType.Bottom);
                        sb.DrawTiledX(r.X, r.Right, r.Y, _b, Color.White);
                        break;
                }

                // Top-left corner
                r = new Rectangle(cX, cY, tlSrc.Width, tlSrc.Height);
                _tl.Draw(sb, r, Color.White);

                // Top-right corner
                r = new Rectangle(cX + cW - trSrc.Width, cY, trSrc.Width, trSrc.Height);
                _tr.Draw(sb, r, Color.White);

                // Bottom-left corner
                r = new Rectangle(cX, cY + cH - blSrc.Height, blSrc.Width, blSrc.Height);
                _bl.Draw(sb, r, Color.White);

                // Bottom-right corner
                r = new Rectangle(cX + cW - brSrc.Width, cY + cH - brSrc.Height, brSrc.Width, brSrc.Height);
                _br.Draw(sb, r, Color.White);
            }
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

            var color = c.BorderColor;

            // Apply the color transformations (if they exist)
            if (_globalColorTransformations.Count > 0)
            {
                lock (_globalColorTransformationsSync)
                {
                    foreach (var t in _globalColorTransformations)
                    {
                        color = t(c, color);
                    }
                }
            }

            Vector2 sp = c.ScreenPosition;
            Rectangle region = new Rectangle(sp.X, sp.Y, c.Size.X, c.Size.Y);
            Draw(sb, region, color);
        }

        /// <summary>
        /// Gets the <see cref="ControlBorderDrawStyle"/> for the given <see cref="ControlBorderSpriteType"/>.
        /// </summary>
        /// <param name="spriteType">The type of <see cref="ControlBorderSpriteType"/>.</param>
        /// <returns>The <see cref="ControlBorderDrawStyle"/> for the <paramref name="spriteType"/>.</returns>
        public ControlBorderDrawStyle GetDrawStyle(ControlBorderSpriteType spriteType)
        {
            return _drawStyles[(int)spriteType];
        }

        /// <summary>
        /// Gets the <see cref="ISprite"/> for the given <see cref="ControlBorderSpriteType"/>.
        /// </summary>
        /// <param name="spriteType">The type of <see cref="ControlBorderSpriteType"/>.</param>
        /// <returns>The <see cref="ISprite"/> for the <paramref name="spriteType"/>.</returns>
        public ISprite GetSprite(ControlBorderSpriteType spriteType)
        {
            return _sprites[(int)spriteType];
        }

        /// <summary>
        /// Loads the <see cref="ControlBorderDrawStyle"/>s from file.
        /// </summary>
        /// <param name="filePath">The path to the file to load the <see cref="ControlBorderDrawStyle"/>s from.</param>
        /// <returns>The loaded <see cref="ControlBorderDrawStyle"/>s, where each element in the array contains the
        /// <see cref="ControlBorderDrawStyle"/> and can be indexed with the <see cref="ControlBorderSpriteType"/>.</returns>
        public static ControlBorderDrawStyle[] LoadDrawStyles(string filePath)
        {
            var ret = new ControlBorderDrawStyle[_numSprites];

            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                    continue;

                var l = line.Trim();
                if (l.Length == 0)
                    continue;

                var split = l.Split(':');
                if (split.Length == 2)
                {
                    var key = split[0];
                    ControlBorderDrawStyle value;

                    if (EnumHelper<ControlBorderDrawStyle>.TryParse(split[1], out value))
                    {
                        // Wild-card
                        if (StringComparer.OrdinalIgnoreCase.Equals(key, "*"))
                        {
                            for (var i = 0; i < ret.Length; i++)
                            {
                                ret[i] = value;
                            }
                            continue;
                        }

                        // Parse enum
                        ControlBorderSpriteType keyEnum;
                        if (EnumHelper<ControlBorderSpriteType>.TryParse(key, out keyEnum))
                        {
                            ret[(int)keyEnum] = value;
                            continue;
                        }
                    }
                }

                // Unable to parse
                const string errmsg = "Unable to parse line in file `{0}`: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, filePath, line);
                Debug.Fail(string.Format(errmsg, filePath, line));
            }

            return ret;
        }

        /// <summary>
        /// Removes a <see cref="Func{T,U}"/> that can be used to apply a transformation on the <see cref="Color"/> of a
        /// <see cref="ControlBorder"/> for a <see cref="Control"/> for every <see cref="ControlBorder"/> being drawn.
        /// </summary>
        /// <param name="transformer">The transformation.</param>
        public static void RemoveGlobalColorTransformation(Func<Control, Color, Color> transformer)
        {
            lock (_globalColorTransformationsSync)
            {
                _globalColorTransformations.Remove(transformer);
            }
        }

        /// <summary>
        /// Gets the <see cref="ISprite"/> for the given <see cref="ControlBorderSpriteType"/>.
        /// </summary>
        /// <param name="spriteType">The type of <see cref="ControlBorderSpriteType"/>.</param>
        /// <param name="value">The <see cref="ControlBorderDrawStyle"/> to assign to the <paramref name="spriteType"/>.</param>
        public void SetDrawStyle(ControlBorderSpriteType spriteType, ControlBorderDrawStyle value)
        {
            _drawStyles[(int)spriteType] = value;

            UpdateCanDraw();
        }

        /// <summary>
        /// Gets the <see cref="ISprite"/> for the given <see cref="ControlBorderSpriteType"/>.
        /// </summary>
        /// <param name="spriteType">The type of <see cref="ControlBorderSpriteType"/>.</param>
        /// <param name="value">The <see cref="ISprite"/> to assign to the <paramref name="spriteType"/>.</param>
        public void SetSprite(ControlBorderSpriteType spriteType, ISprite value)
        {
            _sprites[(int)spriteType] = value;

            UpdateCanDraw();
        }

        /// <summary>
        /// Loads the <see cref="ControlBorderDrawStyle"/>s from file.
        /// </summary>
        /// <param name="filePath">The path to the file to load the <see cref="ControlBorderDrawStyle"/>s from.</param>
        /// <returns>True if successfully loaded the data from the <paramref name="filePath"/>; otherwise false.</returns>
        public bool TrySetDrawStyles(string filePath)
        {
            if (!File.Exists(filePath))
                return false;

            try
            {
                var styles = LoadDrawStyles(filePath);
                for (var i = 0; i < _drawStyles.Length; i++)
                {
                    _drawStyles[i] = styles[i];
                }

                return true;
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to load ControlBorderDrawStyles from `{0}`. Exception: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, filePath, ex);
                Debug.Fail(string.Format(errmsg, filePath, ex));

                return false;
            }
        }

        /// <summary>
        /// Updates the <see cref="_canDraw"/> value.
        /// </summary>
        void UpdateCanDraw()
        {
            Debug.Assert((int)ControlBorderSpriteType.Background == 0);

            _canDraw = true;

            for (var i = 1; i < _sprites.Length; i++)
            {
                if (_sprites[i] == null)
                {
                    _canDraw = false;
                    break;
                }
            }
        }
    }
}