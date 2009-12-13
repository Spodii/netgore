using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics.GUI
{
    public abstract class TextControlSettings : ControlSettings
    {
        Color _foreColor;

        protected TextControlSettings(Color foreColor, ControlBorder border) : base(border)
        {
            _foreColor = foreColor;
            CanFocus = true;
        }

        public Color ForeColor
        {
            get { return _foreColor; }
            set { _foreColor = value; }
        }
    }
}