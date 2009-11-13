using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using NetGore;

namespace NetGore.Graphics.GUI
{
    public class TextBoxSettings : TextControlSettings
    {
        static TextBoxSettings _default = null;

        public TextBoxSettings(ControlBorder border) : base(Color.Black, border)
        {
            CanDrag = false;
        }

        public static TextBoxSettings Default
        {
            get
            {
                if (_default == null)
                    _default = new TextBoxSettings(null);
                return _default;
            }
        }
    }
}