using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.Graphics;

namespace DemoGame.MapEditor
{
    class FocusedSpatialDrawer
    {
        int _steps;
        ISpatial _focused;

        public ISpatial Focused
        {
            get { return _focused; }
            set
            {
                if (_focused == value)
                    return;

                _focused = value;
                _steps = 50;
            }
        }

        Rectangle ApplyStepping(Rectangle rect, int steps)
        {
            rect.X -= steps;
            rect.Y -= steps;
            rect.Width += steps * 2;
            rect.Height += steps * 2;
            return rect;
        }

        static readonly Color _highlightColor = new Color(0, 255, 0, 100);
        static readonly Color _highlightBorderColor = new Color(0, 0, 0, 150);
        static readonly Color _trackColor = new Color(0, 0, 0, 0);
        static readonly Color _trackInnerBorderColor = new Color(255, 255, 255, 150);
        static readonly Color _trackOutterBorderColor = new Color(0, 0, 0, 150);

        public void Draw(ISpatial spatial, SpriteBatch sb)
        {
            Focused = spatial;
            Draw(sb);
        }

        public void Draw(SpriteBatch sb)
        {
            if (Focused == null)
                return;

            var r = Focused.ToRectangle();
            XNARectangle.Draw(sb, r, _highlightColor, _highlightBorderColor);

            if (_steps > 0)
            {
                XNARectangle.Draw(sb, ApplyStepping(r, _steps), _trackColor, _trackInnerBorderColor);
                XNARectangle.Draw(sb, ApplyStepping(r, _steps - 1), _trackColor, _trackOutterBorderColor);
                XNARectangle.Draw(sb, ApplyStepping(r, _steps + 1), _trackColor, _trackOutterBorderColor);
                _steps -= 4;
            }
        }
    }
}