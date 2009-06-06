using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Draws a line
    /// </summary>
    public class XNALine
    {
        static Grh _blankGrh = null;

        /// <summary>
        /// Static instance of the XNALine. Used when doing a static Draw() call.
        /// </summary>
        static XNALine _xnaLine = null;

        float _angle = 0.0f;
        Color _color = Color.White;
        Vector2 _p1;
        Vector2 _p2;
        Vector2 _scale = new Vector2(0.5f, 0.5f);

        /// <summary>
        /// Gets or sets the color of the line
        /// </summary>
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        /// <summary>
        /// Gets or sets the first point of the line
        /// </summary>
        public Vector2 P1
        {
            get { return _p1; }
            set
            {
                if (_p1 != value)
                {
                    _p1 = value;
                    RecalculatePoints();
                }
            }
        }

        /// <summary>
        /// Gets or sets the second point of the line
        /// </summary>
        public Vector2 P2
        {
            get { return _p2; }
            set
            {
                if (_p2 != value)
                {
                    _p2 = value;
                    RecalculatePoints();
                }
            }
        }

        /// <summary>
        /// Gets or sets the thickness of the line
        /// </summary>
        public float Thickness
        {
            get { return _scale.Y; }
            set { _scale.Y = value; }
        }

        /// <summary>
        /// XNALine constructor
        /// </summary>
        public XNALine()
        {
            _p1 = Vector2.Zero;
            _p2 = Vector2.Zero;
            Color = Color.White;
        }

        /// <summary>
        /// XNALine constructor
        /// </summary>
        /// <param name="p1">First point of the line</param>
        /// <param name="p2">Second point of the line</param>
        public XNALine(Vector2 p1, Vector2 p2)
        {
            _p1 = p1;
            _p2 = p2;
            Color = Color.White;
            RecalculatePoints();
        }

        /// <summary>
        /// XNALine constructor
        /// </summary>
        /// <param name="p1">First point of the line</param>
        /// <param name="p2">Second point of the line</param>
        /// <param name="color">Color of the line</param>
        public XNALine(Vector2 p1, Vector2 p2, Color color)
        {
            _p1 = p1;
            _p2 = p2;
            Color = color;
            RecalculatePoints();
        }

        /// <summary>
        /// Draws a line
        /// </summary>
        /// <param name="sb">SpriteBatch to draw to</param>
        /// <param name="p1">First point of the line</param>
        /// <param name="p2">Second point of the line</param>
        /// <param name="color">Color of the line</param>
        public static void Draw(SpriteBatch sb, Vector2 p1, Vector2 p2, Color color)
        {
            // Create the static XNALine instance if needed
            if (_xnaLine == null)
                _xnaLine = new XNALine();

            // Set the values and draw
            _xnaLine.SetPoints(p1, p2);
            _xnaLine.Draw(sb, color);
        }

        /// <summary>
        /// Draws the line
        /// </summary>
        /// <param name="sb">SpriteBatch to draw to</param>
        /// <param name="tex">Texture to use for drawing</param>
        public void Draw(SpriteBatch sb, Texture2D tex)
        {
            Draw(sb, tex, Color);
        }

        /// <summary>
        /// Draws the line
        /// </summary>
        /// <param name="sb">SpriteBatch to draw to</param>
        public void Draw(SpriteBatch sb)
        {
            Draw(sb, Color);
        }

        /// <summary>
        /// Draws the line
        /// </summary>
        /// <param name="sb">SpriteBatch to draw to</param>
        /// <param name="tex">Texture to use for drawing</param>
        /// <param name="color">Color to use instead of the local set color</param>
        public void Draw(SpriteBatch sb, Texture2D tex, Color color)
        {
            if (sb == null)
            {
                Debug.Fail("sb is null.");
                return;
            }
            if (sb.IsDisposed)
            {
                Debug.Fail("sb is disposed.");
                return;
            }
            if (tex == null)
            {
                Debug.Fail("tex is null.");
                return;
            }
            if (tex.IsDisposed)
            {
                Debug.Fail("tex is disposed.");
                return;
            }

            sb.Draw(tex, _p1, null, color, _angle, Vector2.Zero, _scale, SpriteEffects.None, 0f);
        }

        /// <summary>
        /// Draws the line
        /// </summary>
        /// <param name="sb">SpriteBatch to draw to</param>
        /// <param name="color">Color to use instead of the local set color</param>
        public void Draw(SpriteBatch sb, Color color)
        {
            if (sb == null)
            {
                Debug.Fail("sb is null.");
                return;
            }
            if (sb.IsDisposed)
            {
                Debug.Fail("sb is disposed.");
                return;
            }

            LoadBlankGrh();
            if (_blankGrh == null)
                return;

            _blankGrh.Draw(sb, _p1, color, SpriteEffects.None, _angle, Vector2.Zero, _scale);
        }

        /// <summary>
        /// Finds the angle (in radians) between two points
        /// </summary>
        /// <param name="p1">First point</param>
        /// <param name="p2">Second point</param>
        /// <returns>Angle between the two points, in radians</returns>
        static float GetAngle(Vector2 p1, Vector2 p2)
        {
            return (float)Math.Atan2(p2.Y - p1.Y, p2.X - p1.X);
        }

        /// <summary>
        /// Ensures the _blankGrh is loaded
        /// </summary>
        static void LoadBlankGrh()
        {
            if (_blankGrh != null)
                return;

            GrhData gd = GrhInfo.GetData("System", "Blank");
            if (gd == null)
                throw new Exception("Failed to load GrhData System.Blank.");

            _blankGrh = new Grh(gd);
        }

        /// <summary>
        /// Recalculates the locals for the line
        /// </summary>
        void RecalculatePoints()
        {
            _angle = GetAngle(_p1, _p2);
            _scale.X = Vector2.Distance(_p1, _p2) / 2;
        }

        /// <summary>
        /// Sets both the points of the line
        /// </summary>
        /// <param name="p1">First point of the line</param>
        /// <param name="p2">Second point of the line</param>
        public void SetPoints(Vector2 p1, Vector2 p2)
        {
            _p1 = p1;
            _p2 = p2;
            RecalculatePoints();
        }
    }
}