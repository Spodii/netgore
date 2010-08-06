using System.Linq;
using NetGore.World;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Draws an indicator where the focused <see cref="ISpatial"/> is at on the map.
    /// </summary>
    public class FocusedSpatialDrawer
    {
        /// <summary>
        /// The initial number of steps to display.
        /// </summary>
        const int _maxSteps = 700;

        static readonly Color _focusedBorderColorInner = new Color(0, 0, 0, 150);
        static readonly Color _focusedBorderColorOuter = new Color(255, 255, 255, 150);
        static readonly Color _focusedColor = new Color(0, 255, 0, 75);
        static readonly Color _nonfocusedBorderColorInner = new Color(0, 0, 0, 150);
        static readonly Color _nonfocusedBorderColorOuter = new Color(255, 255, 255, 150);
        static readonly Color _nonfocusedColor = new Color(0, 0, 0, 0);
        static readonly Color _trackColor = new Color(0, 0, 0, 0);
        static readonly Color _trackInnerBorderColor = new Color(255, 255, 255, 150);
        static readonly Color _trackOuterBorderColor = new Color(0, 0, 0, 150);

        ISpatial _focused;
        int _steps;

        /// <summary>
        /// Gets or sets the <see cref="ISpatial"/> to focus on.
        /// </summary>
        public ISpatial Focused
        {
            get { return _focused; }
            set
            {
                if (_focused == value)
                    return;

                _focused = value;
                _steps = _maxSteps;
            }
        }

        /// <summary>
        /// Expands a <see cref="Rectangle"/>'s sides out an equal amount in all directions.
        /// </summary>
        /// <param name="rect">The original <see cref="Rectangle"/>.</param>
        /// <param name="steps">The amount to expand the <paramref name="rect"/> out in each direction.</param>
        /// <returns>The <see cref="Rectangle"/> with the sides pushed out an equal amount in all directions.</returns>
        static Rectangle ApplyStepping(Rectangle rect, int steps)
        {
            rect.X -= steps;
            rect.Y -= steps;
            rect.Width += steps * 2;
            rect.Height += steps * 2;
            return rect;
        }

        /// <summary>
        /// Draws the <see cref="FocusedSpatialDrawer"/>.
        /// </summary>
        /// <param name="sb">The <see cref="ISpriteBatch"/> to draw to.</param>
        public void Draw(ISpriteBatch sb)
        {
            if (Focused == null)
                return;

            var r = Focused.ToRectangle();
            RenderRectangle.Draw(sb, r, _focusedColor, _focusedBorderColorInner);

            var r2 = new Rectangle(r.X - 1, r.Y - 1, r.Width + 2, r.Height + 2);
            RenderRectangle.Draw(sb, r2, new Color(0, 0, 0, 0), _focusedBorderColorOuter);

            if (_steps > 0)
            {
                RenderRectangle.Draw(sb, ApplyStepping(r, _steps), _trackColor, _trackInnerBorderColor);
                RenderRectangle.Draw(sb, ApplyStepping(r, _steps - 1), _trackColor, _trackOuterBorderColor);
                RenderRectangle.Draw(sb, ApplyStepping(r, _steps + 1), _trackColor, _trackOuterBorderColor);
                _steps -= 4 + (_steps / 10);
            }
        }

        /// <summary>
        /// Draws the <see cref="FocusedSpatialDrawer"/>.
        /// </summary>
        /// <param name="spatial">The <see cref="ISpatial"/> to focus on.</param>
        /// <param name="sb">The <see cref="ISpriteBatch"/> to draw to.</param>
        public void DrawFocused(ISpatial spatial, ISpriteBatch sb)
        {
            Focused = spatial;
            Draw(sb);
        }

        /// <summary>
        /// Draws a <see cref="ISpatial"/> that is selected, but NOT focused on. This is primarily
        /// intended for when selecting multiple items at once to give an indication on what other
        /// objects are selected but to distinguish that they are not the focused objects.
        /// </summary>
        /// <param name="spatial">The <see cref="ISpatial"/> that is selected but not focused on.</param>
        /// <param name="sb">The <see cref="ISpriteBatch"/> to draw to.</param>
        public static void DrawNotFocused(ISpatial spatial, ISpriteBatch sb)
        {
            if (spatial == null)
                return;

            var r = spatial.ToRectangle();
            RenderRectangle.Draw(sb, r, _nonfocusedColor, _nonfocusedBorderColorInner);

            var r2 = new Rectangle(r.X - 1, r.Y - 1, r.Width + 2, r.Height + 2);
            RenderRectangle.Draw(sb, r2, new Color(0, 0, 0, 0), _nonfocusedBorderColorOuter);
        }

        /// <summary>
        /// Forces the indicator to reset and show where the <see cref="ISpatial"/> is again.
        /// </summary>
        public void ResetIndicator()
        {
            _steps = _maxSteps;
        }
    }
}