using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics.GUI
{
    public abstract class TextControlSettings : ControlSettings
    {
        readonly Color _foreColor;

        public Color ForeColor
        {
            get { return _foreColor; }
        }

        protected TextControlSettings(Color foreColor, ControlBorder border) : base(border)
        {
            _foreColor = foreColor;
        }
    }
}