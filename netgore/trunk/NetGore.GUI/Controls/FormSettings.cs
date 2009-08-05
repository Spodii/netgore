using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics.GUI
{
    public class FormSettings : TextControlSettings
    {
        static FormSettings _default = null;

        public static FormSettings Default
        {
            get
            {
                if (_default == null)
                    new FormSettings(Color.Black, null);
                return _default;
            }
        }

        public FormSettings(Color foreColor, ControlBorder border) : base(foreColor, border)
        {
        }
    }
}