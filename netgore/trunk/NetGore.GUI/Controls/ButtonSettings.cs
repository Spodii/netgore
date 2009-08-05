using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics.GUI
{
    public class ButtonSettings : TextControlSettings
    {
        static ButtonSettings _default = null;
        ControlBorder _mouseOver;
        ControlBorder _pressed;

        public static ButtonSettings Default
        {
            get
            {
                if (_default == null)
                    _default = new ButtonSettings(null, null, null);
                return _default;
            }
        }

        public ControlBorder MouseOver
        {
            get { return _mouseOver; }
            set { _mouseOver = value; }
        }

        public ControlBorder Pressed
        {
            get { return _pressed; }
            set { _pressed = value; }
        }

        public ButtonSettings(ControlBorder border, ControlBorder over, ControlBorder pressed) : base(Color.Black, border)
        {
            MouseOver = over;
            Pressed = pressed;
        }
    }
}